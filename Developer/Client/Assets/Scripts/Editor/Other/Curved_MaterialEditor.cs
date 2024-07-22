using System;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using TopGame.ED;
using System.Collections.Generic;

namespace UnityEditor
{
    public static class CurvedWorld_EditorGlobals
    {
        public static string curvedWorldTag = "CurvedWorldTag";

        public static string curvedWorldAvailableOptions = "CurvedWorldAvailableOptions";
    }
    internal class Curved_MaterialEditor : ShaderGUI
    {
        private MaterialProperty _V_CW_Rim_Color;

        private MaterialProperty _V_CW_Rim_Power;

        private MaterialProperty _V_CW_Rim_Bias;

        private MaterialProperty _EmissionMap;

        private MaterialProperty _EmissionColor;

        private MaterialProperty _V_CW_ReflectColor;

        private MaterialProperty _V_CW_ReflectStrengthAlphaOffset;

        private MaterialProperty _V_CW_Cube;

        private MaterialProperty _V_CW_NormalMap;

        private MaterialProperty _V_CW_NormalMap_UV_Scale;

        private MaterialProperty _V_CW_NormalMapStrength;

        private MaterialProperty _V_CW_SecondaryNormalMap;

        private MaterialProperty _V_CW_SecondaryNormalMap_UV_Scale;

        private MaterialProperty _V_CW_Specular_Intensity;

        private MaterialProperty _V_CW_SpecularOffset;

        private MaterialProperty _V_CW_Specular_Lookup;

        private MaterialProperty _SpecColor;

        private MaterialProperty _Shininess;

        private MaterialProperty _V_CW_IBL_Intensity;

        private MaterialProperty _V_CW_IBL_Contrast;

        private MaterialProperty _V_CW_IBL_Cube;

        private MaterialProperty _V_CW_IBL_Matcap;

        private MaterialProperty _V_CW_Fresnel_Bias;

        private MaterialProperty _V_CW_Fresnel_Power;

        private MaterialProperty _V_CW_LightRampTex;

        private MaterialProperty _V_CW_UnityAmbient;

        private MaterialProperty _V_CW_PerVertexLights;

        private MaterialEditor m_MaterialEditor;

        private Material material;

        private string materialTag;

        private string curvedWorldAvailableOptions;

        public static Curved_MaterialEditor get;

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            base.OnGUI(materialEditor, props);
            this.FindProperties(props);
            this.m_MaterialEditor = materialEditor;
            this.material = (materialEditor.target as Material);
            this.materialTag = this.material.GetTag(CurvedWorld_EditorGlobals.curvedWorldTag, false);
            if (string.IsNullOrEmpty(this.materialTag) || this.materialTag.Contains("Custom"))
            {
                return;
            }
            this.curvedWorldAvailableOptions = this.material.GetTag(CurvedWorld_EditorGlobals.curvedWorldAvailableOptions, false);
            if (string.IsNullOrEmpty(this.curvedWorldAvailableOptions))
            {
                return;
            }
            this.ShaderPropertiesGUI();
        }

