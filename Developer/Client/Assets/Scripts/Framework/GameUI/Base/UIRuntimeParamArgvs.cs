/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	EventTriggerListener
作    者:	HappLI
描    述:	UI 公共监听
*********************************************************************/
using System;
using Framework.Plugin.AT;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.Core;
using VariableString = Framework.Core.VariableString;

namespace TopGame.UI
{
    public enum EParamArgvFlag
    {
        String = 0,
        Int,
        Float,
        Obj,
    }
    //------------------------------------------------------
    [System.Serializable]
    public struct ATEventParam
    {
        public Base.EUIEventType type;
        public string eventName;
    }
    //------------------------------------------------------
    [System.Serializable]
    public struct ADParamArgv
    {
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("Proto3.AdvertisingTypeCode")]
#endif
        public int ADType;
        public int ExternID;
        public int SubID;
    }
    //------------------------------------------------------
    [System.Serializable]
    public struct UIParamArgv
    {
        [Framework.Data.DisplayNameGUI("列表控件")]
        public MonoBehaviour listBehavour;
        public GameObject agentTrigger;
        public byte nFlag;
        public string strParam;
        public int intParam;
        public float floatParam;
        public UnityEngine.Object objParam;

        public bool IsFlag(EParamArgvFlag flag)
        {
            return (nFlag & (1 << (int)flag)) != 0;
        }

        public void SetFlag(EParamArgvFlag flag, bool bEnable)
        {
            if (bEnable) nFlag |= (byte)(1 << (int)flag);
            else nFlag &= (byte)(~((1 << (int)flag)));
        }
    }
    //------------------------------------------------------
    public struct UIRuntimeParamArgvs : Framework.Plugin.AT.IUserData
    {
        public VariablePoolAble param1;
        public VariablePoolAble param2;
        public VariablePoolAble param3;
        public VariablePoolAble param4;
        public void Destroy()
        {
            if (param1 != null) param1.Destroy();
            if (param2 != null) param2.Destroy();
            if (param3 != null) param3.Destroy();
            if (param4 != null) param4.Destroy();
        }

        public void FillParam(UIParamArgv paramArgvs)
        {
            if (paramArgvs.IsFlag(EParamArgvFlag.String) && !string.IsNullOrEmpty(paramArgvs.strParam))
            {
                if (param1 == null) param1 = new VariableString() { strValue = paramArgvs.strParam };
                else if (param1 == null) param2 = new VariableString() { strValue = paramArgvs.strParam };
                else if (param3 == null) param3 = new VariableString() { strValue = paramArgvs.strParam };
                else if (param4 == null) param4 = new VariableString() { strValue = paramArgvs.strParam };
            }
            if (paramArgvs.IsFlag(EParamArgvFlag.Int) && paramArgvs.intParam != 0)
            {
                if (param1 == null) param1 = new Variable1() { intVal = paramArgvs.intParam };
                else if (param1 == null) param2 = new Variable1() { intVal = paramArgvs.intParam };
                else if (param3 == null) param3 = new Variable1() { intVal = paramArgvs.intParam };
                else if (param4 == null) param4 = new Variable1() { intVal = paramArgvs.intParam };
            }
            if (paramArgvs.IsFlag(EParamArgvFlag.Obj) && paramArgvs.objParam != null)
            {
                if (param1 == null) param1 = new VariableObj() { pGO = paramArgvs.objParam };
                else if (param1 == null) param2 = new VariableObj() { pGO = paramArgvs.objParam };
                else if (param3 == null) param3 = new VariableObj() { pGO = paramArgvs.objParam };
                else if (param4 == null) param4 = new VariableObj() { pGO = paramArgvs.objParam };
            }
            if (paramArgvs.IsFlag(EParamArgvFlag.Float) && Mathf.Abs(paramArgvs.floatParam)>0.01f)
            {
                if (param1 == null) param1 = new Variable1() { floatVal = paramArgvs.floatParam };
                else if (param1 == null) param2 = new Variable1() { floatVal = paramArgvs.floatParam };
                else if (param3 == null) param3 = new Variable1() { floatVal = paramArgvs.floatParam };
                else if (param4 == null) param4 = new Variable1() { floatVal = paramArgvs.floatParam };
            }
        }
        //------------------------------------------------------
        public bool GetInt(out int outValue)
        {
            outValue = 0;
            if (param1!=null)
            {
                if (param1 is Variable1)
                {
                    outValue = ((Variable1)param1).intVal;
                    return true;
                }
            }
            if (param2 != null)
            {
                if (param2 is Variable1)
                {
                    outValue = ((Variable1)param2).intVal;
                    return true;
                }
            }
            if (param3 != null)
            {
                if (param3 is Variable1)
                {
                    outValue = ((Variable1)param3).intVal;
                    return true;
                }
            }
            if (param4 != null)
            {
                if (param3 is Variable1)
                {
                    outValue = ((Variable1)param3).intVal;
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public bool GetFloat(out float outValue)
        {
            outValue = 0;
            if (param1 != null)
            {
                if (param1 is Variable1)
                {
                    outValue = ((Variable1)param1).floatVal;
                    return true;
                }
            }
            if (param2 != null)
            {
                if (param2 is Variable1)
                {
                    outValue = ((Variable1)param2).floatVal;
                    return true;
                }
            }
            if (param3 != null)
            {
                if (param3 is Variable1)
                {
                    outValue = ((Variable1)param3).floatVal;
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public bool GetString(out string outValue)
        {
            outValue = null;
            if (param1 != null)
            {
                if (param1 is VariableString)
                {
                    outValue = ((VariableString)param1).strValue;
                    return !string.IsNullOrEmpty(outValue);
                }
            }
            if (param2 != null)
            {
                if (param2 is VariableString)
                {
                    outValue = ((VariableString)param2).strValue;
                    return !string.IsNullOrEmpty(outValue);
                }
            }
            if (param3 != null)
            {
                if (param3 is VariableString)
                {
                    outValue = ((VariableString)param3).strValue;
                    return !string.IsNullOrEmpty(outValue);
                }
            }
            return false;
        }
        //------------------------------------------------------
        public bool GetObj(out UnityEngine.Object outValue)
        {
            outValue = null;
            if (param1 != null)
            {
                if (param1 is VariableObj)
                {
                    outValue = ((VariableObj)param1).pGO;
                    return outValue != null;
                }
                if (param1 is VariableGO)
                {
                    outValue = ((VariableGO)param1).pGO;
                    return outValue != null;
                }
            }
            if (param2 != null)
            {
                if (param2 is VariableObj)
                {
                    outValue = ((VariableObj)param2).pGO;
                    return outValue != null;
                }
                if (param2 is VariableGO)
                {
                    outValue = ((VariableGO)param2).pGO;
                    return outValue != null;
                }
            }
            if (param3 != null)
            {
                if (param3 is VariableObj)
                {
                    outValue = ((VariableObj)param3).pGO;
                    return outValue != null;
                }
                if (param3 is VariableGO)
                {
                    outValue = ((VariableGO)param3).pGO;
                    return outValue != null;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public bool GetUserData(out IUserData outValue)
        {
            outValue = null;
            if (param1 != null)
            {
                outValue = param1;
                return true;
            }
            if (param2 != null)
            {
                outValue = param2;
                return true;
            }
            if (param3 != null)
            {
                outValue = param3;
                return true;
            }
            return false;
        }
    }
    //------------------------------------------------------
    [System.Serializable]
    public struct UserActionData
    {
        public string eventName;
        public string strProperty;
        public string strValue;
        public bool bImmelaySendSvr;

        public bool IsValid
        {
            get {  return !string.IsNullOrEmpty(eventName); }
        }
    }
    //------------------------------------------------------
    [System.Serializable]
    public struct BtnUnLockData
    {
        public TopGame.Logic.UnLockListener listener;
        public bool isCheckLockState;
    }
}