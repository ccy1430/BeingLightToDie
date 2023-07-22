using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swear : MonoBehaviour
{
    public Level_Swear_Pledge swear;
    public GameObject emosPrefab;

    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.backMenu, EndSwearLevel)
            (GenericSign.level_swear_end, EndSwearLevel)
            (GenericSign.level_swear, ShowSwearLevel);
        emosPool = new GenericPool<GameObject>(CreatEmo, Emo_EnPool, Emo_DePool);
    }

    private void ShowSwearLevel()
    {
        int levelIndex = SaveData.Data.levelIndex;
        creatSpeed = 10f / (1 + levelIndex / 10);
        swear.gameObject.SetActive(true);
        if (levelIndex != 1)
        {
            creatCor = StartCoroutine(CreatEmos());
        }
    }

    private float creatSpeed = 0.5f;
    private Coroutine creatCor;
    private GenericPool<GameObject> emosPool;
    private readonly List<GameObject> allEmos = new List<GameObject>();
    private RandomList randomList = new RandomList(4);
    private IEnumerator CreatEmos()
    {
        var wfs = new WaitForSeconds(creatSpeed);
        while (true)
        {
            emosPool.GetT();
            yield return wfs;
        }
    }
    private GameObject CreatEmo()
    {
        var res = Instantiate(emosPrefab, new Vector2(100, 0), Quaternion.identity, transform);
        res.GetComponent<Level_Swear_Emotion>().Init(swear, emosPool);
        allEmos.Add(res);
        return res;
    }
    private void Emo_EnPool(GameObject item)
    {
        item.SetActive(false);
    }
    private void Emo_DePool(GameObject item)
    {
        item.SetActive(true);
    }
    private void EndSwearLevel()
    {
        if (creatCor != null) StopCoroutine(creatCor);
        swear.gameObject.SetActive(false);
        foreach (var item in allEmos)
        {
            if (item.activeSelf)
            {
                emosPool.EnPool(item);
            }
        }
    }

    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.backMenu, EndSwearLevel)
            (GenericSign.level_swear_end, EndSwearLevel)
            (GenericSign.level_swear, ShowSwearLevel);
    }
}
