/********************************************************************
生成日期:	2021-8-11
类    名: 	RandomMoveAnimation
作    者:	jaydenhe
描    述:	随机移动动画
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RandomMoveAnimation : MonoBehaviour
{
    public float RandomRange;
    public float Speed = 1;

    private RectTransform m_Rect;
    private bool m_IsEnable = true;
    private Vector2 m_RandomPos;
    private Vector2 m_DefaultPos;
    private float m_Timer;
    private float m_StepTime = 0.5f;
    private float m_Factor;
    public AnimationCurve m_MoveCurve;
    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
        m_DefaultPos = m_Rect.anchoredPosition;
    }

    private void OnEnable()
    {
        m_IsEnable = true;
    }

    private void SetRandomValue()
    {
        float x = Random.Range(m_DefaultPos.x - RandomRange, m_DefaultPos.x + RandomRange);
        float y = Random.Range(m_DefaultPos.y - RandomRange, m_DefaultPos.y + RandomRange);
        m_RandomPos = new Vector2(x,y);
        m_StepTime = Random.Range(0.3f,0.6f);
    }

    private void SetCurve()
    {

    }

    private void OnDisable()
    {
        m_IsEnable = false;
    }

    void Update()
    {
        if (!m_IsEnable || !m_Rect) return;
        m_Timer += Time.deltaTime * Speed;
        m_Factor = m_MoveCurve.Evaluate(m_Timer);

        if (m_Timer >= m_MoveCurve.keys[m_MoveCurve.keys.Length - 1].time)
        {
            m_Timer = 0;
            SetRandomValue();
        }
        else
        {
            m_Rect.anchoredPosition = Vector2.Lerp(m_Rect.anchoredPosition, m_RandomPos, m_Timer / m_StepTime);
        }
    }
}
