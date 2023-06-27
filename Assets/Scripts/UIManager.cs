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
        "不！\n不……不…不该是这样的，为什么……\n所以…我…我会回到过去，让它改变。",
        "明明我已经……\n为什么不一样了……\n再、再来一次,我可以的。",
        "哪里出错了，对不起，我……",
        "为什么……为什么……为什么……为什么……",
        "对不起，再给我一次机会，我一定会救你。",
        "为什么……这种事情，会发生在你身上…\n为什么，每一次都有阻拦，每一次都有变化。",
        "我……又让你再一次受苦了吗，我……不……",
        "我不能，不能，接受这种事情的发生。我不能。",
        "我会救你的，我发誓。",
        "混蛋的世界！",

        "不，为什么……\n这里是地狱吗。",
        "我不能软弱，我不能认输。再一次。",
        "再一次，再一次。",
        "再来。",
        "再来…再来一次。",
        "我…再一次。",
        "再一次……",
        "再一次……",
        "这是第，第……第十八次。",
        "再来，再快一点，再少些错误。",
        "头发就散着吧，省些时间。",

        "二十一，三乘七。",
        "二十二，十一乘二。",
        "二十三，素数。",
        "二十四，四乘六，二乘二乘二乘三。",
        "二十五，五的平方，完全平方数。",
        "二十六，十三乘二。",
        "二十七，三的三次方。",
        "二十八，十四乘二，二乘二乘七。",
        "二十九，素数。",
        "三十，二乘三乘五",

        "三十一，素数，孪生素数猜想……\n我，我在麻木吗。",
        "我还有对你道歉的资格吗。",
        "我凭着自己的执念使你一次又一次的经历，哪怕，哪怕你一次再一次做出温柔的选择。",
        "我……我在怀疑自己。\n不，我不想，也不该……",
        "一己之力，一厢情愿，私念，执念，就算如此，就算如此。\n我还是要救你。",
        "我发过誓，对自己。所以，所以我不会放弃。",
        "三十七，素数。",
        "我记得你对我打招呼的样子。",
        "我记得你运动会上跑步的样子。",
        "我记得你为了一只受伤的小猫付出的愿望，不该是这样。\n该死！凭什么！",

        "这个城市，这个雨夜，都是它们的错。",
        "既然不是你的错误，为什么要让你承担如此结果。",
        "我不接受，我要战斗，我要反驳。",
        "是这个世界的错。",
        "我发过誓，我一定会救你。\n无论失败多少次，我一定会再重来。",
        "要警惕环境的变化。",
        "不要滑倒。",
        "注意距离。",
        "躲开那个陷阱。",
        "多用脑子，去想，去思考。",

        "去行动，去…不，要快，也要谨慎。",
        "我能行，我必须行。",
        "我，我要铭记每一次苦痛。",
        "记住每一次失败。",
        "不要逃避，不要麻木。",
        "这一次做的要更好。",
        "去用力照亮这段路。",
        "让它们成为我的动力…我在以另一种方式无视吗。",
        "正视痛苦……",
        "我能做到吗，不要以其他任何方式偏视你承受的苦痛。",

        "我……可能做不到。",
        "太痛苦……",
        "原谅我……",
        "我在害怕。我做不到不害怕。",
        "但我，但我不会放弃。",
        "我的誓言，我不会放弃。",
        "六十七。",
        "哪怕我做不到正视，我要偏过视线。",
        "我会不断的嘲笑自己。",
        "没关系，我自作自得。",

        "七十一。",
        "七十二，我还要数这次数吗。",
        "不用了吧，呵。",
        "我只要走出这一步。",
        "再一步。",
        "只要不放弃。",
        "看不见的尽头。",
        "又如何。",
        "我要放弃吗，我会问多少遍这个问题，我会回答多少次不。",
        "怎么可能。", 
        "再走一步。", 

        "只要我不放弃，我就要一直走下去。",
        "而一直走下去，我一定能看到希望。",
        "找到正确的路之前。",
        "我的每一步都行走在错误之上。",
        "但只要，只要我排除了所有的错误。",
        "我就能抵达正确的路径。",
        "只要我走下去。",
        "一步又一步。",
        "它一定有尽头。",
        "若它是有限的，我便能走过所有错误，抵达正确。",
        "若它是无限的，我也有无限的时间去验错，去扔骰子，直到命运之神……。",

        "混蛋的神，哪里有神。", 
        "若有神，如此熟视无睹的神明，也该死。",
        "这一切，为了你，为了你。",
        "不，不，不要感动自己，这是我自己的选择。",
        "如果我不满意，我大可以往前走，不回退时间。",
        "……。",
        "我会救你，我发誓。",
        "我只要再努力一次。",
        "再一次。",
        "这是第几次？无所谓，专注这一次。",

        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",
        "我会救你。",

        "我会救你。",
        "我会救你。",
        "……",
        "再往前踏一步，离开这个循环。",
        "………………",
        "只要一步。",
        "一步……",
        "……\n……\n……\n……\n……\n……\n……\n……\n……\n……",
        "不。",
        "我会救你。",

        "我会救你。",
        "这，什么，为什么。\n‘别伤心，你知道这是我的选择。’\n不……\n‘有你在一起，我已经很幸运了，别哭，你还是笑起来比较好看。’\n好……",
    };
    private void Start()
    {
        //tmp_levelword.text = "";
        t_levelword.text = "";
        waitClick = false;
        panel_set.gameObject.SetActive(false);
        panel_choose.gameObject.SetActive(false);
        panel_ingame.gameObject.SetActive(false);
        //todo 返回主菜单时也有这个
        startGameBtnText.text = SaveData.Data.levelIndex == 0 ? "开始游戏" : "继续游戏";
    }
    public void ShowLevelText(int level)
    {
        if (level == levelwords.Length)
        {
            t_levelword.text = "到此为止";
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