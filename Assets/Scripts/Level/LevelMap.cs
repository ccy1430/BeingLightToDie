using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMap : MonoBehaviour
{
    [Header("water")]

    private GameObject curLevelGo = null;
    private Tilemap map_level, map_trip, map_flower;

    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.level_swear_end, LoadLevel);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.level_swear_end, LoadLevel);
    }

    private void LoadLevel()
    {
        int curLevelIndex = SaveData.Data.levelIndex;
        var go = Resources.Load<GameObject>(string.Format($"Levels/{curLevelIndex}"));
        if (curLevelGo != null)
        {
            Destroy(curLevelGo);
        }
        curLevelGo = Instantiate(go, transform);
        map_level = curLevelGo.transform.Find("level").GetComponent<Tilemap>();
        map_flower = curLevelGo.transform.Find("flower").GetComponent<Tilemap>();
        map_trip = curLevelGo.transform.Find("trip").GetComponent<Tilemap>();
        GenericMsg<TileBase[]>.Trigger(GenericSign.loadLevel, map_level.GetTilesBlock(GameConfig.allTilesBound));
        GenericMsg<TileBase[]>.Trigger(GenericSign.loadFlower, map_flower.GetTilesBlock(GameConfig.allTilesBound));
        GenericMsg.Trigger(GenericSign.startLevel);
    }
}
