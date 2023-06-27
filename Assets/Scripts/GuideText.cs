using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideText : MonoBehaviour
{
    public Text guidetext;
    private readonly string[] guides = new string[]
    {
#if UNITY_ANDROID
        "滑动左边屏幕移动",
        "点击右边屏幕跳跃",
        "长按右边屏幕增加跳跃高度",
        "长按右边屏幕进入游戏",
        "滑动左边屏幕躲避杂念",
#else
        "滑动左边屏幕移动",
        "点击右边屏幕跳跃",
        "长按右边屏幕增加跳跃高度",
        "长按右边屏幕进入游戏",
        "滑动左边屏幕躲避杂念",
#endif
    };
    void Start()
    {
        GenericMsg.AddReceiver
            (GenericSign.startLevel, IsShowGuideText)
            (GenericSign.backMenu, CloseGuide)
            (GenericSign.level_swear, ShowSwear);
    }

    private void IsShowGuideText()
    {
        int curlevel = SaveData.Data.levelIndex - 1;
        if (curlevel < 3)
        {
            guidetext.text = guides[curlevel];
        }
        else guidetext.text = null;
    }
    private void ShowSwear()
    {
        int curlevel = SaveData.Data.levelIndex - 1;
        if (curlevel < 2)
        {
            guidetext.text = guides[3 + curlevel];
        }
        else guidetext.text = null;
    }
    private void CloseGuide()
    {
        guidetext.text = null;
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.startLevel, IsShowGuideText)
            (GenericSign.backMenu, CloseGuide)
            (GenericSign.level_swear, ShowSwear);
    }
}
