using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EscButton : MonoBehaviour
{
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
    private Button button;
    private void Start()
    {
        if (!TryGetComponent<Button>(out button)) this.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.JoystickButton7)) 
        {
            button.onClick.Invoke();
        }
    }
#endif
}
