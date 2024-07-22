using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
	[ExecuteInEditMode]
	public class CustomLight : MonoBehaviour
	{
		public enum CustomLightType
		{
			SphereLight,
		};

		public CustomLightType LightType = CustomLightType.SphereLight;

		[UnityEngine.ColorUsage(true,true)]
		public Color LightColor = Color.white;
		public float LightRadius = 1.0f;
		public int	 LightIndex = 0;

		CustomLightType m_LightType		= CustomLightType.SphereLight;
		int				m_Index			= 0;
		Vector3			m_LightPos		= Vector3.zero;
		float			m_LightRadius	= 1.0f;
		Color			m_LightColor	= Color.white;

		string LightName = "_SphereLight0";

		int LightColorID = 0;
		int LightRadiusID = 0;
		int LightPosID = 0;

		public bool IsForceUpdateAllNextFrame = true;
        //------------------------------------------------------
        void Awake()
		{
			m_LightPos	 = transform.position;
			m_LightRadius = LightRadius;
			m_LightColor = LightColor;
			ResetShaderParamName();
            IsForceUpdateAllNextFrame = true;
        }
        //------------------------------------------------------
        void Update()
		{
			UpdateParams();
		}
        //------------------------------------------------------
        private void OnEnable()
        {
			IsForceUpdateAllNextFrame = true;
		}
        //------------------------------------------------------
        void ResetShaderParamName()
		{
			m_LightType = LightType;
			m_Index		= LightIndex;

            LightName = string.Format("_{0}{1}", m_LightType, m_Index);
            string LightColorName = string.Format("{0}Color", LightName);
            string LightRadiusName = string.Format("{0}Radius", LightName);
            string LightPosName = string.Format("{0}Pos", LightName);

            LightColorID = Shader.PropertyToID(LightColorName);
            LightRadiusID = Shader.PropertyToID(LightRadiusName);
            LightPosID = Shader.PropertyToID(LightPosName);
        }
        //------------------------------------------------------
        void UpdateParams()
		{
			if (m_Index != LightIndex || LightType != m_LightType)
			{
				ResetShaderParamName();
            }

            if ( (m_LightPos - transform.position).sqrMagnitude > float.Epsilon || IsForceUpdateAllNextFrame)
			{
				m_LightPos = transform.position;
				Shader.SetGlobalVector(LightPosID, m_LightPos);
			}

			if (!m_LightColor.Equals(LightColor) || IsForceUpdateAllNextFrame)
			{
				m_LightColor = LightColor;
				Shader.SetGlobalColor(LightColorID, m_LightColor);
			}

			if ( Mathf.Abs(m_LightRadius - LightRadius) > float.Epsilon || IsForceUpdateAllNextFrame)
			{
				m_LightRadius = LightRadius;
				Shader.SetGlobalFloat(LightRadiusID, m_LightRadius);
			}

			IsForceUpdateAllNextFrame = false;
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(CustomLight), true)]
    public class CustomLightEditor : Editor
    {
        private void OnSceneGUI()
        {
            CustomLight light = target as CustomLight;
            if (light.LightType == CustomLight.CustomLightType.SphereLight)
            {
                Handles.CircleHandleCap(0, light.transform.position, Quaternion.Euler(0, 0, 0), light.LightRadius, EventType.Repaint);
                Handles.CircleHandleCap(0, light.transform.position, Quaternion.Euler(90, 0, 0), light.LightRadius, EventType.Repaint);
            }
        }
    }
#endif
}