        public void FindProperties(MaterialProperty[] props)
        {
            this._V_CW_Rim_Color = ShaderGUI.FindProperty("_V_CW_Rim_Color", props, false);
            this._V_CW_Rim_Power = ShaderGUI.FindProperty("_V_CW_Rim_Power", props, false);
            this._V_CW_Rim_Bias = ShaderGUI.FindProperty("_V_CW_Rim_Bias", props, false);
            this._EmissionMap = ShaderGUI.FindProperty("_EmissionMap", props, false);
            this._EmissionColor = ShaderGUI.FindProperty("_EmissionColor", props, false);
            this._V_CW_ReflectColor = ShaderGUI.FindProperty("_V_CW_ReflectColor", props, false);
            this._V_CW_ReflectStrengthAlphaOffset = ShaderGUI.FindProperty("_V_CW_ReflectStrengthAlphaOffset", props, false);
            this._V_CW_Cube = ShaderGUI.FindProperty("_V_CW_Cube", props, false);
            this._V_CW_NormalMap = ShaderGUI.FindProperty("_V_CW_NormalMap", props, false);
            this._V_CW_NormalMap_UV_Scale = ShaderGUI.FindProperty("_V_CW_NormalMap_UV_Scale", props, false);
            this._V_CW_NormalMapStrength = ShaderGUI.FindProperty("_V_CW_NormalMapStrength", props, false);
            this._V_CW_SecondaryNormalMap = ShaderGUI.FindProperty("_V_CW_SecondaryNormalMap", props, false);
            this._V_CW_SecondaryNormalMap_UV_Scale = ShaderGUI.FindProperty("_V_CW_SecondaryNormalMap_UV_Scale", props, false);
            this._V_CW_Specular_Intensity = ShaderGUI.FindProperty("_V_CW_Specular_Intensity", props, false);
            this._V_CW_SpecularOffset = ShaderGUI.FindProperty("_V_CW_SpecularOffset", props, false);
            this._V_CW_Specular_Lookup = ShaderGUI.FindProperty("_V_CW_Specular_Lookup", props, false);
            this._SpecColor = ShaderGUI.FindProperty("_SpecColor", props, false);
            this._Shininess = ShaderGUI.FindProperty("_Shininess", props, false);
            this._V_CW_IBL_Intensity = ShaderGUI.FindProperty("_V_CW_IBL_Intensity", props, false);
            this._V_CW_IBL_Contrast = ShaderGUI.FindProperty("_V_CW_IBL_Contrast", props, false);
            this._V_CW_IBL_Cube = ShaderGUI.FindProperty("_V_CW_IBL_Cube", props, false);
            this._V_CW_IBL_Matcap = ShaderGUI.FindProperty("_V_CW_IBL_Matcap", props, false);
            this._V_CW_Fresnel_Bias = ShaderGUI.FindProperty("_V_CW_Fresnel_Bias", props, false);
            this._V_CW_Fresnel_Power = ShaderGUI.FindProperty("_V_CW_Fresnel_Power", props, false);
            this._V_CW_LightRampTex = ShaderGUI.FindProperty("_V_CW_LightRampTex", props, false);
            this._V_CW_UnityAmbient = ShaderGUI.FindProperty("_V_CW_UnityAmbient", props, false);
            this._V_CW_PerVertexLights = ShaderGUI.FindProperty("_V_CW_PerVertexLights", props, false);
        }

        public void ShaderPropertiesGUI()
        {
            GUILayout.Space(5f);
            using (new EditorKits.GUIBackgroundColor(EditorGUIUtility.isProSkin ? Color.white : Color.grey))
            {
                EditorGUILayout.LabelField("Additional Rendering Options", EditorStyles.toolbarButton, new GUILayoutOption[0]);
            }
            GUILayout.Space(5f);
            this.Draw_VertexColor();
            this.Draw_Fog();
            this.Draw_RecieveUnityAmbientLighting();
            this.Draw_PerVertexLight();
            this.Draw_Rim();
            this.Draw_Reflective();
            this.Draw_IBL();
            this.Draw_Bump();
            this.Draw_Specular_Mobile();
            this.Draw_Specular_HD();
            this.Draw_LightRamp();
            this.Draw_Emission();
            this.Draw_RangeFade();
            GUILayout.Space(10f);
        }

        private void Draw_VertexColor()
        {
            if (!this.curvedWorldAvailableOptions.Contains("VERTEX_COLOR;") || !this.material.HasProperty("_V_CW_IncludeVertexColor"))
            {
                return;
            }
            bool flag = (double)this.material.GetFloat("_V_CW_IncludeVertexColor") > 0.5;
            EditorGUILayout.LabelField("Include Vertex Color", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.material.SetFloat("_V_CW_IncludeVertexColor", (float)(flag ? 1 : 0));
            }
        }

