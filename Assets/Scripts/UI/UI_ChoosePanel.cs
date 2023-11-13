using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChoosePanel : MonoBehaviour
{
    public UIManager uiManager;
    public GameObject panel_ingame;
    public Button backBtn;
    public GameObject levelbtnPrefab;
    private readonly List<Transform> levelsBtns = new List<Transform>();
    private Coroutine selectSlider;
    private void OnEnable()
    {
        if (levelsBtns.Count <= 0) InitChoosePanel();
    }
    public void InitChoosePanel()
    {
        UI_SelectEventBtn lastBtn = null;
        for (int i = 0; i < GameConfig.maxLevelIndex; i++)
        {
            var go = Instantiate(levelbtnPrefab, transform);
            go.transform.localPosition = new Vector3(i * 240, i * -90);
            var tempbtn = go.GetComponent<UI_SelectEventBtn>();
            int tempi = i;
            tempbtn.onSelect += () =>
            {
                SelectSlider(tempi);
                var backNavigation = backBtn.navigation;
                backNavigation.selectOnDown = tempbtn;
                backBtn.navigation = backNavigation;
            };
            tempbtn.onClick.AddListener(() =>
            {
                StartGameWithLevelIndex(tempi);
            });
            var navigation = tempbtn.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnUp = backBtn;
            navigation.selectOnLeft = lastBtn;
            tempbtn.navigation = navigation;

            if(lastBtn != null)
            {
                var lastNavigation = lastBtn.navigation;
                lastNavigation.selectOnRight = tempbtn;
                lastBtn.navigation = lastNavigation;
            }
            lastBtn = tempbtn;

            tempbtn.GetComponentInChildren<Text>().text = (i + 1).ToString();
            levelsBtns.Add(go.transform);
        }
        //TODO Ä¬ÈÏ´æµµÎ»ÖÃ
        int defaultLevel = SaveData.Data.levelIndex;
        defaultLevel = Mathf.Clamp(defaultLevel, 0, levelsBtns.Count - 1);
        var backNavigation = backBtn.navigation;
        backNavigation.selectOnDown = levelsBtns[defaultLevel].GetComponent<Selectable>();
        backBtn.navigation = backNavigation;
        Vector3 firstPos = new Vector3(-240 * defaultLevel, 90 * defaultLevel);
        for (int j = 0; j < levelsBtns.Count; j++)
        {
            levelsBtns[j].localPosition = firstPos + new Vector3(j * 240, j * -90);
        }
    }

    private void SelectSlider(int tempi)
    {
        if(selectSlider!=null)StopCoroutine(selectSlider);
        Vector3 firstPos = new Vector3(-240 * tempi, 90 * tempi);
        Vector3 curPos = levelsBtns[0].transform.localPosition;
        selectSlider = StartCoroutine(GenericTools.DelayFun_Cor(0.5f, (float f) =>
        {
            Vector3 v3 = Vector3.Lerp(curPos, firstPos, f);
            for (int j = 0; j < levelsBtns.Count; j++)
            {
                levelsBtns[j].localPosition = v3 + new Vector3(j * 240, j * -90);
            }
        }, () => selectSlider = null));
    }

    private void StartGameWithLevelIndex(int levelIndex)
    {
        GenericMsg<System.Action>.Trigger(GenericSign.uiInterfaceChange, () =>
        {
            gameObject.SetActive(false);
            panel_ingame.SetActive(true);
            SaveData.Data.levelIndex = levelIndex + 1;
            SaveData.Save();
            if (levelIndex == 0)
            {
                uiManager.ShowLevelText(0, () => GenericMsg.Trigger(GenericSign.level_swear));
            }
            else GenericMsg.Trigger(GenericSign.level_swear);
        });
    }

    private readonly Vector2 chooseLevelDir = new Vector2(8, -3).normalized;
    private float dragPower;
    private Coroutine cacheCor;
    public void OnChoosePanelDragBegin(UnityEngine.EventSystems.BaseEventData data)
    {
        dragPower = 0;
        if (cacheCor != null)
        {
            StopCoroutine(cacheCor);
            cacheCor = null;
        }
        if(selectSlider!= null)
        {
            StopCoroutine(selectSlider); 
            selectSlider = null;
        }
        //Debug.Log("<color=red>drag begin</color>");
    }
    public void OnChoosePanelDrag(UnityEngine.EventSystems.BaseEventData data)
    {
        var pointerEventData = data as UnityEngine.EventSystems.PointerEventData;
        Vector2 delta = pointerEventData.delta;
        float dis = Vector2.Dot(delta, chooseLevelDir);
        dragPower = dis;
        DragingChoosePanel(dis);
        //Debug.Log("<color=green>drag</color>");
    }
    private void DragingChoosePanel(float dis)
    {
        if (dis < 0 && levelsBtns[^1].localPosition.x + dis * chooseLevelDir.x < 0)
        {
            dis = -levelsBtns[^1].localPosition.x / chooseLevelDir.x;
        }
        else if (dis > 0 && levelsBtns[0].localPosition.x + dis * chooseLevelDir.x > 0)
        {
            dis = -levelsBtns[0].localPosition.x / chooseLevelDir.x;
        }
        Vector3 step = chooseLevelDir * dis;
        foreach (var trs in levelsBtns)
        {
            trs.localPosition += step;
        }
    }
    public void OnChoosePanelDragEnd(UnityEngine.EventSystems.BaseEventData data)
    {
        cacheCor = StartCoroutine(DragInertia());
        //Debug.Log("<color=red>drag end</color>");
    }
    private IEnumerator DragInertia()
    {
        int sign = dragPower >= 0 ? 1 : -1;
        dragPower *= sign;
        if (dragPower != 0)
        {
            while (dragPower > 0)
            {
                //Debug.Log("<color=blue>inertia</color>");
                DragingChoosePanel(dragPower * sign);
                dragPower *= 0.98f;
                if (dragPower < 0.01f) dragPower = 0;
                yield return null;
            }
        }
        cacheCor = null;
    }
}
