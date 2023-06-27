using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AllLights : MonoBehaviour
{
    public GameObject lightPrefab;
    private Queue<GameObject> lightQueue = new Queue<GameObject>(256);
    private Dictionary<Transform, PathData> dic_LightsAndPaths = new Dictionary<Transform, PathData>(16);
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
        dic_LightsAndPaths.Add(light.transform, path);
        ReplayAllLight();
        GenericMsg.Trigger(GenericSign.startLevel);
    }
    private void FixedUpdate()
    {
        foreach (var item in dic_LightsAndPaths)
        {
            item.Key.position += (Vector3)item.Value.GetSpeedByTime(fixedIndex);
            if (item.Value.point >= item.Value.timestamp.Count)
            {
                GameTool.CloseLight(item.Key.GetComponent<Light2D>());
            }
        }
        fixedIndex++;
    }
    public void ReplayAllLight()
    {
        fixedIndex = 0;
        Vector3 pos = GameManager.Instance.player.originPos;
        foreach (var item in dic_LightsAndPaths)
        {
            item.Value.ResetPoint();
            item.Key.position = pos;
            GameTool.OpenLight(item.Key.GetComponent<Light2D>());
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
            foreach (var item in dic_LightsAndPaths)
            {
                pathDataPool.EnPool(item.Value);
            }
        }
        dic_LightsAndPaths.Clear();
    }
}
