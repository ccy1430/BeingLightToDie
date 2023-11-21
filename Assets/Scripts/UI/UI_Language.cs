using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Language : MonoBehaviour
{
    public Text cnText;
    public Text enText;
    public Text joytipText;

    private const string LanguageSet = "LanguageSet";

    private void OnEnable()
    {
        joytipText.gameObject.SetActive(Input.GetJoystickNames().Length > 0);
        string language = PlayerPrefs.GetString(LanguageSet, "");
        if (language == "")
        {

        }
        else if (language == "EN")
        {

        }
        else if (language == "CN")
        {

        }
    }
}
