using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private void Awake()
    {
        GenericMsg.AddReceiver
            (GenericSign.level_swear_end, Show)
            (GenericSign.backMenu, Hide)
            (GenericSign.level_swear, Hide);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver
            (GenericSign.level_swear_end, Show)
            (GenericSign.backMenu, Hide)
            (GenericSign.level_swear, Hide);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
