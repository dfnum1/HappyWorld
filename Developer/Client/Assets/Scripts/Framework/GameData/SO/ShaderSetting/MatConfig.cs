/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	全局材质参数
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.Data;
namespace TopGame.Data
{
    [System.Serializable]
    public class MatConfig : AConfig
    {
        public class PropertyID
        {
            public int scene_colorID;
            public int actor_colorID;
        }

        PropertyID propertyIDs = new PropertyID();
        [Framework.Data.DisplayNameGUI("场景全局颜色")]
        public Color sceneColor = Color.white;
        [Framework.Data.DisplayNameGUI("角色全局颜色")]
        public Color actorColor = Color.white;
        public void Init(Framework.Module.AFrameworkBase pFramewrok)
        {
            propertyIDs.scene_colorID = Shader.PropertyToID("game_environment_color");

            propertyIDs.actor_colorID = Shader.PropertyToID("game_actor_color");
        }
        public void Apply()
        {
            if (propertyIDs.scene_colorID == 0) return;
            Shader.SetGlobalVector(propertyIDs.scene_colorID, sceneColor);
            Shader.SetGlobalVector(propertyIDs.actor_colorID, actorColor);

        }
#if UNITY_EDITOR
        public void Save() { }
        //------------------------------------------------------
        public void OnInspector(System.Object param = null)
        {
            UnityEditor.EditorGUILayout.LabelField("材质参数:");
            UnityEditor.EditorGUI.BeginChangeCheck();
            Framework.ED.HandleUtilityWrapper.DrawProperty(this, null);
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                Init(null);
                Apply();
            }
        }
#endif
    }
}
