using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngameUR : UI_LanguageText
{
    public Text joyTip;
    public Button button;
    private bool swearstate = true;
    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.level_swear_end, OnGame)
            (GenericSign.level_swear, OnSwear);
        gameObject.SetActive(false);
    }
    protected override void OnEnable()
    {
        joyTip.text = Input.GetJoystickNames().Length > 0 ? "LB/RB" : "Enter";
        base.OnEnable();
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.level_swear_end, OnGame)
            (GenericSign.level_swear, OnSwear);
    }
    private void OnSwear()
    {
        gameObject.SetActive(SaveData.Data.jumpPledge);
        swearstate = true;
        textCN = "跳过";
        textEN = "Skip";
        SetText();
    }
    private void OnGame()
    {
        gameObject.SetActive(true);
        swearstate = false;
        textCN = "下一次";
        textEN = "Once more";
        SetText();
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
        button.OnDeselect(null);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Return))
        {
            OnClickSelf();
        }
    }
}
