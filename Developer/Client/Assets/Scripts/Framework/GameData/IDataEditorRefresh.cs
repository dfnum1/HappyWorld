#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	IDataEditorRefresh
作    者:	HappLI
描    述:	数据编辑刷新
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.IO;
using Framework.Base;
using System.Reflection;
using System;

namespace TopGame.ED
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataEditorAttribute : System.Attribute
    {
        public System.Type dataType;
        public DataEditorAttribute(System.Type dataType)
        {
            this.dataType = dataType;
        }
    }
    //------------------------------------------------------
    public interface IDataEditorRefresh
    {
        void OnDityRefresh(System.Object pData);
    }
    //------------------------------------------------------
    public class IDataEditorUti
    {
        class InstDataRefresh
        {
            public System.Type dataClassType;
            public IDataEditorRefresh pInstance;
        }
        private static Dictionary<System.Type, InstDataRefresh> ms_DataRefrehMethods = new Dictionary<System.Type, InstDataRefresh>();
        public static void RefreshData(System.Object pData)
        {
            if (ms_DataRefrehMethods == null || ms_DataRefrehMethods.Count <= 0)
            {
                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    System.Type[] types = ass.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        System.Type tp = types[i];
                        if (tp.IsDefined(typeof(DataEditorAttribute), false))
                        {
                            DataEditorAttribute attr = tp.GetCustomAttribute<DataEditorAttribute>();
                            if (attr.dataType != null)
                            {
                                ms_DataRefrehMethods[attr.dataType] = new InstDataRefresh() { dataClassType = tp, pInstance = null };
                            }
                        }
                    }
                }
            }

            InstDataRefresh refreshIns = null;
            if(ms_DataRefrehMethods.TryGetValue(pData.GetType(), out refreshIns))
            {
                if(refreshIns.pInstance == null)
                {
                    refreshIns.pInstance = (IDataEditorRefresh)System.Activator.CreateInstance(refreshIns.dataClassType);
                }
                refreshIns.pInstance.OnDityRefresh(pData);
                return;
            }
        }
    }
}
#endif