/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	HpMark
作    者:	JaydenHe
描    述:	英雄血条标尺
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[AddComponentMenu("UI/HpMark", 11)]
public class HpMark : Image
{
    public Color MarkColor;
    public float m_markShortWidth = 3f;
    public float m_markShortFactor = 0.5f;
    public float m_markLongFactor = 0.7f;

    private int m_marksCnt = 10;
    private float m_divRange;

    public void InitMarks(int marksCnt)
    {
        if(m_marksCnt != marksCnt)
        {
            m_marksCnt = marksCnt;
            SetVerticesDirty();
        }
    }
    //------------------------------------------------------
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();
        if (m_marksCnt <= 0) return;
        Rect rect = GetPixelAdjustedRect();
        m_divRange = rect.width / m_marksCnt;
        for (int i=0;i< m_marksCnt; i++)
        {
            float offsetX = i * m_divRange + rect.xMin;
            float offsetY = rect.yMax-((i % 3 == 0) ? m_markLongFactor : m_markShortFactor) * rect.height;

            int verCnt = toFill.currentVertCount;
            toFill.AddVert(new Vector3(offsetX, rect.yMax), MarkColor, Vector2.zero);
            toFill.AddVert(new Vector3(offsetX, offsetY), MarkColor, Vector2.up);
            toFill.AddVert(new Vector3(offsetX + m_markShortWidth, offsetY), MarkColor, Vector2.one);
            toFill.AddVert(new Vector3(offsetX + m_markShortWidth, rect.yMax), MarkColor, Vector2.right);
            toFill.AddTriangle(verCnt, verCnt+1, verCnt + 2);
            toFill.AddTriangle(verCnt, verCnt + 2, verCnt + 3);
        }
    }
}
