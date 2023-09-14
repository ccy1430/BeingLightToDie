using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public TextMeshProUGUI tmp_levelword;
    public Text t_levelword;
    public WordShowEffect wordShowEffect;
    public CanvasGroup allUIAphla;

    private bool waitClick = false;
    private readonly int[] levelPointers = new int[]
    {
        0,11,22,33,48,57,83,100,109,121,122
    };
    private readonly string[] levelwords =
    {
        "不！\n不……不…不该是这样的，为什么……\n不，不该是你!\n我…我能回到过去，我会改变这一切！\n我们的故事，一定是有一个好结局的。\n好，等着我哦。\n我发誓，我会拯救你。",
        "为什么，我总是犯下这么多错误……对不起……\n再给我一些机会……\n我一定会……一定会拯救你。",
        "为什么！为什么？为什么……\n我已经规避了无数错误……为什么……\n还要给予我这样的结局……\n……\n……\n再来一次！",
        "我……\n我走过了这么多路……这么多颜色的路。\n你……\n你会理解我吗？……",
        "失败了，失败者。\n失败了的失败者。失败者失败了。\n失败了啊，失败了呀，失败了诶，失败者。\n想的字都分了……\n成功之母……",
        "当然**的值得。\n我会抛掉我的所有。",
        "再往前踏一步，不回到过去，就能离开这个循环……",
        "只要放弃你……\n……\n可我是谁？",
        "我是失败者，是逃避者，是无能之人。\n是悲伤的人，是不自力的螳臂，是水流中的石子。\n是小丑，是循环中的困兽，是背弃誓言的人。\n是该死的人。\n是绝望的人……",
        "对不起。",
        "这，什么，为什么?\n" +
            "<color=#FFC0CB>\"别伤心，你知道这是我的选择。\"</color>\n" +
            "不…不…\n" +
            "<color=#FFC0CB>\"有你在一起，我已经很幸运了，别哭，我只是换了一种形态而已。\"</color>\n" +
            "可我再也见不到你了……\n" +
            "<color=#FFC0CB>\"我会一直看着你的，我们会再次相见的。现在请越过我，回到你自己吧。\"</color>\n" +
            "我……我不能，我做不到。\n" +
            "<color=#FFC0CB>\"没关系，你可以的，如果现在太累了，暂且休息一下也无妨。\"</color>\n" +
            "<color=#FFC0CB>\"但是别偷懒哦。终究你是要面对未来的。来，擦擦眼泪。\"</color>",
    };

    private void Start()
    {
        t_levelword.text = "";
        waitClick = false;
        panel_set.gameObject.SetActive(false);
        panel_choose.gameObject.SetActive(false);
        panel_ingame.gameObject.SetActive(false);
        //todo 返回主菜单时也有这个
        startGameBtnText.text = SaveData.Data.levelIndex == 0 ? "开始游戏" : "继续游戏";

        GenericTools.DelayFun(2f, f => allUIAphla.alpha = f, null);
    }

    public void ShowLevelText(int level, System.Action callBack)
    {
        int indexOfLevel = System.Array.IndexOf(levelPointers, level);
        if (indexOfLevel == -1) callBack?.Invoke();
        else
        {
            panel_ingame.SetActive(false);
            t_levelword.text = levelwords[indexOfLevel];
            StartCoroutine(SkipTextClick());
            wordShowEffect.AddOnceCompleteListener(() => StartCoroutine(ShowTextEnd(callBack)));
        }
    }
    private IEnumerator SkipTextClick()
    {
        while (true)
        {
            if (InputSingleton.Instance.Click) break;
            yield return null;
        }

        wordShowEffect.Stop();
    }
    private IEnumerator ShowTextEnd(System.Action callBack)
    {
        StopCoroutine(SkipTextClick());
        yield return null;
        while (true)
        {
            if (InputSingleton.Instance.Click) break;
            yield return null;
        }
        t_levelword.text = "";
        panel_ingame.SetActive(true);
        callBack?.Invoke();
    }

    [Header("Panel")]
    public GameObject panel_start;
    public GameObject panel_set;
    public GameObject panel_choose;
    public GameObject panel_ingame;
    public Text startGameBtnText;
    public void Click_StartGame()
    {
        if (SaveData.Data.levelIndex == 0)
        {
            SaveData.Data.levelIndex = 1;
            SaveData.Save();
            ShowLevelText(0, () => GenericMsg.Trigger(GenericSign.level_swear));
            return;
        }
        GenericMsg.Trigger(GenericSign.level_swear);
    }
    private void StartGameWithLevelIndex(int levelIndex)
    {
        GenericMsg<System.Action>.Trigger(GenericSign.uiInterfaceChange, () =>
        {
            panel_choose.SetActive(false);
            panel_ingame.SetActive(true);
            SaveData.Data.levelIndex = levelIndex + 1;
            SaveData.Save();
            if (levelIndex == 0)
            {
                ShowLevelText(0, () => GenericMsg.Trigger(GenericSign.level_swear));
            }
            else GenericMsg.Trigger(GenericSign.level_swear);
        });
    }
    public void Click_BackMenu()
    {
        GenericMsg.Trigger(GenericSign.backMenu);
    }
    public void Click_ResetLevel()
    {
        GameManager.Instance.player.DieOnce();
    }
    public void Click_Exit()
    {
        Application.Quit();
    }
    public void Click_ResetHelp()
    {
        var data = SaveData.Data;

        data.playerLightSize = 1;
        data.rememerCount = 16;
        data.hadHurt = true;
        data.pledgeSpeed = 1;
        data.jumpPledge = false;
        data.remererLightSize = 1;
        SaveData.Save();
    }
    public void Click_ResetFunny()
    {
        var data = SaveData.Data;

        data.remererLightRandColor = false;
        SaveData.Save();
    }

    #region choose panel
    [Header("Choose Panel")]
    public GameObject levelbtnPrefab;
    private readonly List<Transform> levelsBtns = new List<Transform>();
    public void InitChoosePanel()
    {
        for (int i = levelsBtns.Count; i < GameConfig.maxLevelIndex; i++)
        {
            var go = Instantiate(levelbtnPrefab, panel_choose.transform);
            go.transform.localPosition = new Vector3(i * 240, i * -90);
            var tempbtn = go.GetComponent<Button>();
            int tempi = i;
            tempbtn.onClick.AddListener(() =>
            {
                StartGameWithLevelIndex(tempi);
            });
            tempbtn.GetComponentInChildren<Text>().text = (i + 1).ToString();
            levelsBtns.Add(go.transform);
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