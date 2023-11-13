using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SelectEventBtn : Button
{
    public Action onSelect;
    public override void OnSelect(BaseEventData eventData)
    {
        onSelect?.Invoke();
        base.OnSelect(eventData);
    }
}
