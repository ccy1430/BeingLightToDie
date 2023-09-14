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
        GenericMsg.AddReceiver(GenericSign.nextLevel, NextLevel_Start);
    }

    public AllLights allLights;
    public Player player;
    public GameObject you;
    public UIManager uimanager;

    private void OnDestroy()
    {
        GenericMsg.DelReceiver(GenericSign.nextLevel, NextLevel_Start);
    }
    public void NextLevel_Start()
    {
        int curLevelIndex = SaveData.Data.levelIndex;
        if (curLevelIndex != GameConfig.maxLevelIndex)
        {
            SaveData.Data.levelIndex += 1;
        }
        if (SaveData.Data.levelIndex >= GameConfig.maxLevelIndex)
        {
            SaveData.Data.hadChooseLevel = true;
        }
        SaveData.Save();

        GenericTools.DelayFun(GameTool.LightTime, null, () =>
        {
            uimanager.ShowLevelText(curLevelIndex, LoadNextLevel);
        });
    }
    private void LoadNextLevel()
    {
        if (SaveData.Data.levelIndex < GameConfig.maxLevelIndex)
        {
            GenericMsg.Trigger(GenericSign.level_swear);
        }
        else
        {
            uimanager.panel_ingame.SetActive(false);
            GenericMsg<System.Action>.Trigger(GenericSign.uiInterfaceChange, () =>
            {
                uimanager.panel_start.SetActive(true);
                uimanager.Click_BackMenu();
            });
        }
    }
}
