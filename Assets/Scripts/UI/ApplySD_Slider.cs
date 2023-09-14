using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ApplySD_Slider : MonoBehaviour
{
    public string attr;
    private Slider slider;
    private FieldInfo field;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        field = typeof(SaveDataPart).GetField(attr);
        slider.onValueChanged.AddListener(OnValueChange);
    }
    private void OnValueChange(float value)
    {
        if (slider.wholeNumbers)
        {
            field.SetValue(SaveData.Data, (int)value);
        }
        else
        {
            field.SetValue(SaveData.Data, value);
        }
        SaveData.Save();
    }
    private void OnEnable()
    {
        if (slider != null)
        {
            var data = field.GetValue(SaveData.Data);
            if (data is float) slider.SetValueWithoutNotify((float)data);
            else if (data is int) slider.SetValueWithoutNotify((int)data);
            else Debug.LogError(data + " " + attr);
        }
    }
}
