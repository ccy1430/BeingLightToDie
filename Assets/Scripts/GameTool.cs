using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class GameTool
{
    public static float LightTime = 0.5f;
    public static void OpenLight(Light2D l2d, float rang = 1, Action cb = null)
    {
        GenericTools.DelayFun(LightTime, LightingFun, cb);
        void LightingFun(float t)
        {
            l2d.pointLightOuterRadius = rang * t;
        }
    }
    public static void CloseLight(Light2D l2d, float range = 1, Action cb = null)
    {
        GenericTools.DelayFun(LightTime, LightingFun, cb);
        void LightingFun(float t)
        {
            l2d.pointLightOuterRadius = range * (1 - t);
        }
    }
}
