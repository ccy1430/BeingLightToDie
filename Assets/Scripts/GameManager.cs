using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AllLights allLights;
    public Player player;
    public GameObject you;
    public UIManager uimanager;

    private void Start()
    {
        GenericMsg.AddReceiver(GenericSign.nextLevel, NextLevel_Start);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver(GenericSign.nextLevel, NextLevel_Start);
    }
    public void NextLevel_Start()
    {
        int curLevelIndex = SaveData.Data.levelIndex;
        curLevelIndex++;
        SaveData.Data.levelIndex = curLevelIndex;
        SaveData.Save();
        GenericTools.DelayFun(GameTool.LightTime, null, () =>
        {
            GenericMsg.Trigger(GenericSign.level_swear);
        });
    }
}
