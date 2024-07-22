/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	EditorInitalizeOnLoad
作    者:	HappLI
描    述:	unity 编辑器加载启动回调
*********************************************************************/
#if UNITY_EDITOR
using Framework.ED;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TopGame.ED
{
    [InitializeOnLoad]
    public class EditorInitalizeOnLoad
    {
        static EditorInitalizeOnLoad()
        {
          //  SetMacros();
            SetOrientation();

            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += SceneSavingCallback;
            UnityEditor.SceneManagement.EditorSceneManager.sceneClosing += SceneClosingCallback;
            HandleUtilityWrapper.CheckInspector();

            RenderPipelineAsset urpAsset= AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");
            if(QualitySettings.renderPipeline == null)
                QualitySettings.renderPipeline = urpAsset;

            if (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null)
                UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = urpAsset;
        }
        //------------------------------------------------------
        static void OnPlayModeStateChange(PlayModeStateChange state)
        {
            if (EditorApplication.isPlaying)
                Framework.Module.ModuleManager.getInstance().ResumeJobs();
            else
                Framework.Module.ModuleManager.getInstance().PauseJobs();

            if(state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorKits.StopAllAudioClips();
                UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");
            }
        }
        //------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {

        }
        //------------------------------------------------------
        static void SceneSavingCallback(UnityEngine.SceneManagement.Scene scene, string path)
        {

        }
        //------------------------------------------------------
        static void SceneClosingCallback(UnityEngine.SceneManagement.Scene scene, bool removingScene)
        {
            Base.Util.Desytroy(GameObject.Find("#TerrainBlockRoots#"));
        }
        //------------------------------------------------------
        static void SetMacros()
        {
            AutoMarcros.SetMacros(null);
        }
        //------------------------------------------------------
        static void SetOrientation()
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        }
    }
}
#endif