        bool ContainKeyWorld(string world)
        {
            if (this.material.shaderKeywords == null) return false;
            for(int i = 0; i < this.material.shaderKeywords.Length; ++i)
            {
                if (this.material.shaderKeywords[i].CompareTo(world) == 0) return true;
            }
            return false;
        }
        private void Draw_Reflective()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_REFLECTIVE;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_REFLECTIVE");
            EditorGUILayout.LabelField("Reflection", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_REFLECTIVE", string.Empty, string.Empty);
            }
            if (flag)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    this.m_MaterialEditor.ColorProperty(this._V_CW_ReflectColor, "Color");
                    this.m_MaterialEditor.RangeProperty(this._V_CW_ReflectStrengthAlphaOffset, "Alpha Offset");
                    this._V_CW_Cube.textureValue = ((Cubemap)EditorGUILayout.ObjectField("CubeMap", this._V_CW_Cube.textureValue, typeof(Cubemap), true, new GUILayoutOption[]
                    {
                    GUILayout.MaxHeight(16f)
                    }));
                    if (this.materialTag.Contains("Unlit") || this.materialTag.Contains("One Directional Light"))
                    {
                        this.m_MaterialEditor.RangeProperty(this._V_CW_Fresnel_Bias, "Fresnel Bias");
                    }
                    else
                    {
                        this.m_MaterialEditor.RangeProperty(this._V_CW_Fresnel_Power, "Fresnel Power");
                    }
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_Emission()
        {
            if (!this.curvedWorldAvailableOptions.Contains("_EMISSION;"))
            {
                return;
            }
            if (this.material.shader.name.Contains("Legacy"))
            {
                GUILayout.Space(10f);
                if (this.m_MaterialEditor.EmissionEnabledProperty())
                {
                    this.m_MaterialEditor.ColorProperty(this._EmissionColor, "   Color (HDR)");
                    this._EmissionMap.textureValue =((Texture2D)EditorGUILayout.ObjectField("   Map", this._EmissionMap.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                    {
                    GUILayout.MaxHeight(16f)
                    }));
                    this.m_MaterialEditor.LightmapEmissionFlagsProperty(2, true);
                }
                MaterialEditor.FixupEmissiveFlag(this.material);
                bool value = (this.material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
                this.ModifyKeyWords(value, "_EMISSION", string.Empty, null);
            }
            else
            {
                bool flag = ContainKeyWorld("_EMISSION");
                EditorGUILayout.LabelField(new GUIContent("Emission", "Enable emisssion"), flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
                EditorGUI.BeginChangeCheck();
                flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
                if (EditorGUI.EndChangeCheck())
                {
                    this.ModifyKeyWords(flag, "_EMISSION", string.Empty, null);
                }
                if (flag)
                {
                    this.m_MaterialEditor.ColorProperty(this._EmissionColor, "   Color (HDR)");
                    this._EmissionMap.textureValue = ((Texture2D)EditorGUILayout.ObjectField("   Map", this._EmissionMap.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                    {
                    GUILayout.MaxHeight(16f)
                    }));
                }
            }
            GUILayout.Space(5f);
        }

        private void Draw_Rim()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_RIM;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_RIM");
            EditorGUILayout.LabelField("Rim Effect", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_RIM", string.Empty, null);
            }
            if (flag)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    this.m_MaterialEditor.ColorProperty(this._V_CW_Rim_Color, "Color");
                    if (this.materialTag.Contains("Unlit") || this.materialTag.Contains("VertexLit") || this.materialTag.Contains("One Directional Light") || this.materialTag.Contains("Matcap"))
                    {
                        this.m_MaterialEditor.RangeProperty(this._V_CW_Rim_Bias, "Bias");
                    }
                    else
                    {
                        this.m_MaterialEditor.RangeProperty(this._V_CW_Rim_Power, "Power");
                    }
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_Fog()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_FOG;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_FOG");
            EditorGUILayout.LabelField(new GUIContent("Receive Fog", "Shader will recieve Unity fog in Forward Rendering mode"), flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_FOG", string.Empty, null);
            }
        }

        private void Draw_RecieveUnityAmbientLighting()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_UNITY_AMBIENT;"))
            {
                return;
            }
            bool flag = this._V_CW_UnityAmbient.floatValue > 0.5f;
            EditorGUILayout.LabelField("Recieve Unity Ambient Lighting", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this._V_CW_UnityAmbient.floatValue = ((float)(flag ? 1 : 0));
            }
        }

