using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
{
    public TileBase selfPosTileBase;
    public UnityEngine.Rendering.Universal.Light2D light2d;

    private void Awake()
    {
        GenericMsg<TileBase[]>.AddReceiver(GenericSign.loadFlower, FindAndSetPos);
        GenericMsg.AddReceiver
            (GenericSign.startLevel, StartLevel)
            (GenericSign.nextLevel, ExitLevel)
            (GenericSign.backMenu, ExitLevel)
            (GenericSign.playerDie, ExitLevel);
    }

    private void OnDestroy()
    {
        GenericMsg<TileBase[]>.DelReceiver(GenericSign.loadFlower, FindAndSetPos);
        GenericMsg.DelReceiver
            (GenericSign.startLevel, StartLevel)
            (GenericSign.nextLevel, ExitLevel)
            (GenericSign.backMenu, ExitLevel)
            (GenericSign.playerDie, ExitLevel);
    }

    private void FindAndSetPos(TileBase[] tiles)
    {
        Vector2Int playerPos = Vector2Int.one;
        for (int x = 0; x < 32; x++) for (int y = 0; y < 18; y++)
            {
                int tilesIndex = x + y * 32;
                if (tiles[tilesIndex] == selfPosTileBase)
                {
                    playerPos = new Vector2Int(x, y);
                    break;
                }
            }
        transform.position = new Vector3(playerPos.x - 15.5f, playerPos.y - 8.5f);
    }
    private void StartLevel()
    {
        gameObject.SetActive(true);
        GameTool.OpenLight(light2d);
    }
    private void ExitLevel()
    {
        if (!gameObject.activeSelf) return;
        GameTool.CloseLight(light2d, cb: () => { gameObject.SetActive(false); });
    }
}
