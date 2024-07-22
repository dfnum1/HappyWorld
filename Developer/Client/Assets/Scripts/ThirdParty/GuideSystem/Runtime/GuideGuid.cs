/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GuideGuid
作    者:	HappLI
描    述:	引导GUID 生成器
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
namespace Framework.Plugin.Guide
{
    public class GuideGuidUtl
    {
        public static bool bGuideGuidLog;
        static Dictionary<int, GuideGuid> ms_vGuids = new Dictionary<int, GuideGuid>(128);
        public static void OnAdd(GuideGuid guid)
        {
            if (guid == null || guid.Guid == 0) return;
#if UNITY_EDITOR
            GuideGuid src;
            if (ms_vGuids.TryGetValue(guid.Guid, out src) && src != guid && !guid.name.Contains("Clone") && src.name != guid.name)
            {
                UnityEditor.EditorUtility.DisplayDialog("警告", src.name + "  和 " + guid.name + " 重GUID" + guid.Guid, "检查");
            }
#endif
            ms_vGuids[guid.Guid] = guid;
            if (bGuideGuidLog)
            {
                Log("添加guid:" + guid.Guid + ",组件:" + guid.name);
            }
        }
        public static void OnRemove(GuideGuid guid)
        {
            if (guid == null || guid.Guid == 0) return;

            GuideGuid oldGuid = FindGuide(guid.Guid);
            if(oldGuid == guid)
            {
                ms_vGuids.Remove(guid.Guid);
                if (bGuideGuidLog)
                {
                    Log("移除guid:" + guid.Guid + ",组件:" + guid.name);
                }
            }
        }
        public static GuideGuid FindGuide(int guid)
        {
            if (guid == 0) return null;
            GuideGuid guide;
            if (ms_vGuids.TryGetValue(guid, out guide))
                return guide;
            return null;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public static void PrintAllGuideGuid()
        {
            foreach (var item in ms_vGuids)
            {
                if (item.Value != null)
                {
                    Log("guid:" + item.Value.Guid + ",组件:" + item.Value.name);
                }
                else
                {
                    Log("guid:" + item.Key + ",组件:null");
                }
            }
        }
#endif
        //------------------------------------------------------
        public static void Log(string log)
        {
            if (bGuideGuidLog)
            {
                Debug.LogWarning("GuideLog: " + log);
            }
        }
    }
    [DisallowMultipleComponent]
    public class GuideGuid : MonoBehaviour 
	{
        public int Guid = 0;
        public bool ConvertUIPos = false;
        public void Awake()
        {
            if (Guid != 0 )
                GuideGuidUtl.OnAdd(this);
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            if (Guid != 0)
                GuideGuidUtl.OnAdd(this);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            if (Guid != 0)
                GuideGuidUtl.OnRemove(this);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
//             if (Guid != 0)
//                 GuideGuidUtl.OnRemove(this);
            //      if (Guid != 0 && OnRemoveGuideGuid != null)
            //         OnRemoveGuideGuid(this);
        }
    }
}
