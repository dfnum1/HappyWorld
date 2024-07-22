/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	CustomLayoutLocation
作    者:	HappLI
描    述:	根据数量自定义位置
*********************************************************************/

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [UIWidgetExport]
    public class CustomLayoutLocation : MonoBehaviour
    {
        [System.Serializable]
        public class Custom
        {
            public int gridCount;
            public byte[] indexs;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool bExpand;
#endif
        }
        public Custom[] customs;
        [SerializeField]
        Component[] components;
        //-------------------------------------------------
        public T GetWidget<T>(int index) where T : Component
        {
            if (components == null || index < 0 || index >= components.Length) return null;
            return components[index] as T;
        }
        //-------------------------------------------------
        public byte[] SetCustom(int customCnt)
        {
            if (customs == null || components == null) return null;
            byte[] returnComps = null;
            if(customCnt>0)
            {
                for (int i = 0; i < customs.Length; ++i)
                {
                    if (customs[i].gridCount == customCnt)
                    {
                        returnComps = customs[i].indexs;
                        break;
                    }
                }
            }

            int activeFlag = 0;
            if(returnComps!=null)
            {
                int index = -1;
                for(int i =0; i < returnComps.Length; ++i)
                {
                    index = returnComps[i];
                    if (index >= 0 && index < components.Length && components[index])
                    {
                        activeFlag |= 1 << index;
                        if (components[index] is UISerialized)
                        {
                            UISerialized ui = components[index] as UISerialized;
                            ui.Visible();
                        }
                        else
                        {
                            components[index].gameObject.SetActive(true);
                        }
                    }
                }
            }

            for (int i = 0; i < components.Length; ++i)
            {
                if ( (activeFlag & (1 << i))!= 0) continue;
                if (components[i] != null)
                {
                    if (components[i] is UI.UISerialized) (components[i] as UISerialized).Hidden();
                    else components[i].gameObject.SetActive(false);
                }
            }
            return returnComps;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CustomLayoutLocation), true)]
    [CanEditMultipleObjects]
    public class CustomLayoutLocationEditor : Editor
    {
        bool m_bExpand = false;
        System.Collections.Generic.List<CustomLayoutLocation.Custom> m_vCustoms = new System.Collections.Generic.List<CustomLayoutLocation.Custom>();
        System.Collections.Generic.List<string> m_vPop = new System.Collections.Generic.List<string>();
        void OnEnable()
        {
            RefreshPop();
            CustomLayoutLocation image = target as CustomLayoutLocation;

            if(image.customs!=null) m_vCustoms = new System.Collections.Generic.List<CustomLayoutLocation.Custom>(image.customs);
        }
        //------------------------------------------------------
        void OnDisable()
        {
            CustomLayoutLocation image = target as CustomLayoutLocation;
            image.customs = m_vCustoms.ToArray();
        }
        //------------------------------------------------------
        void RefreshPop()
        {
            m_vPop.Clear();
            SerializedProperty prop = serializedObject.FindProperty("components");
            if(prop!=null && prop.isArray)
            {
                for(int i = 0; i < prop.arraySize; ++i)
                {
                    if (prop.GetArrayElementAtIndex(i).objectReferenceValue == null)
                        m_vPop.Add("丢失" + i);
                    else
                    {
                       Component comp = prop.GetArrayElementAtIndex(i).objectReferenceValue as Component;
                        if (comp == null) m_vPop.Add("丢失" + i);
                        else
                        {
                            UISerialized seri = comp.gameObject.GetComponent<UISerialized>();
                            if (seri != null) prop.GetArrayElementAtIndex(i).objectReferenceValue = seri;
                            m_vPop.Add(comp.name);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomLayoutLocation image = target as CustomLayoutLocation;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("components"), new GUIContent("组件列表"), true);
            if(EditorGUI.EndChangeCheck())
            {
                RefreshPop();
            }

            EditorGUI.BeginChangeCheck();
            if(EditorGUI.EndChangeCheck())
            {
                image.customs = m_vCustoms.ToArray();
            }

            EditorGUILayout.BeginHorizontal();
            m_bExpand = EditorGUILayout.Foldout(m_bExpand, "自定义布局列表");
            if(GUILayout.Button("添加", new GUILayoutOption[] {  GUILayout.Width(40)}))
            {
                m_vCustoms.Add(new CustomLayoutLocation.Custom());
            }
            EditorGUILayout.EndHorizontal();

            if (m_bExpand)
            {
                EditorGUI.indentLevel++;
                for(int i =0; i < m_vCustoms.Count; ++i)
                {
                    CustomLayoutLocation.Custom item = m_vCustoms[i];
                    EditorGUILayout.BeginHorizontal();
                    item.bExpand = EditorGUILayout.Foldout(item.bExpand, "布局"+(i+1));

                    if (GUILayout.Button("预览", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        image.SetCustom(item.gridCount);
                        break;
                    }
                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        m_vCustoms.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    if (item.bExpand)
                    {
                        EditorGUI.indentLevel++;
                        item.gridCount = EditorGUILayout.IntField("数量", item.gridCount);
                        if (item.indexs != null)
                        {
                            for (int j = 0; j < item.indexs.Length; ++j)
                            {
                                EditorGUILayout.BeginHorizontal();
                                item.indexs[j] = (byte)EditorGUILayout.Popup("组件[" + j + "]", (int)item.indexs[j], m_vPop.ToArray());
                                if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                                {
                                    System.Collections.Generic.List<byte> vTemp = new System.Collections.Generic.List<byte>(item.indexs);
                                    vTemp.RemoveAt(j);
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        {
                            EditorGUI.indentLevel++;
                            if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(80) }))
                            {
                                System.Collections.Generic.List<byte> vTemp = null;
                                if (item.indexs != null) vTemp = new System.Collections.Generic.List<byte>(item.indexs);
                                else vTemp = new System.Collections.Generic.List<byte>();
                                vTemp.Add(0xff);
                                item.indexs = vTemp.ToArray();
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;
                    }
                    m_vCustoms[i] = item;
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
            if(GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}
