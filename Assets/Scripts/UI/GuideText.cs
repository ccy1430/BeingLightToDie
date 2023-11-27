using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideText : MonoBehaviour
{
    public Text guidetext;
    private string[] guides_CN = new string[]
    {
        "·½Ïò¼üÒÆ¶¯",
        "¿Õ¸ñ¼üÌøÔ¾",
        "³¤°´ÌøÔ¾¼üÌøµÄ¸ü¸ß",
        "³¤°´ÌøÔ¾¼ü¼á¶¨ÊÄÑÔ",
        "ÒÆ¶¯¶ã±ÜÔÓÄî",
    };
    private string[] guides_EN = new string[]
    {
        "Arrow key move",
        "Spacebar jump",
        "Long press the jump button to jump higher",
        "Long press the jump button to confirm the vow",
        "Moving to avoid distractions",
    };
    private string[] guides = new string[]
    {
#if UNITY_ANDROID&&!UNITY_EDITOR
        "»¬¶¯×ó±ßÆÁÄ»ÒÆ¶¯",
        "µã»÷ÓÒ±ßÆÁÄ»ÌøÔ¾",
        "³¤°´ÓÒ±ßÆÁÄ»ÌøµÄ¸ü¸ß",
        "³¤°´ÓÒ±ßÆÁÄ»¼á¶¨ÊÄÑÔ",
        "»¬¶¯×ó±ßÆÁÄ»¶ã±ÜÔÓÄî",
#else
        
#endif
    };
    void Start()
    {
        GenericMsg.AddReceiver
            (GenericSign.startLevel, IsShowGuideText)
            (GenericSign.backMenu, CloseGuide)
            (GenericSign.level_swear, ShowSwear);
    }

    private void CheckGuide()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (UI_Language.GameLanguage == "CN")
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                guides_CN[1] = "ABXY¼üÌøÔ¾";
                guides_CN[3] = "³¤°´ABXY¼ü¼á¶¨ÊÄÑÔ";
            }
            else
            {
                var keyset = SaveData<InputKeySet>.Data;
                if (keyset.Key_UP != KeyCode.UpArrow ||
                    keyset.Key_DOWN != KeyCode.DownArrow ||
                    keyset.Key_LEFT != KeyCode.LeftArrow ||
                    keyset.Key_RIGHT != KeyCode.RightArrow)
                {
                    guides_CN[0] = $"{keyset.Key_UP} {keyset.Key_DOWN} {keyset.Key_LEFT} {keyset.Key_RIGHT} ÒÆ¶¯";
                }
                if (keyset.Key_JUMP != KeyCode.Space)
                {
                    guides_CN[1] = $"{keyset.Key_JUMP}¼üÌøÔ¾";
                }
            }
            guides = guides_CN;
        }
        else
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                guides_EN[1] = "ABXY Jump";
                guides_EN[3] = "Long press ABXY to confirm the vow";
            }
            else
            {
                var keyset = SaveData<InputKeySet>.Data;
                if (keyset.Key_UP != KeyCode.UpArrow ||
                    keyset.Key_DOWN != KeyCode.DownArrow ||
                    keyset.Key_LEFT != KeyCode.LeftArrow ||
                    keyset.Key_RIGHT != KeyCode.RightArrow)
                {
                    guides_EN[0] = $"{keyset.Key_UP} {keyset.Key_DOWN} {keyset.Key_LEFT} {keyset.Key_RIGHT} move";
                }
                if (keyset.Key_JUMP != KeyCode.Space)
                {
                    guides_EN[1] = $"{keyset.Key_JUMP} jump";
                }
            }
            guides = guides_EN;
        }
#endif
    }

    private void IsShowGuideText()
    {
        CheckGuide();
        int curlevel = SaveData.Data.levelIndex - 1;
        if (curlevel < 3)
        {
            guidetext.text = guides[curlevel];
        }
        else guidetext.text = null;
    }
    private void ShowSwear()
    {
        CheckGuide();
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
