using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideText : MonoBehaviour
{
    public Text guidetext;
    private readonly string[] guides = new string[]
    {
#if UNITY_ANDROID&&!UNITY_EDITOR
        "»¬¶¯×ó±ßÆÁÄ»ÒÆ¶¯",
        "µã»÷ÓÒ±ßÆÁÄ»ÌøÔ¾",
        "³¤°´ÓÒ±ßÆÁÄ»ÌøµÄ¸ü¸ß",
        "³¤°´ÓÒ±ßÆÁÄ»¼á¶¨ÊÄÑÔ",
        "»¬¶¯×ó±ßÆÁÄ»¶ã±ÜÔÓÄî",
#else
        "·½Ïò¼üÒÆ¶¯",
        "¿Õ¸ñ¼üÌøÔ¾",
        "³¤°´ÌøÔ¾¼üÌøµÄ¸ü¸ß",
        "³¤°´ÌøÔ¾¼ü¼á¶¨ÊÄÑÔ",
        "ÒÆ¶¯¶ã±ÜÔÓÄî",
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
        if (Input.GetJoystickNames().Length > 0)
        {
            guides[1] = "ABXY¼üÌøÔ¾";
            guides[3] = "³¤°´ABXY¼ü¼á¶¨ÊÄÑÔ";
        }
        else
        {
            var keyset = SaveData<InputKeySet>.Data;
            if (keyset.Key_UP != KeyCode.UpArrow ||
                keyset.Key_DOWN != KeyCode.DownArrow ||
                keyset.Key_LEFT != KeyCode.LeftArrow ||
                keyset.Key_RIGHT != KeyCode.RightArrow)
            {
                guides[0] = $"{keyset.Key_UP} {keyset.Key_DOWN} {keyset.Key_LEFT} {keyset.Key_RIGHT} ÒÆ¶¯";
            }
            if (keyset.Key_JUMP != KeyCode.Space)
            {
                guides[1] = $"{keyset.Key_JUMP}¼üÌøÔ¾";
            }
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
