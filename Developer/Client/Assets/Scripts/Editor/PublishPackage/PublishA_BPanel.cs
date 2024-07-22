/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PublishA_BPanel
作    者:	HappLI
描    述:	A/B发布包体
*********************************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace TopGame.ED
{
    public class PublishA_BPanel
    {
        PublishPanel.PublishSetting m_Setting;
        List<string> m_vA_BPop = new List<string>();
        PublishPanel.A_BConfig m_ABSelect = null;
        string m_selectName = null;
        Vector2 m_Scroll;
        //------------------------------------------------------
        public void OnDisable()
        {
        }
        //------------------------------------------------------
        public void OnEnable(PublishPanel.PublishSetting setting)
        {
            Refresh(setting);
        }
        //------------------------------------------------------
        public void Refresh(PublishPanel.PublishSetting setting)
        {
            m_Setting = setting;
            RefreshPop();
        }
        //------------------------------------------------------
        void RefreshPop()
        {
            m_vA_BPop.Clear();
            m_vA_BPop.Add("None");
            for (int i = 0; i < m_Setting.A_BAssets.Count; ++i)
            {
                m_vA_BPop.Add(m_Setting.A_BAssets[i].name);
            }
        }
        //------------------------------------------------------
        public void OnGUI(Rect position)
        {
            if (m_Setting == null) return;
            float layoutWidth = position.width - 20;
            
            int sel = 0;
            if(m_ABSelect!=null)
                sel = m_vA_BPop.IndexOf(m_selectName);
            int preSel = sel;
            GUILayout.BeginHorizontal();
            sel = EditorGUILayout.Popup("分体名", sel, m_vA_BPop.ToArray());
            if(GUILayout.Button("新建"))
            {
                PublishPanel.A_BConfig newA_B = new PublishPanel.A_BConfig();
                newA_B.name = "Please ReName";
                if(!m_vA_BPop.Contains(newA_B.name))
                {
                    m_selectName = newA_B.name;
                    sel = m_Setting.A_BAssets.Count;
                    m_Setting.A_BAssets.Add(newA_B);
                    m_ABSelect = newA_B;
                    RefreshPop();
                }
            }
            if(GUILayout.Button("移除"))
            {
                if (sel >= 0 && sel < m_Setting.A_BAssets.Count)
                {
                    m_Setting.A_BAssets.RemoveAt(sel);
                    m_selectName = "None";
                    m_ABSelect = null;
                    RefreshPop();
                }
            }
            GUILayout.EndHorizontal();
            if (sel!= preSel)
            {
                sel--;
                if (sel >= 0 && sel < m_Setting.A_BAssets.Count)
                {
                    m_selectName = m_Setting.A_BAssets[sel].name;
                    m_ABSelect = m_Setting.A_BAssets[sel];
                }
                else
                {
                    m_selectName = "None";
                    m_ABSelect = null;
                }
            }

            if (m_ABSelect == null) return;

            GUILayout.BeginHorizontal();
            m_selectName = EditorGUILayout.TextField("分体包名", m_selectName);
            if (!string.IsNullOrEmpty(m_selectName) && GUILayout.Button("应用"))
            {
                m_ABSelect.name = m_selectName;
                RefreshPop();
            }
            GUILayout.EndHorizontal();

            GUILayoutOption[] titleOp = new GUILayoutOption[] { GUILayout.Width((layoutWidth-30)/2) };
            GUILayoutOption[] ctlOp = new GUILayoutOption[] { GUILayout.Width(40) };
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("资源", titleOp);
            EditorGUILayout.LabelField("分体映射", titleOp);
            EditorGUILayout.LabelField("", ctlOp);
            GUILayout.EndHorizontal();
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            for (int i = 0; i < m_ABSelect.mapping.Count; ++i)
            {
                PublishPanel.A_BConfig.ABMap ab = m_ABSelect.mapping[i];
                GUILayout.BeginHorizontal();
                ab.srcFile = EditorGUILayout.TextField(ab.srcFile, titleOp);
                ab.strMapTo = EditorGUILayout.TextField(ab.strMapTo, titleOp);
                if(GUILayout.Button("移除", ctlOp))
                {
                    m_ABSelect.mapping.RemoveAt(i);
                    break;
                }
                m_ABSelect.mapping[i] = ab;
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("添加"))
            {
                m_ABSelect.mapping.Add(new PublishPanel.A_BConfig.ABMap());
            }
        }
    }
}
