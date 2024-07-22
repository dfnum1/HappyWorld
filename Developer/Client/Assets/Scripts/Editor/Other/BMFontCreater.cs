using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;

namespace TopGame.ED
{
    public class CreateFontEditor : EditorWindow
    {
        [MenuItem("Tools/UI设置为缺省字体")]
        public static void ReplcaeDefaultFont()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI 字体替换为缺省字体", "替换", "取消"))
                return;
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

            Font defaultFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Datas/Fonts/default.ttf");
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
                            var text = scene.GetRootGameObjects()[i].GetComponentsInChildren<UnityEngine.UI.Text>(true);

                            for (int t = 0; t < text.Length; ++t)
                            {
                                var textComp = text[t];

                                if (textComp.font == null)
                                    continue;

                                if (AssetDatabase.GetAssetPath(textComp.font.GetInstanceID()).Contains(".fontsetting"))
                                    continue;

                                textComp.font = defaultFont;
                            }
                        }
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                }
                else
                {
                    var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\', '/'), typeof(UnityEngine.Object));

                    GameObject prefab = obj as GameObject;

                    if (prefab != null)
                    {
                        var text = prefab.GetComponentsInChildren<UnityEngine.UI.Text>(true);

                        for (int i = 0; i < text.Length; ++i)
                        {
                            var textComp = text[i];

                            if (textComp.font == null)
                                continue;

                            if (AssetDatabase.GetAssetPath(textComp.font.GetInstanceID()).Contains(".fontsetting"))
                                continue;

                            textComp.font = defaultFont;
                        }

                        EditorUtility.SetDirty(prefab);
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

        [MenuItem("Tools/艺术字生成器")]
        private static void OpenTools()
        {
            CreateFontEditor bmFont = new CreateFontEditor();
            bmFont.Show();
        }
        private TextAsset _fontTextAsset;
        private Texture _fontTexture;
        private string _fontsDir;

        private string _getAssetPath(string path)
        {
            string pathTemp = path.Replace("\\", "/");
            pathTemp = pathTemp.Replace(Application.dataPath, "Assets");
            return pathTemp;
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            TextAsset taTemp = EditorGUILayout.ObjectField("选择Font文件：", _fontTextAsset, typeof(TextAsset), true) as TextAsset;
            if (taTemp != _fontTextAsset && taTemp != null)
            {
                string assetDir = Directory.GetParent(AssetDatabase.GetAssetPath(taTemp)).FullName;
                assetDir = _getAssetPath(assetDir);
                string imgPath = string.Format("{0}/{1}_0.png", assetDir, taTemp.name);
                _fontTexture = AssetDatabase.LoadAssetAtPath<Texture>(imgPath);
            }
            _fontTextAsset = taTemp;

            _fontTexture = EditorGUILayout.ObjectField("选择Font图片文件：", _fontTexture, typeof(Texture), true) as Texture;

            if (taTemp == null)
                return;

            EditorGUILayout.BeginHorizontal();
            _fontsDir = EditorGUILayout.TextField("字体生成路径:", _fontsDir);
            if(GUILayout.Button("..."))
            {
                _fontsDir = EditorUtility.OpenFolderPanel("选择字体", _fontsDir, _fontsDir);
                _fontsDir = _fontsDir.Replace("\\", "/").Replace(Application.dataPath, "Assets") + "/" + taTemp.name + ".fontsettings";
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Generate Font"))
            {
                if (!string.IsNullOrEmpty(_fontsDir))
                {
                    Material mat = AssetDatabase.LoadAssetAtPath<Material>(_fontsDir.Replace(".fontsettings", ".mat"));
                    if (mat == null)
                    {
                        mat = new Material(Shader.Find("UI/Default Font"));
                        AssetDatabase.CreateAsset(mat, _fontsDir.Replace(".fontsettings", ".mat"));
                    }
                    if (_fontTexture != null)
                    {
                        mat = AssetDatabase.LoadAssetAtPath<Material>(_fontsDir.Replace(".fontsettings", ".mat"));
                        mat.SetTexture("_MainTex", _fontTexture);
                    }
                    else
                    {
                        Debug.LogError("贴图未做配置，请检查配置");
                        return;
                    }

                    Font font = AssetDatabase.LoadAssetAtPath<Font>(_fontsDir);
                    if (font == null)
                    {
                        font = new Font();
                        AssetDatabase.CreateAsset(font, _fontsDir);
                    }

                    _setFontInfo(AssetDatabase.LoadAssetAtPath<Font>(_fontsDir),
                        AssetDatabase.GetAssetPath(_fontTextAsset),
                        _fontTexture);
                    font = AssetDatabase.LoadAssetAtPath<Font>(_fontsDir);
                    font.material = mat;

                    if(mat!=null)
                        EditorUtility.SetDirty(mat);

                    EditorUtility.SetDirty(font);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
                else
                {
                    Debug.LogError("创建失败，请检查配置");
                }
            }
            EditorGUILayout.EndVertical();

        }

        private void _setFontInfo(Font font, string fontConfig, Texture texture)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fontConfig);
            List<CharacterInfo> chtInfoList = new List<CharacterInfo>();
            XmlNode node = xml.SelectSingleNode("font/chars");
            foreach (XmlNode nd in node.ChildNodes)
            {
                XmlElement xe = (XmlElement)nd;
                int x = int.Parse(xe.GetAttribute("x"));
                int y = int.Parse(xe.GetAttribute("y"));
                int width = int.Parse(xe.GetAttribute("width"));
                int height = int.Parse(xe.GetAttribute("height"));
                int advance = int.Parse(xe.GetAttribute("xadvance"));
                CharacterInfo info = new CharacterInfo();
                info.glyphHeight = texture.height;
                info.glyphWidth = texture.width;
                info.index = int.Parse(xe.GetAttribute("id"));
                //这里注意下UV坐标系和从BMFont里得到的信息的坐标系是不一样的哦，前者左下角为（0,0），
                //右上角为（1,1）。而后者则是左上角上角为（0,0），右下角为（图宽，图高）
                info.uvTopLeft = new Vector2((float)x / texture.width, 1 - (float)y / texture.height);
                info.uvTopRight = new Vector2((float)(x + width) / texture.width, 1 - (float)y / texture.height);
                info.uvBottomLeft = new Vector2((float)x / texture.width, 1 - (float)(y + height) / texture.height);
                info.uvBottomRight = new Vector2((float)(x + width) / texture.width, 1 - (float)(y + height) / texture.height);

                info.minX = int.Parse(xe.GetAttribute("xoffset"));
                info.minY = -(height + int.Parse(xe.GetAttribute("yoffset")));
                info.maxX = width+ int.Parse(xe.GetAttribute("xoffset"));
                info.maxY = -int.Parse(xe.GetAttribute("yoffset"));

                info.advance = advance;

                chtInfoList.Add(info);
            }
            font.characterInfo = chtInfoList.ToArray();
            EditorUtility.SetDirty(font);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }
}