using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ApplySD_Toggle : MonoBehaviour
{
    public string attr;
    private Toggle toggle;
    private FieldInfo field;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        field = typeof(SaveDataPart).GetField(attr);
        toggle.onValueChanged.AddListener(OnValueChange);
    }
    private void OnValueChange(bool value)
    {
        field.SetValue(SaveData.Data, value);
        SaveData.Save();
    }
    private void OnEnable()
    {
        if (toggle != null)
        {
            toggle.SetIsOnWithoutNotify((bool)field.GetValue(SaveData.Data));
        }
    }
}
