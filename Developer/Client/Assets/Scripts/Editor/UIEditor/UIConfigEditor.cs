#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIConfigEditor
作    者:	HappLI
描    述:	UI 序列化对象容器，用于界面绑定操作对象 编辑操作
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace TopGame.UI
{
    [CustomEditor(typeof(UIConfig))]
    public class UIConfigEditor : Editor
    {
        bool m_bExpandUI = false;
        float[] inspectorTabWidth = new float[9];
        HashSet<ushort> m_vSets = new HashSet<ushort>();
        public override void OnInspectorGUI()
        {
            UIConfig uis = target as UIConfig;
            uis.uiAnimators = EditorGUILayout.ObjectField("UI动效",uis.uiAnimators, typeof(UIAnimatorAssets), false) as UIAnimatorAssets;
            uis.DefaultSpr = EditorGUILayout.ObjectField("缺省图", uis.DefaultSpr, typeof(Sprite), false) as Sprite;

            m_bExpandUI = EditorGUILayout.Toggle("UI列表",m_bExpandUI);
            Color color = GUI.color;
            m_vSets.Clear();
            if (m_bExpandUI)
            {
                if (uis.UIS != null)
                {
                    GUILayoutOption[] layoutOp = null;
                    if (inspectorTabWidth.Length == 9)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("UI枚举", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[0]) });
                        GUILayout.Label("UIID", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[1]) });
                        GUILayout.Label("资源", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[2]) });
                        GUILayout.Label("常驻", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[3]) });
                        GUILayout.Label("一直显示", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[4]) });
                        GUILayout.Label("信号栈", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[5]) });
                        GUILayout.Label("层级", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[6]) });
                        GUILayout.Label("全屏UI", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[7]) });
                        GUILayout.Label("Z轴深度", new GUILayoutOption[] { GUILayout.MaxWidth(inspectorTabWidth[8]) });
                        GUILayout.EndHorizontal();

                    }
                    for (int i = 0; i < uis.UIS.Length; ++i)
                    {
                        Object asset = AssetDatabase.LoadAssetAtPath<GameObject>(uis.UIS[i].prefab);
                        if (m_vSets.Contains(uis.UIS[i].type))
                        {
                            GUI.color = Color.red;
                        }
                        else
                        {
                            GUI.color = color;
                            m_vSets.Add(uis.UIS[i].type);
                        }

                        if (asset == null || uis.UIS[i].type == 0) GUI.color = Color.red;

                        GUILayout.BeginHorizontal();
                        //  uis.UIS[i].type = (ushort)(EUIType)Framework.ED.HandleUtilityWrapper.PopEnum("",(EUIType)uis.UIS[i].type);
                        uis.UIS[i].type = (ushort)(UI.EUIType)EditorGUILayout.EnumPopup((UI.EUIType)uis.UIS[i].type);
                        if (i == 0) inspectorTabWidth[0] = GUILayoutUtility.GetLastRect().width;
                        uis.UIS[i].type = (ushort)EditorGUILayout.IntField(uis.UIS[i].type, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[1] = GUILayoutUtility.GetLastRect().width;
                        asset = EditorGUILayout.ObjectField(asset, typeof(GameObject), false) as GameObject;
                        if (i == 0) inspectorTabWidth[2] = GUILayoutUtility.GetLastRect().width;
                        if (asset != null)
                        {
                            uis.UIS[i].prefab = AssetDatabase.GetAssetPath(asset);
                        }
                        uis.UIS[i].permanent = EditorGUILayout.Toggle("", uis.UIS[i].permanent, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[3] = GUILayoutUtility.GetLastRect().width;

                        uis.UIS[i].alwayShow = EditorGUILayout.Toggle("", uis.UIS[i].alwayShow, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[4] = GUILayoutUtility.GetLastRect().width;

                        uis.UIS[i].trackAble = EditorGUILayout.Toggle(uis.UIS[i].trackAble, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[5] = GUILayoutUtility.GetLastRect().width;

                        uis.UIS[i].Order = EditorGUILayout.IntField(uis.UIS[i].Order, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[6] = GUILayoutUtility.GetLastRect().width;

                        uis.UIS[i].fullUI = EditorGUILayout.Toggle("", uis.UIS[i].fullUI, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[7] = GUILayoutUtility.GetLastRect().width;

                        uis.UIS[i].uiZValue = EditorGUILayout.IntField(uis.UIS[i].uiZValue, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[8] = GUILayoutUtility.GetLastRect().width;
                        
                        uis.UIS[i].showLog = EditorGUILayout.Toggle(uis.UIS[i].showLog, new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                        if (i == 0) inspectorTabWidth[9] = GUILayoutUtility.GetLastRect().width;

                        if (GUILayout.Button("删除", layoutOp))
                        {
                            List<UIConfig.UI> vData = new List<UIConfig.UI>(uis.UIS);
                            vData.RemoveAt(i);
                            uis.UIS = vData.ToArray();
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            
            GUI.color = color;
            if (GUILayout.Button("新建"))
            {
                List<UIConfig.UI> vData = new List<UIConfig.UI>(uis.UIS);
                vData.Add(new UIConfig.UI() { type = 0, trackAble = true });
                uis.UIS = vData.ToArray();
            }
            if (GUILayout.Button("打开编辑器"))
            {
                UIEditorConfigEditor.Open(uis);
            }
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

            GUI.color = color;
        }
    }
}
#endif