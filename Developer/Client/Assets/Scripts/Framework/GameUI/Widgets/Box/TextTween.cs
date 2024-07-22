/********************************************************************
生成日期:	3:30:2021 10:06
类    名: 	TextTween
作    者:	JaydenHe
描    述:	跑马灯飘字组件
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextTween : MonoBehaviour
{
    public bool IsMoveCompleted = true;
    public float MoveSpeed = 200f;
    private float m_BgLen;
    private float m_BeginX;
    private float m_EndX;
    private RectTransform m_Rect;
    private Text m_Text;
    public System.Action<TextTween> OnCompleteCallBack;
    float m_DurationTime = 1.5f;
    float m_FlyTime = 1.5f;
 
    bool m_StartCountDown = false;
    Coroutine m_CurrentCoroutine;

    //------------------------------------------------------
    public void Init(float bglen)
    {
        m_Rect = gameObject.GetComponent<RectTransform>();
        m_Text = gameObject.GetComponent<Text>();
        m_BgLen = bglen;
        m_BeginX = m_Rect.anchoredPosition.x;
    }

    //------------------------------------------------------
    public void Run(string content,int loop = 1)
    {
        int loopTime = loop;
        if (m_Rect && m_Text)
        {
            m_Text.text = content;
            float len = CalcTextWidth(m_Text);
            Vector2 size = m_Rect.sizeDelta;
            m_Rect.sizeDelta = new Vector2(len,size.y);
            float time = (m_BgLen + len) / MoveSpeed;

            m_Rect.anchoredPosition = new Vector2(m_BeginX, m_Rect.anchoredPosition.y);

            m_FlyTime = time;
            m_DurationTime = time;
            m_EndX = m_BeginX - (m_BgLen + len);
            m_StartCountDown = true;
            m_CurrentCoroutine = StartCoroutine(RealTimeUpdate());
        }
    }

    /// <summary>
    /// 游戏暂停下也能播广播
    /// </summary>
    /// <returns></returns>
    IEnumerator RealTimeUpdate()
    {
        IsMoveCompleted = false;
        while (m_StartCountDown)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            m_DurationTime -= 0.01f;

            float CurrentXPos = Mathf.Lerp(m_BeginX, m_EndX, 1- m_DurationTime/ m_FlyTime);
            if (m_Rect)
                m_Rect.anchoredPosition = new Vector2(CurrentXPos, m_Rect.anchoredPosition.y);

            if (m_DurationTime < 0)
            {
                m_StartCountDown = false;
                IsMoveCompleted = true;
                OnCompleteCallBack?.Invoke(this);
                if(m_CurrentCoroutine != null)
                    StopCoroutine(m_CurrentCoroutine);
                m_CurrentCoroutine = null;
            }

        }
    }
    //------------------------------------------------------
    public void Reset()
    {
        if(m_Rect)
            m_Rect.anchoredPosition = new Vector2(m_BeginX, m_Rect.anchoredPosition.y);
    }
    //------------------------------------------------------
    private float CalcTextWidth(Text text)
    {
        TextGenerator tg = text.cachedTextGeneratorForLayout;
        TextGenerationSettings setting = text.GetGenerationSettings(Vector2.zero);
        float width = tg.GetPreferredWidth(text.text, setting) / text.pixelsPerUnit;
        return width+20;
    }
}
