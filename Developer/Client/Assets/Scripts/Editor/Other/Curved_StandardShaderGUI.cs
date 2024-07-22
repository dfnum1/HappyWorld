using System;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using TopGame.ED;
namespace UnityEditor
{
	internal class Curved_StandardShaderGUI : ShaderGUI
	{
		private enum WorkflowMode
		{
			Specular,
			Metallic,
			Dielectric
		}

		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,
			Transparent
		}

		public enum SmoothnessMapChannel
		{
			SpecularMetallicAlpha,
			AlbedoAlpha
		}

		private static class Styles
		{
			public static GUIContent uvSetLabel = new GUIContent("UV Set");

			public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");

			public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");

			public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A)");

			public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A)");

			public static GUIContent smoothnessText = new GUIContent("Smoothness", "Smoothness value");

			public static GUIContent smoothnessScaleText = new GUIContent("Smoothness", "Smoothness scale factor");

			public static GUIContent smoothnessMapChannelText = new GUIContent("Source", "Smoothness texture and channel");

			public static GUIContent highlightsText = new GUIContent("Specular Highlights", "Specular Highlights");

			public static GUIContent reflectionsText = new GUIContent("Reflections", "Glossy Reflections");

			public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");

			public static GUIContent heightMapText = new GUIContent("Height Map", "Height Map (G)");

			public static GUIContent occlusionText = new GUIContent("Occlusion", "Occlusion (G)");

			public static GUIContent emissionText = new GUIContent("Color", "Emission (RGB)");

			public static GUIContent detailMaskText = new GUIContent("Detail Mask", "Mask for Secondary Maps (A)");

			public static GUIContent detailAlbedoText = new GUIContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");

			public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");

			public static string primaryMapsText = "Main Maps";

			public static string secondaryMapsText = "Secondary Maps";

			public static string forwardText = "Forward Rendering Options";

			public static string renderingMode = "Rendering Mode";

			public static string advancedText = "Advanced Options";

			public static readonly string[] blendNames = Enum.GetNames(typeof(Curved_StandardShaderGUI.BlendMode));
		}

		private MaterialProperty blendMode;

		private MaterialProperty albedoMap;

		private MaterialProperty albedoColor;

		private MaterialProperty alphaCutoff;

		private MaterialProperty specularMap;

		private MaterialProperty specularColor;

		private MaterialProperty metallicMap;

		private MaterialProperty metallic;

		private MaterialProperty smoothness;

		private MaterialProperty smoothnessScale;

		private MaterialProperty smoothnessMapChannel;

		private MaterialProperty highlights;

		private MaterialProperty reflections;

		private MaterialProperty bumpScale;

		private MaterialProperty bumpMap;

		private MaterialProperty occlusionStrength;

		private MaterialProperty occlusionMap;

		private MaterialProperty heigtMapScale;

		private MaterialProperty heightMap;

		private MaterialProperty emissionColorForRendering;

		private MaterialProperty emissionMap;

		private MaterialProperty detailMask;

		private MaterialProperty detailAlbedoMap;

		private MaterialProperty detailNormalMapScale;

		private MaterialProperty detailNormalMap;

		private MaterialProperty uvSetSecondary;

		private MaterialProperty _V_CW_MainTex_Scroll;

		private MaterialProperty _V_CW_DetailTex_Scroll;

		private MaterialProperty _V_CW_OutlineColor;

		private MaterialProperty _V_CW_OutlineWidth;

		private MaterialProperty _V_CW_OutlineSizeIsFixed;

		private MaterialProperty V_CW_RangeFadeDrawer;

		private MaterialEditor m_MaterialEditor;

		private Curved_StandardShaderGUI.WorkflowMode m_WorkflowMode;

		private bool m_FirstTimeApply = true;

		public void FindProperties(MaterialProperty[] props)
		{
			this.blendMode = ShaderGUI.FindProperty("_Mode", props);
			this.albedoMap = ShaderGUI.FindProperty("_MainTex", props);
			this.albedoColor = ShaderGUI.FindProperty("_Color", props);
			this.alphaCutoff = ShaderGUI.FindProperty("_Cutoff", props);
			this.specularMap = ShaderGUI.FindProperty("_SpecGlossMap", props, false);
			this.specularColor = ShaderGUI.FindProperty("_SpecColor", props, false);
			this.metallicMap = ShaderGUI.FindProperty("_MetallicGlossMap", props, false);
			this.metallic = ShaderGUI.FindProperty("_Metallic", props, false);
			if (this.specularMap != null && this.specularColor != null)
			{
				this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Specular;
			}
			else if (this.metallicMap != null && this.metallic != null)
			{
				this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Metallic;
			}
			else
			{
				this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Dielectric;
			}
			this.smoothness = ShaderGUI.FindProperty("_Glossiness", props);
			this.smoothnessScale = ShaderGUI.FindProperty("_GlossMapScale", props, false);
			this.smoothnessMapChannel = ShaderGUI.FindProperty("_SmoothnessTextureChannel", props, false);
			this.highlights = ShaderGUI.FindProperty("_SpecularHighlights", props, false);
			this.reflections = ShaderGUI.FindProperty("_GlossyReflections", props, false);
			this.bumpScale = ShaderGUI.FindProperty("_BumpScale", props);
			this.bumpMap = ShaderGUI.FindProperty("_BumpMap", props);
			this.heigtMapScale = ShaderGUI.FindProperty("_Parallax", props);
			this.heightMap = ShaderGUI.FindProperty("_ParallaxMap", props);
			this.occlusionStrength = ShaderGUI.FindProperty("_OcclusionStrength", props);
			this.occlusionMap = ShaderGUI.FindProperty("_OcclusionMap", props);
			this.emissionColorForRendering = ShaderGUI.FindProperty("_EmissionColor", props);
			this.emissionMap = ShaderGUI.FindProperty("_EmissionMap", props);
			this.detailMask = ShaderGUI.FindProperty("_DetailMask", props);
			this.detailAlbedoMap = ShaderGUI.FindProperty("_DetailAlbedoMap", props);
			this.detailNormalMapScale = ShaderGUI.FindProperty("_DetailNormalMapScale", props);
			this.detailNormalMap = ShaderGUI.FindProperty("_DetailNormalMap", props);
			this.uvSetSecondary = ShaderGUI.FindProperty("_UVSec", props);
			this._V_CW_MainTex_Scroll = ShaderGUI.FindProperty("_V_CW_MainTex_Scroll", props);
			this._V_CW_DetailTex_Scroll = ShaderGUI.FindProperty("_V_CW_DetailTex_Scroll", props);
			this._V_CW_OutlineColor = ShaderGUI.FindProperty("_V_CW_OutlineColor", props, false);
			this._V_CW_OutlineWidth = ShaderGUI.FindProperty("_V_CW_OutlineWidth", props, false);
			this._V_CW_OutlineSizeIsFixed = ShaderGUI.FindProperty("_V_CW_OutlineSizeIsFixed", props, false);
			this.V_CW_RangeFadeDrawer = ShaderGUI.FindProperty("V_CW_RangeFadeDrawer", props, false);
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			this.FindProperties(props);
			this.m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;
			if (this.m_FirstTimeApply)
			{
				Curved_StandardShaderGUI.MaterialChanged(material, this.m_WorkflowMode);
				this.m_FirstTimeApply = false;
			}
			this.ShaderPropertiesGUI(materialEditor, material);
		}

		public void ShaderPropertiesGUI(MaterialEditor materialEditor, Material material)
		{
			EditorGUIUtility.labelWidth =0f;
			EditorGUI.BeginChangeCheck();
			this.DrawStandardShaderType(material);
			this.BlendModePopup();
			if ((int)this.blendMode.floatValue != 0 && this.V_CW_RangeFadeDrawer != null)
			{
				this.m_MaterialEditor.ShaderProperty(this.V_CW_RangeFadeDrawer, new GUIContent("Range Fade"));
			}
			GUILayout.Label(Curved_StandardShaderGUI.Styles.primaryMapsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.DoAlbedoArea(material);
			this.DoSpecularMetallicArea();
			this.DoNormalArea();
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.heightMapText, this.heightMap, (this.heightMap.textureValue != null) ? this.heigtMapScale : null);
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.occlusionText, this.occlusionMap, (this.occlusionMap.textureValue != null) ? this.occlusionStrength : null);
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.detailMaskText, this.detailMask);
			this.DoEmissionArea(material);
			this.Draw_Scroll(this._V_CW_MainTex_Scroll);
			EditorGUI.BeginChangeCheck();
			this.m_MaterialEditor.TextureScaleOffsetProperty(this.albedoMap);
			if (EditorGUI.EndChangeCheck())
			{
				this.emissionMap.textureScaleAndOffset = this.albedoMap.textureScaleAndOffset;
			}
			EditorGUILayout.Space();
			GUILayout.Label(Curved_StandardShaderGUI.Styles.secondaryMapsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.detailAlbedoText, this.detailAlbedoMap);
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.detailNormalMapText, this.detailNormalMap, this.detailNormalMapScale);
			this.Draw_Scroll(this._V_CW_DetailTex_Scroll);
			this.m_MaterialEditor.TextureScaleOffsetProperty(this.detailAlbedoMap);
			this.m_MaterialEditor.ShaderProperty(this.uvSetSecondary, Curved_StandardShaderGUI.Styles.uvSetLabel.text);
			GUILayout.Label(Curved_StandardShaderGUI.Styles.forwardText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			if (this.highlights != null)
			{
				this.m_MaterialEditor.ShaderProperty(this.highlights, Curved_StandardShaderGUI.Styles.highlightsText);
			}
			if (this.reflections != null)
			{
				this.m_MaterialEditor.ShaderProperty(this.reflections, Curved_StandardShaderGUI.Styles.reflectionsText);
			}
			if (EditorGUI.EndChangeCheck())
			{
				Object[] targets = this.blendMode.targets;
				for (int i = 0; i < targets.Length; i++)
				{
					Curved_StandardShaderGUI.MaterialChanged((Material)targets[i], this.m_WorkflowMode);
				}
			}
			EditorGUILayout.Space();
			GUILayout.Label(Curved_StandardShaderGUI.Styles.advancedText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.m_MaterialEditor.EnableInstancingField();
			this.m_MaterialEditor.DoubleSidedGIField();
			this.Draw_Outline(materialEditor, material);
		}

		internal void DetermineWorkflow(MaterialProperty[] props)
		{
			if (ShaderGUI.FindProperty("_SpecGlossMap", props, false) != null && ShaderGUI.FindProperty("_SpecColor", props, false) != null)
			{
				this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Specular;
				return;
			}
			if (ShaderGUI.FindProperty("_MetallicGlossMap", props, false) != null && ShaderGUI.FindProperty("_Metallic", props, false) != null)
			{
				this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Metallic;
				return;
			}
			this.m_WorkflowMode = Curved_StandardShaderGUI.WorkflowMode.Dielectric;
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			if (material.HasProperty("_Emission"))
			{
				material.SetColor("_EmissionColor", material.GetColor("_Emission"));
			}
			base.AssignNewShaderToMaterial(material, oldShader, newShader);
			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
			{
				Curved_StandardShaderGUI.SetupMaterialWithBlendMode(material, (Curved_StandardShaderGUI.BlendMode)material.GetFloat("_Mode"));
				return;
			}
			Curved_StandardShaderGUI.BlendMode blendMode = Curved_StandardShaderGUI.BlendMode.Opaque;
			if (oldShader.name.Contains("/Transparent/Cutout/"))
			{
				blendMode = Curved_StandardShaderGUI.BlendMode.Cutout;
			}
			else if (oldShader.name.Contains("/Transparent/"))
			{
				blendMode = Curved_StandardShaderGUI.BlendMode.Fade;
			}
			material.SetFloat("_Mode", (float)blendMode);
			Object[] array = new Material[]
			{
				material
			};
			this.DetermineWorkflow(MaterialEditor.GetMaterialProperties(array));
			Curved_StandardShaderGUI.MaterialChanged(material, this.m_WorkflowMode);
		}

		private void BlendModePopup()
		{
            EditorGUI.showMixedValue = this.blendMode.hasMixedValue;
            Curved_StandardShaderGUI.BlendMode blendMode = (Curved_StandardShaderGUI.BlendMode)this.blendMode.floatValue;
            EditorGUI.BeginChangeCheck();
            blendMode = (Curved_StandardShaderGUI.BlendMode)EditorGUILayout.Popup(Curved_StandardShaderGUI.Styles.renderingMode, (int)blendMode, Curved_StandardShaderGUI.Styles.blendNames, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                this.blendMode.floatValue = (float)blendMode;
            }
            EditorGUI.showMixedValue = false;
        }

		private void DoNormalArea()
		{
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.normalMapText, this.bumpMap, (this.bumpMap.textureValue != null) ? this.bumpScale : null);
            if (this.bumpScale.floatValue != 1f && InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget) && this.m_MaterialEditor.HelpBoxWithButton(new GUIContent("Bump scale is not supported on mobile platforms"), new GUIContent("Fix Now")))
            {
                this.bumpScale.floatValue = 1f;
            }
        }

		private void DoAlbedoArea(Material material)
		{
			this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.albedoText, this.albedoMap, this.albedoColor);
			if ((int)material.GetFloat("_Mode") == 1)
			{
				this.m_MaterialEditor.ShaderProperty(this.alphaCutoff, Curved_StandardShaderGUI.Styles.alphaCutoffText.text, 3);
			}
		}

		private void DoEmissionArea(Material material)
		{
			if (this.m_MaterialEditor.EmissionEnabledProperty())
			{
				bool flag = this.emissionMap.textureValue != null;
				this.m_MaterialEditor.TexturePropertyWithHDRColor(Curved_StandardShaderGUI.Styles.emissionText, this.emissionMap, this.emissionColorForRendering, false);
				float maxColorComponent = this.emissionColorForRendering.colorValue.maxColorComponent;
				if (this.emissionMap.textureValue != null && !flag && maxColorComponent <= 0f)
				{
					this.emissionColorForRendering.colorValue = Color.white;
				}
				this.m_MaterialEditor.LightmapEmissionFlagsProperty(2, true);
			}
		}

		private void DoSpecularMetallicArea()
		{
			bool flag = false;
			if (this.m_WorkflowMode == Curved_StandardShaderGUI.WorkflowMode.Specular)
			{
				flag = (this.specularMap.textureValue != null);
				this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.specularMapText, this.specularMap, flag ? null : this.specularColor);
			}
			else if (this.m_WorkflowMode == Curved_StandardShaderGUI.WorkflowMode.Metallic)
			{
				flag = (this.metallicMap.textureValue != null);
				this.m_MaterialEditor.TexturePropertySingleLine(Curved_StandardShaderGUI.Styles.metallicMapText, this.metallicMap, flag ? null : this.metallic);
			}
			bool flag2 = flag;
            if (this.smoothnessMapChannel != null && (int)this.smoothnessMapChannel.floatValue == 1)
            {
                flag2 = true;
            }
            int num = 2;
			this.m_MaterialEditor.ShaderProperty(flag2 ? this.smoothnessScale : this.smoothness, flag2 ? Curved_StandardShaderGUI.Styles.smoothnessScaleText : Curved_StandardShaderGUI.Styles.smoothnessText, num);
			num++;
			if (this.smoothnessMapChannel != null)
			{
				this.m_MaterialEditor.ShaderProperty(this.smoothnessMapChannel, Curved_StandardShaderGUI.Styles.smoothnessMapChannelText, num);
			}
		}

		public static void SetupMaterialWithBlendMode(Material material, Curved_StandardShaderGUI.BlendMode blendMode)
		{
			switch (blendMode)
			{
			case Curved_StandardShaderGUI.BlendMode.Opaque:
				material.SetOverrideTag("RenderType", "CurvedWorld_Opaque");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue =-1;
				return;
			case Curved_StandardShaderGUI.BlendMode.Cutout:
				material.SetOverrideTag("RenderType", "CurvedWorld_TransparentCutout");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue =2450;
				return;
			case Curved_StandardShaderGUI.BlendMode.Fade:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				return;
			case Curved_StandardShaderGUI.BlendMode.Transparent:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				return;
			default:
				return;
			}
		}

		private static Curved_StandardShaderGUI.SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
		{
			if ((int)material.GetFloat("_SmoothnessTextureChannel") == 1)
			{
				return Curved_StandardShaderGUI.SmoothnessMapChannel.AlbedoAlpha;
			}
			return Curved_StandardShaderGUI.SmoothnessMapChannel.SpecularMetallicAlpha;
		}

		private static void SetMaterialKeywords(Material material, Curved_StandardShaderGUI.WorkflowMode workflowMode)
		{
			Curved_StandardShaderGUI.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
			if (workflowMode == Curved_StandardShaderGUI.WorkflowMode.Specular)
			{
				Curved_StandardShaderGUI.SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
			}
			else if (workflowMode == Curved_StandardShaderGUI.WorkflowMode.Metallic)
			{
				Curved_StandardShaderGUI.SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
			}
			Curved_StandardShaderGUI.SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
			Curved_StandardShaderGUI.SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));
			MaterialEditor.FixupEmissiveFlag(material);
			bool state = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
			Curved_StandardShaderGUI.SetKeyword(material, "_EMISSION", state);
			if (material.HasProperty("_SmoothnessTextureChannel"))
			{
				Curved_StandardShaderGUI.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", Curved_StandardShaderGUI.GetSmoothnessMapChannel(material) == Curved_StandardShaderGUI.SmoothnessMapChannel.AlbedoAlpha);
			}
		}

		private static void MaterialChanged(Material material, Curved_StandardShaderGUI.WorkflowMode workflowMode)
		{
			Curved_StandardShaderGUI.SetupMaterialWithBlendMode(material, (Curved_StandardShaderGUI.BlendMode)material.GetFloat("_Mode"));
			Curved_StandardShaderGUI.SetMaterialKeywords(material, workflowMode);
		}

		private static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
			{
				m.EnableKeyword(keyword);
				return;
			}
			m.DisableKeyword(keyword);
		}

		private void DrawStandardShaderType(Material material)
		{
			int num = 0;
			if (material.shader.name.Contains("Specular"))
			{
				num = 1;
			}
			else if (material.shader.name.Contains("Roughness"))
			{
				num = 2;
			}
			bool flag = material.shader.name.Contains("Outline");
			EditorGUI.BeginChangeCheck();
			num = EditorGUILayout.IntPopup("Setup", num, new string[]
			{
				"Metallic",
				"Specular",
				"Roughness"
			}, new int[]
			{
				0,
				1,
				2
			}, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (num == 0)
				{
					if (flag)
					{
						material.shader =Shader.Find("Shaders/Curved World/Outline/Standard");
						return;
					}
					material.shader = Shader.Find("SD/Object/CurvedStandard");
					return;
				}
				else if (num == 1)
				{
					if (flag)
					{
						material.shader  =Shader.Find("Hidden/VacuumShaders/Curved World/Outline/Standard/Specular");
						return;
					}
					material.shader = Shader.Find("SD/Object/CurvedSpecularStandard");
					return;
				}
				else if (num == 2)
				{
					if (flag)
					{
						material.shader = Shader.Find("Hidden/VacuumShaders/Curved World/Outline/Standard/Roughness");
						return;
					}
					material.shader = Shader.Find("SD/Object/CurvedRoughnessStandard");
				}
			}
		}

		private void Draw_Scroll(MaterialProperty prop)
		{
			EditorGUILayout.LabelField(string.Empty, new GUILayoutOption[0]);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			Vector2 vector = prop.vectorValue;
			float labelWidth = EditorGUIUtility.labelWidth;
			float num = lastRect.x + labelWidth;
			float num2 = lastRect.x + (float)EditorGUI.indentLevel * 15f;
			Rect rect = new Rect(num2, lastRect.y, labelWidth, 16f);
			Rect arg_96_0 = new Rect(num, lastRect.y, lastRect.width - labelWidth, 16f);
			EditorGUI.PrefixLabel(rect, new GUIContent("Scroll"));
			EditorGUI.BeginChangeCheck();
			vector = EditorGUI.Vector2Field(arg_96_0, GUIContent.none, vector);
			if (EditorGUI.EndChangeCheck())
			{
				prop.vectorValue = vector;
			}
		}

		private void Draw_Outline(MaterialEditor materialEditor, Material _targetMaterial)
		{
			if (!_targetMaterial.shader.name.Contains("Outline"))
			{
				return;
			}
			if (this._V_CW_OutlineColor == null)
			{
				return;
			}
			EditorGUILayout.Space();
			GUILayout.Label("Outline", EditorStyles.boldLabel, new GUILayoutOption[0]);
            if ((int)this.blendMode.floatValue != 0)
            {
                EditorGUILayout.HelpBox("For proper outline effect use 'Opaque' Rendering mode.", MessageType.Warning);
            }

            using (new EditorKits.EditorGUIUtilityFieldWidth(64f))
            {
                this._V_CW_OutlineColor.colorValue = (EditorGUILayout.ColorField("Color", this._V_CW_OutlineColor.colorValue, new GUILayoutOption[0]));
                this._V_CW_OutlineWidth.floatValue =EditorGUILayout.FloatField("Width", this._V_CW_OutlineWidth.floatValue, new GUILayoutOption[0]);
                if (this._V_CW_OutlineWidth.floatValue < 0f)
                {
                    this._V_CW_OutlineWidth.floatValue = 0f;
                }
                this.m_MaterialEditor.ShaderProperty(this._V_CW_OutlineSizeIsFixed, "Fized Size");
            }
        }
	}
}
