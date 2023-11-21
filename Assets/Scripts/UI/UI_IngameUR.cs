using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngameUR : MonoBehaviour
{
    public Text text;
    public GameObject joyTip;
    private bool swearstate = true;
    private Button button;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        GenericMsg.AddReceiver
            (GenericSign.level_swear_end, OnGame)
            (GenericSign.level_swear, OnSwear);
        joyTip.SetActive(Input.GetJoystickNames().Length > 0);
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
        text.text = "跳过";
        gameObject.SetActive(SaveData.Data.jumpPledge);
    }
    private void OnGame()
    {
        gameObject.SetActive(true);
        text.text = "下一次"; 
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
        button.OnDeselect(null);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            OnClickSelf();
        }
    }
}
