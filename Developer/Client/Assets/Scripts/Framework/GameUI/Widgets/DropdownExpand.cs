/********************************************************************
生成日期:	1:29:2021 10:06
类    名: 	DropdownExpand
作    者:	zdq
描    述:	扩展 Dropdown 组件,添加禁止点击功能
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.UI;
#endif

namespace TopGame.UI
{
    public class DropdownExpand : Dropdown
    {

        [SerializeField]
        [HideInInspector]
        private List<int> m_DefaultDisables = new List<int>();
        public List<int> DefaultDisables { get { return m_DefaultDisables; } }

        private List<Toggle> m_ItemToggles = new List<Toggle>();

        public Toggle GetItemToggle(int index)
        {
            if (index < 0 || index >= m_ItemToggles.Count)
            {
                return null;
            }

            return m_ItemToggles[index];
        }

        protected override void Awake()
        {
            base.Awake();

            if (template == null)
            {
                Transform tra = transform.Find("Template");
                if (tra != null) template = tra as RectTransform;
            }

            if (captionText == null)
            {
                Transform tra = transform.Find("Label");
                if (tra)
                {
                    captionText = tra.GetComponent<Text>();
                }
            }

            if (itemText == null)
            {
                Transform tra = transform.Find("Template/Viewport/Content/Item/Item Label");
                if (tra)
                {
                    itemText = tra.GetComponent<Text>();
                }
            }
        }

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            DropdownItem item = base.CreateItem(itemTemplate);

            //处理交互
            int index = m_ItemToggles.Count;
            m_ItemToggles.Add(item.toggle);

            for (int i = 0; i < m_DefaultDisables.Count; i++)
            {
                if (index == m_DefaultDisables[i])
                {
                    item.toggle.interactable = false;//处理无法点击
                    break;
                }
            }

            bool isSelect = value == index;

            //处理背景
            Transform bg = item.transform.Find("Item Background");
            if (bg)
            {
                bg.gameObject.SetActive(isSelect);
            }

            if (item.text)
            {
                if (item.toggle.interactable == false)
                {
                    item.text.color = new Color(0.6f, 0.6f, 0.6f);
                }
                else
                {
                    item.text.color = isSelect ? new Color(0.93f, 0.97f, 0.99f) : new Color(0.65f, 0.86f, 0.98f);
                }

            }

            return item;
        }
        //------------------------------------------------------
        protected override void DestroyItem(DropdownItem item)
        {
            if (m_ItemToggles.Contains(item.toggle))
            {
                m_ItemToggles.Remove(item.toggle);
            }
            base.DestroyItem(item);
        }
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DropdownExpand),true)]
    public class DropdownExpandEditor:DropdownEditor
    {
        private ReorderableList m_List;

        public DropdownExpand drop
        {
            get
            {
                return target as DropdownExpand;
            }
        }
        //------------------------------------------------------
        protected override void OnEnable()
        {
            base.OnEnable();

            ReCreateList();
        }
        //------------------------------------------------------
        private void ReCreateList()
        {
            m_List = new ReorderableList(serializedObject, serializedObject.FindProperty("m_DefaultDisables"));

            //头部描述
            m_List.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect,"默认禁用项");
            };
            //绘制列表
            m_List.drawElementCallback = (Rect rect, int index, bool active, bool isFocused) =>
            {
                SerializedProperty property = m_List.serializedProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, property,new GUIContent("Index:" + index));
                property.intValue = Mathf.Max(property.intValue, 0);
            };
            //设置添加回调
            m_List.onAddCallback = (list) =>
            {
                list.serializedProperty.arraySize++;
                list.index = list.serializedProperty.arraySize - 1;
            };
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            m_List.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
#endif
}
