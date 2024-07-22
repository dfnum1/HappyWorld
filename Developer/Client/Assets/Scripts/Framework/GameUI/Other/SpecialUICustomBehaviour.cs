/********************************************************************
生成日期:	6:11:2020 16:27
类    名: 	SpecialUICustom
作    者:	zdq
描    述:	自定义特殊UI行为
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class SpecialUICustomBehaviour : MonoBehaviour
    {
        [System.Serializable]
        public class CustomUIDate
        {
            [Header("UI组件")]
            public Component ui;
            [Header("操作类型")]
            public CustomOperateType customOperateType;
            [Header("激活状态/灰态")]
            public bool active;
            [Header("颜色")]
            public Color color;
            [Header("锚点坐标")]
            public Vector3 anchorPosition;
        }

        public enum CustomOperateType
        {
            Active = 1,
            Color = 2,
            AnchorPosition = 3,
            Gray=4
        }


        public List<CustomUIDate> vNormalUI;
        //[Space(50)]
        public List<CustomUIDate> vSpecialUI;

        public List<CustomUIDate> vSpecialUI2;

        //------------------------------------------------------
        void SetUIData(CustomUIDate data)
        {
            if (data.ui == null) return;
            switch (data.customOperateType)
            {
                case CustomOperateType.Active:
                    data.ui.gameObject.SetActive(data.active);
                    break;
                case CustomOperateType.Color:
                    Graphic graphic = data.ui as Graphic;
                    if (graphic)
                    {
                        graphic.color = data.color;
                    }
                    break;
                case CustomOperateType.AnchorPosition:
                    RectTransform rect = data.ui as RectTransform;
                    if (rect)
                    {
                        rect.anchoredPosition = data.anchorPosition;
                    }
                    break;
                case CustomOperateType.Gray:
                    Transform transform = data.ui.transform;
                    if (transform)
                    {
                        Material mat = TopGame.Data.GlobalDefaultResources.UIGrayMat;
                        Graphic[] childs = transform.GetComponentsInChildren<Graphic>(true);
                        for (int i = 0; i < childs.Length; i++)
                        {
                            childs[i].material = data.active ? mat : null;
                            //childs[i].color = data.color;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        public void OnNormal()
        {
#if UNITY_EDITOR
            List<Object> list = new List<Object>();
            foreach (var item in vNormalUI)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnNormal");
#endif
            CustomUIDate data;
            for (int i = 0; i < vNormalUI.Count; i++)
            {
                data = vNormalUI[i];
                SetUIData(data);
            }
        }
        //------------------------------------------------------
        public void OnSpecial()
        {
#if UNITY_EDITOR
            List<Object> list = new List<Object>();
            foreach (var item in vSpecialUI)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnSpecial");
#endif

            CustomUIDate data;
            for (int i = 0; i < vSpecialUI.Count; i++)
            {
                data = vSpecialUI[i];
                SetUIData(data);
            }
        }
        //------------------------------------------------------
        public void OnSpecial2()
        {
#if UNITY_EDITOR
            List<Object> list = new List<Object>();
            foreach (var item in vSpecialUI2)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnSpecial2");
#endif

            CustomUIDate data;
            for (int i = 0; i < vSpecialUI2.Count; i++)
            {
                data = vSpecialUI2[i];
                SetUIData(data);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SpecialUICustomBehaviour), true)]
    //[CanEditMultipleObjects]
    public class SpecialUICustomBehaviourEditor : Editor
    {
        SpecialUICustomBehaviour m_Targer;

        private SerializedProperty m_vNormalUI;
        private SerializedProperty m_vSpecialUI;
        private SerializedProperty m_vSpecialUI2;

        string m_text1= "正常";
        string m_text2 = "特殊";
        string m_text3 = "无";

        //------------------------------------------------------
        void OnEnable()
        {
            m_Targer = target as SpecialUICustomBehaviour;
            m_vNormalUI = serializedObject.FindProperty("vNormalUI");
            m_vSpecialUI = serializedObject.FindProperty("vSpecialUI");
            m_vSpecialUI2 = serializedObject.FindProperty("vSpecialUI2");

        }
        //------------------------------------------------------
        void OnDisable()
        {
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            m_text1 = EditorGUILayout.TextField(m_text1);
            EditorGUILayout.PropertyField(m_vNormalUI);

            m_text2 = EditorGUILayout.TextField(m_text2);
            EditorGUILayout.PropertyField(m_vSpecialUI);

            m_text3 = EditorGUILayout.TextField(m_text3);
            EditorGUILayout.PropertyField(m_vSpecialUI2);

            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();

            if (GUILayout.Button("OnNormal"))
            {
                if (m_Targer != null)
                {
                    m_Targer.OnNormal();
                }
            }
            if (GUILayout.Button("OnSpecial"))
            {
                if (m_Targer != null)
                {
                    m_Targer.OnSpecial();
                }
            }
            if (GUILayout.Button("OnSpecial2"))
            {
                if (m_Targer != null)
                {
                    m_Targer.OnSpecial2();
                }
            }

            //if (GUILayout.Button("刷新保存"))
            //{
            //    EditorUtility.SetDirty(target);
            //    AssetDatabase.SaveAssets();
            //    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            //}
        }
    }
    ////------------------------------------------------------
    //[CustomEditor(typeof(SpecialUICustomBehaviour.CustomUIDate), true)]
    //public class CustomUIDateEditor : Editor
    //{
    //    private SerializedProperty m_vNormalUI;
    //    SpecialUICustomBehaviour.CustomUIDate m_Targer;

    //    //------------------------------------------------------
    //    void OnEnable()
    //    {
    //        m_Targer = target as SpecialUICustomBehaviour.CustomUIDate;
    //        m_vNormalUI = serializedObject.FindProperty("vNormalUI");

    //    }
    //    //------------------------------------------------------
    //    void OnDisable()
    //    {
    //    }
    //    //------------------------------------------------------
    //    public override void OnInspectorGUI()
    //    {
    //        serializedObject.Update();

    //        //EditorGUILayout.PropertyField(m_vNormalUI);

    //        serializedObject.ApplyModifiedProperties();
    //        base.OnInspectorGUI();

    //        if (GUILayout.Button("OnNormal"))
    //        {
    //            if (m_Targer != null)
    //            {
    //                m_Targer.OnNormal();
    //            }
    //        }
    //        if (GUILayout.Button("OnSpecial"))
    //        {
    //            if (m_Targer != null)
    //            {
    //                m_Targer.OnSpecial();
    //            }
    //        }
    //    }
    //}
#endif
}
