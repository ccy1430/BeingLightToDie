using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AllLights : MonoBehaviour
{
    public GameObject lightPrefab;
    private Queue<GameObject> lightQueue = new Queue<GameObject>(256);
    private List<Light2D> lights = new List<Light2D>();
    private List<PathData> paths = new List<PathData>();

    private int fixedIndex = 0;
    private GenericPool<PathData> pathDataPool;
    private void Awake()
    {
        GenericMsg<PathData>.AddReceiver(GenericSign.playerDie, SetLight);
        GenericMsg.AddReceiver
            (GenericSign.nextLevel, RecyleLights)
            (GenericSign.backMenu, RecyleLights);
        pathDataPool = GenericPool<PathData>.LinkSignPool(GenericSign.playerDie, () =>
             {
                 return new GenericPool<PathData>(() => { return new PathData(); }, (pd) => { pd.Clear(); }, null);
             }
            );
    }
    private void OnDestroy()
    {
        GenericMsg<PathData>.DelReceiver(GenericSign.playerDie, SetLight);
        GenericMsg.DelReceiver
            (GenericSign.nextLevel, RecyleLights)
            (GenericSign.backMenu, RecyleLights);
        GenericPool<PathData>.UnLinkSignPool(GenericSign.playerDie);
    }

    public void SetLight(PathData path)
    {
        if (SaveData.Data.rememerCount == 0) return;
        if (lights.Count == SaveData.Data.rememerCount)
        {
            lightQueue.Enqueue(lights[0].gameObject);
            lights.RemoveAt(0);
            pathDataPool.EnPool(paths[0]);
            paths.RemoveAt(0);
        }
        GameObject light;
        if (lightQueue.Count == 0)
        {
            light = Instantiate(lightPrefab, transform);
        }
        else
        {
            light = lightQueue.Dequeue();
        }
        light.SetActive(true);
        lights.Add(light.GetComponent<Light2D>());
        paths.Add(path);
        ReplayAllLight();
        GenericMsg.Trigger(GenericSign.startLevel);
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            lights[i].transform.position += (Vector3)paths[i].GetSpeedByTime(fixedIndex);
            if (paths[i].point >= paths[i].timestamp.Count)
            {
                GameTool.CloseLight(lights[i]);
            }
        }
        fixedIndex++;
    }
    public void ReplayAllLight()
    {
        fixedIndex = 0;
        Vector3 pos = GameManager.Instance.player.originPos;
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].ResetPoint();
            lights[i].transform.position = pos;
            GameTool.OpenLight(lights[i]);
        }
    }
    public void RecyleLights()
    {
        lightQueue.Clear();
        foreach (Transform trs in transform)
        {
            lightQueue.Enqueue(trs.gameObject);
            if (trs.gameObject.activeSelf)
            {
                var l2d = trs.gameObject.GetComponent<Light2D>();
                if (l2d != null)
                {
                    GameTool.CloseLight(l2d, cb: () => { trs.gameObject.SetActive(false); });
                }
            }
        }
        if (pathDataPool != null)
        {
            foreach (var item in paths)
            {
                pathDataPool.EnPool(item);
            }
        }
        paths.Clear();
    }
}
