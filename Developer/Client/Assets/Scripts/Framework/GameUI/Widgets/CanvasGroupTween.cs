/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	CanvasGroupTween
作    者:	zdq
描    述:	控制CanvasGroup透明度
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupTween : MonoBehaviour
{
    public AnimationCurve AlphaCurve;
    bool m_IsEnable = false;
    float m_PlayTimer = 0f;

    public CanvasGroup canvasGroup;
    
    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    //------------------------------------------------------
    void Update()
    {
        if (m_IsEnable == false || canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = AlphaCurve.Evaluate(m_PlayTimer);
        m_PlayTimer += Time.deltaTime;
        if (m_PlayTimer >= AlphaCurve.keys[AlphaCurve.keys.Length - 1].time)
        {
            float value = AlphaCurve.Evaluate(AlphaCurve.keys[AlphaCurve.keys.Length - 1].time);
            canvasGroup.alpha = value;
            OnTweenCompleted();
        }
    }
    //------------------------------------------------------
    public void Clear()
    {
        m_IsEnable = false;
        m_PlayTimer = 0;
    }
    //------------------------------------------------------
    public void OnTweenCompleted()
    {
        Stop();
    }
    //------------------------------------------------------
    public void Play()
    {
        m_IsEnable = true;
    }
    //------------------------------------------------------
    public void Stop()
    {
        Clear();
    }
}
