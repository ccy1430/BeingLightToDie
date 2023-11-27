using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : UI_LanguageText
{
    public GameObject chooselevelBtn;
    protected override void OnEnable()
    {
        bool hadChooseLevel = SaveData.Data.hadChooseLevel;
        chooselevelBtn.SetActive(hadChooseLevel);
        var btn = chooselevelBtn.GetComponent<Button>();
        if (hadChooseLevel)
        {
            
            var upSeclect = btn.navigation.selectOnUp;
            var upNavigation = upSeclect.navigation;
            upNavigation.selectOnDown = btn;
            upSeclect.navigation = upNavigation;

            var downSeclect = btn.navigation.selectOnDown;
            var downNavigation = downSeclect.navigation;
            downNavigation.selectOnUp = btn;
            downSeclect.navigation = downNavigation;
        }
        else
        {
            var upSeclect = btn.navigation.selectOnUp;
            var downSeclect = btn.navigation.selectOnDown;

            var upNavigation = upSeclect.navigation;
            upNavigation.selectOnDown = downSeclect;
            upSeclect.navigation = upNavigation;

            var downNavigation = downSeclect.navigation;
            downNavigation.selectOnUp = upSeclect;
            downSeclect.navigation = downNavigation;
        }

        if(SaveData.Data.levelIndex == 0)
        {
            textCN = "开始游戏";
            textEN = "Start";
        }
        else
        {
            textCN = "继续游戏";
            textEN = "Contiune";
        }
        base .OnEnable();
    }
}
