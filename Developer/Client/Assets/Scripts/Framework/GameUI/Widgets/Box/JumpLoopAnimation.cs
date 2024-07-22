/********************************************************************
生成日期:	2021-8-11
类    名: 	JumpLoopAnimation
作    者:	jaydenhe
描    述:	弹跳动画
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLoopAnimation : MonoBehaviour
{
    public int JumpYPosOffset = 50;
    public float JumpYScaleOffset = 1.5f;
    public float JumpSpeed = 1;

    private bool m_IsEnable = true;
    private float m_PlayTimer = 0f;
    private float m_ScaleFactorValue = 1;
    private float m_PosFactorValue = 1;

    private RectTransform m_Rect;
    private AnimationCurve m_ScaleCurve;
    private AnimationCurve m_PosCurve;
    private Vector2 m_DefaultPos;
    private Vector2 m_DefaultScale;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        m_Rect.anchorMax = new Vector2(0.5f,0);
        m_Rect.anchorMin = new Vector2(0.5f, 0);
        m_Rect.pivot = new Vector2(0.5f, 0);
        m_DefaultPos = m_Rect.anchoredPosition;
        m_DefaultScale = m_Rect.localScale;
        SetCurve();
    }

    private void SetCurve()
    {
        Keyframe[] keys = new Keyframe[3];
        keys[0] = new Keyframe(0, 0, 2, 2,0.33f, 0.33f);
        keys[1] = new Keyframe(0.5f, 1, 0, 0, 0.33f, 0.33f);
        keys[2] = new Keyframe(1, 0, -2, -2, 0.33f, 0.33f);
        m_PosCurve = new AnimationCurve(keys);

        Keyframe[] keys2 = new Keyframe[3];
        keys2[0] = new Keyframe(0, 1, 2, 2, 0, 0);
        keys2[1] = new Keyframe(0.5f, 1.5f, 1.3f, 1.33f, 0.33f, 0.17f);
        keys2[2] = new Keyframe(1, 1, 0, 0, 0, 0);
        m_ScaleCurve = new AnimationCurve(keys2);      
    }

    private void OnEnable()
    {
        m_IsEnable = true;
    }

    private void OnDisable()
    {
        m_IsEnable = false;
    }

    void Update()
    {
        if (!m_IsEnable || m_ScaleCurve == null || m_PosCurve == null) return;

        m_PlayTimer += Time.deltaTime * JumpSpeed;
        m_ScaleFactorValue = m_ScaleCurve.Evaluate(m_PlayTimer);
        m_PosFactorValue = m_PosCurve.Evaluate(m_PlayTimer);
        m_Rect.localScale = new Vector2(m_DefaultScale.x, m_DefaultScale.y*m_ScaleFactorValue);
        m_Rect.anchoredPosition = new Vector2(m_DefaultPos.x, m_DefaultPos.y + JumpYPosOffset * m_PosFactorValue);
        if (m_PlayTimer >= m_ScaleCurve.keys[m_ScaleCurve.keys.Length - 1].time)
        {
            m_PlayTimer = 0;
        }
    }
}
