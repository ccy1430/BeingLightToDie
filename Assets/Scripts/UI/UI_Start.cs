using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    public GameObject chooselevel;
    private void OnEnable()
    {
        bool hadChooseLevel = SaveData.Data.hadChooseLevel;
        chooselevel.SetActive(hadChooseLevel);
        var btn = chooselevel.GetComponent<Button>();
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
    }
}
