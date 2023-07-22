using UnityEngine;
using UnityEngine.UI;

public class ApplySD_Toggle : MonoBehaviour
{
    public string attr;
    private Toggle toggle;
    private void OnEnable()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            var property = typeof(SaveDataPart).GetField(attr);
            toggle.SetIsOnWithoutNotify((bool)property.GetValue(SaveData.Data));
        }
    }
}
