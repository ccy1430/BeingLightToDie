using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public GameObject rainDrop;
    private GenericPool<RainDrop> rainPool;

    public float dropSpeedMin;
    public float dropSpeedMax;
    public float creatCountPerSecond;

    private const float height = 9f;
    private const float weight = 16;

    private int creatCountPerOnce = 1;
    private float inter;

    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.startLevel, BeginRain)
            (GenericSign.nextLevel, EndRain)
            (GenericSign.backMenu, EndRain);
        inter = 1 / creatCountPerSecond;
        if (inter < 0.02f)
        {
            creatCountPerOnce = (int)(0.02f / inter) + 1;
            inter *= creatCountPerOnce;
        }
        rainPool = new GenericPool<RainDrop>(CreatRain, DropEnd, DropOne);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.startLevel, BeginRain)
            (GenericSign.nextLevel, EndRain)
            (GenericSign.backMenu, EndRain);
    }

    private void BeginRain()
    {
        CancelInvoke(nameof(CreatRainDrop));
        InvokeRepeating(nameof(CreatRainDrop), 0, inter);
    }
    private void EndRain()
    {
        CancelInvoke(nameof(CreatRainDrop));
        var rains = GetComponentsInChildren<RainDrop>(true);
        foreach (var item in rains)
        {
            if (item.gameObject.activeSelf)
            {
                rainPool.EnPool(item);
            }
        }
    }

    private void CreatRainDrop()
    {
        for (int i = 0; i < creatCountPerOnce; i++)
        {
            rainPool.GetT();
        }
    }
    private RainDrop CreatRain()
    {
        var rd = Instantiate(rainDrop, transform).GetComponent<RainDrop>();
        rd.belongPool = rainPool;
        rd.CreatInit();
        DropOne(rd);
        return rd;
    }
    private void DropOne(RainDrop rd)
    {
        rd.Init(Random.Range(dropSpeedMin, dropSpeedMax), new Vector2(Random.Range(-weight, weight), height));
    }
    private void DropEnd(RainDrop rd)
    {
        rd.gameObject.SetActive(false);
    }
}
