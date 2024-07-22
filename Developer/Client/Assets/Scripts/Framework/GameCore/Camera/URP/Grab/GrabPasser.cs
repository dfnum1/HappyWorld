/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	GrabPasser
作    者:	Happli
描    述:   GrabPass 开关
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.URP
{
    [ExecuteInEditMode]
    public class GrabPasser : MonoBehaviour
    {
        public bool bAutoBlur = false;
        public bool bGrabAlways = true;
        public AnimationCurve BlurCurve = null;
        public int nCullingMask = 0;
        public Material passMaterial;
        [Range(-1,1)]
        public float renderScale = -1;
        public bool bCheckView = false;
        bool m_bPassed = false;
        int m_nLastFrame = 0;
        Transform m_pTransfrom;
        RectTransform m_pRectTransform;

        float m_fBlurTime = 0;
        //------------------------------------------------------
        private void Awake()
        {
            m_pTransfrom = transform;
            m_pRectTransform = transform as RectTransform;
        }
        //------------------------------------------------------
        protected bool IsUpdateGrab()
        {
            if (Data.GameQuality.Qulity <= Data.EGameQulity.Low)
                return false;
            return bGrabAlways;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            if (m_bPassed) return;
            m_bPassed = true;
            m_fBlurTime = 0;
            if(Framework.Core.BaseUtil.IsValidCurve(BlurCurve) && bGrabAlways)
                Framework.Core.MaterailBlockUtil.SetFloat(GrabPassFeature.CurGrabMaterial, "_Strength", Mathf.Clamp01(BlurCurve.Evaluate(m_fBlurTime)));
            else
                Framework.Core.MaterailBlockUtil.SetFloat(GrabPassFeature.CurGrabMaterial, "_Strength", 1);
            GrabPassFeature.ActiveRef(m_bPassed, renderScale, nCullingMask, IsUpdateGrab(), bAutoBlur, GetPassMaterial());
            OnPassed(m_bPassed);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            if (!m_bPassed) return;
            m_bPassed = false;
            m_fBlurTime = 0;
            GrabPassFeature.ActiveRef(m_bPassed);
            OnPassed(m_bPassed);
        }
        //------------------------------------------------------
        public virtual Material GetPassMaterial() { return passMaterial; }
        //------------------------------------------------------
        protected void CheckView()
        {
            if (Time.frameCount > m_nLastFrame)
            {
                if (m_pRectTransform)
                {
                    if (UI.UIKits.IsInView(m_pRectTransform))
                        OnEnable();
                    else OnDisable();
                }
                else if (Core.CameraKit.IsInView(m_pTransfrom.position))
                {
                    OnEnable();
                }
                else OnDisable();
                m_nLastFrame = Time.frameCount + 2;
            }
        }
        //------------------------------------------------------
        private void Update()
        {
#if UNITY_EDITOR
            if (!Framework.Module.ModuleManager.startUpGame || !Application.isPlaying)
                return;
#endif
            if (Framework.Module.ModuleManager.mainModule == null) return;
            if (bCheckView || ViewCheck())
                CheckView();

            if(bGrabAlways)
            {
                if(GrabPassFeature.GrabStatMaskFlag == 0 && BlurCurve !=null)
                {
                    m_fBlurTime += Time.fixedDeltaTime;
                   Framework.Core.MaterailBlockUtil.SetFloat(GrabPassFeature.CurGrabMaterial, "_Strength",  Mathf.Clamp01(BlurCurve.Evaluate(m_fBlurTime)));
                }
            }

            InnerUpdate();
        }
        //------------------------------------------------------
        protected virtual void InnerUpdate() { }
        //------------------------------------------------------
        public bool isPassed()
        {
            return m_bPassed;
        }
        //------------------------------------------------------
        protected virtual void OnPassed(bool bPassed) 
        {
#if UNITY_EDITOR
            Vector3 lookat = Core.CameraKit.MainCameraLookAt;
            Quaternion rotation = Quaternion.Euler(Core.CameraKit.MainCameraEulerAngle);
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
            //    sceneView.camera.gameObject.SetActive(!bPassed);
                sceneView.pivot = Core.CameraKit.MainCameraPosition + Core.CameraKit.MainCameraDirection * (lookat - Core.CameraKit.MainCameraPosition).magnitude;
                sceneView.rotation = rotation;
                sceneView.LookAt(lookat, rotation, 35f, sceneView.orthographic, true);
            }
            sceneView = SceneView.currentDrawingSceneView;
            if (sceneView != null)
            {
              //  sceneView.camera.gameObject.SetActive(!bPassed);
                sceneView.pivot = Core.CameraKit.MainCameraPosition + Core.CameraKit.MainCameraDirection * (lookat - Core.CameraKit.MainCameraPosition).magnitude;
                sceneView.rotation = rotation;
                sceneView.LookAt(lookat, rotation, 35f, sceneView.orthographic, true);
            }
#endif
        }
        //------------------------------------------------------
        protected virtual bool ViewCheck() { return false; }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(GrabPasser), true)]
    public class GrabPasserEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GrabPasser slot = target as GrabPasser;

            slot.passMaterial = EditorGUILayout.ObjectField("PassMaterial", slot.passMaterial, typeof(Material), true) as Material;
            slot.bCheckView = EditorGUILayout.Toggle("视野检测(不在屏幕内将关闭)", slot.bCheckView);
            slot.bAutoBlur = EditorGUILayout.Toggle("AutoBlur", slot.bAutoBlur);
            slot.bGrabAlways = EditorGUILayout.Toggle("更新", slot.bGrabAlways);
            if (slot.bGrabAlways)
                slot.BlurCurve = EditorGUILayout.CurveField("过度曲线", slot.BlurCurve);
            EditorGUILayout.BeginHorizontal();
            slot.renderScale = EditorGUILayout.Slider("抓屏缩放", slot.renderScale,-1,1);
            EditorGUILayout.HelpBox("<0，则默认配置", MessageType.Info);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("生效层");
            EditorGUILayout.HelpBox("什么都不选则默认配置", MessageType.Info);
            EditorGUILayout.EndHorizontal();
            slot.nCullingMask = Framework.ED.HandleUtilityWrapper.PopRenderLayerMask(null, slot.nCullingMask);
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}