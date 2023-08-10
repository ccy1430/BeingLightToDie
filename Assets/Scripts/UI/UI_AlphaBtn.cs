using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class UI_AlphaBtn : MonoBehaviour
{
    public GameObject curGo;
    public GameObject nextGo;
    public UnityEvent unityEvent;

    private Button selfBtn;
    private void Awake()
    {
        selfBtn = GetComponent<Button>();
        selfBtn.onClick.AddListener(OnBtnClicked);
    }
    private void OnBtnClicked()
    {
        GenericMsg<System.Action>.Trigger(GenericSign.uiInterfaceChange, Fun);
    }
    private void Fun()
    {
        curGo.SetActive(false);
        nextGo.SetActive(true);
        if (unityEvent != null) unityEvent?.Invoke();
    }
}
