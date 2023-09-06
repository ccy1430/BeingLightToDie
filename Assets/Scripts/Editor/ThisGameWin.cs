using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class ThisGameWin : EditorWindow
{

    [MenuItem("ThisGame/GameWin")]
    public static void Init()
    {
        var gamewin = new ThisGameWin();
        gamewin.Show();
    }

    public int levelIndex;

    public int baseLevel;
    private string copyLevel;
    public int tripToLevel;
    public bool saveTrip;
    private void OnGUI()
    {
        levelIndex = EditorGUILayout.IntField("关卡下标", levelIndex);
        if (GUILayout.Button("跳转关卡"))
        {
            SaveData.Data.levelIndex = levelIndex;
            SaveData.Save();
        }

        GUILayout.Space(20);
        if (GUILayout.Button("删除存档"))
        {
            SaveData.Delete();
        }

        GUILayout.Space(20);
        baseLevel = EditorGUILayout.IntField("被复制关卡", baseLevel);
        EditorGUILayout.LabelField("————  先分割| 再分割-  ————");
        copyLevel = EditorGUILayout.TextField("复制关卡表(string)", copyLevel);
        if (GUILayout.Button("复制关卡"))
        {
            CopyLevel();
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("保持陷阱");
        saveTrip = EditorGUILayout.Toggle(saveTrip);
        tripToLevel = EditorGUILayout.IntField("<-save trip | trip_to_level->", tripToLevel);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("tripToLevel"))
        {
            TripToLevel();
        }
        GUILayout.Space(20);
        if (GUILayout.Button("运行时跳过关卡"))
        {
            JumpLevel();
        }
    }
    const string frontPath = "Assets/Resources/Levels/";
    BoundsInt bound = GameConfig.allTilesBound;
    private void CopyLevel()
    {

        var go = AssetDatabase.LoadAssetAtPath<GameObject>($"{frontPath}{baseLevel}.prefab");
        var level = go.transform.Find("level").GetComponent<Tilemap>().GetTilesBlock(bound);
        var trip = go.transform.Find("trip").GetComponent<Tilemap>().GetTilesBlock(bound);
        List<int> copyIs = new List<int>();
        var ss1 = copyLevel.Split('|');
        foreach (var item in ss1)
        {
            var ss2 = item.Split('-');
            if (ss2.Length == 1)
            {
                copyIs.Add(int.Parse(ss2[0]));
            }
            else
            {
                for (int i = int.Parse(ss2[0]); i <= int.Parse(ss2[1]); i++)
                {
                    copyIs.Add(i);
                }
            }
        }
        foreach (var item in copyIs)
        {
            var prefabpath = $"{frontPath}{item}.prefab";
            var copyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabpath);

            var instance = PrefabUtility.InstantiatePrefab(copyPrefab) as GameObject;

            instance.transform.Find("level").GetComponent<Tilemap>().SetTilesBlock(bound, level);
            instance.transform.Find("trip").GetComponent<Tilemap>().SetTilesBlock(bound, trip);

            instance.transform.Find("level").GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
            instance.transform.Find("trip").GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
            PrefabUtility.SaveAsPrefabAsset(instance, prefabpath);
            DestroyImmediate(instance);
            //EditorUtility.SetDirty(copyPrefab);
            //AssetDatabase.SaveAssetIfDirty(copyPrefab);
        }
        Debug.Log($"copy success {copyIs.Count}");
    }
    private void TripToLevel()
    {
        var prefabpath = $"{frontPath}{tripToLevel}.prefab";
        var copyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabpath);
        var instance = PrefabUtility.InstantiatePrefab(copyPrefab) as GameObject;

        var trip = instance.transform.Find("trip").GetComponent<Tilemap>().GetTilesBlock(bound);

        instance.transform.Find("level").GetComponent<Tilemap>().SetTilesBlock(bound, trip);
        if (!saveTrip) instance.transform.Find("trip").GetComponent<Tilemap>().ClearAllTiles();

        instance.transform.Find("level").GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
        instance.transform.Find("trip").GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
        PrefabUtility.SaveAsPrefabAsset(instance, prefabpath);
        DestroyImmediate(instance);
    }
    private void JumpLevel()
    {
        var player = GameObject.FindObjectOfType<Player>();
        if (player != null && player.gameObject.activeSelf)
        {
            player.ThroughLevel();
        }
    }
}
