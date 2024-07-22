using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

namespace TopGame.ED
{
    public class DynamicType
    {
        private readonly Assembly _assembly;

        public DynamicType(Type type)
        {
            _assembly = type.Assembly;
        }

        public ProfilerDllLinker GetType(string typeName)
        {
            return new ProfilerDllLinker(_assembly.GetType(typeName));
        }
    }
    //------------------------------------------------------
    public static class ProfilerHelper
    {
        private static List<ProfilerDllLinker> _windows = null;
        //------------------------------------------------------
        private static ProfilerDllLinker _GetWindow(ProfilerArea area)
        {
            if (null == _windows || _windows.Count<=0)
            {
                var dynamicType = new DynamicType(typeof(EditorWindow));
                var type = dynamicType.GetType("UnityEditor.ProfilerWindow");
                var list = type.PrivateStaticField<IList>("m_ProfilerWindows");
                _windows = new List<ProfilerDllLinker>();
                foreach (var window in list)
                {
                    _windows.Add(new ProfilerDllLinker(window));
                }
            }
            foreach (var dynamic in _windows)
            {
                var val = (ProfilerArea)dynamic.PrivateInstanceField("m_CurrentArea");
                if (val == area)
                {
                    return dynamic;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public static ProfilerMemoryElement GetMemoryDetailRoot(int filterDepth, float filterSize)
        {
//             UnityEditorInternal.ProfilerDriver.SetAreaEnabled(UnityEngine.Profiling.ProfilerArea.Memory, true);
//             UnityEditorInternal.ProfilerDriver.RequestObjectMemoryInfo(true);
            var windowDynamic = _GetWindow(ProfilerArea.Memory);
            if (windowDynamic == null) return null;

            ProfilerDllLinker listViewDynamic = null;
            object memoryListView= windowDynamic.PrivateInstanceField("m_MemoryListView");
            object detailView = null;
            if (memoryListView == null)
            {
                object MemoryProfilerModule = null;
                Array list = windowDynamic.PrivateInstanceField<Array>("m_ProfilerModules");
                foreach(var db in list)
                {
                    if(db.GetType().Name.Contains("MemoryProfilerModule"))
                    {
                        MemoryProfilerModule = db;
                        break;
                    }
                }

                if(MemoryProfilerModule!=null)
                {
                    ProfilerDllLinker profilerModule = new ProfilerDllLinker(MemoryProfilerModule);
                    profilerModule.CallPrivateInstanceMethod("RefreshMemoryData");
                    memoryListView = profilerModule.PrivateInstanceField("m_ReferenceListView");
                }
            }
            if (memoryListView == null) return null;
            if (memoryListView != null)
            {
                listViewDynamic = new ProfilerDllLinker(memoryListView);
                detailView = listViewDynamic.PrivateInstanceField("m_DetailView");
            }
            var rootDynamic = listViewDynamic.PrivateInstanceField("m_Root");

            if(rootDynamic == null)
            {
                if(detailView != null)
                {
                    listViewDynamic = new ProfilerDllLinker(detailView);
                    rootDynamic = listViewDynamic.PrivateInstanceField("m_Root");
                }
            }
            if(rootDynamic == null)
            {
                rootDynamic = listViewDynamic.PrivateInstanceField("m_MemorySelection");
                if(rootDynamic!=null)
                {
                    ProfilerDllLinker select = new ProfilerDllLinker(rootDynamic);
                    rootDynamic = select.PrivateInstanceField("m_Selected");
                }
            }
            return rootDynamic != null ? ProfilerMemoryElement.Create(new ProfilerDllLinker(rootDynamic), 0, filterDepth, filterSize) : null;
        }
        //------------------------------------------------------
        public static void WriteMemoryDetail(StreamWriter writer, ProfilerMemoryElement root)
        {
            if (null == root) return;
            writer.WriteLine(root.ToString());
            foreach (var memoryElement in root.children)
            {
                if (null != memoryElement)
                {
                    WriteMemoryDetail(writer, memoryElement);
                }
            }
        }
        //------------------------------------------------------
        public static void WriteReferenceDetail(XmlDocument rootXml, XmlElement parent, ProfilerMemoryElement root)
        {
            if (null == root) return;
            int cnt = root.GetReferenceCnt();
            if(cnt>1)
            {
//                 string value = root.WriteHasRefPath();
//                 if (value.Length > 0 && !ProfilerWindowEx.Instance.vChecked.Contains(value))
//                 {
//                     XmlElement ele_node = rootXml.CreateElement("item");
// 
//                     ele_node.SetAttribute("Path", value);
//                     ele_node.SetAttribute("RefCount", cnt.ToString());
// 
//                     parent.AppendChild(ele_node);
//                     ProfilerWindowEx.Instance.vChecked.Add(value);
//                 }
            }

            foreach (var memoryElement in root.children)
            {
                if (null != memoryElement)
                {
                    WriteReferenceDetail(rootXml, parent, memoryElement);
                }
            }
        }
        //------------------------------------------------------
        public static void WriteReferenceDetail(StreamWriter writer, ProfilerMemoryElement root)
        {
            if (null == root) return;
            writer.WriteLine(root.WriteHasRefPath());
            foreach (var memoryElement in root.children)
            {
                if (null != memoryElement)
                {
                    WriteReferenceDetail(writer, memoryElement);
                }
            }
        }
        //------------------------------------------------------
        public static void RefreshMemoryData()
        {

            var dynamic = _GetWindow(ProfilerArea.Memory);
            if (null == dynamic)
            {
                EditorUtility.DisplayDialog("tips", "请打开Profiler 窗口的 Memory 视图", "ok");
            }
        }
    }
}
