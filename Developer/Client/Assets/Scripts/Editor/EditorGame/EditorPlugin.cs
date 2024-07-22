/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	EditorModule
作    者:	HappLI
描    述:	编辑器主模块
*********************************************************************/
using Framework.Core;
using Framework.Plugin.AT;
using System;
using TopGame.Data;
using UnityEngine;

namespace TopGame.ED
{
    [Framework.Plugin.PluginGame]
    public class EditorPlugin
    {
        public static void StartUp(System.Object pPointer = null, string tablName = null)
        {
            if (Application.isPlaying)
                DataManager.StartInit();
            else
            {
                if (FileSystemUtil.GetFileSystem() != null) FileSystemUtil.GetFileSystem().Shutdown();
                DataManager.ReInit();
            }

            if (pPointer != null && !string.IsNullOrEmpty(tablName))
            {
                System.Reflection.MethodInfo method = pPointer.GetType().GetMethod("SetDataPointer", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (method != null)
                {
                    System.Reflection.PropertyInfo field = DataManager.getInstance().GetType().GetProperty(tablName);
                    if (field != null)
                        method.Invoke(pPointer, new object[] { field.GetValue(DataManager.getInstance()) });
                }
            }
        }
        //------------------------------------------------------
        public static IUserData OnBuildEvent(string strEvent)
        {
            if (string.IsNullOrEmpty(strEvent)) return null;
            return BaseEventParameter.NewEvent(null, strEvent);
        }
        //------------------------------------------------------
        public static string SaveEvent(IUserData pEvent)
        {
            if (pEvent == null || !(pEvent is BaseEventParameter)) return null;
            return (pEvent as BaseEventParameter).WriteCmd();
        }
        //------------------------------------------------------
        public static void SaveTable(string strTable)
        {
            if (!DataManager.getInstance().bInited) return;
            System.Reflection.PropertyInfo field = DataManager.getInstance().GetType().GetProperty(strTable);
            if (field != null)
            {
                var table = field.GetValue(DataManager.getInstance());
                if (table == null) return;
                System.Reflection.MethodInfo save = table.GetType().GetMethod("Save");
                if (save == null) return;
                save.Invoke(table, new object[] { null });
            }
        }
    }
}
