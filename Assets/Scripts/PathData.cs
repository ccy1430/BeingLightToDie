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
    public int point = 0;
    public void TryAddSpeed(int timer, Vector3 pos)
    {
        Vector2 tempv2 = pos;
        if (tempv2 != speed_add[^1])
        {
            timestamp.Add(timer);
            speed_add.Add(tempv2);
        }
    }
    public Vector2 GetSpeedByTime(int i)
    {
        if (point >= speed_add.Count) return Vector2.zero;
        if (i >= timestamp[point]) point++;
        if (point >= speed_add.Count || point == 0) return Vector2.zero;
        return speed_add[point - 1];
    }
    public void ResetPoint()
    {
        point = 0;
    }
    public void Clear()
    {
        speed_add.Clear();
        timestamp.Clear();
        point = 0;
        Init();
    }
}