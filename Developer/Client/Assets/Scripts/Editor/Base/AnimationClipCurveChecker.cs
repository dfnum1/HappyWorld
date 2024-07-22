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
    public class AnimationClipCurveChecker : EditorWindow
    {
        static AnimationClipCurveChecker ms_instance = null;

        [MenuItem("Assets/动画剪辑曲线数据检测")]
        public static void CheckDependces()
        {
            if (Selection.activeObject == null) return;
            if (ms_instance == null)
            {
                ms_instance = EditorWindow.GetWindow<AnimationClipCurveChecker>();
                ms_instance.titleContent = new GUIContent("动画剪辑曲线数据检测");
                ms_instance.Show();
            }
            ms_instance.Check(Selection.activeObject);
        }

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
            m_vOutPut.Clear();
            m_vFiles.Clear();
            m_vFiles.Add(pObj);
        }
        //------------------------------------------------------
        void DoCheck(string path)
        {
            bool bDirty = false;
            List<AnimationClip> animationClipList = new List<AnimationClip>();
           // animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(pGO));
           if(path.EndsWith(".anim"))
            {
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if(clip)
                    animationClipList.Add(clip);
            }
            if (animationClipList.Count == 0)
            {
                Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
                foreach (Object o in objs)
                {
                    if (o.name.Contains("__preview__")) continue;
                    if (o is AnimationClip)
                    {
                        animationClipList.Add(o as AnimationClip);
                    }
                }
            }
            if (animationClipList.Count <= 0) return;

            foreach (AnimationClip theAnimation in animationClipList)
			{
                //浮点数精度压缩到f3
                EditorCurveBinding[] curves = null;
                curves = AnimationUtility.GetCurveBindings(theAnimation);
                Keyframe key;
                Keyframe[] keyFrames;
                for (int ii = 0; ii < curves.Length; ++ii)
                {
                    EditorCurveBinding binding = curves[ii];
                    if (binding == null) continue;
                    AnimationCurve curveDate = AnimationUtility.GetEditorCurve(theAnimation, binding);
                    if (curveDate == null || curveDate.keys == null)
                    {
                        continue;
                    }
                    keyFrames = curveDate.keys;
                    for (int i = 0; i < keyFrames.Length; i++)
                    {
                        key = keyFrames[i];
                        key.value = float.Parse(key.value.ToString("f3"));
                        key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                        key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                        keyFrames[i] = key;
                    }
                    curveDate.keys = keyFrames;
                    theAnimation.SetCurve(binding.path, binding.type, binding.propertyName, curveDate);
                    bDirty = true;

                    bool bValidValue = false;
                    if (!bValidValue)
                    {
                        name = binding.propertyName.ToLower();
                        if (name.Contains("scale"))
                        {
                            bool bCanNull = true;
                            float maxTime = Framework.Core.CommonUtility.GetCurveMaxTime(curveDate, false);
                            if (curveDate != null && curveDate.length > 0)
                            {
                                int delScale = 1000;
                                float fDelta = 0;
                                while (fDelta <= maxTime)
                                {
                                    int val = (int)(curveDate.Evaluate(fDelta) * 1000);
                                    fDelta += 0.0333333f;
                                    if (val != delScale)
                                    {
                                        bCanNull = false;
                                        break;
                                    }
                                }
                            }
                            if (bCanNull)
                            {
                                AnimationUtility.SetEditorCurve(theAnimation, binding, null);
                                bDirty = true;
                            }
                        }
                    }
                }
                EditorUtility.SetDirty(theAnimation);
            }
            if (bDirty)
            {
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<Object>(path));
                AssetDatabase.SaveAssets();
                m_vOutPut.Add(path + " 检测完毕");
            }
        }
        //------------------------------------------------------
        void OnGUI()
        {
            m_list.DoLayoutList();
            if (GUILayout.Button("检测"))
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
                    if (path.EndsWith(".fbx") || path.EndsWith(".Fbx") || path.EndsWith(".FBX") || path.EndsWith(".anim"))
                    {
                        if(!vObj.Contains(path)) vObj.Add(path);
                    }
                }

                if (vPath.Count > 0)
                {
                    string[] assets = AssetDatabase.FindAssets("t:GameObject", vPath.ToArray());
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                        if (assetPath.EndsWith(".fbx") || assetPath.EndsWith(".Fbx") || assetPath.EndsWith(".FBX") || assetPath.EndsWith(".anim"))
                        {
                            if (!vObj.Contains(assetPath)) vObj.Add(assetPath);
                        }
                    }
                }

                m_vOutPut.Clear();
                EditorUtility.DisplayProgressBar("检测", "", 0);
                for (int i = 0; i < vObj.Count; ++i)
                {
                    EditorUtility.DisplayProgressBar("检测", vObj[i], (float)i/(float)vObj.Count);
                    DoCheck(vObj[i]);
                    break;
                }
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
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

