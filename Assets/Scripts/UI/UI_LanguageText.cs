using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LanguageText : MonoBehaviour
{
    public string textEN;
    public string textCN;
    public Text text;
    private void Awake()
    {
        if(text == null)
        {
            text = GetComponent<Text>(); 
        }
    }
    protected virtual void OnEnable()
    {
        SetText();
        GenericMsg.AddReceiver(GenericSign.updateLanguage, SetText);
    }
    protected virtual void OnDisable()
    {
        GenericMsg.DelReceiver(GenericSign.updateLanguage, SetText);
    }
    public void SetText()
    {
        text.text = UI_Language.GameLanguage == "EN" ? textEN : textCN;
    }
}
