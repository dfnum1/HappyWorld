using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.UI;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class UIRecord : MonoBehaviour
    {
        [Serializable]
        public class RecordData
        {
            public RectTransform ui;
            [Header("锚点位置")]
            public Vector2 anchorPos;
            [Header("锚点")]
            public Vector4 anchors;
            [Header("大小")]
            public Vector2 size;
            [Header("缩放")]
            public Vector3 scale;

            public RecordData(RectTransform rectTransform)
            {
                this.ui = rectTransform ?? throw new ArgumentNullException(nameof(ui));
                UpdateData();
            }
            //------------------------------------------------------
            /// <summary>
            /// 将记录得数据同步到当前UI上
            /// </summary>
            public void SynData()
            {
                if (ui == null)
                {
                    Debug.LogError("ui is  null!");
                    return;
                }

                ui.anchoredPosition= anchorPos;
                ui.anchorMin = new Vector2(anchors.x, anchors.y);
                ui.anchorMax = new Vector2(anchors.z, anchors.w);
                ui.sizeDelta = size;
                ui.localScale= scale;
            }
            //------------------------------------------------------
            /// <summary>
            /// 将当前UI坐标更新到记录数据中
            /// </summary>
            public void UpdateData()
            {
                if (ui == null)
                {
                    Debug.LogError("ui is  null!");
                    return;
                }

                anchorPos = ui.anchoredPosition;
                anchors = new Vector4(ui.anchorMin.x, ui.anchorMin.y, ui.anchorMax.x, ui.anchorMax.y);
                this.size = ui.sizeDelta;
                this.scale = ui.localScale;
            }
            //------------------------------------------------------

            public override bool Equals(object obj)
            {
                //规则:UI,锚点位置,锚点,缩放,大小都相同,则相同
                if (obj is RecordData)
                {
                    RecordData other = (RecordData)obj;

                    if (ui && other.ui && ui.GetInstanceID() != other.ui.GetInstanceID())//不是同一个UI不进行比较
                    {
                        return false;
                    }

                    if (anchorPos != other.anchorPos || anchors != other.anchors || size != other.size || scale != other.scale)
                    {
                        return false;
                    }
                    return true;
                }
                return base.Equals(obj);
            }
            //------------------------------------------------------
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        [NonReorderable]
        /// <summary>
        /// 横屏ui数据
        /// </summary>
        public List<RecordData> vDataLandscape = new List<RecordData>();
        [NonReorderable]
        /// <summary>
        /// 竖屏ui数据
        /// </summary>
        public List<RecordData> vDataPortrait = new List<RecordData>();

        /// <summary>
        /// 记录横屏数据
        /// </summary>
        public void RecordLandscapeData()
        {
            //遍历所有子物体,记录坐标,
            vDataLandscape.Clear();
            var uis = transform.GetComponentsInChildren<RectTransform>(true);
            for (int i = 0; i < uis.Length; i++)
            {
                vDataLandscape.Add(new RecordData(uis[i]));
            }

            Debug.Log($"记录了横屏UI {vDataLandscape.Count} 个");
        }
        //------------------------------------------------------
        /// <summary>
        /// 记录竖屏数据
        /// </summary>
        public void RecordPortraitData()
        {
            vDataPortrait.Clear();
            var uis = transform.GetComponentsInChildren<RectTransform>(true);
            for (int i = 0; i < uis.Length; i++)
            {
                vDataPortrait.Add(new RecordData(uis[i]));
            }
            Debug.Log($"记录了竖屏UI {vDataPortrait.Count} 个");
        }
        //------------------------------------------------------
        /// <summary>
        /// 比较数据,剔除掉未改变得ui
        /// </summary>
        public void CompareData()
        {
            //确保两个记录得数组长度一样长
            if (vDataLandscape.Count == vDataPortrait.Count)
            {
                for (int i = vDataLandscape.Count-1; i >= 0; i--)
                {
                    var landscape = vDataLandscape[i];
                    var portrait = vDataPortrait[i];
                    if (landscape.Equals(portrait))
                    {
                        vDataLandscape.Remove(landscape);
                        vDataPortrait.Remove(portrait);
                    }
                }
                Debug.Log($"比较后,剩余修改得UI {vDataPortrait.Count} 个");
            }
            else
            {
                Debug.LogError("数组长度不一样长!请检查数据");
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 横屏
        /// </summary>
        public void OnLandscape()
        {
#if UNITY_EDITOR
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            foreach (var item in vDataLandscape)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnLandscape");
#endif

            //设置数据
            for (int i = 0; i < vDataLandscape.Count; i++)
            {
                vDataLandscape[i].SynData();
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 竖屏数据
        /// </summary>
        public void OnPortrait()
        {
#if UNITY_EDITOR
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            foreach (var item in vDataPortrait)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnPortrait");
#endif

            for (int i = 0; i < vDataPortrait.Count; i++)
            {
                vDataPortrait[i].SynData();
            }
        }

#if UNITY_EDITOR
        //------------------------------------------------------
        public void OnExportUI()
        {
            //获取当前屏幕类型
            var canvasScaler =  FindObjectOfType<CanvasScaler>();
            if (!canvasScaler)
            {
                Debug.LogError("获取不到Canvas Scaler 无法判断横竖屏,不执行适配UI坐标更新");
                return;
            }

            if (canvasScaler.referenceResolution.x < canvasScaler.referenceResolution.y)//竖屏
            {
                for (int i = 0; i < vDataPortrait.Count; i++)
                {
                    vDataPortrait[i].UpdateData();
                }
            }
            else//横屏
            {
                for (int i = 0; i < vDataLandscape.Count; i++)
                {
                    vDataLandscape[i].UpdateData();
                }
            }
        }
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIRecord), true)]
//[CanEditMultipleObjects]
public class UIRecordEditor : Editor
{
    UIRecord m_Targer;

    private SerializedProperty m_vNormalUI;
    private SerializedProperty m_vSpecialUI;

    string m_text1 = "横屏";
    string m_text2 = "竖屏";

    //------------------------------------------------------
    void OnEnable()
    {
        m_Targer = target as UIRecord;
        m_vNormalUI = serializedObject.FindProperty("vDataLandscape");
        m_vSpecialUI = serializedObject.FindProperty("vDataPortrait");

    }
    //------------------------------------------------------
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PrefixLabel(m_text1);
        EditorGUILayout.PropertyField(m_vNormalUI);

        EditorGUILayout.PrefixLabel(m_text2);
        EditorGUILayout.PropertyField(m_vSpecialUI);


        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();

        if (GUILayout.Button("记录横屏数据"))
        {
            if (m_Targer != null)
            {
                m_Targer.RecordLandscapeData();
            }
        }
        if (GUILayout.Button("记录竖屏数据"))
        {
            if (m_Targer != null)
            {
                m_Targer.RecordPortraitData();
            }
        }
        if (GUILayout.Button("比较数据"))
        {
            if (m_Targer != null)
            {
                m_Targer.CompareData();
            }
        }

        if (GUILayout.Button("横屏预览"))
        {
            if (m_Targer != null)
            {
                m_Targer.OnLandscape();
            }
        }
        if (GUILayout.Button("竖屏预览"))
        {
            if (m_Targer != null)
            {
                m_Targer.OnPortrait();
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
#endif