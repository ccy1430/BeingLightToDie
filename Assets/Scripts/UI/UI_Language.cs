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
    public static string GameLanguage;

    private void OnEnable()
    {
        joytipText.gameObject.SetActive(Input.GetJoystickNames().Length > 0);
        string language = PlayerPrefs.GetString(LanguageSet, "");
        if (language == "")
        {
            if (Application.systemLanguage == SystemLanguage.Chinese) language = "CN";
            else language = "EN";
        }
        SetLangeuage(language);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            SwitchLanguage();
        }
    }
    private void SwitchLanguage()
    {
        if (GameLanguage == "EN") SetLangeuage("CN");
        else SetLangeuage("EN");
    }
    public void SetLangeuage(string language)
    {
        if(GameLanguage==language) { return; }
        GameLanguage = language;
        PlayerPrefs.SetString(LanguageSet, GameLanguage);
        GenericMsg.Trigger(GenericSign.updateLanguage);
        cnText.color = language == "CN" ? Color.white : Color.gray;
        enText.color = language != "CN" ? Color.white : Color.gray;
    }
}
