using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_KeySetPanel : MonoBehaviour
{
    public Text[] keyNameText;
    public Text[] keyText;
    public GameObject changePanel;

    private int curChangeIndex = -1;

    private void OnEnable()
    {
        var inputKeySetData = SaveData<InputKeySet>.Data;
        for (int i = 0; i < inputKeySetData.keys.Count; i++)
        {
            keyText[i].text = inputKeySetData.keys[i].ToString();

            var eventtrigger = keyText[i].gameObject.AddComponent<EventTrigger>();
            int temp = i;
            var enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerClick;
            enter.callback.AddListener((data) => { OpenChangeInputPanel(temp); });
            eventtrigger.triggers.Add(enter);
        }
    }

    private void OnGUI()
    {
        if (curChangeIndex != -1)
        {
            Event e = Event.current;
            if (e != null && e.isKey && e.keyCode != KeyCode.None)
            {
                if (e.keyCode == KeyCode.Escape || e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Return)
                {
                    goto Exit;
                }
                var keys = SaveData<InputKeySet>.Data.keys;
                int oldkeyHolderIndex = keys.FindIndex(i => i == e.keyCode);
                if (oldkeyHolderIndex != -1)
                {
                    keys[oldkeyHolderIndex] = keys[curChangeIndex];
                    keyText[oldkeyHolderIndex].text = keys[oldkeyHolderIndex].ToString();
                }

                keys[curChangeIndex] = e.keyCode;
                SaveData<InputKeySet>.Save();
                keyText[curChangeIndex].text = e.keyCode.ToString();
            Exit:
                CloseChangeInputPanel();
            }
        }
    }

    public void OpenChangeInputPanel(int changeIndex)
    {
        curChangeIndex = changeIndex;
        changePanel.SetActive(true);
    }
    public void CloseChangeInputPanel()
    {
        curChangeIndex = -1;
        changePanel.SetActive(false);
    }
}
