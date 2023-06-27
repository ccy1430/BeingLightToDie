using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject[] prefabs;
    private Dictionary<int, string> dic_is = new Dictionary<int, string>();
    private Dictionary<string, GameObject> dic_prefabs = new Dictionary<string, GameObject>();
    private List<GameObject> hadCreat = new List<GameObject>();

    private void Start()
    {
        InitDic_Prefabs();
    }
    private void InitDic_Prefabs()
    {
        foreach(GameObject go in prefabs)
        {
            dic_prefabs.Add(go.name, go);
        }
    }
    public void LoadLevel(int level)
    {
        if (dic_prefabs.Count == 0) InitDic_Prefabs();
        foreach(var go in hadCreat)
        {
            RecyleMapGo(go);
        }
        hadCreat.Clear();
        if(dic_is.TryGetValue(level,out string leveldata))
        {
            LoadLevel(leveldata);
        }
        else
        {
            var temp = Resources.Load<TextAsset>("map/" + level.ToString());
            if (temp == null) return;
            leveldata = temp.text;
            dic_is.Add(level, leveldata);
            LoadLevel(leveldata);
        }
    }
    private void LoadLevel(string leveldata)
    {
        string[] props = leveldata.Split('\n');
        foreach(string s in props)
        {
            string[] oneprop = s.Split('|');
            GameObject go;
            if(oneprop[0]=="You "|| oneprop[0] == "Play")
            {
                if (oneprop[0] == "You ") go = GameObject.Find("You ");
                else go = GameObject.FindObjectOfType<Player>().gameObject;
            }
            else
            {
                go = GetMapGo(oneprop[0]);
                hadCreat.Add(go);
            } 
            go.transform.position = StringToVector3(oneprop[1]);
            go.transform.eulerAngles = StringToVector3(oneprop[2]);
            go.transform.localScale = StringToVector3(oneprop[3]);
        }
    }

    private Dictionary<string, List<GameObject>> mapPool = new Dictionary<string, List<GameObject>>();
    private void RecyleMapGo(GameObject go)
    {
        string propname = go.name.Substring(0, go.name.IndexOf('('));
        if (!mapPool.ContainsKey(propname)) mapPool.Add(propname, new List<GameObject>());
        mapPool[propname].Add(go);
        go.SetActive(false);
    }
    private GameObject GetMapGo(string propname)
    {
        Debug.Log(propname);
        GameObject res;
        if (mapPool.TryGetValue(propname, out List<GameObject> list))
        {
            if (list.Count > 0)
            {
                res = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                res.SetActive(true);
                return res;
            }
            return Instantiate(dic_prefabs[propname], transform);
        }
        return Instantiate(dic_prefabs[propname], transform);
    }
    private Vector3 StringToVector3(string data)
    {
        string[] thrfloat;
        if (data[0] == '(')
        {
            thrfloat = data[1..^1].Split(',');
        }
        else thrfloat = data.Split(',');
        float[] floats = new float[3];
        bool parsesuccess = true;
        for (int i = 0; i < 3; i++) parsesuccess &= float.TryParse(thrfloat[i], out floats[i]);
        if (!parsesuccess) Debug.LogError("float parse fail "+ data);
        return new Vector3(floats[0], floats[1], floats[2]);
    }
}
