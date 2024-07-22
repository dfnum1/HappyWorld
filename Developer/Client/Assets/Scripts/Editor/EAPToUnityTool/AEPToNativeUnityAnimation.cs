#if UNITY_EDITOR
using TopGame.AEPToUnity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TopGame.AEPToUnity
{
    public class AEPToNativeUnityAnimation : EditorWindow
    {
        private const int DATE_EXPITE = 736266;

        private const string VERION_NEXT = "2.2";

        internal static AEPToNativeUnityAnimation instance;

        [SerializeField]
        public int tabBuildAtlas = 0;

        [SerializeField]
        public SpriteType buildSpriteType = SpriteType.SpriteRenderer;

        [SerializeField]
        public string pathImages = "";

        [SerializeField]
        public TrimType trimType = TrimType.Trim2nTexture;

        [SerializeField]
        public BuildAtlasType buildAtlasType = BuildAtlasType.ReferenceImage;

        public Texture2D mainTexture;

        [SerializeField]
        public List<TextAsset> listAnimationInfo = new List<TextAsset>
    {
        null
    };

        [SerializeField]
        public List<AnimationStyle> listAnimationLoop = new List<AnimationStyle>
    {
        AnimationStyle.Normal
    };

        private const string DEFAULT_OUTPUT = "Assets/AEP Output";

        private string pathOutputs = "Assets/AEP Output";

        private bool onlyBuildAnimation = false;
        private bool showAnimationFiles = true;

        private Vector2 mScroll = Vector2.zero;

        [SerializeField]
        private string aepName = "AEP Animation";

        [SerializeField]
        private string atlasName = "AEP_Atlas";

        [SerializeField]
        public bool autoOverride = true;

        [SerializeField]
        public SortLayerType sortType = SortLayerType.Depth;

        [SerializeField]
        public int defaultSortDepthValue = 0;

        [SerializeField]
        public int padingSize = 1;

        [SerializeField]
        public float scaleTextureImage = 1f;

        private int tabBuild = 0;

        private float scaleInfo = 100f;

        private GameObject objectRoot;

        private List<DataAnimAnalytics> listInfoFinal = new List<DataAnimAnalytics>();

        private List<string> listPathNotAtlas = new List<string>();

        [MenuItem("Tools/AEPToUnity")]
        public static void CreateWindow()
        {
            AEPToNativeUnityAnimation.instance = EditorWindow.GetWindow<AEPToNativeUnityAnimation>();
            AEPToNativeUnityAnimation.instance.title = ("AE To Unity");
            AEPToNativeUnityAnimation.instance.minSize = (new Vector2(380f, 450f));
            AEPToNativeUnityAnimation.instance.Show();
            AEPToNativeUnityAnimation.instance.ShowUtility();
            AEPToNativeUnityAnimation.instance.autoRepaintOnSceneChange = (false);
            AEPToNativeUnityAnimation.instance.wantsMouseMove = (false);
        }

        public static void DrawHeader(string text)
        {
            GUILayout.Space(3f);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Space(3f);
            GUI.changed = (false);
            text = "<b><size=11>" + text + "</size></b>";
            text = "▼ " + text;
            GUILayout.Toggle(true, text, "dragtab", new GUILayoutOption[]
            {
            GUILayout.MinWidth(20f)
            });
            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
        }

        public static void BeginContents()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Space(4f);
            EditorGUILayout.BeginHorizontal("AS TextArea", new GUILayoutOption[]
            {
            GUILayout.MinHeight(10f)
            });
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.Space(2f);
        }

        public static void BeginContentsMaxHeight()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Space(4f);
            EditorGUILayout.BeginHorizontal("AS TextArea", new GUILayoutOption[]
            {
            GUILayout.MaxHeight(20000f)
            });
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.Space(2f);
        }

        public static void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
            GUILayout.Space(3f);
        }

        private void OnGUI()
        {
            if (AEPToNativeUnityAnimation.instance == null)
            {
                AEPToNativeUnityAnimation.CreateWindow();
            }
            this.DrawToolbar();
        }

        public bool CheckNewUpdate()
        {
            return false;
            DateTime now = DateTime.Now;
            int num = now.Year * 365 + now.Month * 30 + now.Day;
            int num2 = EditorPrefs.GetInt("AEPToUnity_EXPIRE", -1);
            if (num2 < 0 || num2 < num)
            {
                num2 = num;
                EditorPrefs.SetInt("AEPToUnity_EXPIRE", num2);
            }
            bool result;
            if (num2 > 736266)
            {
                Debug.LogError("New Update Available");
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void ChangePathImage()
        {
            this.pathImages = EditorUtility.OpenFolderPanel("Directory All Fire", "", "");
            string dataPath = Application.dataPath;
            if (this.pathImages.Length < 1)
            {
                this.pathImages = "";
            }
            else if (!this.pathImages.Contains(dataPath))
            {
                this.pathImages = "";
                EditorUtility.DisplayDialog("Error", "Please Choose Folder Images inside Project", "OK");
            }
            else
            {
                this.pathImages = "Assets" + this.pathImages.Replace(dataPath, "");
                string[] files = Directory.GetFiles(this.pathImages);
                bool flag = false;
                for (int i = 0; i < files.Length; i++)
                {
                    Object @object = AssetDatabase.LoadAssetAtPath(files[i], typeof(Object));
                    if (@object != null)
                    {
                        if (@object is Texture2D)
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                {
                    this.pathImages = "";
                    EditorUtility.DisplayDialog("Error", "Can not found any image texture in this folder", "OK");
                }
            }
        }

        private HashSet<string> GetAllReferenceImage()
        {
            HashSet<string> hashSet = new HashSet<string>();
            if (this.listInfoFinal != null)
            {
                for (int i = 0; i < this.listInfoFinal.Count; i++)
                {
                    DataAnimAnalytics dataAnimAnalytics = this.listInfoFinal[i];
                    foreach (KeyValuePair<string, EAPInfoAttachment> current in dataAnimAnalytics.jsonFinal.dicPivot)
                    {
                        if (!hashSet.Contains(current.Value.spriteName))
                        {
                            hashSet.Add(current.Value.spriteName);
                        }
                    }
                }
            }
            return hashSet;
        }

        private void ChooseOutput()
        {
            this.pathOutputs = EditorUtility.OpenFolderPanel("Choose Directory For  Output Data", "", "");
            string dataPath = Application.dataPath;
            if (this.pathOutputs.Length < 1)
            {
                this.pathOutputs = "Assets/AEP Output";
            }
            else if (!this.pathOutputs.Contains(dataPath))
            {
                this.pathOutputs = "Assets/AEP Output";
                EditorUtility.DisplayDialog("Error", "Please Choose Dicrectory Folder inside Project", "OK");
            }
            else
            {
                this.pathOutputs = "Assets" + this.pathOutputs.Replace(dataPath, "");
            }
        }

        private void GUIShowEditorChooseOrCreateAtlas()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("SETTING IMAGES ATLAS BUILDER:", EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUILayout.Box("", new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(2f)
            });
            GUI.color = Color.white;
            this.tabBuildAtlas = GUILayout.Toolbar(this.tabBuildAtlas, new string[]
            {
            "新建图集",
            "使用已有图集"
            }, new GUILayoutOption[0]);
            if (this.tabBuildAtlas == 0)
            {
                GUI.color = Color.white;
                this.GUIShowEditorCreateAtlas();
            }
            else
            {
                this.GUIShowEditorChooseAtlas();
            }
            GUI.color = Color.cyan;
            GUILayout.Box("", new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(2f)
            });
            GUI.color = Color.white;
        }

        private void GUIShowEditorChooseAtlas()
        {
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (this.mainTexture != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(this.mainTexture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (textureImporter.textureType != TextureImporterType.Sprite || textureImporter.spriteImportMode != SpriteImportMode.Multiple)
                {
                    EditorUtility.DisplayDialog("Texture Input Warning", "Please choose Texture Sprite in multiple mode", "OK");
                    this.mainTexture = null;
                }
            }
            GUI.color = Color.white;
            if (this.mainTexture == null)
            {
                EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Info);
            }
            if (this.mainTexture == null)
            {
                GUI.color = Color.red;
            }
            else
            {
                GUI.color = Color.white;
            }
            this.mainTexture = (EditorGUILayout.ObjectField("", this.mainTexture, typeof(Texture2D), true, new GUILayoutOption[]
            {
            GUILayout.Width(70f),
            GUILayout.Height(70f)
            }) as Texture2D);
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
            this.scaleTextureImage = EditorGUILayout.FloatField(new GUIContent("Resize Texture Ratio: ", "scaleTextureImage Scale, Default is 1"), this.scaleTextureImage, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            if (this.scaleTextureImage < 0f)
            {
                this.scaleTextureImage = 0.1f;
            }
        }

        private void GUIShowEditorCreateAtlas()
        {
            if (this.pathImages.Length < 1)
            {
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.color = Color.red;
                EditorGUILayout.HelpBox("没有设置图集生成目录 ! 请先设置目录 !", MessageType.Warning, true);
                if (GUILayout.Button("Open Directory", new GUILayoutOption[]
                {
                GUILayout.Height(37f)
                }))
                {
                    this.ChangePathImage();
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                AEPToNativeUnityAnimation.BeginContents();
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.color = Color.white;
                EditorGUILayout.HelpBox("Directory Choose: " + this.pathImages, MessageType.Info, true);
                if (GUILayout.Button("Change Directory", new GUILayoutOption[]
                {
                GUILayout.Height(37f)
                }))
                {
                    this.ChangePathImage();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.color = Color.white;
                EditorGUILayout.LabelField("Choose Option to Build Atlas:", EditorStyles.boldLabel, new GUILayoutOption[0]);
                this.buildAtlasType = (BuildAtlasType)EditorGUILayout.EnumPopup(this.buildAtlasType, "DropDown", new GUILayoutOption[]
                {
                GUILayout.Width(150f),
                GUILayout.Height(20f)
                });
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.color = Color.white;
                EditorGUILayout.LabelField("Choose Option Trimming:", EditorStyles.boldLabel, new GUILayoutOption[0]);
                this.trimType = (TrimType)EditorGUILayout.EnumPopup(this.trimType, "DropDown", new GUILayoutOption[]
                {
                GUILayout.Width(150f),
                GUILayout.Height(20f)
                });
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUI.color = Color.white;
                this.padingSize = EditorGUILayout.IntField("Padding Size:", this.padingSize, new GUILayoutOption[]
                {
                GUILayout.MaxWidth(2000f)
                });
                if (this.padingSize < 1)
                {
                    this.padingSize = 1;
                }
                if (this.padingSize > 8)
                {
                    this.padingSize = 8;
                }
                EditorGUILayout.EndHorizontal();
                AEPToNativeUnityAnimation.EndContents();
            }
        }

        private void GUIShowAnimationInfo()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("INPUT ANIMATIONS JSON FILE:", EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUI.color = Color.white;
            GUI.color = Color.white;
            this.onlyBuildAnimation = EditorGUILayout.Toggle("仅生成动画", onlyBuildAnimation);
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.showAnimationFiles = EditorGUILayout.Foldout(this.showAnimationFiles, "List Animation Json Files");
            EditorGUILayout.EndHorizontal();
            if (this.showAnimationFiles)
            {
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                EditorGUILayout.LabelField("\t", new GUILayoutOption[]
                {
                GUILayout.Width(20f)
                });
                int num = this.listAnimationInfo.Count;
                num = Mathf.Clamp(EditorGUILayout.IntField("Size:", num, new GUILayoutOption[]
                {
                GUILayout.MaxWidth(1000f)
                }), 0, 50);
                if (num < 0)
                {
                    num = 0;
                }
                if (num != this.listAnimationInfo.Count)
                {
                    if (num == 0)
                    {
                        this.listAnimationInfo.Clear();
                        this.listAnimationLoop.Clear();
                    }
                    else if (num > this.listAnimationInfo.Count)
                    {
                        for (int i = this.listAnimationInfo.Count; i < num; i++)
                        {
                            this.listAnimationInfo.Add(null);
                            this.listAnimationLoop.Add(AnimationStyle.Normal);
                        }
                    }
                    else
                    {
                        int count = this.listAnimationInfo.Count;
                        for (int j = num; j < count; j++)
                        {
                            this.listAnimationInfo.RemoveAt(this.listAnimationInfo.Count - 1);
                            this.listAnimationLoop.RemoveAt(this.listAnimationLoop.Count - 1);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                for (int k = 0; k < this.listAnimationInfo.Count; k++)
                {
                    EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                    EditorGUILayout.LabelField("\t", new GUILayoutOption[]
                    {
                    GUILayout.Width(20f)
                    });
                    this.listAnimationInfo[k] = (EditorGUILayout.ObjectField("Element " + (k + 1).ToString(), this.listAnimationInfo[k], typeof(TextAsset), true, new GUILayoutOption[]
                    {
                    GUILayout.MaxWidth(1000f)
                    }) as TextAsset);
                    this.listAnimationLoop[k] = (AnimationStyle)EditorGUILayout.EnumPopup(this.listAnimationLoop[k], new GUILayoutOption[]
                    {
                    GUILayout.Width(50f)
                    });
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void ShowOutputData()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("OUTPUT SETTING:", EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUI.color = Color.white;
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            EditorGUILayout.HelpBox("Choose Directory for all data output !\nCurrent: " + this.pathOutputs, 0, true);
            if (GUILayout.Button("Change Directory", new GUILayoutOption[]
            {
            GUILayout.Height(30f),
            GUILayout.Width(150f)
            }))
            {
                this.ChooseOutput();
            }
            EditorGUILayout.EndHorizontal();
            this.buildSpriteType = (SpriteType)EditorGUILayout.EnumPopup(new GUIContent("Export Layer Type", "Object Generate Scale, Default is 100"), this.buildSpriteType, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            if (this.buildSpriteType == SpriteType.SpriteRenderer)
            {
                this.scaleInfo = 100f;
            }
            else
            {
                this.scaleInfo = 1f;
            }
            this.scaleInfo = EditorGUILayout.FloatField(new GUIContent("Scale Object Create", "Object Generate Scale, Default is 100"), this.scaleInfo, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            if (this.scaleInfo < 0f)
            {
                this.scaleInfo = 100f;
            }
            this.aepName = EditorGUILayout.TextField("Name Object Create", this.aepName, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            if (this.aepName.Length == 0)
            {
                this.aepName = "AEP Animation";
            }
            this.atlasName = EditorGUILayout.TextField("Name Atlas Image Create ", this.atlasName, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            if (this.atlasName.Length == 0)
            {
                this.atlasName = "AEP_Atlas";
            }
            this.defaultSortDepthValue = EditorGUILayout.IntField("Default Sorting Layer Value:", this.defaultSortDepthValue, new GUILayoutOption[]
            {
            GUILayout.MaxHeight(2000f)
            });
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            this.autoOverride = EditorGUILayout.Toggle("Auto Override Output Data", this.autoOverride, new GUILayoutOption[]
            {
            GUILayout.MaxWidth(2000f)
            });
            GUILayout.Label("Sort Layer By", new GUILayoutOption[]
            {
            GUILayout.Width(75f)
            });
            this.sortType = (SortLayerType)EditorGUILayout.EnumPopup(this.sortType, new GUILayoutOption[]
            {
            GUILayout.Width(60f)
            });
            EditorGUILayout.EndHorizontal();
        }

        private void ShowSettingBuildAnimation()
        {
            GUI.color = Color.cyan;
            GUILayout.Label("BUILD SETTING", EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUI.color = Color.white;
            this.tabBuild = GUILayout.Toolbar(this.tabBuild, new string[]
            {
            "Auto Build",
            "Custom Build"
            }, new GUILayoutOption[0]);
            if (this.tabBuild == 0)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("BUILD", new GUILayoutOption[]
                {
                GUILayout.Height(40f),
                GUILayout.MaxWidth(2000f)
                }))
                {
                    if (Mathf.Abs(this.scaleTextureImage - 1f) > Mathf.Epsilon)
                    {
                        if (this.tabBuildAtlas == 1)
                        {
                            if (!EditorUtility.DisplayDialog("Warning", "Your Scale Texture Image is not 1, Make sure you scale atlas image by yourseft first?.\n Press YES to continue", "YES", "Let's me resize image first"))
                            {
                                return;
                            }
                        }
                    }
                    bool flag = this.BuildSprite();
                    if (flag)
                    {
                        flag = this.CreateBoneSkeleton();
                    }
                    if (flag)
                    {
                        flag = this.BuildAnimation(!onlyBuildAnimation);
                    }
                    if (flag)
                    {
                        this.scaleTextureImage = 1f;
                        EditorUtility.DisplayDialog("Finish", "Skeleton " + this.aepName + " had created in scene, all files reference in " + this.pathOutputs, "OK");
                    }
                }
            }
            else
            {
                GUI.color = Color.green;
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                if (GUILayout.Button("Build Atlas Sprite", new GUILayoutOption[]
                {
                GUILayout.Height(40f),
                GUILayout.MaxWidth(2000f)
                }))
                {
                    this.BuildSprite();
                }
                if (GUILayout.Button("Build Skeleton", new GUILayoutOption[]
                {
                GUILayout.Height(40f),
                GUILayout.MaxWidth(2000f)
                }))
                {
                    this.CreateBoneSkeleton();
                }
                if (GUILayout.Button("Build Animation", new GUILayoutOption[]
                {
                GUILayout.Height(40f),
                GUILayout.MaxWidth(2000f)
                }))
                {
                    this.BuildAnimation(!onlyBuildAnimation);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawToolbar()
        {
            this.mScroll = GUILayout.BeginScrollView(this.mScroll, new GUILayoutOption[0]);
            EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
            GUI.color = Color.green;
            GUILayout.Label("Adobe After Effect json 导入,使用说明:\r\n", EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal();
            GUILayout.Label("1:将脚本放置到Adobe AE 的Scripts 目录下", EditorStyles.boldLabel, new GUILayoutOption[0]);
            if(GUILayout.Button("脚本目录"))
            {
                ED.EditorKits.OpenPathInExplorer("Assets/Scripts/Editor/EAPToUnityTool/AEPScript");
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.cyan;
            GUILayout.Box("", new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(2f)
            });
            GUI.color = Color.white;
            this.GUIShowEditorChooseOrCreateAtlas();
            this.GUIShowAnimationInfo();
            GUI.color = Color.cyan;
            GUILayout.Box("", new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(2f)
            });
            GUI.color = Color.white;
            this.ShowOutputData();
            GUI.color = Color.cyan;
            GUILayout.Box("", new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(2f)
            });
            this.ShowSettingBuildAnimation();
            EditorGUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void Test()
        {
            AnimationClip animationClip = AssetDatabase.LoadAssetAtPath("Assets/Test/Testing.anim", typeof(AnimationClip)) as AnimationClip;
            if (animationClip == null)
            {
                Debug.LogError("Anim Null");
            }
            else
            {
                AnimationClipCurveData[] allCurves = AnimationUtility.GetAllCurves(animationClip, true);
                Debug.LogError(JsonFx.Json.JsonWriter.Serialize(allCurves));
                EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animationClip);
                Debug.LogError(JsonFx.Json.JsonWriter.Serialize(curveBindings));
            }
        }

        private bool ReadInfo(bool isShowError = true)
        {
            bool result;
            if (this.CheckNewUpdate())
            {
                EditorUtility.ClearProgressBar();
                if (EditorUtility.DisplayDialog("Require Update", "New free update version are avaible, Please update to continue !", "OK"))
                {
                    Application.OpenURL("http://u3d.as/j5o");
                    result = false;
                    return result;
                }
            }
            bool flag = false;
            if (this.listAnimationInfo.Count < 1)
            {
                flag = true;
            }
            else
            {
                this.listInfoFinal.Clear();
                for (int i = 0; i < this.listAnimationInfo.Count; i++)
                {
                    TextAsset textAsset = this.listAnimationInfo[i];
                    if (textAsset != null)
                    {
                        AnimationStyle animationStyle = AnimationStyle.Normal;
                        if (i < this.listAnimationLoop.Count)
                        {
                            animationStyle = this.listAnimationLoop[i];
                        }
                        string text = textAsset.text;
                        string text2 = AssetDatabase.GetAssetPath(textAsset);
                        int num = text2.LastIndexOf("/");
                        text2 = text2.Substring(num + 1, text2.Length - num - 6);
                        RawAEPJson raw = JsonFx.Json.JsonReader.Deserialize(text, typeof(RawAEPJson)) as RawAEPJson;
                        AEPJsonFinal aEPJsonFinal = new AEPJsonFinal(raw);
                        if (aEPJsonFinal != null)
                        {
                            this.listInfoFinal.Add(new DataAnimAnalytics(aEPJsonFinal, text2, animationStyle));
                        }
                        float num2 = (float)((i + 1) / this.listAnimationInfo.Count);
                        EditorUtility.DisplayCancelableProgressBar("Read Animation Json Info", "processing...", num2);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            if (flag)
            {
                if (isShowError)
                {
                    EditorUtility.DisplayDialog("Error Input Data", "Data Animation is not correct format !", "OK");
                }
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool BuildSprite()
        {
            Debug.Log("BuildSprite");
            if (!Directory.Exists(this.pathOutputs))
            {
                Directory.CreateDirectory(this.pathOutputs);
            }
            bool result;
            if (!this.ReadInfo(true))
            {
                result = false;
            }
            else if (this.tabBuildAtlas == 0)
            {
                if (this.pathImages.Length < 1)
                {
                    EditorUtility.DisplayDialog("Error Input Data", "Folder directory Path is empty !", "OK");
                    result = false;
                }
                else
                {
                    string[] files = Directory.GetFiles(this.pathImages);
                    bool flag = false;
                    HashSet<string> allReferenceImage = this.GetAllReferenceImage();
                    if (this.buildAtlasType == BuildAtlasType.AllImageInDirectory || this.buildAtlasType == BuildAtlasType.ReferenceImage)
                    {
                        List<Texture2D> list = new List<Texture2D>();
                        for (int i = 0; i < files.Length; i++)
                        {
                            Object @object = AssetDatabase.LoadAssetAtPath(files[i], typeof(Object));
                            if (@object != null)
                            {
                                if (@object is Texture2D)
                                {
                                    Texture2D texture2D = (Texture2D)@object;
                                    if (this.buildAtlasType == BuildAtlasType.AllImageInDirectory)
                                    {
                                        flag = true;
                                        list.Add(texture2D);
                                    }
                                    else if (this.buildAtlasType == BuildAtlasType.ReferenceImage)
                                    {
                                        if (allReferenceImage.Contains(texture2D.name))
                                        {
                                            flag = true;
                                            list.Add(texture2D);
                                        }
                                        else
                                        {
                                            Debug.LogError("khong co ref:" + texture2D.name);
                                        }
                                    }
                                }
                            }
                        }
                        if (!flag)
                        {
                            this.pathImages = "";
                            EditorUtility.DisplayDialog("Error Input Data", "Can not found any image texture in folder directory " + this.pathImages, "OK");
                        }
                        string text = this.pathOutputs + "/" + this.atlasName + ".png";
                        if (File.Exists(text))
                        {
                            File.Delete(text);
                        }
                        bool flag2 = TexturePacker.AutoBuildAtlasFromListTexture(list, this.listInfoFinal, this.trimType, text, this.padingSize);
                        if (flag2 && this.listInfoFinal.Count > 0)
                        {
                            flag2 = TexturePacker.UpdateAtlasSpriteInfo(text, this.listInfoFinal, 1f);
                        }
                        Object object2 = AssetDatabase.LoadAssetAtPath(text, typeof(Object));
                        if (object2 != null)
                        {
                            if (object2 is Texture2D)
                            {
                                this.mainTexture = (Texture2D)object2;
                            }
                        }
                        result = flag2;
                    }
                    else if (this.buildAtlasType == BuildAtlasType.NotBuildAtlas)
                    {
                        List<Texture2D> list2 = new List<Texture2D>();
                        for (int j = 0; j < files.Length; j++)
                        {
                            Object object3 = AssetDatabase.LoadAssetAtPath(files[j], typeof(Object));
                            if (object3 != null)
                            {
                                if (object3 is Texture2D)
                                {
                                    Texture2D texture2D2 = (Texture2D)object3;
                                    if (allReferenceImage.Contains(texture2D2.name))
                                    {
                                        list2.Add(texture2D2);
                                    }
                                    else
                                    {
                                        Debug.LogError("khong co ref:" + texture2D2.name);
                                    }
                                }
                            }
                        }
                        bool flag3 = TexturePacker.BuildToEachTexture(list2, this.listInfoFinal, this.trimType, this.pathOutputs);
                        this.listPathNotAtlas.Clear();
                        if (flag3)
                        {
                            for (int k = 0; k < list2.Count; k++)
                            {
                                this.listPathNotAtlas.Add(this.pathOutputs + "/" + list2[k].name + ".png");
                            }
                        }
                        result = flag3;
                    }
                    else
                    {
                        Debug.LogError("Not Support Yet:");
                        result = false;
                    }
                }
            }
            else if (this.mainTexture == null)
            {
                EditorUtility.DisplayDialog("Error Input Data", "Texture Sprite is Null, Please choose text Atlas Sprite first !", "OK");
                result = false;
            }
            else if (this.listInfoFinal.Count < 1)
            {
                EditorUtility.DisplayDialog("Error Input Data", "Animation json input files are empty!", "OK");
                result = false;
            }
            else
            {
                string assetPath = AssetDatabase.GetAssetPath(this.mainTexture);
                bool flag4 = TexturePacker.UpdateAtlasSpriteInfo(assetPath, this.listInfoFinal, this.scaleTextureImage);
                result = flag4;
            }
            return result;
        }

        private bool CreateBoneSkeleton()
        {
            Debug.Log("CreateBoneSkeleton");
            if (!Directory.Exists(this.pathOutputs))
            {
                Directory.CreateDirectory(this.pathOutputs);
            }
            this.ReadInfo(false);
            bool result;
            if (this.listInfoFinal.Count < 1)
            {
                EditorUtility.DisplayDialog("Error Input Data", "Animation Json Files input are empty!", "OK");
                result = false;
            }
            else
            {
                Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();
                if (this.buildAtlasType == BuildAtlasType.AllImageInDirectory || this.buildAtlasType == BuildAtlasType.ReferenceImage)
                {
                    string text = this.pathOutputs + "/" + this.atlasName + ".png";
                    if (this.tabBuildAtlas == 1)
                    {
                        if (this.mainTexture == null)
                        {
                            EditorUtility.DisplayDialog("Error Input Data", "Texture Sprite is Null, Please choose text Atlas Sprite first !" + this.pathImages, "OK");
                            result = false;
                            return result;
                        }
                        text = AssetDatabase.GetAssetPath(this.mainTexture);
                    }
                    Sprite[] array = AssetDatabase.LoadAllAssetsAtPath(text).OfType<Sprite>().ToArray<Sprite>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        dictionary[array[i].name] = array[i];
                    }
                }
                else if (this.buildAtlasType == BuildAtlasType.NotBuildAtlas)
                {
                    for (int j = 0; j < this.listPathNotAtlas.Count; j++)
                    {
                        string text2 = this.listPathNotAtlas[j];
                        Sprite[] array2 = AssetDatabase.LoadAllAssetsAtPath(text2).OfType<Sprite>().ToArray<Sprite>();
                        for (int k = 0; k < array2.Length; k++)
                        {
                            dictionary[array2[k].name] = array2[k];
                        }
                    }
                }
                GameObject gameObject = null;
                GameObject gameObject2 = null;
                if (this.buildSpriteType == SpriteType.UGUI)
                {
                    gameObject = GameObject.Find("CanvasUI");
                    if (gameObject == null)
                    {
                        gameObject = new GameObject();
                        gameObject.name=("CanvasUI");
                        Canvas canvas = gameObject.AddComponent<Canvas>();
                        canvas.renderMode = RenderMode.WorldSpace;
                        CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
                        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
                        gameObject.AddComponent<GraphicRaycaster>();
                    }
                    if (gameObject != null)
                    {
                        gameObject2 = GameObject.Find(this.aepName);
                    }
                }
                else
                {
                    gameObject2 = GameObject.Find(this.aepName);
                }
                if (gameObject2 != null)
                {
                    if (!this.autoOverride)
                    {
                        if (!EditorUtility.DisplayDialog("Warning", "Object " + this.aepName + " has exist in scene,Do you want replace this file?", "YES", "NO"))
                        {
                            result = false;
                            return result;
                        }
                        Object.DestroyImmediate(gameObject2);
                    }
                    else
                    {
                        Object.DestroyImmediate(gameObject2);
                    }
                }
                Dictionary<string, GameObject> dictionary2 = new Dictionary<string, GameObject>();
                Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
                Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
                for (int l = 0; l < this.listInfoFinal.Count * 2; l++)
                {
                    GameObject rootSkeleton;
                    gameObject2 = (rootSkeleton = GameObject.Find(this.aepName));
                    DataAnimAnalytics dataAnimAnalytics = this.listInfoFinal[l % this.listInfoFinal.Count];
                    Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
                    Dictionary<string, string> objShowWhenStartAnim = new Dictionary<string, string>();
                    if (gameObject2 != null)
                    {
                        dictionary2.Clear();
                        for (int m = 0; m < dataAnimAnalytics.jsonFinal.bones.Count; m++)
                        {
                            BoneElement boneElement = dataAnimAnalytics.jsonFinal.bones[m];
                            GameObject refenreceObject = AEPAnimationClipElement.GetRefenreceObject(dataAnimAnalytics.jsonFinal.GetFullPathBone(boneElement.name), gameObject2);
                            if (refenreceObject != null)
                            {
                                dictionary2[boneElement.name] = refenreceObject;
                            }
                        }
                    }
                    if (gameObject2 != null)
                    {
                        Transform[] componentsInChildren = gameObject2.GetComponentsInChildren<Transform>(true);
                        int n = 0;
                        while (n < componentsInChildren.Length)
                        {
                            GameObject gameObject3 = componentsInChildren[n].gameObject;
                            if (gameObject2 != componentsInChildren[n].gameObject)
                            {
                                if (this.buildSpriteType == SpriteType.SpriteRenderer)
                                {
                                    if (gameObject3.GetComponent<SpriteRenderer>() != null)
                                    {
                                        goto IL_531;
                                    }
                                }
                                else if (gameObject3.GetComponent<Image>() != null)
                                {
                                    goto IL_531;
                                }
                                bool flag = false;
                                for (int num = 0; num < gameObject2.transform.childCount; num++)
                                {
                                    Transform child = gameObject2.transform.GetChild(num);
                                    if (child.GetComponent<Image>() != null)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    if (!dataAnimAnalytics.jsonFinal.dicBones.ContainsKey(componentsInChildren[n].name))
                                    {
                                        string text3 = componentsInChildren[n].name;
                                        Transform transform = componentsInChildren[n];
                                        while (!(transform.parent == null) && !(transform.parent == gameObject2.transform))
                                        {
                                            text3 = transform.parent.name + "/" + text3;
                                            transform = transform.parent;
                                        }
                                        dictionary5[componentsInChildren[n].name] = text3;
                                    }
                                }
                            }
                            IL_531:
                            n++;
                            continue;
                            goto IL_531;
                        }
                    }
                    for (int num2 = 0; num2 < dataAnimAnalytics.jsonFinal.bones.Count; num2++)
                    {
                        BoneElement boneElement2 = dataAnimAnalytics.jsonFinal.bones[num2];
                        if (boneElement2.parent == null)
                        {
                            if (gameObject2 == null)
                            {
                                GameObject gameObject4 = new GameObject();
                                if (this.buildSpriteType == SpriteType.UGUI)
                                {
                                    gameObject4.transform.parent=(gameObject.transform);
                                    gameObject4.AddComponent<RectTransform>();
                                }
                                rootSkeleton = gameObject4;
                                gameObject4.transform.localPosition=(new Vector3(0f, 0f, 0f));
                                gameObject4.name=(boneElement2.name);
                                dictionary2[boneElement2.name] = gameObject4;
                                this.objectRoot = gameObject4;
                                if (this.aepName.Length > 0)
                                {
                                    gameObject4.name=(this.aepName);
                                }
                                List<SlotElelement> slotMappingWithBone = dataAnimAnalytics.jsonFinal.GetSlotMappingWithBone(boneElement2.name);
                                for (int num3 = 0; num3 < slotMappingWithBone.Count; num3++)
                                {
                                    this.BuildSlotAndAttachment(slotMappingWithBone[num3], gameObject4, rootSkeleton, dictionary, dataAnimAnalytics, this.buildSpriteType, ref dictionary3, ref dictionary4);
                                }
                            }
                        }
                        else
                        {
                            GameObject gameObject5 = null;
                            if (dictionary2.TryGetValue(boneElement2.parent, out gameObject5))
                            {
                                GameObject gameObject6 = null;
                                dictionary2.TryGetValue(boneElement2.name, out gameObject6);
                                if (gameObject6 == null)
                                {
                                    GameObject gameObject7 = new GameObject();
                                    gameObject7.transform.parent=(gameObject5.transform);
                                    gameObject7.transform.localPosition=(new Vector3(boneElement2.x, boneElement2.y, 0f));
                                    gameObject7.transform.localScale=(new Vector3(boneElement2.scaleX, boneElement2.scaleY, 1f));
                                    Quaternion localRotation = gameObject7.transform.localRotation;
                                    localRotation.eulerAngles=(new Vector3(0f, 0f, boneElement2.rotation));
                                    gameObject7.transform.localRotation=(localRotation);
                                    gameObject7.name=(boneElement2.name);
                                    if (this.buildSpriteType == SpriteType.UGUI)
                                    {
                                        int index = boneElement2.index;
                                        int num4 = 0;
                                        for (int num5 = 0; num5 < gameObject5.transform.childCount; num5++)
                                        {
                                            Transform child2 = gameObject5.transform.GetChild(num5);
                                            BoneElement boneElement3 = null;
                                            if (dataAnimAnalytics.jsonFinal.dicBones.TryGetValue(child2.name, out boneElement3))
                                            {
                                                if (boneElement3.index > index)
                                                {
                                                    num4++;
                                                }
                                            }
                                        }
                                        gameObject7.transform.SetSiblingIndex(num4);
                                        RectTransform rectTransform = gameObject7.AddComponent<RectTransform>();
                                    }
                                    dictionary2[boneElement2.name] = gameObject7;
                                    List<SlotElelement> slotMappingWithBone2 = dataAnimAnalytics.jsonFinal.GetSlotMappingWithBone(boneElement2.name);
                                    for (int num6 = 0; num6 < slotMappingWithBone2.Count; num6++)
                                    {
                                        this.BuildSlotAndAttachment(slotMappingWithBone2[num6], gameObject7, rootSkeleton, dictionary, dataAnimAnalytics, this.buildSpriteType, ref dictionary3, ref dictionary4);
                                    }
                                    if (gameObject2 != null)
                                    {
                                        gameObject7.SetActive(false);
                                    }
                                    gameObject6 = gameObject7;
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Parent Null: " + boneElement2.name);
                            }
                        }
                    }
                    dataAnimAnalytics.AddObjectHideWhenStartAnim(dictionary5);
                    dataAnimAnalytics.AddObjectShowWhenStartAnim(objShowWhenStartAnim);
                }
                result = true;
            }
            return result;
        }

        public void BuildSlotAndAttachment(SlotElelement slot, GameObject objBone, GameObject rootSkeleton, Dictionary<string, Sprite> dicSprite, DataAnimAnalytics dataAnimAnalytics, SpriteType buildSpriteType, ref Dictionary<string, string> objHideForAllAnim, ref Dictionary<string, string> objShowForAllAnim)
        {
            if (!(objBone == null))
            {
                for (int i = 0; i < slot.listAcceptAttachment.Count; i++)
                {
                    EAPInfoAttachment eAPInfoAttachment = slot.listAcceptObj[i];
                    EAPInfoAttachment eAPInfoAttachment2 = null;
                    dataAnimAnalytics.jsonFinal.dicPivot.TryGetValue(eAPInfoAttachment.spriteName, out eAPInfoAttachment2);
                    if (eAPInfoAttachment != null)
                    {
                        GameObject gameObject = new GameObject();
                        gameObject.transform.parent=(objBone.transform);
                        gameObject.transform.localScale=(new Vector3(this.scaleInfo / this.scaleTextureImage * eAPInfoAttachment.scaleX, this.scaleInfo / this.scaleTextureImage * eAPInfoAttachment.scaleY, this.scaleInfo / this.scaleTextureImage));
                        Quaternion localRotation = gameObject.transform.localRotation;
                        gameObject.transform.localPosition=(new Vector3(0f, 0f, 0f));
                        if (eAPInfoAttachment2 != null)
                        {
                            Vector3 zero = Vector3.zero;
                            zero.x = eAPInfoAttachment.offsetX - eAPInfoAttachment2.offsetX;
                            zero.y = eAPInfoAttachment.offsetY - eAPInfoAttachment2.offsetY;
                            gameObject.transform.localPosition=(zero);
                        }
                        gameObject.name=(eAPInfoAttachment.spriteName);
                        localRotation.eulerAngles=(new Vector3(0f, 0f, eAPInfoAttachment.rotation));
                        gameObject.transform.localRotation=(localRotation);
                        if (buildSpriteType == SpriteType.SpriteRenderer)
                        {
                            gameObject.AddComponent<SpriteRenderer>();
                            SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
                            if (dicSprite.ContainsKey(eAPInfoAttachment.spriteName))
                            {
                                component.sprite=(dicSprite[eAPInfoAttachment.spriteName]);
                            }
                            else if (dicSprite.ContainsKey(eAPInfoAttachment.slotName))
                            {
                                component.sprite=(dicSprite[eAPInfoAttachment.slotName]);
                            }
                            else
                            {
                                Debug.LogWarning("not found suitable spite:" + eAPInfoAttachment.spriteName + " in " + eAPInfoAttachment.slotName);
                            }
                            if (this.sortType == SortLayerType.Depth)
                            {
                                component.sortingOrder=(this.defaultSortDepthValue - slot.index);
                            }
                            else
                            {
                                component.sortingOrder=(this.defaultSortDepthValue);
                                float num = 0.1f * (float)slot.index;
                                Vector3 localPosition = gameObject.transform.localPosition;
                                gameObject.transform.localPosition=(new Vector3(localPosition.x, localPosition.y, num));
                            }
                        }
                        else
                        {
                            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
                            gameObject.AddComponent<Image>();
                            Image component2 = gameObject.GetComponent<Image>();
                            if (dicSprite.ContainsKey(eAPInfoAttachment.spriteName))
                            {
                                Sprite sprite = dicSprite[eAPInfoAttachment.spriteName];
                                component2.rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
                                Bounds bounds = sprite.bounds;
                                float num2 = -bounds.center.x / bounds.extents.x / 2f + 0.5f;
                                float num3 = -bounds.center.y / bounds.extents.y / 2f + 0.5f;
                                Vector2 pivot = new Vector2(num2, num3);
                                component2.rectTransform.pivot=(pivot);
                                gameObject.transform.localPosition=(new Vector3(0f, 0f, 0f));
                                component2.sprite=(dicSprite[eAPInfoAttachment.spriteName]);
                            }
                            else if (dicSprite.ContainsKey(eAPInfoAttachment.slotName))
                            {
                                component2.sprite=(dicSprite[eAPInfoAttachment.slotName]);
                            }
                            else
                            {
                                Debug.LogWarning("not found suitable spite:" + eAPInfoAttachment.spriteName + " in " + eAPInfoAttachment.slotName);
                            }
                            if (this.sortType != SortLayerType.Depth)
                            {
                                float num4 = 0.1f * (float)slot.index;
                                Vector3 localPosition2 = gameObject.transform.localPosition;
                                gameObject.transform.localPosition=(new Vector3(localPosition2.x, localPosition2.y, num4));
                            }
                        }
                        string fullPathBone = EditorUtil.GetFullPathBone(rootSkeleton.transform, gameObject.transform);
                        if (slot.attachment != null && eAPInfoAttachment.spriteName == slot.attachment)
                        {
                            objShowForAllAnim[fullPathBone] = fullPathBone;
                        }
                        else
                        {
                            gameObject.SetActive(false);
                            objHideForAllAnim[fullPathBone] = fullPathBone;
                        }
                    }
                }
            }
        }

        private bool BuildAnimation(bool bController = true)
        {
            Debug.Log("BuildAnimation");
            if (!Directory.Exists(this.pathOutputs))
            {
                Directory.CreateDirectory(this.pathOutputs);
            }
            if (this.listInfoFinal.Count < 1)
            {
                this.CreateBoneSkeleton();
            }
            GameObject gameObject = GameObject.Find(this.aepName);
            this.objectRoot = gameObject;
            string str = this.pathOutputs;
            bool result;
            if (this.objectRoot == null)
            {
                EditorUtility.DisplayDialog("Reference object Error", "Can not find Object name " + this.aepName + " in scene", "OK");
                result = false;
            }
            else
            {
                if (this.objectRoot != null)
                {
                    int i = 0;
                    while (i < this.listInfoFinal.Count)
                    {
                        AnimatorController animatorController = null;
                        DataAnimAnalytics dataAnimAnalytics = this.listInfoFinal[i];
                        {
                            if (bController)
                            {
                                string text = this.aepName + ".controller";
                                string text2 = str + "/" + text;
                                Animator component = this.objectRoot.GetComponent<Animator>();
                                if (component == null)
                                {
                                    this.objectRoot.AddComponent<Animator>();
                                    component = this.objectRoot.GetComponent<Animator>();
                                }
                                animatorController = AssetDatabase.LoadAssetAtPath(text2, typeof(AnimatorController)) as AnimatorController;
                                if (animatorController == null)
                                {
                                    animatorController = this.OnProcessCreateAnimatorController(text, text2);
                                    component.runtimeAnimatorController = (animatorController);
                                    goto IL_1E3;
                                }
                                if (i == 0)
                                {
                                    bool flag = false;
                                    if (this.autoOverride)
                                    {
                                        flag = true;
                                    }
                                    else if (EditorUtility.DisplayDialog("Confimation", "File Anim " + text2 + " has already Exist, do you want to replace", "YES", "NO"))
                                    {
                                        flag = true;
                                    }
                                    if (!flag)
                                    {
                                        goto IL_3E5;
                                    }
                                    AssetDatabase.DeleteAsset(text2);
                                    AssetDatabase.Refresh();
                                    animatorController = this.OnProcessCreateAnimatorController(text, text2);
                                }
                                component.runtimeAnimatorController = (animatorController);
                            }
                            goto IL_1E3;
                        }
                        IL_3E5:
                        {
                            i++;
                            continue;
                        }
                        IL_1E3:
                        {
                            string text3 = dataAnimAnalytics.filename + ".anim";
                            string text4 = str + "/" + text3;
                            AnimationClip animationClip = AssetDatabase.LoadAssetAtPath(text4, typeof(AnimationClip)) as AnimationClip;
                            if (animationClip == null)
                            {
                                animationClip = this.OnProcessAnimFile(dataAnimAnalytics, text3, text4);
                            }
                            else
                            {
                                bool flag2 = false;
                                if (this.autoOverride)
                                {
                                    flag2 = true;
                                }
                                else if (EditorUtility.DisplayDialog("Confimation", "File Anim " + text4 + " has already Exist, do you want to replace", "YES", "NO"))
                                {
                                    flag2 = true;
                                }
                                if (!flag2)
                                {
                                    goto IL_3E5;
                                }
                                AssetDatabase.DeleteAsset(text4);
                                AssetDatabase.Refresh();
                                animationClip = this.OnProcessAnimFile(dataAnimAnalytics, text3, text4);
                            }
                            if (animationClip != null && animatorController!=null)
                            {
                                AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;
                                ChildAnimatorState[] states = stateMachine.states;
                                bool flag3 = false;
                                for (int j = 0; j < states.Length; j++)
                                {
                                    if (states[j].state.name == animationClip.name)
                                    {
                                        states[j].state.motion = (animationClip);
                                        flag3 = true;
                                        stateMachine.defaultState = (states[j].state);
                                        break;
                                    }
                                }
                                if (!flag3)
                                {
                                    AnimatorState animatorState = stateMachine.AddState(animationClip.name);
                                    animatorState.name = (animationClip.name);
                                    animatorState.motion = (animationClip);
                                    stateMachine.defaultState = (animatorState);
                                    animatorController.AddParameter(animationClip.name, AnimatorControllerParameterType.Trigger);
                                    AnimatorStateTransition animatorStateTransition = stateMachine.AddAnyStateTransition(animatorState);
                                    animatorStateTransition.name = (animationClip.name);
                                    animatorStateTransition.duration = (0f);
                                    animatorStateTransition.exitTime = (1f);
                                    animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0f, animationClip.name);
                                }
                            }
                            goto IL_3E5;
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        private AnimationClip OnProcessAnimFile(DataAnimAnalytics dataAnimAnalytics, string name, string animPath)
        {
            AnimationClip animationClip = new AnimationClip();
            animationClip.name=(name);
            animationClip.frameRate=(30f);
            if (dataAnimAnalytics.animationStyle == AnimationStyle.Loop)
            {
                SerializedObject serializedObject = new SerializedObject(animationClip);
                EditorUtil.AnimationClipSettings animationClipSettings = new EditorUtil.AnimationClipSettings(serializedObject.FindProperty("m_AnimationClipSettings"));
                animationClipSettings.loopTime = true;
                serializedObject.ApplyModifiedProperties();
            }
            for (int i = 0; i < dataAnimAnalytics.jsonFinal.bones.Count; i++)
            {
                BoneElement boneElement = dataAnimAnalytics.jsonFinal.bones[i];
                string fullPathBone = dataAnimAnalytics.jsonFinal.GetFullPathBone(boneElement.name);
                GameObject refenreceObject = AEPAnimationClipElement.GetRefenreceObject(fullPathBone, this.objectRoot);
                if (refenreceObject != null && !refenreceObject.activeSelf)
                {
                    AEPAnimationClipElement aEPAnimationClipElement = new AEPAnimationClipElement();
                    aEPAnimationClipElement.AddStartVisible(boneElement.name, fullPathBone, this.objectRoot, dataAnimAnalytics.jsonFinal, true);
                    for (int j = 0; j < aEPAnimationClipElement.listCurve.Count; j++)
                    {
                        AnimationUtility.SetEditorCurve(animationClip, aEPAnimationClipElement.listCurve[j].binding, aEPAnimationClipElement.listCurve[j].curve);
                    }
                }
            }
            foreach (KeyValuePair<string, string> current in dataAnimAnalytics.objHideWhenStartAnim)
            {
                AEPAnimationClipElement aEPAnimationClipElement2 = new AEPAnimationClipElement();
                aEPAnimationClipElement2.AddStartVisible(current.Key, current.Value, this.objectRoot, dataAnimAnalytics.jsonFinal, false);
                for (int k = 0; k < aEPAnimationClipElement2.listCurve.Count; k++)
                {
                    AnimationUtility.SetEditorCurve(animationClip, aEPAnimationClipElement2.listCurve[k].binding, aEPAnimationClipElement2.listCurve[k].curve);
                }
            }
            foreach (KeyValuePair<string, string> current2 in dataAnimAnalytics.objShowWhenStartAnim)
            {
                AEPAnimationClipElement aEPAnimationClipElement3 = new AEPAnimationClipElement();
                if (dataAnimAnalytics.filename.Contains("hit"))
                {
                }
                aEPAnimationClipElement3.AddStartVisible(current2.Key, current2.Value, this.objectRoot, dataAnimAnalytics.jsonFinal, true);
                for (int l = 0; l < aEPAnimationClipElement3.listCurve.Count; l++)
                {
                    AnimationUtility.SetEditorCurve(animationClip, aEPAnimationClipElement3.listCurve[l].binding, aEPAnimationClipElement3.listCurve[l].curve);
                }
            }
            foreach (KeyValuePair<string, AEPBoneAnimationElement> current3 in dataAnimAnalytics.jsonFinal.dicAnimation)
            {
                string fullPathBone2 = dataAnimAnalytics.jsonFinal.GetFullPathBone(current3.Key);
                AEPAnimationClipElement aEPAnimationClipElement4 = new AEPAnimationClipElement();
                aEPAnimationClipElement4.AddTranformAnimation(current3.Value, fullPathBone2, this.objectRoot, dataAnimAnalytics.jsonFinal);
                for (int m = 0; m < aEPAnimationClipElement4.listCurve.Count; m++)
                {
                    AnimationUtility.SetEditorCurve(animationClip, aEPAnimationClipElement4.listCurve[m].binding, aEPAnimationClipElement4.listCurve[m].curve);
                }
            }
            foreach (KeyValuePair<string, AEPSlotAnimationElement> current4 in dataAnimAnalytics.jsonFinal.dicSlotAttactment)
            {
                string fullPathBone3 = dataAnimAnalytics.jsonFinal.GetFullPathBone(current4.Key);
                AEPAnimationClipElement aEPAnimationClipElement5 = new AEPAnimationClipElement();
                aEPAnimationClipElement5.AddAttactmentAnimation(current4.Value, fullPathBone3, this.objectRoot, dataAnimAnalytics.jsonFinal, this.buildSpriteType);
                for (int n = 0; n < aEPAnimationClipElement5.listCurve.Count; n++)
                {
                    AnimationUtility.SetEditorCurve(animationClip, aEPAnimationClipElement5.listCurve[n].binding, aEPAnimationClipElement5.listCurve[n].curve);
                }
            }
            EditorUtility.SetDirty(animationClip);
            AssetDatabase.CreateAsset(animationClip, animPath);
            AssetDatabase.ImportAsset(animPath);
            AssetDatabase.Refresh();
            return animationClip;
        }

        private AnimatorController OnProcessCreateAnimatorController(string name, string animPath)
        {
            AnimatorController.CreateAnimatorControllerAtPath(animPath);
            AnimatorController result = AssetDatabase.LoadAssetAtPath(animPath, typeof(AnimatorController)) as AnimatorController;
            AssetDatabase.Refresh();
            return result;
        }
    }
}
#endif