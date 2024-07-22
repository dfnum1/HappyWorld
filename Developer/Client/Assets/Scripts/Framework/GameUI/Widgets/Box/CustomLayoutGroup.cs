/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GridCustomLayoutGroup
作    者:	HappLI
描    述:	根据数量自定义布局
*********************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [UIWidgetExport]
    public class CustomLayoutGroup : MonoBehaviour
    {
        [System.Serializable]
        public struct Item
        {
            public Vector2 position;
            public Vector2 size;
        }
        [System.Serializable]
        public struct Custom
        {
            public List<Item> items;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool expand;
#endif
        }
        public Custom[] customs;

        Transform m_pTransfrom =null;
        public void Refresh()
        {
            if (customs == null || customs.Length <= 0) return;
            int childCnt = 0;
            if (m_pTransfrom == null) m_pTransfrom = transform;
            int cnt = m_pTransfrom.childCount;
            for(int i = 0; i < cnt; ++i)
            {
                if(m_pTransfrom.GetChild(i).gameObject.activeSelf)
                {
                    childCnt++;
                }
            }
            
            List<Item> positions = null;
            for(int i = 0; i < customs.Length; ++i)
            {
                if(customs[i].items != null && customs[i].items.Count == childCnt)
                {
                    positions = customs[i].items;
                    break;
                }
            }
            if(positions == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("不存在布局为" + childCnt + "的个数");
#endif
                return;
            }
            int index = 0;
            for (int i = 0; i < cnt && index< positions.Count; ++i)
            {
                RectTransform trans = m_pTransfrom.GetChild(i) as RectTransform;
                if (trans && trans.gameObject.activeSelf)
                {
                    trans.anchoredPosition = positions[index].position;
                    trans.sizeDelta = positions[index].size;
                    index++;
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CustomLayoutGroup), true)]
    [CanEditMultipleObjects]
    public class CustomLayoutGroupEditor : Editor
    {
        List<CustomLayoutGroup.Custom> m_vCustoms = new List<CustomLayoutGroup.Custom>();
        //------------------------------------------------------
        private void OnEnable()
        {
            CustomLayoutGroup gp = target as CustomLayoutGroup;
            m_vCustoms = new List<CustomLayoutGroup.Custom>(gp.customs);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            CustomLayoutGroup gp = target as CustomLayoutGroup;
            gp.customs = m_vCustoms.ToArray();
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomLayoutGroup image = target as CustomLayoutGroup;
            for(int i =0; i < m_vCustoms.Count; ++i)
            {
                CustomLayoutGroup.Custom cust = m_vCustoms[i];
                GUILayout.BeginHorizontal();
                cust.expand = EditorGUILayout.Foldout(cust.expand, "布局个数:" + (cust.items != null ? cust.items.Count : 0));
                if (Selection.transforms != null && Selection.transforms.Length > 0 && GUILayout.Button("设置", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    //排序
                    List<GameObject> list = new List<GameObject>(Selection.gameObjects);
                    list.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

                    cust.items = new List<CustomLayoutGroup.Item>();
                    for (int j = 0; j < list.Count; ++j)
                    {
                        if (list[j] == image.gameObject) continue;
                        RectTransform rectTrans = list[j].transform as RectTransform;
                        cust.items.Add( new CustomLayoutGroup.Item() { position = rectTrans.anchoredPosition, size = rectTrans.sizeDelta });
                    }
                }
                if(GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    m_vCustoms.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();
                if (cust.items!=null)
                {
                    if(cust.expand)
                    {
                        EditorGUI.indentLevel++;
                        for(int j =0; j < cust.items.Count; ++j)
                        {
                            CustomLayoutGroup.Item item = cust.items[j];
                            item.position = EditorGUILayout.Vector2Field("Pos" + j, item.position);
                            item.size = EditorGUILayout.Vector2Field("Size" + j, item.size);
                            cust.items[j] = item;
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                m_vCustoms[i] = cust;
                image.customs = m_vCustoms.ToArray();
            }
            if(GUILayout.Button("添加"))
            {
                m_vCustoms.Add(new CustomLayoutGroup.Custom());
            }
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            if (GUILayout.Button("模拟"))
            {
                image.Refresh();
            }
        }
    }
#endif
}
