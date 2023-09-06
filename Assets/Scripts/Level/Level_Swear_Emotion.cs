using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swear_Emotion : MonoBehaviour
{
    private const float rectW = GameConfig.mapWidth / 2f + 1;
    private const float rectH = GameConfig.mapHeight / 2f + 1;
    

    public SpriteRenderer sprr;
    public Sprite[] sprites;
    public float speedAcce;

    private Level_Swear_Pledge pledge;
    private GenericPool<GameObject> belongPool;
    [HideInInspector]
    public int selfIndex;
    private Vector3 dir;
    private float moveSpeed;


    public void Init(Level_Swear_Pledge pledge, GenericPool<GameObject> belongPool)
    {
        this.pledge = pledge;
        this.belongPool = belongPool;
        Init();
    }

    public void OnEnable()
    {
        if (pledge != null) Init();
    }
    private void Init()
    {
        moveSpeed = 1;
        selfIndex = Random.Range(0, sprites.Length);
        sprr.sprite = sprites[selfIndex];
        //随机取一个角度 形成射线 反算轨迹
        var angle = Random.Range(-30, 90);
        transform.localRotation = Quaternion.Euler(0, 0, angle > 30 ? angle - 60 : angle);
        if (angle > 30) angle += 120;
        Vector2 v2 = Quaternion.Euler(0, 0, angle) * Vector3.right;
        var pos = pledge.transform.position;
        moveSpeed += speedAcce * SaveData.Data.levelIndex / 100f;
        switch (selfIndex)
        {
            case 0:
                moveSpeed += SaveData.Data.levelIndex / 100f; break;
            case 1:
            case 2:
            case 3:
                var angle2 = Random.Range(0, 4) * 90;
                pos += Quaternion.Euler(0, 0, angle) * Vector3.right * (selfIndex / 3f);
                break;

        }

        Vector2 endPos = pos;
        int y_sign = v2.y > 0 ? 1 : -1;
        if (v2.x >= -0.01f && v2.x <= 0.01f)
        {
            endPos.y = y_sign * rectH;
        }
        else
        {
            float x = (v2.x > 0 ? 1 : -1) * rectW;
            float t = (x - pos.x) / v2.x;
            float y_cross = pos.y + t * v2.y;
            if (y_cross < rectH && y_cross > -rectH)
            {
                endPos.x = x;
                endPos.y = y_cross;
            }
            else
            {
                float y = y_sign * rectH;
                t = (y - pos.y) / v2.y;
                float x_cross = pos.x + t * v2.x;
                endPos.y = y;
                endPos.x = x_cross;
            }
        }
        transform.position = endPos;
        dir = -v2;
    }
    private void FixedUpdate()
    {
        transform.position += dir * moveSpeed * Time.fixedDeltaTime;
        if (transform.position.x > rectW + 1 ||
            transform.position.x < -rectW - 1 ||
            transform.position.y < -rectH - 1 ||
            transform.position.y > rectH + 1)
        {
            belongPool.EnPool(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pledge"))
        {
            var pledge = collision.GetComponent<Level_Swear_Pledge>();
            pledge.CollEmo(this);
        }
    }
    public void BackPool()
    {
        belongPool.EnPool(gameObject);
    }

    [ContextMenu("Splite")]
    public GameObject Splite()
    {
        var tempparent = new GameObject();
        var rect = new Rect(sprr.bounds.center, new Vector2(2.4f, 0.8f));
        Material mat = new Material(sprr.sharedMaterial);
        mat.mainTexture = sprr.sprite.texture;
        var gos = SpriteExploder.GenerateTriangularPieces(gameObject, rect, 3, mat);
        foreach (var item in gos)
        {
            item.transform.parent = tempparent.transform;
        }
        return tempparent;
    }
}
