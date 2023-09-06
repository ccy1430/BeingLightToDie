using UnityEngine;

public class RainDrop : MonoBehaviour
{
    public GenericPool<RainDrop> belongPool;

    private const float lifeTime = 5;
    private float dropSpeed = 5;
    private float startTime = 0;
    private SpriteRenderer self_sp;
    private Color originColor;
    private Vector3 dropStep;
    private Vector3 originScale;

    public void CreatInit()
    {
        self_sp = GetComponent<SpriteRenderer>();
        originColor = self_sp.color;
        originScale = transform.localScale;
    }

    public void Init(float _speed, Vector2 initPos)
    {
        gameObject.SetActive(true);
        dropSpeed = _speed;
        transform.position = initPos;
        transform.localScale = originScale * (0.6f + _speed / 10);
        startTime = Time.time;
        self_sp.color = originColor;
        dropStep = Time.fixedDeltaTime * dropSpeed * Vector3.down;
    }

    private void FixedUpdate()
    {
        if (Time.time - startTime > lifeTime) gameObject.SetActive(false);
        transform.position += dropStep;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (fading) return;
        if (coll.CompareTag("Water_hov"))
        {
            coll.GetComponent<Water_hor>().AddForce(transform.position);
        }
        Fade();
    }
    private bool fading = false;
    private void Fade()
    {
        Color tempColor = originColor;
        GenericTools.DelayFun(0.5f, FadeFun, FadeCallBack);
        fading = true;
        dropStep /= 5;
        void FadeFun(float f)
        {
            tempColor.a = 1 - f / 0.5f;
            self_sp.color = tempColor;
        }
        void FadeCallBack()
        {
            fading = false;
            belongPool.EnPool(this);
        }
    }
}
