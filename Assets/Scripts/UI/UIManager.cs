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
    private readonly string[] words_CN =
    {
        "不！\n不……不…不该是这样的，为什么……\n不，不该是你!\n我…我能回到过去，我会改变这一切！\n我们的故事，一定是有一个好结局的。\n好，等着我。\n我发誓，我会拯救你。",
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

    private readonly string[] words_EN =
    {
        "NO！\nNo……no…it's wrong，why……\nNo，don't be you!\nI…I can go back in time and I'll change everything！\nOur story must have a good ending.\nWait for me. \nI swear, I will save you.",
        "Why, I always make so many mistakes…… I'm sorry… \nGive me one more chance…… \nI will definitely…Will save you.",
        "WHY！WHY？Why……\nI've avoided a million mistakes……Why……\nWhy……And give me such an end...\n……\n……\nOnce more！",
        "I……\nI've come so far……so many choices.\nYou……\nWill you forgive me?……",
        "Failed,loser.\nThe loser who failed. The loser failed。\nFailure, failure, failure, failure, failure.\nThe words are dividing……\nThe mother of success……",
        "It's fucking worth it。\nI'll throw away everything of me.",
        "To take one more step, not to go back in time, is to get out of the cycle……",
        "Just give you up……\n……\nBut what am I?",
        "I am a loser, a avoider, a incompetent.\nA sad person, a mantis arm that does not support itself, a stone in the river. \nA clown, a trapped animal in a cycle, a person who is ashamed of the oath. \nA person who deserve to die.\nA desperate person……",
        "I'm sorry.",
        "This, what, why?\n" +
            "<color=#FFC0CB>\"Don't be sad, you know it was my choice.\"</color>\n" +
            "No…no…\n" +
            "<color=#FFC0CB>\"I'm so lucky to have you with me, don't cry, I'm just in a different shape.\"</color>\n" +
            "But I'll never see you again……\n" +
            "<color=#FFC0CB>\"I'll be watching you and we'll meet again. Now please go past me and return to yourself.\"</color>\n" +
            "I……I can't. I can't.\n" +
            "<color=#FFC0CB>\"It's okay, you can do it. If you're too tired right now, it's okay to take a break.\"</color>\n" +
            "<color=#FFC0CB>\"But don't be lazy. Eventually, you have to face the future. Here, wipe your tears.\"</color>",
    };
    private string[] levelwords;

    protected virtual void OnEnable()
    {
        SetText();
        GenericMsg.AddReceiver(GenericSign.updateLanguage, SetText);
    }
    protected virtual void OnDestroy()
    {
        GenericMsg.DelReceiver(GenericSign.updateLanguage, SetText);
    }
    private void SetText()
    {
        levelwords = UI_Language.GameLanguage == "EN" ? words_EN : words_CN;
    }

    private void Start()
    {
        t_levelword.text = "";
        waitClick = false;
        panel_set.gameObject.SetActive(false);
        panel_choose.gameObject.SetActive(false);
        panel_ingame.gameObject.SetActive(false);

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
    public void Click_ResetInputKeySet()
    {
        SaveData<InputKeySet>.Reset();
    }
    public void Click_ResetFunny()
    {
        var data = SaveData.Data;

        data.remererLightRandColor = false;
        SaveData.Save();
    }
}