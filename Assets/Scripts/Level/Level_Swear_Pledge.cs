using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swear_Pledge : MonoBehaviour
{
    private const float rectW = GameConfig.mapWidth / 2f - 1;
    private const float rectH = GameConfig.mapHeight / 2f - 1;
    private const float baseSpeed = 10;

    public Transform maskTrs;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        transform.position = new Vector3(-0.35f, 0, 0);
        baseNum = SaveData.Data.levelIndex / 10f + 1;
        moveSpeed = originMoveSpeed = baseSpeed - SaveData.Data.levelIndex / 15f;
        progressSpeed = originProgressSpeed = baseSpeed * 10 / SaveData.Data.levelIndex + 2;
        progress = 0;
        MaskSize();
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
            gameObject.SetActive(false);
        }
    }
    private Coroutine cor1, cor2, cor3;

    public void CollEmo(Level_Swear_Emotion emo)
    {
        Debug.Log("coll emo :" + emo.selfIndex);
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
        MaskSize();
    }

    private const float maskMaxSize = 5.4f;
    private void MaskSize()
    {
        float scaleX = 0.7f + (progress * (maskMaxSize - 0.7f)) / 100;
        maskTrs.localScale = new Vector3(scaleX, 1, 1);
        maskTrs.localPosition = new Vector3(scaleX / 2, 0, 0);
    }
}