        private void Draw_PerVertexLight()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_PERVERTEX_LIGHT;"))
            {
                return;
            }
            bool flag = this._V_CW_PerVertexLights.floatValue > 0.5f;
            EditorGUILayout.LabelField("Recieve Per-Vertex Lights", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this._V_CW_PerVertexLights.floatValue = ((float)(flag ? 1 : 0));
            }
        }

        private void Draw_IBL()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_IBL;"))
            {
                return;
            }
            int num = 0;
            if (ContainKeyWorld("V_CW_IBL_CUBE"))
            {
                num = 1;
            }
            else if (ContainKeyWorld("V_CW_IBL_MATCAP"))
            {
                num = 2;
            }
            EditorGUILayout.LabelField("Image Based Lighting", (num > 0) ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            num = EditorGUI.IntPopup(GUILayoutUtility.GetLastRect(), " ", num, new string[]
            {
            "None",
            "Simple Cube",
            "Matcap"
            }, new int[]
            {
            0,
            1,
            2
            });
            if (EditorGUI.EndChangeCheck())
            {
                if (num == 0)
                {
                    this.ModifyKeyWords(false, "V_CW_IBL_CUBE", string.Empty, "V_CW_IBL_MATCAP");
                }
                else if (num == 1)
                {
                    this.ModifyKeyWords(true, "V_CW_IBL_CUBE", string.Empty, "V_CW_IBL_MATCAP");
                }
                else if (num == 2)
                {
                    this.ModifyKeyWords(true, "V_CW_IBL_MATCAP", "V_CW_IBL_CUBE", string.Empty);
                }
            }
            if (num != 0)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    if (num == 1)
                    {
                        this.m_MaterialEditor.FloatProperty(this._V_CW_IBL_Intensity, "Brightness");
                        this.m_MaterialEditor.FloatProperty(this._V_CW_IBL_Contrast, "Contrast");
                        this._V_CW_IBL_Cube.textureValue = ((Cubemap)EditorGUILayout.ObjectField("CubeMap", this._V_CW_IBL_Cube.textureValue, typeof(Cubemap), true, new GUILayoutOption[]
                        {
                        GUILayout.MaxHeight(16f)
                        }));
                    }
                    else if (num == 2)
                    {
                        num = (ContainKeyWorld("V_CW_MATCAP_BLEND_ADD") ? 1 : 0);
                        EditorGUI.BeginChangeCheck();
                        num = EditorGUILayout.IntPopup("Blend Mode", num, new string[]
                        {
                        "Multiply",
                        "Add"
                        }, new int[]
                        {
                        0,
                        1
                        }, new GUILayoutOption[0]);
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (num == 0)
                            {
                                this.ModifyKeyWords(false, "V_CW_MATCAP_BLEND_ADD", "V_CW_MATCAP_BLEND_MULTIPLY", null);
                            }
                            else
                            {
                                this.ModifyKeyWords(true, "V_CW_MATCAP_BLEND_ADD", "V_CW_MATCAP_BLEND_MULTIPLY", null);
                            }
                        }
                        this.m_MaterialEditor.FloatProperty(this._V_CW_IBL_Intensity, "Intensity");
                        this._V_CW_IBL_Matcap.textureValue = ((Texture2D)EditorGUILayout.ObjectField("Matcap", this._V_CW_IBL_Matcap.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                        {
                        GUILayout.MaxHeight(16f)
                        }));
                    }
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_Bump()
        {
            if (!this.curvedWorldAvailableOptions.Contains("_NORMALMAP;"))
            {
                return;
            }
            bool flag = this.materialTag.Contains("Unlit");
            bool flag2 = this.materialTag.Contains("One Directional Light") || this.materialTag.Contains("Matcap") || this.materialTag.Contains("Legacy");
            if ((flag && (ContainKeyWorld("V_CW_REFLECTIVE") || ContainKeyWorld("V_CW_IBL_CUBE") || ContainKeyWorld("V_CW_IBL_MATCAP"))) || flag2)
            {
                bool flag3 = ContainKeyWorld("_NORMALMAP");
                EditorGUILayout.LabelField(new GUIContent("Bump", flag ? "For Unlit shaders available only with Reflection or IBL." : "Enable Bump"), flag3 ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
                EditorGUI.BeginChangeCheck();
                flag3 = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag3);
                if (EditorGUI.EndChangeCheck())
                {
                    this.ModifyKeyWords(flag3, "_NORMALMAP", string.Empty, null);
                }
                if (flag3)
                {
                    using (new EditorKits.EditorGUIIndentLevel(2))
                    {
                        this.m_MaterialEditor.FloatProperty(this._V_CW_NormalMapStrength, "Strength");
                        this._V_CW_NormalMap.textureValue = ((Texture2D)EditorGUILayout.ObjectField("Normal Map", this._V_CW_NormalMap.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                        {
                        GUILayout.MaxHeight(16f)
                        }));
                        this.m_MaterialEditor.FloatProperty(this._V_CW_NormalMap_UV_Scale, "   UV Scale");
                        if (this.materialTag.Contains("Decal") || this.materialTag.Contains("Detail") || this.materialTag.Contains("Vertex Alpha"))
                        {
                            this._V_CW_SecondaryNormalMap.textureValue = ((Texture2D)EditorGUILayout.ObjectField("Secondary Normal Map", this._V_CW_SecondaryNormalMap.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                            {
                            GUILayout.MaxHeight(16f)
                            }));
                            this.m_MaterialEditor.FloatProperty(this._V_CW_SecondaryNormalMap_UV_Scale, "   UV Scale");
                        }
                    }
                    GUILayout.Space(5f);
                    return;
                }
            }
            else if (flag)
            {
                using (new EditorKits.GUIEnabled(false))
                {
                    EditorGUILayout.Toggle(new GUIContent("Bump", "For Unlit shaders available only with Reflection or IBL."), ContainKeyWorld("_NORMALMAP"), new GUILayoutOption[0]);
                }
            }
        }

        private void Draw_Specular_Mobile()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_SPECULAR_LOOKUP;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_SPECULAR");
            EditorGUILayout.LabelField(new GUIContent("Specular", "Fast specular caluclation using lookup texture."), flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_SPECULAR", string.Empty, null);
            }
            if (flag)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    if (this._V_CW_Specular_Lookup.type == MaterialProperty.PropType.Texture)
                    {
                        this._V_CW_Specular_Lookup.textureValue = ((Texture2D)EditorGUILayout.ObjectField("Lookup Map", this._V_CW_Specular_Lookup.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                        {
                        GUILayout.MaxHeight(16f)
                        }));
                        this.CheckTextureWarpModeForLookup(ref this._V_CW_Specular_Lookup);
                    }
                    this.m_MaterialEditor.RangeProperty(this._V_CW_Specular_Intensity, "Intensity");
                    this.m_MaterialEditor.RangeProperty(this._V_CW_SpecularOffset, "Value Offset");
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_Specular_HD()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_SPECULAR_HD;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_SPECULAR");
            EditorGUILayout.LabelField(new GUIContent("Specular", "Enable Gloss and Specular."), flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_SPECULAR", string.Empty, null);
            }
            if (flag)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    this.m_MaterialEditor.ColorProperty(this._SpecColor, "Color");
                    this.m_MaterialEditor.RangeProperty(this._Shininess, "Shininess");
                    if (this._Shininess.floatValue > this._Shininess.rangeLimits.y)
                    {
                        this._Shininess.floatValue = (this._Shininess.rangeLimits.y);
                    }
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_LightRamp()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_USE_LIGHT_RAMP_TEXTURE;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_USE_LIGHT_RAMP_TEXTURE");
            EditorGUILayout.LabelField(new GUIContent("Light Ramp (Toon Shading)", "Use light ramp texture"), flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_USE_LIGHT_RAMP_TEXTURE", string.Empty, null);
            }
            if (flag)
            {
                using (new EditorKits.EditorGUIIndentLevel(2))
                {
                    this._V_CW_LightRampTex.textureValue = ((Texture2D)EditorGUILayout.ObjectField("Lookup Texture", this._V_CW_LightRampTex.textureValue, typeof(Texture2D), true, new GUILayoutOption[]
                    {
                    GUILayout.MaxHeight(16f)
                    }));
                    this.CheckTextureWarpModeForLookup(ref this._V_CW_LightRampTex);
                }
                GUILayout.Space(5f);
            }
        }

        private void Draw_RangeFade()
        {
            if (!this.curvedWorldAvailableOptions.Contains("V_CW_RANGE_FADE;"))
            {
                return;
            }
            bool flag = ContainKeyWorld("V_CW_RANGE_FADE");
            EditorGUILayout.LabelField("Range Fade", flag ? EditorStyles.boldLabel : EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            flag = EditorGUI.Toggle(GUILayoutUtility.GetLastRect(), " ", flag);
            if (EditorGUI.EndChangeCheck())
            {
                this.ModifyKeyWords(flag, "V_CW_RANGE_FADE", string.Empty, null);
            }
            if (flag)
            {
                EditorGUILayout.HelpBox("Range fade has effect only with 'Spiral' bend types.\nParameter values must be updated globaly from script, e.gapAngle. using RangeFadeController script.", MessageType.Warning);
            }
        }

        private void ModifyKeyWords(bool _value, string _enableStr, string _disableStr_1, string _disableStr_2 = null)
        {
            List<string> list = new List<string>(this.material.shaderKeywords);
            list.Remove(_enableStr);
            list.Remove(_disableStr_1);
            if (!string.IsNullOrEmpty(_disableStr_2))
            {
                list.Remove(_disableStr_2);
            }
            if (_value)
            {
                list.Add(_enableStr);
            }
            Undo.RecordObject(this.material, "Curved World - Toggle " + _enableStr);
            this.material.shaderKeywords = list.ToArray();
            EditorUtility.SetDirty(this.material);
        }

        public static bool IsShaderValid(Material _material)
        {
            return !(_material == null) && !string.IsNullOrEmpty(_material.GetTag(CurvedWorld_EditorGlobals.curvedWorldTag, false));
        }

        public void CheckTextureWarpModeForLookup(ref MaterialProperty _texture)
        {
            if (_texture == null || _texture.textureValue == null)
            {
                return;
            }
            if (_texture.textureValue.wrapMode ==  TextureWrapMode.Repeat && this.m_MaterialEditor.HelpBoxWithButton(new GUIContent("Lookup textures need \"Clamp\" wrap mode."), new GUIContent("Fix")))
            {
                string assetPath = AssetDatabase.GetAssetPath(_texture.textureValue.GetInstanceID());
                TextureImporter textureImporter = null;
                try
                {
                    textureImporter = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                }
                catch
                {
                    textureImporter = null;
                }
                if (textureImporter != null)
                {
                    textureImporter.wrapMode = TextureWrapMode.Clamp;
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}
