using UnityEngine;
using UnityEngine.UI;

public class ApplySD_Slider : MonoBehaviour
{
    public string attr;
    private Slider slider;
    private void OnEnable()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (slider != null)
        {
            var property = typeof(SaveDataPart).GetField(attr);
            var data = property.GetValue(SaveData.Data);
            if (data is float) slider.SetValueWithoutNotify((float)data);
            else if (data is int) slider.SetValueWithoutNotify((int)data);
            else Debug.LogError(data + " " + attr);
        }
    }
}
