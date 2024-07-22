using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using UnityEngine.UI;
using static TopGame.ED.PublishPanel;

namespace TopGame.ED
{
    public class StaticImageReplaceTool : EditorWindow
    {
        [MenuItem("Tools/UI静态图片引用处理")]
        public static void ReplcaeDefaultFont()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI 图片控件检查是否引用Datas里面的资源", "确定", "取消"))
                return;

            PublishSetting m_BuildSetting = PublishPanel.LoadSetting();
            List<string> buildDirs = m_BuildSetting.buildDirs;
            List<string> unbuildDirs = m_BuildSetting.unbuildDirs;

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

            int startIndex = 0;

            
            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("UI文件", file, (float)startIndex / (float)vFilesss.Count);

                if(file.Contains(".unity"))
                {
                    UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(file);
                    if(scene.IsValid())
                    {
                        for(int i = 0; i < scene.GetRootGameObjects().Length; ++i)
                        {
                            GameObject rootObject = scene.GetRootGameObjects()[i];
                            var images = rootObject.GetComponentsInChildren<UnityEngine.UI.Image>(true);
                            var rawImages = rootObject.GetComponentsInChildren<UnityEngine.UI.RawImage>(true);
                            for (int j = 0; j < images.Length; ++j)
                            {
                                Image img = images[j];
                                string path = AssetDatabase.GetAssetPath(img.sprite);
                                if (path == null) continue;
                                if (UIEditorHelper.CanReplaceImageByPath(path))
                                {
                                    if (img == null) continue;
                                    GameObject root = img.gameObject;
                                    GameObject.DestroyImmediate(img, true);
                                    TopGame.UI.RawImageEx ex = root.AddComponent<TopGame.UI.RawImageEx>();
                                    ex.texturePath = path;

                                    var textureObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                                    ex.texture = textureObj as Texture;
                                    Debug.Log(scene.name + ":" + path);
                                }
                            }
                        }
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                }
                //else
                //{
                //    string prefabPath = file.Replace('\\', '/');
                //    var obj = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(UnityEngine.Object));

                //    GameObject prefab = obj as GameObject;

                //    if (prefab != null)
                //    {
                //        var images = prefab.GetComponentsInChildren<UnityEngine.UI.Image>(true);
                //        for (int i = 0; i < images.Length; ++i)
                //        {
                //            Image img = images[i];
                //            string path = AssetDatabase.GetAssetPath(img.sprite);
                //            if (path == null) continue;
                //            if (unbuildDirs.Count != 0)
                //            {
                //                for (int unbuild = 0; unbuild < unbuildDirs.Count; unbuild++)
                //                {
                //                    for (int build = 0; build < buildDirs.Count; build++)
                //                    {
                //                        if (buildDirs[build].CompareTo("Assets/DatasRef/UI/Textures") == 0) continue;
                //                        if (!path.Contains(unbuildDirs[unbuild]) && (path.Contains("Assets/Datas/") || path.Contains(buildDirs[build])))
                //                        {
                //                            if (img == null) continue;
                //                            GameObject root = img.gameObject;
                //                            GameObject.DestroyImmediate(img, true);
                //                            RawImageEx ex = root.AddComponent<RawImageEx>();
                //                            ex.texturePath = path;

                //                            var textureObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                //                            ex.texture = textureObj as Texture;
                //                            Debug.Log(prefab.name + ":" + path);
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                for (int build = 0; build < buildDirs.Count; build++)
                //                {
                //                    if (buildDirs[build].CompareTo("Assets/DatasRef/UI/Textures") == 0) continue;
                //                    if ((path.Contains("Assets/Datas/") || path.Contains(buildDirs[build])))
                //                    {
                //                        if (img == null) continue;
                //                        GameObject root = img.gameObject;
                //                        GameObject.DestroyImmediate(img,true);
                //                        RawImageEx ex = root.AddComponent<RawImageEx>();
                //                        ex.texturePath = path;

                //                        var textureObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                //                        ex.texture = textureObj as Texture;
                //                        Debug.Log(prefab.name + ":" + path);
                //                    }
                //                }
                //            }
                //        }

                //        EditorUtility.SetDirty(prefab);
                //        AssetDatabase.SaveAssets();
                //    }
                //}


                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {
                   
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("替换结束结束");

                }

            };
        }

        [MenuItem("Tools/UI滚动窗口遮罩替换")]
        public static void ReplcaeScrollRectMaskToRectMask2D()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI 图片控件检查是否引用Datas里面的资源", "确定", "取消"))
                return;

            PublishSetting m_BuildSetting = PublishPanel.LoadSetting();
            List<string> buildDirs = m_BuildSetting.buildDirs;
            List<string> unbuildDirs = m_BuildSetting.unbuildDirs;

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
                        for (int i = 0; i < scene.GetRootGameObjects().Length; ++i)
                        {
                            GameObject rootObject = scene.GetRootGameObjects()[i];
                            var scrollrects = rootObject.GetComponentsInChildren<UnityEngine.UI.ScrollRect>(true);
                            int scrollrectCnt = 0;
                            for (int j = 0; j < scrollrects.Length; ++j)
                            {
                                ScrollRect scroll = scrollrects[j];
                                if (scroll)
                                {
                                    scrollrectCnt++;
                                    Image img = scroll.GetComponent<Image>();
                                    Mask mask = scroll.GetComponent<Mask>();
                                    RectMask2D mask2d = scroll.GetComponent<RectMask2D>();
                                    if (mask2d) continue;

                                    if (mask)
                                    {
                                        if (img)
                                        {
                                            bool isShowMaskGraphic = mask.showMaskGraphic;
                                            if (!isShowMaskGraphic)
                                            {
                                                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                                            }
                                        }

                                        GameObject.DestroyImmediate(mask, true);
                                        scroll.gameObject.AddComponent<RectMask2D>();
                                    }
                                    else
                                    {
                                        scroll.gameObject.AddComponent<RectMask2D>();
                                    }
                                }
                            }

                            if (scrollrectCnt != 0)
                            {
                                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                            }
                        }
                    }
                }

                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {

                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log("替换结束结束");

                }

            };
        }
    }
}