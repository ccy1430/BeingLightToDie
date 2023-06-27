using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public TextMeshProUGUI tmp_levelword;
    public Text t_levelword;
    private bool waitClick = false;
    private readonly string[] levelwords =
    {
        "����\n���������������������ģ�Ϊʲô����\n���ԡ��ҡ��һ�ص���ȥ�������ı䡣",
        "�������Ѿ�����\nΪʲô��һ���ˡ���\n�١�����һ��,�ҿ��Եġ�",
        "��������ˣ��Բ����ҡ���",
        "Ϊʲô����Ϊʲô����Ϊʲô����Ϊʲô����",
        "�Բ����ٸ���һ�λ��ᣬ��һ������㡣",
        "Ϊʲô�����������飬�ᷢ���������ϡ�\nΪʲô��ÿһ�ζ���������ÿһ�ζ��б仯��",
        "�ҡ�����������һ���ܿ������ҡ���������",
        "�Ҳ��ܣ����ܣ�������������ķ������Ҳ��ܡ�",
        "�һ����ģ��ҷ��ġ�",
        "�쵰�����磡",

        "����Ϊʲô����\n�����ǵ�����",
        "�Ҳ����������Ҳ������䡣��һ�Ρ�",
        "��һ�Σ���һ�Ρ�",
        "������",
        "����������һ�Ρ�",
        "�ҡ���һ�Ρ�",
        "��һ�Ρ���",
        "��һ�Ρ���",
        "���ǵڣ��ڡ�����ʮ�˴Ρ�",
        "�������ٿ�һ�㣬����Щ����",
        "ͷ����ɢ�Űɣ�ʡЩʱ�䡣",

        "��ʮһ�������ߡ�",
        "��ʮ����ʮһ�˶���",
        "��ʮ����������",
        "��ʮ�ģ��ĳ��������˶��˶�������",
        "��ʮ�壬���ƽ������ȫƽ������",
        "��ʮ����ʮ���˶���",
        "��ʮ�ߣ��������η���",
        "��ʮ�ˣ�ʮ�ĳ˶������˶����ߡ�",
        "��ʮ�ţ�������",
        "��ʮ������������",

        "��ʮһ�������������������롭��\n�ң�������ľ��",
        "�һ��ж����Ǹ���ʸ���",
        "��ƾ���Լ���ִ��ʹ��һ����һ�εľ��������£�������һ����һ�����������ѡ��",
        "�ҡ������ڻ����Լ���\n�����Ҳ��룬Ҳ���á���",
        "һ��֮����һ����Ը��˽�ִ�������ˣ�������ˡ�\n�һ���Ҫ���㡣",
        "�ҷ����ģ����Լ������ԣ������Ҳ��������",
        "��ʮ�ߣ�������",
        "�Ҽǵ�����Ҵ��к������ӡ�",
        "�Ҽǵ����˶������ܲ������ӡ�",
        "�Ҽǵ���Ϊ��һֻ���˵�Сè������Ը����������������\n������ƾʲô��",

        "������У������ҹ���������ǵĴ�",
        "��Ȼ������Ĵ���ΪʲôҪ����е���˽����",
        "�Ҳ����ܣ���Ҫս������Ҫ������",
        "���������Ĵ�",
        "�ҷ����ģ���һ������㡣\n����ʧ�ܶ��ٴΣ���һ������������",
        "Ҫ���軷���ı仯��",
        "��Ҫ������",
        "ע����롣",
        "�㿪�Ǹ����塣",
        "�������ӣ�ȥ�룬ȥ˼����",

        "ȥ�ж���ȥ������Ҫ�죬ҲҪ������",
        "�����У��ұ����С�",
        "�ң���Ҫ����ÿһ�ο�ʹ��",
        "��סÿһ��ʧ�ܡ�",
        "��Ҫ�ӱܣ���Ҫ��ľ��",
        "��һ������Ҫ���á�",
        "ȥ�����������·��",
        "�����ǳ�Ϊ�ҵĶ�������������һ�ַ�ʽ������",
        "����ʹ�࡭��",
        "���������𣬲�Ҫ�������κη�ʽƫ������ܵĿ�ʹ��",

        "�ҡ���������������",
        "̫ʹ�࡭��",
        "ԭ���ҡ���",
        "���ں��¡��������������¡�",
        "���ң����Ҳ��������",
        "�ҵ����ԣ��Ҳ��������",
        "��ʮ�ߡ�",
        "���������������ӣ���Ҫƫ�����ߡ�",
        "�һ᲻�ϵĳ�Ц�Լ���",
        "û��ϵ���������Եá�",

        "��ʮһ��",
        "��ʮ�����һ�Ҫ���������",
        "�����˰ɣ��ǡ�",
        "��ֻҪ�߳���һ����",
        "��һ����",
        "ֻҪ��������",
        "�������ľ�ͷ��",
        "����Ρ�",
        "��Ҫ�������һ��ʶ��ٱ�������⣬�һ�ش���ٴβ���",
        "��ô���ܡ�", 
        "����һ����", 

        "ֻҪ�Ҳ��������Ҿ�Ҫһֱ����ȥ��",
        "��һֱ����ȥ����һ���ܿ���ϣ����",
        "�ҵ���ȷ��·֮ǰ��",
        "�ҵ�ÿһ���������ڴ���֮�ϡ�",
        "��ֻҪ��ֻҪ���ų������еĴ���",
        "�Ҿ��ִܵ���ȷ��·����",
        "ֻҪ������ȥ��",
        "һ����һ����",
        "��һ���о�ͷ��",
        "���������޵ģ��ұ����߹����д��󣬵ִ���ȷ��",
        "���������޵ģ���Ҳ�����޵�ʱ��ȥ���ȥ�����ӣ�ֱ������֮�񡭡���",

        "�쵰������������", 
        "��������������޶õ�������Ҳ������",
        "��һ�У�Ϊ���㣬Ϊ���㡣",
        "����������Ҫ�ж��Լ����������Լ���ѡ��",
        "����Ҳ����⣬�Ҵ������ǰ�ߣ�������ʱ�䡣",
        "������",
        "�һ���㣬�ҷ��ġ�",
        "��ֻҪ��Ŭ��һ�Ρ�",
        "��һ�Ρ�",
        "���ǵڼ��Σ�����ν��רע��һ�Ρ�",

        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",
        "�һ���㡣",

        "�һ���㡣",
        "�һ���㡣",
        "����",
        "����ǰ̤һ�����뿪���ѭ����",
        "������������",
        "ֻҪһ����",
        "һ������",
        "����\n����\n����\n����\n����\n����\n����\n����\n����\n����",
        "����",
        "�һ���㡣",

        "�һ���㡣",
        "�⣬ʲô��Ϊʲô��\n�������ģ���֪�������ҵ�ѡ�񡣡�\n������\n��������һ�����Ѿ��������ˣ���ޣ��㻹��Ц�����ȽϺÿ�����\n�á���",
    };
    private void Start()
    {
        //tmp_levelword.text = "";
        t_levelword.text = "";
        waitClick = false;
        panel_set.gameObject.SetActive(false);
        panel_choose.gameObject.SetActive(false);
        panel_ingame.gameObject.SetActive(false);
        //todo �������˵�ʱҲ�����
        startGameBtnText.text = SaveData.Data.levelIndex == 0 ? "��ʼ��Ϸ" : "������Ϸ";
    }
    public void ShowLevelText(int level)
    {
        if (level == levelwords.Length)
        {
            t_levelword.text = "����Ϊֹ";
        }
        else
        {
            //tmp_levelword.text = levelwords[level - 1];
            t_levelword.text = levelwords[level - 1];
        }
        Invoke(nameof(CanClick), 1f);
    }
    private void CanClick()
    {
        waitClick = true;
    }
    //private void Update()
    //{
    //    if (waitClick)
    //    {
    //        if (Input.anyKeyDown)
    //        {
    //            waitClick = false;
    //            //tmp_levelword.text = "";
    //            t_levelword.text = "";
    //            GameManager.Instance.NextLevel_Start();
    //        }
    //    }
    //}

    public void LogClick()
    {
        Debug.Log("click");
    }
    [Header("Panel")]
    public GameObject panel_start;
    public GameObject panel_set;
    public GameObject panel_choose;
    public GameObject panel_ingame;
    public Text startGameBtnText;
    [Space]
    [Header("Set Panel")]
    public Slider bgmVolume;
    public Slider otherVolume;
    public void Click_StartGame()
    {
        StartGameWithLevelIndex(SaveData.Data.levelIndex);
    }
    private void StartGameWithLevelIndex(int levelIndex)
    {
        GenericMsg.Trigger(GenericSign.level_swear);
        panel_start.SetActive(false);
        panel_ingame.SetActive(true);
    }
    public void Click_ChooseLevel()
    {
        panel_start.SetActive(false);
        panel_choose.SetActive(true);
        OpenChoosePanel();
    }
    public void Click_Set()
    {
        panel_start.SetActive(false);
        panel_set.SetActive(true);
        bgmVolume.SetValueWithoutNotify(SaveData.Data.bgmVolume);
        otherVolume.SetValueWithoutNotify(SaveData.Data.audVolume);
    }
    public void Click_BackMenu()
    {
        GenericMsg.Trigger(GenericSign.backMenu);
        panel_start.SetActive(true);
        panel_ingame.SetActive(false);
    }
    public void Click_Exit()
    {
        Application.Quit();
    }

    #region choose panel
    [Header("Choose Panel")]
    public GameObject levelbtnPrefab;
    private readonly List<Transform> levelsBtns = new List<Transform>();
    public void OpenChoosePanel()
    {
        if (levelsBtns.Count != SaveData.Data.levelIndex)
        {
            for(int i = levelsBtns.Count; i < SaveData.Data.levelIndex; i++)
            {
                var go = Instantiate(levelbtnPrefab, panel_choose.transform);
                go.transform.localPosition = new Vector3(i * 240, i * -90);
                var tempbtn = go.GetComponent<Button>();
                int tempi = i + 1;
                tempbtn.onClick.AddListener(() => {
                    panel_choose.SetActive(false);
                    StartGameWithLevelIndex(tempi);
                });
                tempbtn.GetComponentInChildren<Text>().text = tempi.ToString();
                levelsBtns.Add(go.transform);
            }
        }
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
    #endregion
}