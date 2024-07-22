/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	全局参数设置
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
#endif
namespace TopGame.Data
{
 //   [CreateAssetMenu]
    [ExecuteInEditMode]
    public class GlobalShaderConfig : ScriptableObject
    {
        public PBRLab.PBRConfig pbrConfig = new PBRLab.PBRConfig();
        public bool IsApplyChangePerFrame = true;

        static GlobalShaderConfig ms_Instance = null;
        public static GlobalShaderConfig Instance
        {
            get { return ms_Instance; }
        }
        private void OnEnable()
        {
            ms_Instance = this;
            if (pbrConfig != null)
            {
                pbrConfig.Init(null);
                pbrConfig.Apply();
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
    }
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(GlobalShaderConfig), true)]
    public class GlobalShaderConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GlobalShaderConfig controller = target as GlobalShaderConfig;
            controller = (GlobalShaderConfig)Framework.ED.HandleUtilityWrapper.DrawProperty(controller, null);

            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}
