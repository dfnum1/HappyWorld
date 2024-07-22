/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	RunerSceneBG
作    者:	HappLI
描    述:	场景背景
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Module;
using Framework.Plugin.AT;

namespace TopGame.Logic
{
    public class SceneTheme : ASceneTheme
    {
        //------------------------------------------------------
        public override bool UseTheme(uint theme, bool bClear = false, bool bLerpTo = true)
        {
            DungonTheme themeData = DataManager.getInstance().DungonThemes.GetData((int)theme);
            if (themeData == null)
            {
                if (bClear) Clear();
                return false;
            }
             UseTheme(themeData, bClear, bLerpTo);
            return true;
        }
        //------------------------------------------------------
        public override bool UseTheme(IUserData pData, bool bClear = false, bool bLerpTo = true)
        {
            if (pData == null || !(pData is SceneThemeData)) return false;
            UseTheme(pData as SceneThemeData, bClear, bLerpTo);
            return true;
        }
        //------------------------------------------------------
        public void UseTheme(SceneThemeData theme, bool bClear = false, bool bLerpTo = true)
        {
            if (bClear) Clear();
            if (theme == null) return;
            if (m_nCurTheme == theme.nID) return;
            m_nCurTheme = theme.nID;
            UpdateFog(theme.EnableFog, theme.FogColor, theme.FogStart, theme.FogEnd, theme.FogDensity, bLerpTo?theme.fLerp:0);

            m_bCleared = false;
            SetFarDistance(theme.BGKeepDistance, theme.FarOffset, theme.FarRotate);
            SetNearDistance(theme.NearKeepDistance,theme.NearOffset,theme.NearRotate);

            UpdateBG(theme.FarBGScene);
            UpdateNearEffect(theme.NearEffect);
            UpdateSkyBox(theme.SkyBoxMat);

            if (!Framework.BattlePlus.BattleKits.IsStarting(GameInstance.getInstance()))
                UpdateMusic(theme.PrefabMusic, theme.MusicFade);
            else
                UpdateMusic(theme.BattleMusic, theme.MusicFade);

            SetEnvironmentColor(theme.EnvironmentColor, bLerpTo ? theme.fLerp : 0);

            Base.GlobalShaderController.EnableCurve(theme.CurveBlendAixs.sqrMagnitude>0);
            Base.GlobalShaderController.SetBlend(theme.CurveBlendAixs, theme.CurveBlendOffset);

            if (theme.lightDatas!=null)
            {
                SceneThemeData.DirLightData lightData;
                Transform transform;
                for (int i = 0; i < theme.lightDatas.Length; ++i)
                {
                    lightData = theme.lightDatas[i];
                    Light light = AddLight(lightData.lightName);
                    if(light == null) continue;
                    light.type = LightType.Directional;
#if UNITY_EDITOR
                    light.lightmapBakeType = lightData.lightmapType;
#endif
                    transform = light.transform;
                    transform.position = lightData.position;
                    transform.eulerAngles = lightData.eulerAngle;
                    light.color = lightData.color;
                    light.intensity = lightData.intensity;
                    light.bounceIntensity = lightData.indirectMultiplier;
                    light.shadows = lightData.shadowType;
                    light.shadowStrength = lightData.shadowStrength;
                    light.shadowNearPlane = lightData.shadowNearPlane;
                    light.renderMode = lightData.renderMode;
                }
            }
        }
    }
}

