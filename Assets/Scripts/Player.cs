using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class Player : MonoBehaviour
{
    public enum PlayerState
    {
        none,
        idle,
        die,
    }

    public BoxCollider2D bodyColl;

    private UnityEngine.Rendering.Universal.Light2D light2d;
    private float light2dRange;
    private SpriteRenderer self_sp;
    private Animator self_anim;
    private Rigidbody2D self_rb2d;
    private PlayerState self_state = PlayerState.none;
    private GenericPool<PathData> pathDataPool;
    public Vector3 originPos = new Vector3(-9.5f, 0, 0);
    [Space]
    public TileBase selfPosTileBase;

    private const float lightBaseSize = 1.5f;
    private void Awake()
    {
        light2d = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        self_anim = GetComponent<Animator>();
        self_sp = GetComponent<SpriteRenderer>();
        self_rb2d = GetComponent<Rigidbody2D>();
        AddReceiver();
    }
    void Start()
    {

        Physics2D.queriesStartInColliders = false;
        InitAction();
        InitAni();

        light2d.pointLightOuterRadius = 0;

        whenJumpStateChange += (oldv, newv) =>
        {
            if (oldv == JumpState.idle && newv == JumpState.jumping) JumpWater(0.03f);
            if (oldv == JumpState.fall && newv == JumpState.jumpending) JumpWater(-0.03f);
        };
    }
    private void AddReceiver()
    {
        pathDataPool = GenericPool<PathData>.LinkSignPool(GenericSign.playerDie, CreatPool);
        GenericMsg<TileBase[]>.AddReceiver(GenericSign.loadFlower, FindAndSetPos);
        GenericMsg.AddReceiver(GenericSign.startLevel, StartLevel);
        GenericMsg.AddReceiver(GenericSign.backMenu, ExitLevel);
    }
    private void OnDestroy()
    {
        GenericMsg<TileBase[]>.DelReceiver(GenericSign.loadFlower, FindAndSetPos);
        GenericMsg.DelReceiver(GenericSign.startLevel, StartLevel);
        GenericMsg.DelReceiver(GenericSign.backMenu, ExitLevel);
        GenericPool<PathData>.UnLinkSignPool(GenericSign.playerDie);
    }
    private GenericPool<PathData> CreatPool()
    {
        return new GenericPool<PathData>(() => { return new PathData(); }, (pd) => { pd.Clear(); }, null);
    }
    void Update()
    {
        if (self_state == PlayerState.idle)
        {
            ActionUpdate();
        }
    }
    private void FixedUpdate()
    {
        if (self_state == PlayerState.idle)
        {
            ActionFixedUpdate();
            AniFixedUpdate();
        }
    }

    private void FindAndSetPos(TileBase[] tiles)
    {
        Vector2Int playerPos = Vector2Int.one;
        for (int x = 0; x < 32; x++) for (int y = 0; y < 18; y++)
            {
                int tilesIndex = x + y * 32;
                if (tiles[tilesIndex] == selfPosTileBase)
                {
                    playerPos = new Vector2Int(x, y);
                    break;
                }
            }
        originPos = new Vector3(playerPos.x - 15.5f, playerPos.y - 8.5f);
    }
    private void StartLevel()
    {
        transform.position = originPos;
        light2dRange = SaveData.Data.playerLightSize * lightBaseSize;
        Lighting(ResumeIdle);
        ResetAction();
        ResetAnim();
        fixedTime = 0;
        cachePath = pathDataPool.GetT();
    }
    private void ExitLevel()
    {
        if (self_state == PlayerState.none) return;
        self_state = PlayerState.none;
        ResetAction();
        Fade(null);
        pathDataPool.EnPool(cachePath);
        cachePath = null;
    }
    private void ResumeIdle()
    {
        self_state = PlayerState.idle;
    }
    private void ResetAnim()
    {
        self_anim.ResetTrigger("jumps");
        self_anim.ResetTrigger("jumpe");
        self_anim.ResetTrigger("fall");
        self_anim.SetBool("run", false);
        self_anim.Play("idle");
        self_anim.speed = 1;
        self_sp.flipX = false;
    }

    public void DieOnce()
    {
        if (self_state != PlayerState.idle) return;
        self_state = PlayerState.die;
        ResetAction();
        cachePath.TryAddSpeed(fixedTime, Vector3.zero);
        self_anim.speed = 0;

        AudioManager.Instance.PlayAudio("die");
        AudioManager.Instance.Stop("walk");

        GenericMsg.Trigger(GenericSign.playerDie);
        Fade(TriggerDie);
    }
    private void TriggerDie()
    {
        GenericMsg<PathData>.Trigger(GenericSign.playerDie, cachePath);
    }
    public void UnAcive()
    {
        self_state = PlayerState.none;
        Fade(null);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Trap"))
        {
            if (SaveData.Data.hadHurt)
            {
                DieOnce();
            }
        }
        else if (coll.CompareTag("You"))
        {
            self_anim.speed = 0;

            AudioManager.Instance.PlayAudio("levelend");
            AudioManager.Instance.Stop("walk");

            Debug.Log("Find you,save you,i promise");
            cachePath.Clear();
            GenericMsg.Trigger(GenericSign.nextLevel);
            ExitLevel();
        }
    }

    #region tools
    public void Lighting(System.Action cb)
    {
        GameTool.OpenLight(light2d, rang: light2dRange, cb: cb);
    }

    public void Fade(System.Action cb)
    {
        GameTool.CloseLight(light2d, rang: light2dRange, cb: cb);
    }

    private void JumpWater(float force)
    {
        var water = WaterMgr.Instance.GetPosWater(transform.position);
        if (water != null)
        {
            water.AddForce(transform.position, force);
            water.AddForce(transform.position + 2 * Time.fixedDeltaTime * speed_now, force * 3);
        }
    }
    #endregion

    #region animator
    private void InitAni()
    {
        whenJumpStateChange += (oldv, newv) =>
        {
            if (oldv == JumpState.idle && newv == JumpState.jumping)
            {
                self_anim.SetTrigger("jumps");
                AudioManager.Instance.PlayAudio("jumps");
                return;
            };
            if (oldv == JumpState.fall && newv == JumpState.idle)
            {
                self_anim.SetTrigger("jumpe");
                AudioManager.Instance.PlayAudio("jumpe");
                return;
            }
            if (oldv == JumpState.idle && newv == JumpState.fall)
            {
                self_anim.SetTrigger("fall");
            }
        };
    }
    private void AniFixedUpdate()
    {
        self_anim.SetBool("run", input_lr != 0);

        if (input_lr != 0 && canStand) AudioManager.Instance.PlayAudio("walk", true);
        else AudioManager.Instance.Stop("walk");

        if (input_lr > 0) self_sp.flipX = false;
        else if (input_lr < 0) self_sp.flipX = true;
    }
    #endregion
}
