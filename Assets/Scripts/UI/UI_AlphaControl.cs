using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AlphaControl : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject eventSystem;
    public float tranTimer = 0.5f;
    private void Awake()
    {
        GenericMsg<System.Action>.AddReceiver(GenericSign.uiInterfaceChange, FadeAndShow);
    }
    private void OnDestroy()
    {
        GenericMsg<System.Action>.DelReceiver(GenericSign.uiInterfaceChange, FadeAndShow);
    }
    private void FadeAndShow(System.Action midfun)
    {
        StartCoroutine(FadeAndShow_IE(midfun));
    }
    private IEnumerator FadeAndShow_IE(System.Action midfun)
    {
        eventSystem.SetActive(false);
        yield return StartCoroutine(GenericTools.DelayFun_Cor(tranTimer, f => canvasGroup.alpha = 1 - f, null));
        midfun?.Invoke();
        yield return StartCoroutine(GenericTools.DelayFun_Cor(tranTimer, f => canvasGroup.alpha = f, null));
        eventSystem.SetActive(true);
    }
}
