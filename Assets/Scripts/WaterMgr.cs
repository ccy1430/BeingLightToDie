using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterMgr : MonoBehaviour
{
    public static WaterMgr Instance { get; private set; }

    public GameObject horWaterPrefab;
    public GameObject verWaterPrefab;
    private int[,] waterHorIndex = new int[GameConfig.mapWidth, GameConfig.mapHeight + 1];
    private List<Water_hor> horWaterList = new List<Water_hor>();
    private List<Water_ver> verWaterList = new List<Water_ver>();


    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        GenericMsg<TileBase[]>.AddReceiver(GenericSign.loadLevel, CreatWater);
    }
    private void OnDestroy()
    {
        GenericMsg<TileBase[]>.DelReceiver(GenericSign.loadLevel, CreatWater);
    }

    public void CreatWater(TileBase[] tiles)
    {
        foreach (var item in horWaterList)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in verWaterList)
        {
            Destroy(item.gameObject);
        }
        horWaterList.Clear();
        verWaterList.Clear();
        Array.Clear(waterHorIndex, 0, GameConfig.mapWidth * (GameConfig.mapHeight + 1));

        bool HadTile(int x, int y)
        {
            if (x < 0 || x >= GameConfig.mapWidth || y < 0 || y >= GameConfig.mapHeight) return false;
            return tiles[x + y * GameConfig.mapWidth] != null;
        }
        for (int i = 0; i < GameConfig.mapWidth; i++) waterHorIndex[i, 0] = 1;
        for (int x = 0; x < GameConfig.mapWidth; x++)
        {
            for (int y = 0; y < 18; y++)
            {
                if (HadTile(x, y))
                {
                    waterHorIndex[x, y] = 0;
                    waterHorIndex[x, y + 1] = 1;
                }
            }
        }
        List<Vector2Int> horwater = new List<Vector2Int>();
        int horstate = 0;//0 find 1 continue
        for (int y = 18; y >= 0; y--)
        {
            for (int x = 0; x < GameConfig.mapWidth; x++)
            {
                if (waterHorIndex[x, y] == 1 && horstate == 0)
                {
                    horwater.Add(new Vector2Int(x, y));
                    horstate = 1;
                }
                else if ((waterHorIndex[x, y] == 0 || x == 31) && horstate == 1)
                {
                    if (x == 31) horwater.Add(new Vector2Int(x, y));
                    else horwater.Add(new Vector2Int(x - 1, y));
                    horstate = 0;
                }
            }
        }
        List<Vector2Int> verwater = new List<Vector2Int>();
        {
            int sub = 1;
            foreach (var item in horwater)
            {
                sub *= -1;
                int x = item.x + sub, y = item.y;
                if (x >= 0 && x < GameConfig.mapWidth && tiles[x + y * GameConfig.mapWidth] != null)
                {
                    continue;
                }
                Vector2Int upone = item + (sub == 1 ? Vector2Int.right : Vector2Int.zero);
                while (y > 0 && (x < 0 || x > 31 || waterHorIndex[x, y] == 0)) y--;
                Vector2Int downone = new Vector2Int(upone.x, y);
                for (int i = 1; i < verwater.Count; i += 2)
                {
                    if (upone.x == verwater[i].x && upone.y > verwater[i].y)
                    {
                        verwater[i] = upone;
                        break;
                    }
                }
                verwater.Add(upone);
                verwater.Add(downone);
            }
        }
        Array.Clear(waterHorIndex, 0, GameConfig.mapWidth * (GameConfig.mapHeight + 1));
        horWaterList.Clear();
        for (int i = 0; i < horwater.Count; i += 2)
        {
            float y = horwater[i].y - 9;
            float left = horwater[i].x - 16;
            float right = horwater[i + 1].x - 15;
            float mid = (left + right) / 2;
            var newWaterHor = Instantiate(horWaterPrefab, new Vector3(mid, y + 0.2f, 0), Quaternion.identity, transform)
                .GetComponent<Water_hor>();
            int lrRadian = 3;
            if (HadTile(horwater[i].x - 1, horwater[i].y)) lrRadian -= 1;
            if (HadTile(horwater[i + 1].x + 1, horwater[i + 1].y)) lrRadian -= 2;
            newWaterHor.Init(left - mid, right - mid, lrRadian);
            horWaterList.Add(newWaterHor);
            for (int x = horwater[i].x; x <= horwater[i + 1].x; x++)
            {
                waterHorIndex[x, horwater[i].y] = horWaterList.Count;
            }
        }
        for (int i = 0; i < verwater.Count; i += 2)
        {
            float x = verwater[i].x - 16;
            float up = verwater[i].y - 9;
            float down = verwater[i + 1].y - 9;
            if (up == down) continue;
            var water_ver = Instantiate(verWaterPrefab, new Vector3(x, (up + down) / 2 - 0.05f, 0), Quaternion.identity, transform)
                .GetComponent<Water_ver>();
            water_ver.Init(up - down - 0.1f);
            verWaterList.Add(water_ver);
        }
    }

    public Water_hor GetPosWater(Vector3 pos)
    {
        float xx = pos.x + 16;
        int x = Mathf.FloorToInt(xx);
        int y = Mathf.FloorToInt(pos.y) + 9;
        if (y < 0 || y >= 19) return null;
        if (x < 0 || x >= 32) return null;
        int index = waterHorIndex[x, y] - 1;
        if (index != -1)
        {
            Water_hor res = horWaterList[index];
            return res;
        }
        if (xx % 1 >= 0.9f && x < 31)
        {
            index = waterHorIndex[x + 1, y] - 1;
            if (index != -1)
            {
                Water_hor res = horWaterList[index];
                return res;
            }
        }
        if (xx % 1 <= 0.1f && x > 0)
        {
            index = waterHorIndex[x - 1, y] - 1;
            if (index != -1)
            {
                Water_hor res = horWaterList[index];
                return res;
            }
        }
        return null;
    }
}
