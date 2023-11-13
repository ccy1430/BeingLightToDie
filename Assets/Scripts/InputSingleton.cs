using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    private static InputSingleton instance = null;
    public static InputSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("InputSingleton");
                instance = go.AddComponent<InputSingleton>();
                DontDestroyOnLoad(go);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                instance.keySet = SaveData<InputKeySet>.Data;
#endif
            }
            if (!instance.updateonce)
            {
                instance.GetInput();
            }
            return instance;
        }
    }

    [SerializeField] private bool updateonce = false;
    [SerializeField] private float input_lr = 0;
    [SerializeField] private float input_ud = 0;
    [SerializeField] private bool input_jump = false;
    [SerializeField] private bool input_click = false;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private InputKeySet keySet;
#endif

    public float LR
    {
        get
        {
            return input_lr;
        }
    }
    public float UD
    {
        get
        {
            return input_ud;
        }
    }
    public bool Jump
    {
        get
        {
            return input_jump;
        }
    }
    public bool Click
    {
        get { return input_click; }
    }
    private void GetInput()
    {
        input_jump = false;
        input_lr = 0;
        input_ud = 0;
        input_click = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetKey(keySet.Key_RIGHT)) input_lr = 1;
        else if (Input.GetKey(keySet.Key_LEFT)) input_lr = -1;

        if (Input.GetKey(keySet.Key_UP)) input_ud = 1;
        else if (Input.GetKey(keySet.Key_DOWN)) input_ud = -1;

        input_jump = Input.GetKey(keySet.Key_JUMP);

        if (Input.GetJoystickNames().Length > 0)
        {
            float temp = Input.GetAxisRaw("JoyX");
            if(temp!= 0) { input_lr = temp; }
            temp = Input.GetAxisRaw("JoyY");
            if (temp != 0) input_ud = -temp;
            input_jump |= Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.JoystickButton3);
        }

        if (Input.anyKeyDown)
        {
            input_click = true;
        }
        //Debug.Log($"{input_jump} {input_lr} {input_ud} {input_click}");
#elif UNITY_ANDROID
        var touches = Input.touches;
        foreach (var item in touches)
        {
            if (item.rawPosition.x < Screen.width / 2)
            {
                float subx = item.position.x - item.rawPosition.x;
                if (item.phase != TouchPhase.Ended)
                {
                    if (subx > 50) input_lr = Mathf.Min(1, (subx - 50) / 150f);
                    else if (subx < -50) input_lr = -Mathf.Min(1, (subx + 50) / -150f);
                }
                float suby = item.position.y - item.rawPosition.y;
                if (item.phase != TouchPhase.Ended)
                {
                    if (suby > 50) input_ud = Mathf.Min(1, (suby - 50) / 150f);
                    else if (suby < -50) input_ud = -Mathf.Min(1, (suby + 50) / -150f);
                }
            }
            else
            {
                input_jump = true;
            }
            if (item.tapCount == 1 && item.phase == TouchPhase.Began)
            {
                input_click = true;
            }
        }
#endif

        updateonce = true;
    }

    private void LateUpdate()
    {
        updateonce = false;
    }
}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
[System.Serializable]
public class InputKeySet
{
    public const int VERSION = 0;
    public int version = 0;

    public const int UP = 0;
    public const int DOWN = 1;
    public const int LEFT = 2;
    public const int RIGHT = 3;
    public const int JUMP = 4;

    public KeyCode Key_UP { get { return keys[UP]; } }
    public KeyCode Key_DOWN { get { return keys[DOWN]; } }
    public KeyCode Key_LEFT { get { return keys[LEFT]; } }
    public KeyCode Key_RIGHT { get { return keys[RIGHT]; } }
    public KeyCode Key_JUMP { get { return keys[JUMP]; } }

    public List<string> keyNames = new List<string> { "ÉÏ", "ÏÂ", "×ó", "ÓÒ", "Ìø" };
    public List<KeyCode> keys = new List<KeyCode> { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space };
}
#endif