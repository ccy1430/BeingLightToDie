using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IntoLevel : MonoBehaviour
{
    public Text text;
    private Color originColor;
    private void Awake()
    {
        GenericMsg.AddReceiver(GenericSign.level_swear_end, IntoLevel);
        originColor = text.color;
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver(GenericSign.level_swear_end, IntoLevel);
    }
    public void IntoLevel()
    {
        StartCoroutine(IntoLevel_IE());
    }
    private IEnumerator IntoLevel_IE()
    {
        //var waitForSce0_1 = new WaitForSeconds(0.1f);
        float timer = 0;
        int[] needCounts = new int[7];
        int[] endCounts = new int[7] { 4, 3, 0, 2, 2, 0, 3 };
        float stepTimer = 0.02f;
        System.Func<string> CreatText;
        if (UI_Language.GameLanguage == "CN")
        {
            CreatText = () =>
            {
                return $"{needCounts[0]}ÔÂ{needCounts[1]}{needCounts[2]}ÈÕ  {needCounts[3]}{needCounts[4]}£º{needCounts[5]}{needCounts[6]}  ´óÓê";
            };
        }
        else
        {
            CreatText = () =>
            {
                return $"{needCounts[0]}/{needCounts[1]}{needCounts[2]}  {needCounts[3]}{needCounts[4]}£º{needCounts[5]}{needCounts[6]}  RAIN";
            };
        }
        while (timer < 0.7f)
        {
            for (int i = 0; i < 7; i++)
            {
                if (timer > 0.5f)
                {
                    needCounts[i] = endCounts[i];
                }
                else
                {
                    transform.localScale = Vector3.one * (2f - timer * 2);
                    int noRepeatRand = Random.Range(0, 10);
                    while (needCounts[i] == noRepeatRand)
                    {
                        noRepeatRand = Random.Range(0, 10);
                    }
                    needCounts[i] = noRepeatRand;
                }
            }
            text.text = CreatText();
            yield return new WaitForSeconds(stepTimer);
            timer += stepTimer;
        }
        yield return GenericTools.DelayFun_Cor(1f, f => text.color = Color.Lerp(originColor, new Color(1, 1, 1, 0), f), null);
        text.text = null;
        text.color = originColor;
    }
}
