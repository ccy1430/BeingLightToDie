using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Start : MonoBehaviour
{
    public GameObject chooselevel;
    private void OnEnable()
    {
        chooselevel.SetActive(SaveData.Data.hadChooseLevel);
    }
}
