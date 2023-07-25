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
            }
            if (!instance.updateonce)
            {
                instance.GetInput();
            }
            return instance;
        }
    }

    private bool updateonce = false;
    private int input_lr = 0;
    private int input_ud = 0;
    //private bool input_jumps = false;
    private bool input_jump = false;
    private bool input_click = false;
    public int LR
    {
        get
        {
            return input_lr;
        }
    }
    public int UD
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
        if (Input.GetKey(KeyCode.RightArrow)) input_lr = 1;
        else if (Input.GetKey(KeyCode.LeftArrow)) input_lr = -1;

        if (Input.GetKey(KeyCode.UpArrow)) input_ud = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) input_ud = -1;

        input_jump = Input.GetKey(KeyCode.Space);
        if (Input.GetMouseButtonDown(0))
        {
            input_click = true;
        }
#elif UNITY_ANDROID
        var touches = Input.touches;
        foreach (var item in touches)
        {
            if (item.rawPosition.x < Screen.width / 2) 
            {
                float subx = item.position.x - item.rawPosition.x;
                if (item.phase != TouchPhase.Ended)
                {
                    if (subx > 50) input_lr = 1;
                    else if (subx < -50) input_lr = -1;
                }
                float suby = item.position.y - item.rawPosition.y;
                if (item.phase != TouchPhase.Ended)
                {
                    if (subx > 50) input_ud = 1;
                    else if (subx < -50) input_ud = -1;
                }
            }
            else
            {
                input_jump = true;
            }
            if (item.tapCount == 1)
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
