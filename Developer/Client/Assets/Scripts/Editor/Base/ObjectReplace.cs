using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TopGame.ED
{
    public class ObjectReplace : EditorWindow
    {
        static ObjectReplace ms_instance = null;

        [MenuItem("Assets/资源替换")]
        public static void CheckDependces()
        {
            if (Selection.activeObject == null) return;
            if (ms_instance == null)
            {
                ms_instance = EditorWindow.GetWindow<ObjectReplace>();
                ms_instance.titleContent = new GUIContent("资源替换");
                ms_instance.Show();
            }
            ms_instance.Check(Selection.activeObject);
        }
        UnityEngine.Object m_pSourceObj = null;
        UnityEngine.Object m_pToObj = null;
        Vector2 m_Scroll = Vector2.zero;
        List<string> m_vOutPut = new List<string>();

        List<UnityEngine.Object> m_vFiles = new List<UnityEngine.Object>();
        private ReorderableList m_list;
        //------------------------------------------------------
        private void OnEnable()
        {
            m_list = new ReorderableList(m_vFiles, typeof(UnityEngine.Object), true, true, true, true);
            m_list.drawElementCallback = DrawElement;

            Check(Selection.activeObject);
        }
        //------------------------------------------------------
        void Check(UnityEngine.Object pObj)
        {
            GUI.FocusControl("");
            m_pSourceObj = pObj;
            m_vOutPut.Clear();
        }
        //------------------------------------------------------
        void DoCheck(string file)
        {
            if (m_pSourceObj == null || m_pToObj == m_pSourceObj) return;
            string source = AssetDatabase.GetAssetPath(m_pSourceObj);
            string dest = AssetDatabase.GetAssetPath(m_pToObj);
            string sourceGuid = AssetDatabase.AssetPathToGUID(source);
            string destGuid = AssetDatabase.AssetPathToGUID(dest);
            string content = File.ReadAllText(file);

            bool bDirty = false;
            if (content.IndexOf(source) > 0)
            {
                content = content.Replace(source, dest);
                bDirty = true;
            }
            if (content.IndexOf(sourceGuid)>0)
            {
                content = content.Replace(sourceGuid, destGuid);
                bDirty = true;
            }

            if(bDirty)
            {
                m_vOutPut.Add(file + " 替换完毕");
                File.WriteAllText(file, content);
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file));
            }
        }
        //------------------------------------------------------
        void OnGUI()
        {
            m_pSourceObj = EditorGUILayout.ObjectField("查找目标", m_pSourceObj, typeof(UnityEngine.Object), false);
            m_pToObj = EditorGUILayout.ObjectField("替换为", m_pToObj, typeof(UnityEngine.Object), false);
            m_list.DoLayoutList();
            if (m_list.count>0 && m_pToObj != m_pSourceObj && GUILayout.Button("替换"))
            {
                List<string> vObj = new List<string>();
                List<string> vPath = new List<string>();
                for (int i = 0; i < m_vFiles.Count; ++i)
                {
                    if (m_vFiles[i] == null) continue;
                    string path = AssetDatabase.GetAssetPath(m_vFiles[i]);
                    if (AssetDatabase.IsValidFolder(path))
                    {
                        vPath.Add(path);
                        continue;
                    }
                    if (m_vFiles[i] != m_pSourceObj && m_vFiles[i] != m_pToObj)
                    {
                        vObj.Add(path);
                        continue;
                    }
                }

                if (vPath.Count > 0)
                {
                    string[] assets = AssetDatabase.FindAssets("t:Object", vPath.ToArray());
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                        if (assetPath.Contains("."))
                        {
                            if (!vObj.Contains(assetPath)) vObj.Add(assetPath);
                        }
                    }
                }

                m_vOutPut.Clear();
                EditorUtility.DisplayProgressBar("检测", "", 0);
                for (int i = 0; i < vObj.Count; ++i)
                {
                    EditorUtility.DisplayProgressBar("替换", vObj[i], (float)i/(float)vObj.Count);
                    DoCheck(vObj[i]);
                }
                EditorUtility.ClearProgressBar();
            }
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll, new GUILayoutOption[] { GUILayout.Width(position.width) });
            for (int i = 0; i < m_vOutPut.Count; ++i)
                GUILayout.Label(m_vOutPut[i]);
            EditorGUILayout.EndScrollView();
        }
        //------------------------------------------------------
        void DrawElement(Rect rect, int index, bool selected, bool focused)
        {
            m_vFiles[index] = EditorGUI.ObjectField(rect, m_vFiles[index], typeof(UnityEngine.Object), true);
        }
    }

}

