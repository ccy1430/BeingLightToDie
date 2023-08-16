using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swear_Pledge : MonoBehaviour
{
    private const float rectW = GameConfig.mapWidth / 2f - 1;
    private const float rectH = GameConfig.mapHeight / 2f - 1;
    private const float baseSpeed = 10;

    public Transform body;
    public Transform maskTrs;
    public GameObject hurtPrefab;

    private void Awake()
    {
        GenericMsg.AddReceiver(GenericSign.level_swear_end, OutPledge);
    }
    private void OutPledge()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GenericMsg.DelReceiver(GenericSign.level_swear_end, OutPledge);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        transform.position = new Vector3(-0.35f, 0, 0);
        baseNum = SaveData.Data.levelIndex / 10f + 1;
        moveSpeed = originMoveSpeed = (baseSpeed - SaveData.Data.levelIndex / 15f) * SaveData.Data.pledgeSpeed;
        progressSpeed = originProgressSpeed = (baseSpeed * 10 / SaveData.Data.levelIndex + 2) * SaveData.Data.pledgeSpeed;
        progress = 0;
        MaskSize();
        selfLightSize = 0;
        StartCoroutine(GenericTools.DelayFun_Cor(0.5f, SelfLight, null));
    }

    public float moveSpeed = 10;
    private float originMoveSpeed;
    /// <summary>
    /// 0-100
    /// </summary>
    private float progress = 0;
    private float progressSpeed = 10;
    private float originProgressSpeed;
    private float baseNum;
    private void FixedUpdate()
    {
        Vector3 dir = new Vector2();
        dir.x = InputSingleton.Instance.LR;
        dir.y = InputSingleton.Instance.UD;
        var pos = transform.position + Time.fixedDeltaTime * moveSpeed * dir.normalized;
        pos.x = Mathf.Clamp(pos.x, -rectW, rectW);
        pos.y = Mathf.Clamp(pos.y, -rectH, rectH);
        transform.position = pos;

        if (InputSingleton.Instance.Jump)
        {
            progress += progressSpeed * Time.fixedDeltaTime;
            MaskSize();
        }
        if (progress >= 100)
        {
            GenericMsg.Trigger(GenericSign.level_swear_end);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            progress = 100;
        }
    }
#endif

    private Coroutine cor1, cor2, cor3;
    private List<GameObject> hurts = new List<GameObject>();

    public void CollEmo(Level_Swear_Emotion emo)
    {
        Debug.Log("coll emo :" + emo.selfIndex);

        float originProgress = progress;
        progress -= 5;
        switch (emo.selfIndex)
        {
            case 0:
                if (cor1 != null) GenericTools.StopCoroutine(cor1);
                var dir = transform.position - emo.transform.position;
                cor1 = GenericTools.DelayFun(1, (float f) =>
                {
                    var num = baseNum * (1 - f);
                    transform.position += Time.deltaTime * dir;
                }, () => cor1 = null);
                break;
            case 1:
                if (cor2 != null) GenericTools.StopCoroutine(cor2);
                moveSpeed = originMoveSpeed / 2;
                cor2 = GenericTools.DelayFun(1, (float f) =>
                {
                    moveSpeed = originMoveSpeed / 2 * (1 + f);
                }, () => cor2 = null);
                break;
            case 2:
                if (cor3 != null) GenericTools.StopCoroutine(cor3);
                progressSpeed = originProgressSpeed / 2;
                cor3 = GenericTools.DelayFun(1, (float f) =>
                {
                    progressSpeed = originProgressSpeed / 2 * (1 + f);
                }, () => cor3 = null);
                break;
            case 3:
                progress -= 1 + baseNum / 2;
                break;
        }
        if (progress < 0) progress = 0;

        SplitEmoAndSelf(emo, originProgress - progress);

        MaskSize();

        emo.BackPool();
    }
    private void SplitEmoAndSelf(Level_Swear_Emotion emo, float subSize)
    {
        var emoSize = new Vector2(emo.sprr.sprite.texture.width, emo.sprr.sprite.texture.height) / 100;
        var rect = new Rect((Vector2)emo.transform.position - emoSize / 2, emoSize);
        var go = SpriteExploder.GenerateTriangularPieces(emo.sprr, rect, Color.red);

        subSize = subSize * (maskMaxSize - 0.7f) / 100;
        var bodysprr = body.GetComponent<SpriteRenderer>();
        Vector2 size = new Vector2(subSize, emo.sprr.sprite.texture.height / 100f);
        Vector2 center = transform.position + Vector3.right * (maskTrs.localScale.x / 2 - subSize / 2);
        var go2 = SpriteExploder.GenerateTriangularPieces(bodysprr, new Rect(center - size / 2, size), Color.white);

        var centerPos = emo.transform.position;
        float[] pieceDis = new float[go2.transform.childCount];
        for (int i = 0; i < go2.transform.childCount; i++)
        {
            pieceDis[i] = (go2.transform.GetChild(i).position - centerPos).magnitude;
        }
        const float costTime = 1f;
        GenericTools.DelayFun(costTime, (float f) =>
        {
            foreach (Transform item in go.transform)
            {
                Vector3 v3 = item.position - centerPos;
                v3 = Quaternion.Euler(0, 0, 90 / costTime * Time.deltaTime) * v3;
                v3 = v3 * 0.9f + 0.1f * 1 * Mathf.Sin(f * Mathf.PI) * v3.normalized;
                item.position = centerPos + v3;
            }
            for (int i = 0; i < go2.transform.childCount; i++)
            {
                Transform item = go2.transform.GetChild(i);
                Vector3 v3 = item.position - centerPos;
                v3 = Quaternion.Euler(0, 0, (360 + i * 30) / costTime * Time.deltaTime) * v3;
                v3 = v3.normalized * (pieceDis[i] * (1 - f));
                item.position = centerPos + v3;
            }

        }, () =>
        {
            Destroy(go);
            Destroy(go2);
        });
    }

    private const float maskMaxSize = 5.4f;
    private float selfLightSize = 0.7f;
    private void SelfLight(float f)
    {
        selfLightSize = 0.7f * f;
        MaskSize();
    }
    private void MaskSize()
    {
        float scaleX = selfLightSize + (progress * (maskMaxSize - 0.7f)) / 100;
        maskTrs.localScale = new Vector3(scaleX, 1, 1);
        body.localPosition = new Vector3(maskMaxSize / 2 - scaleX / 2, 0, 0);
    }

    private void OnDisable()
    {
        foreach (var item in hurts)
        {
            Destroy(item);
        }
        hurts.Clear();
    }
}
