/********************************************************************
生成日期:	1:11:2020 10:08
类    名: 	GlobalShaderController
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using UnityEngine;
namespace TopGame.Base
{
    [ExecuteAlways]
    [ExecuteInEditMode]
    public class GlobalShaderController : MonoBehaviour
    {
        public Data.GlobalShaderConfig globalShaderConfig;

        public enum ECurveType
        {
            Blend = 0,
            CurvePivot,
        }
        ECurveType curveType = ECurveType.Blend;

        int _V_CW_PivotPoint_Position;
        int _V_CW_PivotPoint_2_Position;
        int _V_CW_Angle;
        int _V_CW_BendAxis;
        int _V_CW_BendOffset;
        int _V_CW_MinimalRadius;
        int _V_CW_Curve;

        float m_fRuntimeCurveOffset = 0;
        float m_fRuntimeCurveOffsetX = 0;

        AnimationCurve m_CurveAngle = null;
        AnimationCurve m_CurveRadius = null;
        AnimationCurve m_CurvePivotPointY = null;
        AnimationCurve m_CurvePivotPointX = null;

        AnimationCurve m_BlendAxisX = null;
        AnimationCurve m_BlendAxisY = null;
        AnimationCurve m_BlendOffsetZ = null;
        bool m_bAbs = false;

        Vector3 CW_PivotPoint_Position = Vector3.zero;
        Vector3 CW_PivotPoint_2_Position = Vector3.zero;
        Vector3 CW_Angle = Vector3.zero;
        Vector3 CW_MinimalRadius = Vector3.zero;
        Vector3 CW_BendAxis = Vector3.zero;
        Vector3 CW_BendOffset = Vector3.zero;

        Vector3 CW_TO_Angle = Vector3.zero;
        Vector3 CW_TO_MinimalRadius = Vector3.zero;
        Vector3 CW_TO_BendAxis = Vector3.zero;
        Vector3 CW_TO_BendOffset = Vector3.zero;

        float   CW_Curve  =1;

        float m_fCurveDelta = 0;
        float m_fCurveDuration = 0;

        float m_fBlendDelta = 0;
        float m_fBlendDuration = -1;

        static GlobalShaderController ms_Instance;
        public static GlobalShaderController Instance
        {
            get { return ms_Instance; }
        }
        //------------------------------------------------------
        private void Awake()
        {
            _V_CW_Curve = Shader.PropertyToID("_V_CW_Curve");
            _V_CW_MinimalRadius = Shader.PropertyToID("_V_CW_MinimalRadius");
            _V_CW_Angle = Shader.PropertyToID("_V_CW_Angle");
            _V_CW_BendAxis = Shader.PropertyToID("_V_CW_BendAxis");
            _V_CW_BendOffset = Shader.PropertyToID("_V_CW_BendOffset");
            _V_CW_PivotPoint_Position = Shader.PropertyToID("_V_CW_PivotPoint_Position");
            _V_CW_PivotPoint_2_Position = Shader.PropertyToID("_V_CW_PivotPoint_2_Position");

            SetDefaultCurve();
            m_fCurveDelta = 0;
            m_fCurveDuration = 0;

            m_fBlendDelta = 0;
            m_fBlendDuration = -1;
            ms_Instance = this;
        }

        public void SetDefaultCurve()
        {
            m_fRuntimeCurveOffsetX = 0;
            m_fRuntimeCurveOffset = 0;
            CW_PivotPoint_Position = Vector3.zero;
            CW_PivotPoint_2_Position = Vector3.zero;
            CW_Angle = Vector3.zero;
            CW_MinimalRadius = Vector3.zero;
            CW_BendAxis = Vector3.zero;
            CW_BendOffset = Vector3.zero;
            CW_PivotPoint_Position = Vector3.zero;
            CW_PivotPoint_2_Position = Vector3.zero;
            CW_TO_Angle = Vector3.zero;
            CW_TO_MinimalRadius = Vector3.zero;
            CW_TO_BendAxis = Vector3.zero;
            CW_TO_BendOffset = Vector3.zero;
            CW_Curve = 1;
            if (curveType == ECurveType.CurvePivot)
            {
                Shader.SetGlobalVector(_V_CW_PivotPoint_Position, Vector3.zero);
                Shader.SetGlobalVector(_V_CW_PivotPoint_2_Position, Vector3.zero);
                Shader.SetGlobalVector(_V_CW_Angle, Vector3.zero);
                Shader.SetGlobalVector(_V_CW_MinimalRadius, Vector3.zero);
            }
            else
            {
                Shader.SetGlobalVector(_V_CW_BendAxis, CW_BendAxis);
                Shader.SetGlobalVector(_V_CW_BendOffset, CW_BendOffset);
            }
            Shader.SetGlobalFloat(_V_CW_Curve, CW_Curve);
        }

        private void Update()
        {
            if (globalShaderConfig!=null && globalShaderConfig.IsApplyChangePerFrame && globalShaderConfig.pbrConfig != null)
            {
                globalShaderConfig.pbrConfig.Apply();
            }
            if (Data.GlobalSetting.Instance == null)
            {
                SetDefaultCurve();
                return;
            }

#if UNITY_EDITOR
            if (UnityEditor.EditorWindow.focusedWindow != null && UnityEditor.EditorWindow.focusedWindow is UnityEditor.SceneView)
            {
                TestUpdate();
                return;
            }
#endif
            if (curveType == ECurveType.CurvePivot)
            {
                if (m_fCurveDuration > 0 && m_fCurveDelta < m_fCurveDuration)
                {
                    m_fCurveDelta += Time.deltaTime;

                    if (m_CurvePivotPointY != null)
                    {
                        if (m_bAbs)
                            CW_PivotPoint_2_Position.y = m_CurvePivotPointY.Evaluate(m_fCurveDelta);
                        else
                            CW_PivotPoint_2_Position.y += m_CurvePivotPointY.Evaluate(m_fCurveDelta);
                    }
                    if (m_CurvePivotPointX != null)
                    {
                        CW_PivotPoint_2_Position.x = m_fRuntimeCurveOffsetX + m_CurvePivotPointX.Evaluate(m_fCurveDelta);
                    }
                    if (m_CurveAngle != null)
                    {
                        if (m_bAbs)
                            CW_TO_Angle.y = m_CurveAngle.Evaluate(m_fCurveDelta);
                        else
                            CW_TO_Angle.y += m_CurveAngle.Evaluate(m_fCurveDelta);
                    }
                    if (m_CurveRadius != null)
                    {
                        if (m_bAbs)
                            CW_TO_MinimalRadius.y = m_CurveRadius.Evaluate(m_fCurveDelta);
                        else
                            CW_TO_MinimalRadius.y += m_CurveRadius.Evaluate(m_fCurveDelta);
                    }
					if (m_fBlendDelta >= Framework.Base.GlobalDef.POINGPONG_MAX_TIME) m_fBlendDelta = 0;
                    if (m_fCurveDelta >= m_fCurveDuration)
                    {
                        m_fCurveDuration = 0;
                        m_fCurveDelta = 0;
                        m_CurvePivotPointY = null;
                        m_CurvePivotPointX = null;
                        m_CurveRadius = null;
                        m_CurveAngle = null;
                        m_bAbs = false;
                    }
                }
                CW_Angle = Vector3.Lerp(CW_Angle, CW_TO_Angle, Time.fixedDeltaTime);
                CW_MinimalRadius = Vector3.Lerp(CW_MinimalRadius, CW_TO_MinimalRadius, Time.fixedDeltaTime);

                CW_Angle.y = Mathf.Max(0, CW_Angle.y);
                Shader.SetGlobalVector(_V_CW_PivotPoint_2_Position, CW_PivotPoint_2_Position);
                Shader.SetGlobalVector(_V_CW_Angle, CW_Angle);
                Shader.SetGlobalVector(_V_CW_MinimalRadius, CW_MinimalRadius);
            }
            else
            {
                if (m_fBlendDuration >= 0 && m_fBlendDelta <= m_fBlendDuration)
                {
                    float delta = Time.deltaTime;
                    m_fBlendDelta += Time.deltaTime;
                    if (m_BlendAxisX != null)
                    {
                        if (m_bAbs)
                            CW_TO_BendAxis.x = Mathf.Lerp(CW_TO_BendAxis.x, m_BlendAxisX.Evaluate(m_fBlendDelta), delta);
                        else
                            CW_TO_BendAxis.x = Mathf.Lerp(CW_TO_BendAxis.x, CW_TO_BendAxis.x+m_BlendAxisX.Evaluate(m_fBlendDelta), delta);
                    }
                    if (m_BlendAxisY != null)
                    {
                        if (m_bAbs)
                            CW_TO_BendAxis.y = Mathf.Lerp(CW_TO_BendAxis.y, m_BlendAxisY.Evaluate(m_fBlendDelta), delta);
                        else
                            CW_TO_BendAxis.y = Mathf.Lerp(CW_TO_BendAxis.y, CW_TO_BendAxis.y+m_BlendAxisY.Evaluate(m_fBlendDelta), delta);
                    }
                    if (m_BlendOffsetZ != null)
                    {
                        if (m_bAbs)
                            CW_TO_BendAxis.z = Mathf.Lerp(CW_TO_BendAxis.z, m_BlendOffsetZ.Evaluate(m_fBlendDelta), delta);
                        else
                            CW_TO_BendAxis.z = Mathf.Lerp(CW_TO_BendAxis.z, CW_TO_BendAxis.z+m_BlendOffsetZ.Evaluate(m_fBlendDelta), delta);
                    }
                    if (m_fBlendDelta >= Framework.Base.GlobalDef.POINGPONG_MAX_TIME) m_fBlendDelta = 0;
                    if (m_fBlendDelta >= m_fBlendDuration)
                    {
                        m_fBlendDuration = -1;
                        m_fBlendDelta = 0;
                        m_BlendAxisX = null;
                        m_BlendAxisY = null;
                        m_BlendOffsetZ = null;
                        m_bAbs = false;
                    }
                }

                CW_BendAxis = Vector3.Lerp(CW_BendAxis, CW_TO_BendAxis, Time.fixedDeltaTime);
                CW_BendOffset = Vector3.Lerp(CW_BendOffset, CW_TO_BendOffset, Time.fixedDeltaTime);

                Shader.SetGlobalVector(_V_CW_BendAxis, CW_BendAxis);
                Shader.SetGlobalVector(_V_CW_BendOffset, CW_BendOffset);
                Shader.SetGlobalVector(_V_CW_PivotPoint_Position, CW_PivotPoint_Position);
            }
        }
        //------------------------------------------------------
        public Vector3 GetCWBendAxis()
        {
            return CW_BendAxis;
        }
        //------------------------------------------------------
        public Vector3 GetCWBendOffset()
        {
            return CW_BendOffset;
        }
        //------------------------------------------------------
        public Vector3 GetCWPivotPointPosition()
        {
            return CW_PivotPoint_Position;
        }
        //------------------------------------------------------
        public float GetCWCurve()
        {
            return CW_Curve;
        }
        //------------------------------------------------------
        public void SetCurvePivotPointX(float fX)
        {
            CW_PivotPoint_Position.z = m_fRuntimeCurveOffset + fX + CW_TO_BendOffset.z;
            CW_PivotPoint_2_Position.z = m_fRuntimeCurveOffset + fX;
        }
        //------------------------------------------------------
        public static void SetCurve(AnimationCurve Angle, AnimationCurve Radius, AnimationCurve PivotPointY, AnimationCurve PivotPointX, Vector3 PivotPostion, bool bAbs)
        {
            if (ms_Instance == null) return;
            ms_Instance.m_fRuntimeCurveOffsetX = PivotPostion.x;
            ms_Instance.m_fRuntimeCurveOffset = PivotPostion.z;
            ms_Instance.CW_PivotPoint_2_Position.x = PivotPostion.x;
            ms_Instance.m_CurvePivotPointY = PivotPointY;
            ms_Instance.m_CurvePivotPointX = PivotPointX;
            ms_Instance.m_CurveRadius = Radius;
            ms_Instance.m_CurveAngle= Angle;
            ms_Instance.m_bAbs = bAbs;
            ms_Instance.m_fCurveDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_CurvePivotPointY, true), ms_Instance.m_fCurveDuration);
            ms_Instance.m_fCurveDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_CurvePivotPointX, true), ms_Instance.m_fCurveDuration);
            ms_Instance.m_fCurveDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_CurveRadius, true), ms_Instance.m_fCurveDuration);
            ms_Instance.m_fCurveDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_CurveAngle, true), ms_Instance.m_fCurveDuration);
            ms_Instance.m_fCurveDelta = 0;
        }
        //------------------------------------------------------
        public static void SetBlend(AnimationCurve BlendX, AnimationCurve BlendY, AnimationCurve OffsetZ, bool bAbs, bool bImmediately = false)
        {
            if (ms_Instance == null) return;
            EnableCurve(true);
            ms_Instance.m_fBlendDelta = 0;
            ms_Instance.m_bAbs = bAbs;
            ms_Instance.m_BlendAxisX = BlendX;
            ms_Instance.m_BlendAxisY = BlendY;
            ms_Instance.m_BlendOffsetZ = OffsetZ;
            ms_Instance.m_fBlendDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_BlendAxisX, true), ms_Instance.m_fBlendDuration);
            ms_Instance.m_fBlendDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_BlendAxisY, true), ms_Instance. m_fBlendDuration);
            ms_Instance.m_fBlendDuration = Mathf.Max(Framework.Core.BaseUtil.GetCurveMaxTime(ms_Instance.m_BlendOffsetZ, true), ms_Instance.m_fBlendDuration);
            if(bImmediately)
            {
                ms_Instance.Update();
                ms_Instance.CW_Angle = ms_Instance.CW_TO_Angle;
                ms_Instance.CW_MinimalRadius = ms_Instance.CW_TO_MinimalRadius;
            }
        }
        //------------------------------------------------------
        public static void SetBlend(Vector3 blendAxis, Vector3 blendOffset)
        {
            if (ms_Instance == null) return;
            EnableCurve(true);
            ms_Instance.m_BlendAxisX = null;
            ms_Instance.m_BlendAxisY = null;
            ms_Instance.m_BlendOffsetZ = null;
            ms_Instance.m_fBlendDelta = 0;
            ms_Instance.m_fBlendDuration = -1;
            ms_Instance.CW_TO_BendAxis = blendAxis;
            ms_Instance.CW_TO_BendOffset = blendOffset;
        }
        //------------------------------------------------------
        public static void EnableCurve(bool bEnable)
        {
            if(ms_Instance == null)
            {
                Shader.SetGlobalFloat("_V_CW_Curve", bEnable?1:0);
                return;
            }
            ms_Instance.CW_Curve = bEnable ? 1 : 0;
            Shader.SetGlobalFloat(ms_Instance._V_CW_Curve, ms_Instance.CW_Curve);
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public static void TestUpdate()
        {
            Shader.SetGlobalVector("_V_CW_MinimalRadius", Vector3.zero);
            Shader.SetGlobalVector("_V_CW_Angle", Vector3.zero);
            Shader.SetGlobalVector("_V_CW_PivotPoint_Position", Vector3.zero);
            Shader.SetGlobalVector("_V_CW_PivotPoint_2_Position", Vector3.zero);
            Shader.SetGlobalVector("_V_CW_BendAxis", Vector3.zero);
            Shader.SetGlobalVector("_V_CW_BendOffset", Vector3.zero);
            Shader.SetGlobalFloat("_V_CW_Curve", 1);
        }
#endif
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
    }
}
