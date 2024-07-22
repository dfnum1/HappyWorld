/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AgentTreeFillVariable_Func
作    者:	HappLI
描    述:	图形编程自定义事件参数数据填充
*********************************************************************/
using System;
using UnityEngine;

namespace Framework.Plugin.AT
{
	public static class AgentTreeFillVariable_Func
	{
		public static bool OnFillCustomVariable(AgentTreeTask pTask, Variable pCustomVariable, IUserData pData)
		{
            //bool
            if (pCustomVariable is Framework.Plugin.AT.VariableBool)
            {
                if (pData is Core.Variable1) (pCustomVariable as Framework.Plugin.AT.VariableBool).mValue = ((Core.Variable1)pData).intVal != 0;
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableBoolList)
            {
                Framework.Plugin.AT.VariableBoolList list = pCustomVariable as Framework.Plugin.AT.VariableBoolList;
                if (pData is Core.Variable2)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<bool>(2);
                    list.mValue.Add(((Core.Variable2)pData).intVal0 != 0);
                    list.mValue.Add(((Core.Variable2)pData).intVal1 != 0);
                }
                else if (pData is Core.Variable3)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<bool>(3);
                    list.mValue.Add(((Core.Variable3)pData).intVal0 != 0);
                    list.mValue.Add(((Core.Variable3)pData).intVal1 != 0);
                    list.mValue.Add(((Core.Variable3)pData).intVal2 != 0);
                }
                else if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<bool>(4);
                    list.mValue.Add(((Core.Variable4)pData).intVal0 != 0);
                    list.mValue.Add(((Core.Variable4)pData).intVal1 != 0);
                    list.mValue.Add(((Core.Variable4)pData).intVal2 != 0);
                    list.mValue.Add(((Core.Variable4)pData).intVal3 != 0);
                }
                return true;
            }

            //byte
            if (pCustomVariable is Framework.Plugin.AT.VariableByte)
            {
                if (pData is Core.Variable1) (pCustomVariable as Framework.Plugin.AT.VariableByte).mValue = (byte)((Core.Variable1)pData).intVal;
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    int outValue;
                    if(uiArgv.GetInt(out outValue))  (pCustomVariable as Framework.Plugin.AT.VariableByte).mValue = (byte)outValue;
                }
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableByteList)
            {
                Framework.Plugin.AT.VariableByteList list = pCustomVariable as Framework.Plugin.AT.VariableByteList;
                if (pData is Core.Variable2)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<byte>(2);
                    list.mValue.Add((byte)((Core.Variable2)pData).intVal0);
                    list.mValue.Add((byte)((Core.Variable2)pData).intVal1);
                }
                else if (pData is Core.Variable3)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<byte>(3);
                    list.mValue.Add((byte)((Core.Variable3)pData).intVal0);
                    list.mValue.Add((byte)((Core.Variable3)pData).intVal1);
                    list.mValue.Add((byte)((Core.Variable3)pData).intVal2);
                }
                else if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<byte>(4);
                    list.mValue.Add((byte)((Core.Variable4)pData).intVal0);
                    list.mValue.Add((byte)((Core.Variable4)pData).intVal1);
                    list.mValue.Add((byte)((Core.Variable4)pData).intVal2);
                    list.mValue.Add((byte)((Core.Variable4)pData).intVal3);
                }
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    int outValue;
                    if (uiArgv.GetInt(out outValue))
                    {
                        if (list.mValue == null) list.mValue = new System.Collections.Generic.List<byte>(1);
                        list.mValue.Add((byte)outValue);
                    }
                }
                return true;
            }

            //int
            if (pCustomVariable is Framework.Plugin.AT.VariableInt)
            {
                if (pData is Core.Variable1) (pCustomVariable as Framework.Plugin.AT.VariableInt).mValue =((Core.Variable1)pData).intVal;
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    int outValue;
                    if (uiArgv.GetInt(out outValue)) (pCustomVariable as Framework.Plugin.AT.VariableInt).mValue = outValue;
                }
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableIntList)
            {
                Framework.Plugin.AT.VariableIntList list = pCustomVariable as Framework.Plugin.AT.VariableIntList;
                if (pData is Core.Variable2)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<int>(2);
                    list.mValue.Add(((Core.Variable2)pData).intVal0);
                    list.mValue.Add(((Core.Variable2)pData).intVal1);
                }
                else if (pData is Core.Variable3)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<int>(3);
                    list.mValue.Add(((Core.Variable3)pData).intVal0);
                    list.mValue.Add(((Core.Variable3)pData).intVal1);
                    list.mValue.Add(((Core.Variable3)pData).intVal2);
                }
                else if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<int>(4);
                    list.mValue.Add(((Core.Variable4)pData).intVal0);
                    list.mValue.Add(((Core.Variable4)pData).intVal1);
                    list.mValue.Add(((Core.Variable4)pData).intVal2);
                    list.mValue.Add(((Core.Variable4)pData).intVal3);
                }
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    int outValue;
                    if (uiArgv.GetInt(out outValue))
                    {
                        if (list.mValue == null) list.mValue = new System.Collections.Generic.List<int>(1);
                        list.mValue.Add(outValue);
                    }
                }
                return true;
            }

            //long
            if (pCustomVariable is Framework.Plugin.AT.VariableLong)
            {
                if (pData is Core.Variable2) (pCustomVariable as Framework.Plugin.AT.VariableLong).mValue = ((Core.Variable2)pData).longValue;
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableLongList)
            {
                Framework.Plugin.AT.VariableLongList list = pCustomVariable as Framework.Plugin.AT.VariableLongList;
                if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<long>(2);
                    list.mValue.Add(((Core.Variable4)pData).longValue0);
                    list.mValue.Add(((Core.Variable4)pData).longValue1);
                }
                return true;
            }

            //float
            if (pCustomVariable is Framework.Plugin.AT.VariableFloat)
            {
                if (pData is Core.Variable1) (pCustomVariable as Framework.Plugin.AT.VariableFloat).mValue = ((Core.Variable1)pData).floatVal;
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    float outValue;
                    if (uiArgv.GetFloat(out outValue)) (pCustomVariable as Framework.Plugin.AT.VariableFloat).mValue = outValue;
                }
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableFloatList)
            {
                Framework.Plugin.AT.VariableFloatList list = pCustomVariable as Framework.Plugin.AT.VariableFloatList;
                if (pData is Core.Variable2)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<float>(2);
                    list.mValue.Add(((Core.Variable2)pData).floatVal0);
                    list.mValue.Add(((Core.Variable2)pData).floatVal1);
                }
                else if (pData is Core.Variable3)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<float>(3);
                    list.mValue.Add(((Core.Variable3)pData).floatVal0);
                    list.mValue.Add(((Core.Variable3)pData).floatVal1);
                    list.mValue.Add(((Core.Variable3)pData).floatVal2);
                }
                else if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<float>(4);
                    list.mValue.Add(((Core.Variable4)pData).floatVal0);
                    list.mValue.Add(((Core.Variable4)pData).floatVal1);
                    list.mValue.Add(((Core.Variable4)pData).floatVal2);
                    list.mValue.Add(((Core.Variable4)pData).floatVal3);
                }
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    float outValue;
                    if (uiArgv.GetFloat(out outValue))
                    {
                        if (list.mValue == null) list.mValue = new System.Collections.Generic.List<float>(1);
                        list.mValue.Add(outValue);
                    }
                }
                return true;
            }

            //Vector2
            if (pCustomVariable is Framework.Plugin.AT.VariableVector2)
            {
                if (pData is Core.Variable2) (pCustomVariable as Framework.Plugin.AT.VariableVector2).mValue = ((Core.Variable2)pData).ToVector2();
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableVector2Int)
            {
                if (pData is Core.Variable2) (pCustomVariable as Framework.Plugin.AT.VariableVector2Int).mValue = ((Core.Variable2)pData).ToVector2Int();
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableVector2List)
            {
                Framework.Plugin.AT.VariableVector2List list = pCustomVariable as Framework.Plugin.AT.VariableVector2List;
                if (pData is Core.Variable4)
                {
                    if (list.mValue == null) list.mValue = new System.Collections.Generic.List<Vector2>(4);
                    list.mValue.Add(((Core.Variable4)pData).ToVector2_0());
                    list.mValue.Add(((Core.Variable4)pData).ToVector2_1());
                }
                return true;
            }
            //Vector3
            if (pCustomVariable is Framework.Plugin.AT.VariableVector3)
            {
                if (pData is Core.Variable3) (pCustomVariable as Framework.Plugin.AT.VariableVector3).mValue = ((Core.Variable3)pData).ToVector3();
                return true;
            }
            if (pCustomVariable is Framework.Plugin.AT.VariableVector3Int)
            {
                if (pData is Core.Variable3) (pCustomVariable as Framework.Plugin.AT.VariableVector3Int).mValue = ((Core.Variable3)pData).ToVector3Int();
                return true;
            }
            //Vector4
            if (pCustomVariable is Framework.Plugin.AT.VariableVector4)
            {
                if (pData is Core.Variable4) (pCustomVariable as Framework.Plugin.AT.VariableVector3).mValue = ((Core.Variable4)pData).ToVector4();
                return true;
            }
            //Quaternion
            if (pCustomVariable is Framework.Plugin.AT.VariableQuaternion)
            {
                if (pData is Core.Variable4) (pCustomVariable as Framework.Plugin.AT.VariableQuaternion).mValue = ((Core.Variable4)pData).ToQuaternion();
                return true;
            }
            //Color
            if (pCustomVariable is Framework.Plugin.AT.VariableColor)
            {
                if (pData is Core.Variable4) (pCustomVariable as Framework.Plugin.AT.VariableColor).mValue = ((Core.Variable4)pData).ToColor();
                return true;
            }
            //string
            if (pCustomVariable is Framework.Plugin.AT.VariableString)
            {
                if (pData is Core.VariableString) (pCustomVariable as Framework.Plugin.AT.VariableString).mValue = ((Core.VariableString)pData).strValue;
                else if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    string outValue;
                    if (uiArgv.GetString(out outValue)) (pCustomVariable as Framework.Plugin.AT.VariableString).mValue = outValue;
                }
                return true;
            }
            //userdata
            if (pCustomVariable is Framework.Plugin.AT.IUserData)
            {
                if (pData is TopGame.UI.UIRuntimeParamArgvs)
                {
                    TopGame.UI.UIRuntimeParamArgvs uiArgv = (TopGame.UI.UIRuntimeParamArgvs)pData;
                    Framework.Plugin.AT.IUserData outValue;
                    if (uiArgv.GetUserData(out outValue)) (pCustomVariable as Framework.Plugin.AT.VariableUser).mValue = outValue;
                }
                else if (pData is Core.VariableUserData)
                {
                    (pCustomVariable as Framework.Plugin.AT.VariableUser).mValue = ((Core.VariableUserData)pData).pUserData;
                }
                return true;
            }
            return false;
		}
	}
}
