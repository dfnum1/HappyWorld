/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	SceneTheme
作    者:	HappLI
描    述:   场景主题数据
*********************************************************************/
using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Data
{
    [System.Serializable]
    public class SceneThemeData : BaseData
    {
        [System.Serializable]
        public struct DirLightData
        {
            public string lightName;
            public Vector3 position;
            public Vector3 eulerAngle;
            public LightmapBakeType lightmapType;
            public Color color;
            public float intensity;
            public float indirectMultiplier;
            public LightShadows shadowType;
            public float shadowStrength;
            public float shadowNearPlane;
            public LightRenderMode renderMode;

            [Framework.Data.DisplayEnumBitGUI(typeof(Base.EMaskLayer), true)]
            public int layerMask;
#if UNITY_EDITOR
            [System.NonSerialized]
            public Light m_pEditor;
            public void OnInspector(System.Object param = null)
            {
                if(param!=null)
                {
                    UnityEditor.EditorGUILayout.LabelField("灯光[" + param.ToString() + "]");
                    UnityEditor.EditorGUI.indentLevel++;
                }
                m_pEditor = UnityEditor.EditorGUILayout.ObjectField("灯光数据", m_pEditor, typeof(Light), true) as Light;
                if (m_pEditor != null)
                {
                    lightName = m_pEditor.name;
                    position = m_pEditor.transform.position;
                    eulerAngle = m_pEditor.transform.eulerAngles;
                    lightmapType = m_pEditor.lightmapBakeType;
                    color = m_pEditor.color;
                    intensity = m_pEditor.intensity;
                    indirectMultiplier = m_pEditor.bounceIntensity;
                    shadowType = m_pEditor.shadows;
                    renderMode = m_pEditor.renderMode;
                    layerMask = m_pEditor.cullingMask;
                    shadowStrength = m_pEditor.shadowStrength;
                    shadowNearPlane = m_pEditor.shadowNearPlane;
                }
                //        else
                {
                    lightName = UnityEditor.EditorGUILayout.TextField("Name", lightName);
                    position = UnityEditor.EditorGUILayout.Vector3Field("position", position);
                    eulerAngle = UnityEditor.EditorGUILayout.Vector3Field("eulerAngle", eulerAngle);
                    lightmapType = (LightmapBakeType)UnityEditor.EditorGUILayout.EnumPopup("Mode", lightmapType);
                    color = UnityEditor.EditorGUILayout.ColorField("Color", color);
                    intensity = UnityEditor.EditorGUILayout.FloatField("Intensity", intensity);
                    indirectMultiplier = UnityEditor.EditorGUILayout.FloatField("Indirect Multiplier", indirectMultiplier);
                    shadowType = (LightShadows)UnityEditor.EditorGUILayout.EnumPopup("Shadow Type", shadowType);
                    if(shadowType == LightShadows.Soft)
                    {
                        shadowStrength = UnityEditor.EditorGUILayout.Slider("Strength", shadowStrength,0,1);
                        shadowNearPlane = UnityEditor.EditorGUILayout.Slider("Near Plane", shadowNearPlane, 0.1f, 10);
                    }
                    renderMode = (LightRenderMode)UnityEditor.EditorGUILayout.EnumPopup("Render Mode", renderMode);

                    this = (DirLightData)Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(this, "layerMask");
                }
                
                if (param != null)
                    UnityEditor.EditorGUI.indentLevel--;
            }
#endif
        }

        [Framework.Plugin.DisableGUI]
        public int nID = 0;

        [Framework.Data.DisplayNameGUI("主题描述")]
        public string strName="";

        [Framework.Data.DisplayNameGUI("Loading 背景")]
        [Framework.Data.StringViewGUI(typeof(Texture2D))]
        public string loading = "";

        [DisplayNameGUI("战斗预备音乐"), Framework.Data.StringViewGUI("FMODUnity.EventReference", -1), Framework.Data.StringViewGUI(typeof(AudioClip))]
        public string PrefabMusic = "";

        [DisplayNameGUI("战斗开始音乐"), Framework.Data.StringViewGUI("FMODUnity.EventReference", -1), Framework.Data.StringViewGUI(typeof(AudioClip))]
        public string BattleMusic = "";

        [DisplayNameGUI("天空盒材质")]
        [StringViewGUI(typeof(Material))]
        public string SkyBoxMat = "";

        [DisplayNameGUI("近景资源")]
        [StringViewGUI(typeof(GameObject))]
        public string NearEffect = "";
        [DisplayNameGUI("近景保持距离")]
        public float NearKeepDistance = 2;
        [DisplayNameGUI("近景资源位置偏移")]
        public Vector3 NearOffset = Vector3.zero;
        [DisplayNameGUI("近景资源旋转")]
        public Vector3 NearRotate = Vector3.zero;

        [DisplayNameGUI("远景资源")]
        [StringViewGUI(typeof(GameObject))]
        public string FarBGScene = "";
        [DisplayNameGUI("远景保持距离")]
        public float BGKeepDistance = 200;
        [DisplayNameGUI("远景资源位置偏移")]
        public Vector3 FarOffset = Vector3.zero;
        [DisplayNameGUI("远景资源旋转偏移")]
        public Vector3 FarRotate = Vector3.zero;

        [DisplayNameGUI("是否开启雾")]
        public bool EnableFog = true;
        [DisplayNameGUI("雾颜色")]
        [StateGUIByField("EnableFog", "true")]
        public Color FogColor = new Color(0.28f, 0.75f, 1f, 1f);
        [DisplayNameGUI("雾强度")]
        [StateGUIByField("EnableFog", "true")]
        [Framework.Plugin.DisableGUI]
        public float FogDensity = 1;
        [DisplayNameGUI("雾起始距离")]
        [StateGUIByField("EnableFog", "true")]
        public float FogStart = 20;
        [DisplayNameGUI("雾结束距离")]
        [StateGUIByField("EnableFog", "true")]
        public float FogEnd = 200;

        [DisplayNameGUI("环境色")]
        public Color EnvironmentColor = Color.white;

        [DisplayNameGUI("过渡快慢(<=0为没过渡)")]
        public float fLerp = 0.5f;

        [Framework.Data.DisplayNameGUI("灯光信息")]
        public DirLightData[] lightDatas;

        [DisplayNameGUI("背景音乐过渡")]
        public AnimationCurve MusicFade;

        [DisplayNameGUI("渲染扭曲轴向")]
        public Vector3 CurveBlendAixs = Vector3.zero;

        [DisplayNameGUI("渲染扭曲偏移")]
        public Vector3 CurveBlendOffset = Vector3.zero;
        public void Mapping(Framework.Module.AFrameworkBase moudle)
        {

        }
#if UNITY_EDITOR
        public void Save()
        {

        }
        public void Copy(SceneThemeData pData)
        {
            if (pData == null) return;
            PrefabMusic = pData.PrefabMusic;
            BattleMusic = pData.BattleMusic;
            SkyBoxMat = pData.SkyBoxMat;
            FarBGScene = pData.FarBGScene;
            BGKeepDistance = pData.BGKeepDistance;
            EnableFog = pData.EnableFog;
            FogColor = pData.FogColor;
            FogDensity = pData.FogDensity;
            FogStart = pData.FogStart;
            FogEnd = pData.FogEnd;
            strName = pData.strName;
            CurveBlendOffset = pData.CurveBlendOffset;
            CurveBlendAixs = pData.CurveBlendAixs;
            lightDatas = null;
            if(pData.lightDatas!=null && pData.lightDatas.Length>0)
            {
                lightDatas = new DirLightData[pData.lightDatas.Length];
                for(int i =0;i < lightDatas.Length; ++i)
                {
                    lightDatas[i] = pData.lightDatas[i];
                }
            }
        }
#endif
    }
}

