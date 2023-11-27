using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 描边
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("LFramework/UI/Effects/Outline", 2)]
public class UI_TextOutline : BaseMeshEffect
{
    public int m_OutlineWidth;
    public string shderName = "Unlit/FallWater2";

    //顶点缓存
    List<UIVertex> m_VertexCache = new List<UIVertex>();

    public override void ModifyMesh(VertexHelper vh)
    {
        if (graphic.material == null || graphic.material.shader.name != shderName) return;

        vh.GetUIVertexStream(m_VertexCache);

        ApplyOutline();

        vh.Clear();
        vh.AddUIVertexTriangleStream(m_VertexCache);
        m_VertexCache.Clear();
    }

    void ApplyOutline()
    {
        for (int i = 0, count = m_VertexCache.Count - 3; i <= count; i += 3)
        {
            var v1 = m_VertexCache[i];
            var v2 = m_VertexCache[i + 1];
            var v3 = m_VertexCache[i + 2];
            //计算原顶点坐标中心点
            var minX = Min(v1.position.x, v2.position.x, v3.position.x);
            var minY = Min(v1.position.y, v2.position.y, v3.position.y);
            var maxX = Max(v1.position.x, v2.position.x, v3.position.x);
            var maxY = Max(v1.position.y, v2.position.y, v3.position.y);
            var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
            //计算原始顶点坐标和UV的方向
            Vector2 triX, triY, uvX, uvY;
            Vector2 pos1 = v1.position;
            Vector2 pos2 = v2.position;
            Vector2 pos3 = v3.position;
            if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
            {
                triX = pos2 - pos1;
                triY = pos3 - pos2;
                uvX = v2.uv0 - v1.uv0;
                uvY = v3.uv0 - v2.uv0;
            }
            else
            {
                triX = pos3 - pos2;
                triY = pos2 - pos1;
                uvX = v3.uv0 - v2.uv0;
                uvY = v2.uv0 - v1.uv0;
            }
            //计算原始UV框
            var uvMin = Min(v1.uv0, v2.uv0, v3.uv0);
            var uvMax = Max(v1.uv0, v2.uv0, v3.uv0);
            //为每个顶点设置新的Position和UV，并传入原始UV框
            v1 = SetNewPosAndUV(v1, m_OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
            v2 = SetNewPosAndUV(v2, m_OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
            v3 = SetNewPosAndUV(v3, m_OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
            //应用设置后的UIVertex
            m_VertexCache[i] = v1;
            m_VertexCache[i + 1] = v2;
            m_VertexCache[i + 2] = v3;
        }
    }

    static UIVertex SetNewPosAndUV(UIVertex vertex, int width,
       Vector2 pPosCenter,
       Vector2 triangleX, Vector2 triangleY,
       Vector2 uvX, Vector2 uvY,
       Vector2 uvOriginMin, Vector2 uvOriginMax)
    {
        //Position
        var pos = vertex.position;
        var posXOffset = pos.x > pPosCenter.x ? width : -width;
        var posYOffset = pos.y > pPosCenter.y ? width : -width;
        pos.x += posXOffset;
        pos.y += posYOffset;
        vertex.position = pos;
        //UV
        var uv = vertex.uv0;
        Vector2 uvadd = uvX / triangleX.magnitude * posXOffset * (Vector2.Dot(triangleX, Vector2.right) > 0 ? 1 : -1) + 
            uvY / triangleY.magnitude * posYOffset * (Vector2.Dot(triangleY, Vector2.up) > 0 ? 1 : -1);
        uv.x += uvadd.x;
        uv.y += uvadd.y;
        vertex.uv0 = uv;

        vertex.uv1 = uvOriginMin;
        vertex.uv2 = uvOriginMax;

        return vertex;
    }

    static float Min(float pA, float pB, float pC)
    {
        return Mathf.Min(Mathf.Min(pA, pB), pC);
    }

    static float Max(float pA, float pB, float pC)
    {
        return Mathf.Max(Mathf.Max(pA, pB), pC);
    }

    static Vector2 Min(Vector2 pA, Vector2 pB, Vector2 pC)
    {
        return new Vector2(Min(pA.x, pB.x, pC.x), Min(pA.y, pB.y, pC.y));
    }

    static Vector2 Max(Vector2 pA, Vector2 pB, Vector2 pC)
    {
        return new Vector2(Max(pA.x, pB.x, pC.x), Max(pA.y, pB.y, pC.y));
    }
}