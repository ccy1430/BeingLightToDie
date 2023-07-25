using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngameUR : MonoBehaviour
{
    private Text text;
    private bool swearstate = true;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        GenericMsg.AddReceiver
            (GenericSign.level_swear_end, OnGame)
            (GenericSign.level_swear, OnSwear);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.level_swear_end, OnGame)
            (GenericSign.level_swear, OnSwear);
    }
    private void OnSwear()
    {
        swearstate = true;
        text.text = "Ìø¹ý";
        gameObject.SetActive(SaveData.Data.jumpPledge);
    }
    private void OnGame()
    {
        gameObject.SetActive(true);
        text.text = "ÖØÀ´";
        swearstate = false;
    }
    public void OnClickSelf()
    {
        if (swearstate)
        {
            GenericMsg.Trigger(GenericSign.level_swear_end);
        }
        else
        {
            GameManager.Instance.player.DieOnce();
        }
    }
}
