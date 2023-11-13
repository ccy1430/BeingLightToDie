using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DefaultSelect : MonoBehaviour
{
    public GameObject defaultSelectHander;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(defaultSelectHander);
    }
    private void Update()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) &&
            EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectHander);
        }
    }
}
