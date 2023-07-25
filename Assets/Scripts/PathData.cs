using System.Collections.Generic;
using UnityEngine;

public class PathData
{
    public PathData()
    {
        Init();
    }
    public void Init()
    {
        speed_add.Add(Vector2.zero);
        timestamp.Add(-1);
    }
    public List<Vector2> speed_add = new List<Vector2>();
    public List<int> timestamp = new List<int>();
    public int pointer = 0;
    public void TryAddSpeed(int timer, Vector3 pos)
    {
        if (speed_add.Count >= 1024) return;
        Vector2 tempv2 = pos;
        if (tempv2 != speed_add[^1])
        {
            timestamp.Add(timer);
            speed_add.Add(tempv2);
        }
        //最多装下1022个数据
        if (speed_add.Count >= 1023)
        {
            speed_add.Add(Vector2.zero);
            timestamp.Add(timer + 2);
        }
    }
    public Vector2 GetSpeedByTime(int i)
    {
        if (pointer >= speed_add.Count) return Vector2.zero;
        if (i >= timestamp[pointer]) pointer++;
        if (pointer >= speed_add.Count || pointer == 0) return Vector2.zero;
        return speed_add[pointer - 1];
    }
    public void Clear()
    {
        speed_add.Clear();
        timestamp.Clear();
        pointer = 0;
        Init();
    }
}