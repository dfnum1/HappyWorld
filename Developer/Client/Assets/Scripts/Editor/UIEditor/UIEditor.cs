using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class UIEditor : EditorWindow
    {
        static UIEditor ms_pInstance;
        GameObject m_pUISystem = null;
        Canvas m_pRootCanvs = null;
        GameObject m_pReferToPic = null;
        //------------------------------------------------------
        [MenuItem("Tools/UI/编辑器")]
        public static void OpenTool()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (ms_pInstance == null)
            {
                EditorKits.NewScene();
                UIEditor window = EditorWindow.GetWindow<UIEditor>();
                window.titleContent = new GUIContent("UI 编辑器");
            }
        }
        //------------------------------------------------------
        [MenuItem("Tools/UI/全部导出")]
        public static void ExportTool()
        {
            List<FileInfo> files = new List<FileInfo>();
            EditorKits.FindDirFiles(UIEditorConfig.UIScenesPath, files);
            List<string> vScenes = new List<string>();
            for(int i = 0; i < files.Count; ++i)
            {
                if (files[i].Extension.ToLower().CompareTo(".unity")!=0) continue;
                vScenes.Add((files[i].FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets")));
            }
            if (vScenes.Count <= 0)
            {
                EditorUtility.DisplayDialog("警告", "当前目录:[" + UIEditorConfig.UIScenesPath + "] 为空", "确定");
                return;
            }

            UnityEngine.SceneManagement.Scene curScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            for (int i = 0; i < vScenes.Count; ++i)
            {
                UnityEngine.SceneManagement.Scene scene;
                bool bUnload = true;
                if (curScene.path.CompareTo(vScenes[i]) == 0)
                {
                    scene = curScene;
                    bUnload = false;
                }
                else
                    scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(vScenes[i], UnityEditor.SceneManagement.OpenSceneMode.Additive);
                if(scene.IsValid())
                {
                    List<UI.UserInterface> uis = new List<UI.UserInterface>();
                    GameObject[] objects = scene.GetRootGameObjects();
                    for(int j = 0; j < objects.Length; ++j)
                    {
                        uis.Clear();
                        EditorKits.FindComponents<UI.UserInterface>(objects[j], uis);
                        for(int k = 0; k < uis.Count; ++k)
                        {
                            UIEditorHelper.ExportUIPrefab(uis[k]);
                        }
                    }
                }
                if (bUnload)
                {
                    AsyncOperation op = UnityEditor.SceneManagement.EditorSceneManager.UnloadSceneAsync(scene);
                    while (!op.isDone)
                        break;
                }
            }
        }
        //------------------------------------------------------
        public static UIEditor Instance
        {
            get { return ms_pInstance; }
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            ms_pInstance.OnSceneGUI(sceneView);
        }
        //------------------------------------------------------
        public GameObject referTo
        {
            get { return m_pReferToPic; }
        }
        //------------------------------------------------------
        protected void OnEnable()
        {
            ms_pInstance = this;
     //       minSize = new Vector2(1280, 720);
            SceneView.duringSceneGui += OnSceneFunc;
            CheckUIRoot();
        }
        //------------------------------------------------------
        protected void OnDisable()
        {
            ms_pInstance = null;
            SceneView.duringSceneGui -= OnSceneFunc;
            if (m_pReferToPic) GameObject.DestroyImmediate(m_pReferToPic);
//             if(m_pUISystem)
//                 GameObject.DestroyImmediate(m_pUISystem);
        }
        //------------------------------------------------------
        public void CheckUIRoot()
        {
            UnityEngine.SceneManagement.Scene curScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            if (curScene.name == "Startup") return;
            if (GameObject.Find("UIRoot") != null) return;

            if (m_pUISystem != null) return;
            GameObject pAsset = AssetDatabase.LoadAssetAtPath<GameObject>(UIEditorConfig.UISystemPrefab);
            if (pAsset)
            {
                m_pReferToPic = new GameObject("referTo", new System.Type[] { typeof(Canvas), typeof(UnityEngine.UI.Image) });
                //m_pReferToPic.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.HideAndDontSave;
                Base.Util.ResetGameObject(m_pReferToPic, Base.EResetType.All);
                RectTransform trans = m_pReferToPic.transform as RectTransform;
                
                m_pUISystem = GameObject.Instantiate(pAsset);
                m_pUISystem.name = "UIRoot";
                m_pRootCanvs = m_pUISystem.GetComponentInChildren<Canvas>();

                trans.SetParent(m_pRootCanvs.transform);
                trans.sizeDelta = new Vector2(720, 1280);               
                trans.pivot = Vector2.zero;
                trans.anchorMax = Vector3.one;
                trans.anchorMin = Vector3.zero;
                trans.localScale = Vector3.one;
                trans.anchoredPosition = Vector3.zero;
                trans.offsetMin = Vector2.zero;
                trans.offsetMax = Vector2.zero;

                m_pReferToPic.SetActive(false);

                Canvas.ForceUpdateCanvases();

            }
        }
        //------------------------------------------------------
        public void SetNewInterface(UI.UserInterface pUI)
        {
            CheckUIRoot();
            if (pUI == null) return;
            pUI.transform.SetParent(m_pRootCanvs.transform);
            RectTransform rect = pUI.GetComponent<RectTransform>();        
            rect.offsetMax = Vector3.zero;
            rect.offsetMin = Vector3.zero;
            rect.localScale = Vector3.one;
        }
        //------------------------------------------------------
        private void OnGUI()
        {

        }       
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (Event.current != null && Event.current.button == 1 && Event.current.type == EventType.MouseUp)
            {
                if (Selection.gameObjects == null || Selection.gameObjects.Length == 0 || Selection.gameObjects[0].transform is RectTransform)
                {
                    if(UIMenuFunc.AddCommonItems(Selection.gameObjects))
                        Event.current.Use();
                }
            }
        }
    }

}

