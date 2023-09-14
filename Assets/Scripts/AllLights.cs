using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AllLights : MonoBehaviour
{
    public GameObject lightPrefab;
    //已经生成的预制体
    private Queue<GameObject> lightQueue = new Queue<GameObject>(256);
    //成对的light和pathdata
    private List<Light2D> lights = new List<Light2D>();
    private List<Coroutine> lightFade = new List<Coroutine>();
    private List<PathData> paths = new List<PathData>();
    //已经跑完全程的light
    private HashSet<int> hadClose = new HashSet<int>();

    private int fixedIndex = 0;
    private GenericPool<PathData> pathDataPool;
    private const float baseLightSize = 1.5f;
    private void Awake()
    {
        GenericMsg<PathData>.AddReceiver(GenericSign.playerDie, SetLight);
        GenericMsg.AddReceiver
            (GenericSign.playerDie, CloseLight)
            (GenericSign.nextLevel, RecyleLights)
            (GenericSign.backMenu, RecyleLights);
        pathDataPool = GenericPool<PathData>.LinkSignPool(GenericSign.playerDie, () =>
             {
                 return new GenericPool<PathData>(() => { return new PathData(); }, (pd) => { pd.Clear(); }, null);
             });
    }
    private void OnDestroy()
    {
        GenericMsg<PathData>.DelReceiver(GenericSign.playerDie, SetLight);
        GenericMsg.DelReceiver
            (GenericSign.playerDie, CloseLight)
            (GenericSign.nextLevel, RecyleLights)
            (GenericSign.backMenu, RecyleLights);
        GenericPool<PathData>.UnLinkSignPool(GenericSign.playerDie);
    }

    public void SetLight(PathData path)
    {
        int rc = SaveData.Data.rememerCount;
        if (rc == 0) return;
        if (lights.Count >= rc && paths.Count >= rc)
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
        var l2d = light.GetComponent<Light2D>();
        if (SaveData.Data.remererLightRandColor)
        {
            l2d.color = Color.HSVToRGB(Random.Range(0, 1f), 0.6f, 1);
        }
        else
        {
            l2d.color = Color.white;
        }
        lights.Add(l2d);
        lightFade.Add(null);
        paths.Add(path);
        ReplayAllLight();
    }
    public void ReplayAllLight()
    {
        fixedIndex = 0;
        hadClose.Clear();
        Vector3 pos = GameManager.Instance.player.originPos;
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].pointer = 0;
            lights[i].transform.position = pos;
            GameTool.OpenLight(lights[i], baseLightSize * SaveData.Data.remererLightSize);
        }
    }
    private void CloseLight()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (lightFade[i] != null)
            {
                GenericTools.StopCoroutine(lightFade[i]);
                lightFade[i] = null;
            }
            var l2d = lights[i];
            GameTool.CloseLight(l2d, l2d.pointLightOuterRadius);
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            if (hadClose.Contains(i)) continue;
            lights[i].transform.position += (Vector3)paths[i].GetSpeedByTime(fixedIndex);
            if (paths[i].pointer >= paths[i].timestamp.Count)
            {
                float startSize = baseLightSize * SaveData.Data.remererLightSize;
                const float leftSize = 0.5f;
                Light2D l2d = lights[i];
                int temp = i;
                lightFade[temp] = GenericTools.DelayFun(GameTool.LightTime, (float t) =>
                {
                    l2d.pointLightOuterRadius = startSize * (1 - t) + leftSize * t;
                }, () => lightFade[temp] = null);
                hadClose.Add(i);
            }
        }
        fixedIndex++;
    }

    public void RecyleLights()
    {
        lightQueue.Clear();
        for (int i = 0; i < lights.Count; i++)
        {
            lightQueue.Enqueue(lights[i].gameObject);
            if (lightFade[i] != null)
            {
                GenericTools.StopCoroutine(lightFade[i]);
                lightFade[i] = null;
            }
            var l2d = lights[i];
            GameTool.CloseLight(l2d, l2d.pointLightOuterRadius, () => l2d.gameObject.SetActive(false));
        }
        if (pathDataPool != null)
        {
            foreach (var item in paths)
            {
                pathDataPool.EnPool(item);
            }
        }
        lights.Clear();
        paths.Clear();
        hadClose.Clear();
    }
}
