/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	MenuTools
作    者:	HappLI
描    述:	Csv 解析代码自动生成器
*********************************************************************/
using Framework.Core;
using Framework.Plugin.Guide;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopGame.ED
{
    public class MenuTools
    {
        [MenuItem("Tools/RunGame _F5")]
        static void RunGame()
        {
            if (!EditorWindowMgr.CloseAll())
            {
                return;
            }
            ED.EditorHelp.OpenStartUpApplication("Assets/Scenes/Startup.unity");
        }
        //------------------------------------------------------
        [MenuItem("Tools/Pause _F10")]
        static void PauseGame()
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        //------------------------------------------------------
        [MenuItem("Tools/Step _F11")]
        static void NextStep()
        {
            EditorApplication.Step();
        }
        //------------------------------------------------------
        [MenuItem("Tools/同步磁盘")]
        static void ForceSyncSave()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //------------------------------------------------------
        [MenuItem("Tools/代码/代码自动化")]
        static void AutoCode()
        {
          //  ExternEngine.EditorCreateLUT.CreateAllLUT();
            UI.UICodeGenerator.Build();
            Logic.StateCodeGenerator.Build();
            ActionEventCoreGenerator.Build();
            Data.DBTypeAutoCode.Build();
            Framework.Data.AutoCodeAttrParam.Build();
            //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //------------------------------------------------------
        [MenuItem("Tools/代码/重新编译 _#F3")]
        static void ReCompilation()
        {
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }
        //------------------------------------------------------
        [MenuItem("Tools/代码/检测宏")]
        static void CheckMarco()
        {
            AutoMarcros.SetMacros(null);
        }
        //------------------------------------------------------
        [MenuItem("Tools/编辑相机 _F2")]
        static void EditorGameCamera()
        {
            if (!Application.isPlaying || GameInstance.getInstance()== null) return;
            if (GameInstance.getInstance().GetState() < Logic.EGameState.Login) return;
            if (GameInstance.getInstance().cameraController == null || GameInstance.getInstance().cameraController.GetTransform() == null)
                return;
            Selection.activeObject = GameInstance.getInstance().cameraController.GetTransform().gameObject;
            ((Core.CameraController)GameInstance.getInstance().cameraController).SetEditor(true);
        }
        //------------------------------------------------------
        [MenuItem("Tools/地表笔刷 _F4")]
        static void EditorTerrainBrush()
        {
            if (!Application.isPlaying || GameInstance.getInstance() == null) return;
            Core.Brush.TerrainBrush brush = GameObject.FindObjectOfType<Core.Brush.TerrainBrush>();
            if (!brush) return;
            Selection.activeObject = brush.gameObject;
        }
        //------------------------------------------------------
        [MenuItem("Tools/更新Layer")]
        static void CheckTagAndLayer()
        {
            ClearLayerAndTag();
            AddLayer("BackLayer", (int)EMaskLayer.BackLayer);
            AddLayer("ForeLayer", (int)EMaskLayer.ForeLayer);
            AddLayer("EffectLayer", (int)EMaskLayer.EffectLayer);
            AddLayer("UI_3D", (int)EMaskLayer.UI_3D);
            AddLayer("UIBG_3D", (int)EMaskLayer.UIBG_3D);
            AddLayer("UIBGAfter_3D", (int)EMaskLayer.UIBGAfter_3D);

            AddLayer("PhysicLayer", (int)EMaskLayer.PhysicLayer);
            AddLayer("Editor", 31);

            AddTag("Terrain");
            AddTag("Timeline");
        }
        //------------------------------------------------------
        [MenuItem("Assets/ExportNavToObj")]
        static void ExportNavMeshToObj()
        {
            if (Selection.activeObject == null) return;
            UnityEngine.AI.NavMeshData navMesh = Selection.activeObject as UnityEngine.AI.NavMeshData;
            if (navMesh == null)
            {
                EditorUtility.DisplayDialog("提示", "不是一个有效的导航网格!", "诶");
                return;
            }
            UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
            UnityEngine.AI.NavMeshDataInstance instance = UnityEngine.AI.NavMesh.AddNavMeshData(navMesh);

            UnityEngine.AI.NavMeshTriangulation tmpNavMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            string saveFile = EditorUtility.SaveFilePanel("导出", Application.dataPath, navMesh.name, "obj");

            StreamWriter tmpStreamWriter = new StreamWriter(saveFile);

            //顶点
            for (int i = 0; i < tmpNavMeshTriangulation.vertices.Length; i++)
            {
                tmpStreamWriter.WriteLine("v  " + tmpNavMeshTriangulation.vertices[i].x + " " + tmpNavMeshTriangulation.vertices[i].y + " " + tmpNavMeshTriangulation.vertices[i].z);
            }

            tmpStreamWriter.WriteLine("gapAngle pPlane1");

            //索引
            for (int i = 0; i < tmpNavMeshTriangulation.indices.Length;)
            {
                tmpStreamWriter.WriteLine("f " + (tmpNavMeshTriangulation.indices[i] + 1) + " " + (tmpNavMeshTriangulation.indices[i + 1] + 1) + " " + (tmpNavMeshTriangulation.indices[i + 2] + 1));
                i = i + 3;
            }

            tmpStreamWriter.Flush();
            tmpStreamWriter.Close();

            UnityEngine.AI.NavMesh.RemoveNavMeshData(instance);
        }
        //------------------------------------------------------
        static void ClearLayerAndTag()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            for (int i = 0; i < layers.arraySize; i++)
            {
                SerializedProperty data = layers.GetArrayElementAtIndex(i);
                data.stringValue = "";
            }

            SerializedProperty tags = tagManager.FindProperty("tags");
            for (int i = 0; i < tags.arraySize; i++)
            {
                SerializedProperty data = tags.GetArrayElementAtIndex(i);
                data.stringValue = "";
            }
            tagManager.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        static void AddLayer(string layer, int idLayer)
        {
            var targetName = "Element " + idLayer;
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            for (int i = 0; i < layers.arraySize; i++)
            {
                SerializedProperty data = layers.GetArrayElementAtIndex(i);
                if (data.displayName == targetName)
                {
                    data.stringValue = layer;
                    tagManager.ApplyModifiedProperties();
                    return;
                }
            }
        }
        //------------------------------------------------------
        static void AddTag(string tag)
        {
            if (!isHasTag(tag))
            {
                UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                if ((asset != null) && (asset.Length > 0))
                {
                    SerializedObject so = new SerializedObject(asset[0]);
                    SerializedProperty tags = so.FindProperty("tags");

                    for (int i = 0; i < tags.arraySize; ++i)
                    {
                        if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                        {
                            return;     // Tag already present, nothing to do.
                        }
                    }
                    int size = tags.arraySize;
                    tags.InsertArrayElementAtIndex(size);
                    tags.GetArrayElementAtIndex(size).stringValue = tag;
                    so.ApplyModifiedProperties();
                    so.Update();
                }
            }
        }
        //------------------------------------------------------
        static bool isHasTag(string tag)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                    return true;
            }
            return false;
        }
        //------------------------------------------------------
        [MenuItem("Tools/MacTest")]
        static void MacTest()
        {
            Debug.Log("click Mac Test");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("open","/Users/a2020026//Desktop/work_sd/JSJ/Server/"));
            //System.Diagnostics.Process.Start(
            //"open",
            //" -a Terminal --args -open /Users/a2020026/Desktop/work_sd/JSJ/Server/"
            //);
        }
        //------------------------------------------------------
        [MenuItem("Tools/获取所有UI上的Guide组件对应id")]
        static void GetAllUIWithGuid()
        {
            string[] scenes = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
            string[] prefabs = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);

            List<string> textList = new List<string>();
            Dictionary<int, GuideGuid> sceneDic = new Dictionary<int, GuideGuid>();
            List<string> repeatSceneGuid = new List<string>();

            Dictionary<int, GuideGuid> prefabsDic = new Dictionary<int, GuideGuid>();
            List<string> repeatPrefabsGuid = new List<string>();

            repeatSceneGuid.Add("====================================================");
            repeatPrefabsGuid.Add("====================================================");

            for (int i = 0; i < scenes.Length; i++)
            {
                EditorUtility.DisplayProgressBar("搜索 unity 文件中", scenes[i], (float)i / scenes.Length);

                Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenes[i]);

                foreach (var item in scene.GetRootGameObjects())
                {
                    GuideGuid[] guides = item.GetComponentsInChildren<GuideGuid>(true);

                    if (guides.Length > 0)
                    {
                        textList.Add("***************" + scenes[i] + "***************************");
                    }

                    foreach (var guide in guides)
                    {
                        textList.Add("id:" + guide.Guid + ",name:" + guide.name);
                        if (!sceneDic.ContainsKey(guide.Guid))
                        {
                            sceneDic.Add(guide.Guid, guide);
                        }
                        else
                        {
                            repeatSceneGuid.Add("重复的guid:" + guide.Guid + ",name:" + guide.name + ",scene:" + scenes[i]);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();

            for (int i = 0; i < prefabs.Length; i++)
            {
                EditorUtility.DisplayProgressBar("搜索 prefab 文件中", prefabs[i], (float)i / prefabs.Length);

                string path = prefabs[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null)
                {
                    continue;
                }
                GuideGuid[] guides = prefab.GetComponentsInChildren<GuideGuid>(true);

                if (guides.Length > 0)
                {
                    textList.Add("***************" + path + "***************************");
                }

                foreach (var guide in guides)
                {
                    textList.Add("id:" + guide.Guid + ",name:" + guide.name);

                    if (!prefabsDic.ContainsKey(guide.Guid))
                    {
                        prefabsDic.Add(guide.Guid, guide);
                    }
                    else
                    {
                        repeatPrefabsGuid.Add("重复的guid:" + guide.Guid + ",name:" + guide.name + ",prefab:" + path);
                    }
                }
            }
            EditorUtility.ClearProgressBar();

            string filePath = Application.dataPath + "/../Local/AllUIWithGuid.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter sw;
            sw = File.CreateText(filePath);
            foreach (var item in textList)
            {
                sw.WriteLine(item);
            }
            foreach (var item in repeatSceneGuid)
            {
                sw.WriteLine(item);
            }
            foreach (var item in repeatPrefabsGuid)
            {
                sw.WriteLine(item);
            }
            sw.Close();

            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filePath,0,0);
        }
        //------------------------------------------------------
        [MenuItem("Tools/批量处理TimelineURP")]
        public static void BatchUrpAsset()
        {
            if (!EditorUtility.DisplayDialog("提示", "批量处理Timeline URP 资源", "执行", "取消"))
                return;
            List<string> vFilesss = new List<string>();
            string RootParticlePath = "Assets/Datas/Role";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            vFilesss = new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());

            UnityEngine.Rendering.RenderPipelineAsset urpAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");
            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset1 = urpAsset as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
            if (urpAsset1 == null) return;
            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("Timeline Prefab", file, (float)startIndex / (float)vFilesss.Count);


                var obj = AssetDatabase.LoadAssetAtPath(file.Replace('\\', '/'), typeof(UnityEngine.Object));

                GameObject prefab = obj as GameObject;

                if (prefab != null)
                {
                    var text = prefab.GetComponent<Core.TimelineController>();

                    if (text && text.urpAsset != urpAsset1)
                    {
                        text.urpAsset = urpAsset1;
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
                    Debug.Log("处理结束");

                }
            };
        }
    }
}

