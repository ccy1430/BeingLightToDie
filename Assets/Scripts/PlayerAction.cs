using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Player : MonoBehaviour
{
    private enum JumpState
    {
        none, idle, jumping, fall, jumpending,
    }

    [Header("Action")]
    public Vector2 speed_limit_l = Vector2.one;
    public Vector2 speed_limit_r = Vector2.one;
    public int jumpframe_max = 25;
    public int jumpframe_min = 10;
    public float speed_hor_acce = 0.1f;
    public float speed_hor_acce2 = 0.2f;
    public float speed_ver_acce = 0.1f;
    public BoxCollider2D self_collider;
    public LayerMask collLayer;

    private Vector3 speed_now = Vector3.zero;
    private Vector3 speed_finalAdd = Vector3.zero;
    private int jumpframes = 0;
    [SerializeField]
    private bool canStand = true;

    //input
    private int input_lr;
    private bool input_jump;

    private readonly float skin = 0.1f;
    private float self_box_h;
    private float self_box_v;

    private PathData cachePath;
    private int fixedTime = 0;

    private Action<JumpState, JumpState> whenJumpStateChange;
    private JumpState js = JumpState.none;
    private JumpState _JumpState
    {
        get { return js; }
        set
        {
            whenJumpStateChange?.Invoke(js, value);
            js = value;
        }
    }

    private void InitAction()
    {
        self_box_h = self_collider.size.x / 2;
        self_box_v = self_collider.size.y / 2;
        self_collider.size -= 0.2f * Vector2.one;
        js = JumpState.none;
        _JumpState = JumpState.idle;
    }
    private void ActionUpdate()
    {
        input_lr = InputSingleton.Instance.LR;
        //if (InputSingleton.Instance.Jumps) whenStartJump();
        input_jump = InputSingleton.Instance.Jump;
    }
    private void ActionFixedUpdate()
    {
        int dir = speed_now.x > 0 ? 1 : -1;
        if (input_lr == 0)
        {
            if (speed_now.x != 0)
            {
                if (Mathf.Abs(speed_now.x) < speed_hor_acce) speed_now.x = 0;
                else speed_now.x -= dir * speed_hor_acce;
            }
        }
        else
        {
            if (speed_now.x == 0)
            {
                speed_now.x = input_lr * speed_hor_acce;
            }
            else
            {
                speed_now.x += dir * (input_lr == dir ? speed_hor_acce : -speed_hor_acce2);
            }
            speed_now.x = Mathf.Clamp(speed_now.x, speed_limit_l.x, speed_limit_r.x);
        }

        switch (_JumpState)
        {
            case JumpState.idle:
                if (canStand)
                {
                    if (input_jump)
                    {
                        _JumpState = JumpState.jumping;
                    }
                }
                else _JumpState = JumpState.fall;
                break;
            case JumpState.jumping:
                jumpframes++;
                if (input_jump)
                {
                    if (jumpframes > jumpframe_max) _JumpState = JumpState.fall;
                }
                else
                {
                    if (jumpframes > jumpframe_min) _JumpState = JumpState.fall;
                }
                if (jumpframes < 3) speed_now = Vector2.zero;
                else speed_now.y = speed_limit_r.y;
                //speed_now.y = speed_limit_r.y;
                break;
            case JumpState.fall:
                if (canStand)
                {
                    _JumpState = JumpState.idle;
                    jumpframes = 0;
                    speed_now.x = 0;
                }
                speed_now.y = Mathf.Clamp(speed_now.y - speed_ver_acce, speed_limit_l.y, speed_limit_r.y);
                break;
            case JumpState.jumpending:
                speed_now.x *=0.9f;
                jumpframes++;
                if (jumpframes >= 15)
                {
                    _JumpState = JumpState.idle;
                    jumpframes = 0;
                }
                break;
        }

        canStand = CanStand();
        RayCastSpeed();
        transform.position += speed_finalAdd;

        cachePath.TryAddSpeed(fixedTime++, speed_finalAdd);
        //Debug.LogFormat("jumpinput {0}  finalAdd {1}", input_jump, speed_finalAdd);
    }

    bool CanStand()
    {
        Vector2 pos = transform.position;
        RaycastHit2D one;
        one = Physics2D.BoxCast(pos, new Vector2((self_box_h - skin) * 2, 0.1f), 0, Vector2.down, self_box_v, collLayer);
        return one;
    }

    private void RayCastSpeed()
    {
        Vector2 pos = transform.position;
        speed_finalAdd = speed_now * Time.fixedDeltaTime;
        if (speed_finalAdd == Vector3.zero) return;
        RaycastHit2D one;
        int xmul = speed_now.x > 0 ? 1 : -1;
        float xraydis = self_box_h + speed_finalAdd.x * xmul;
        one = Physics2D.BoxCast(pos, new Vector2(0.1f, (self_box_v - skin) * 2), 0, Vector2.right * xmul, xraydis, collLayer);
        if (one)
        {
            float xhitdis = Mathf.Max(0, one.distance + 0.05f - self_box_h);
            speed_finalAdd.x = xhitdis * xmul;
        }
        pos.x += speed_finalAdd.x;

        int ymul = speed_now.y > 0 ? 1 : -1;
        float yraydis = self_box_v + speed_finalAdd.y * ymul;
        RaycastHit2D two = Physics2D.BoxCast(pos, new Vector2((self_box_h - skin) * 2, 0.1f), 0, Vector2.up * ymul, yraydis, collLayer);
        if (two)
        {
            float yhitdis = Mathf.Max(0, two.distance + 0.05f - self_box_v);
            speed_finalAdd.y = yhitdis * ymul;
        }
    }

    private void ResetAction()
    {
        jumpframes = 0;
        speed_now = Vector3.zero;
        speed_finalAdd = Vector3.zero;
        js = JumpState.none;
        _JumpState = JumpState.idle;
    }
}
