using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Renderer>().material.SetVector("_MainTex_ST", new Vector4(1, 1, 0, 0));
        RaycastHit2D two = Physics2D.BoxCast(Vector2.up * 0.5f,
            new Vector2(0.1f, 0.2f), 0, Vector2.down, 1f);
        Debug.Log(two.distance);
        Debug.Log(two.point);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
