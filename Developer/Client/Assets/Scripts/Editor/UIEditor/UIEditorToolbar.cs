using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class UIEditorToolBar
    {
        [MenuItem("Tools/UI/UI动效批量检测")]
        public static void CheckTweener()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI动效批量检测", "检测", "取消"))
                return;

            Dictionary<int, UI.UIAnimatorGroupData> vUIAnims = new Dictionary<int, UI.UIAnimatorGroupData>();
            UI.UIAnimatorAssets animator = AssetDatabase.LoadAssetAtPath<UI.UIAnimatorAssets>("Assets/DatasRef/UI/Animation/UIAnimations.asset");
            if (animator != null)
            {
                for (int i = 0; i < animator.animations.Length; ++i)
                {
                    if (animator.animations[i].Parameters == null)
                    {
                        continue;
                    }
                    bool bHasAlpha = false;
                    for (int j = 0; j < animator.animations[i].Parameters.Length; ++j)
                    {
                        if (animator.animations[i].Parameters[j].type == UI.UIAnimatorElementType.ALPAH)
                        {
                            bHasAlpha = true;
                            break; ;
                        }
                    }
                    if (bHasAlpha)
                        vUIAnims[animator.animations[i].nID] = animator.animations[i];
                }
            }

            List<string> vFilesss = new List<string>();
            string RootParticlePath = "Assets/Datas/uis";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            vFilesss = new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());

            RootParticlePath = "Assets/DatasRef/UI";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            withoutExtensions = new List<string>() { ".unity" };
            RootParticlePath = "Assets/DatasRef/UI/UIScenes";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            Font defaultFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/DatasRef/Fonts/default.ttf");
            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("UI文件", file, (float)startIndex / (float)vFilesss.Count);

                if (file.Contains(".unity"))
                {
                    UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(file);
                    if (scene.IsValid())
                    {
                        int bDirty = 0;
                        for (int i = 0; i < scene.GetRootGameObjects().Length; ++i)
                        {
                            if (UIEditorHelper.CheckUITweener(scene.GetRootGameObjects()[i], vUIAnims))
                                bDirty++;
                        }

                        if(bDirty>0)
                            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                }
                else
                {
                    var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\', '/'), typeof(UnityEngine.Object));
                    if (UIEditorHelper.CheckUITweener(obj as GameObject, vUIAnims))
                    {
                        EditorUtility.SetDirty(obj);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }


                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    Debug.Log("检测结束结束");

                }

            };
        }
        //------------------------------------------------------
        [MenuItem("Tools/UI/UI字体批量处理")]
        public static void CheckFontSwtich()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI字体批量处理", "检测", "取消"))
                return;

            Dictionary<int, UI.UIAnimatorGroupData> vUIAnims = new Dictionary<int, UI.UIAnimatorGroupData>();
            UI.UIAnimatorAssets animator = AssetDatabase.LoadAssetAtPath<UI.UIAnimatorAssets>("Assets/DatasRef/UI/Animation/UIAnimations.asset");
            if (animator != null)
            {
                for (int i = 0; i < animator.animations.Length; ++i)
                {
                    if (animator.animations[i].Parameters == null)
                    {
                        continue;
                    }
                    bool bHasAlpha = false;
                    for (int j = 0; j < animator.animations[i].Parameters.Length; ++j)
                    {
                        if (animator.animations[i].Parameters[j].type == UI.UIAnimatorElementType.ALPAH)
                        {
                            bHasAlpha = true;
                            break; ;
                        }
                    }
                    if (bHasAlpha)
                        vUIAnims[animator.animations[i].nID] = animator.animations[i];
                }
            }

            List<string> vFilesss = new List<string>();
            string RootParticlePath = "Assets/Datas/uis";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            vFilesss = new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());

            RootParticlePath = "Assets/DatasRef/UI";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("UI文件", file, (float)startIndex / (float)vFilesss.Count);

                var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\', '/'), typeof(UnityEngine.Object));
                if (UIEditorHelper.CheckDefaultFont(obj as GameObject))
                {
                    EditorUtility.SetDirty(obj);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }

                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    Debug.Log("检测结束结束");

                }

            };
        }
        //-----------------------------------------------------

        [MenuItem("Tools/UI/CheckUIImage")]
        public static void CheckUIImage()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI 检测图片组件资源引用", "检测", "取消"))
                return;
            List<string> vFilesss = new List<string>();
            string RootParticlePath = "Assets/Datas/uis";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            vFilesss = new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());

            RootParticlePath = "Assets/DatasRef/UI";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("UI文件", file, (float)startIndex / (float)vFilesss.Count);

                var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\', '/'), typeof(UnityEngine.Object));
                GameObject prefab = obj as GameObject;
                if (prefab != null)
                {
                    if (UIEditorHelper.ReplaceWrongRefImage(prefab, false))
                    {
                        EditorUtility.SetDirty(prefab);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }

                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("检测结束");

                }
            };
        }
    }
}

