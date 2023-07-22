using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swear_Hurt : MonoBehaviour
{
    public float moveSpeed;
    private Transform followTrs;
    private Vector3 targetPos;
    private Vector3 dirV3;

    public void Init(Transform follow, Vector3 targetPos)
    {
        followTrs = follow;
        this.targetPos = targetPos;
        dirV3 = Vector3.up;
        transform.position = followTrs.position + targetPos;
        StartCoroutine(GenericTools.DelayFun_Cor(1f, (f) =>
        {
            transform.localScale = Vector3.one * (f * 0.2f);
        }, null));
    }

    private void FixedUpdate()
    {
        targetPos += Time.fixedDeltaTime * moveSpeed * dirV3;
        transform.position = Vector3.Lerp(transform.position, followTrs.position + targetPos, 0.1f);
        if (targetPos.y > 0.6f)
        {
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            dirV3 = Vector3.down;
        }
        else if (targetPos.y < -0.6f)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
            dirV3 = Vector3.up;
        }
    }
}
