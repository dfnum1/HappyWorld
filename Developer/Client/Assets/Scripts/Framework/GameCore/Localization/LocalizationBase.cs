/********************************************************************
生成日期:	2022-03-24
类    名: 	LocalizationBase
作    者:	zdq
描    述:	多语言组件基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame;
using TopGame.Core;
using UnityEngine;

public abstract class LocalizationBase : MonoBehaviour
{
    public uint ID = 0;

    public abstract void OnLanguageChangeCallback(SystemLanguage languageType);

    public abstract void RefreshShow();

    public virtual void Start()
    {
        ALocalizationManager.OnLanguageChangeEvent += OnLanguageChangeCallback;
    }
    //------------------------------------------------------
    public virtual void OnEnable()
    {
        RefreshShow();
    }
    //------------------------------------------------------
    public virtual void OnDestroy()
    {
        ALocalizationManager.OnLanguageChangeEvent -= OnLanguageChangeCallback;
    }
}
