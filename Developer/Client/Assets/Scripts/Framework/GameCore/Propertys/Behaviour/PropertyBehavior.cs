/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	PropertyBehavior
作    者:	HappLI
描    述:	属性挂件
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace TopGame.Core
{
    public class PropertyBehavior : MonoBehaviour
    {
        [Framework.Plugin.PluginDisplay("属性名")]
        [Framework.Plugin.DisableGUI]
        public string strPropertyName;
        [Framework.Plugin.PluginDisplay("过渡曲线")]
        public AnimationCurve curve;
        [Framework.Plugin.PluginDisplay("是否使用Block")]
        public bool bUseBlock;
        [Framework.Plugin.PluginDisplay("共享材质")]
        public bool materialShare = true;
        [Framework.Plugin.PluginDisplay("材质索引")]
        public int materialIndex = -1;
        [Framework.Plugin.PluginDisplay("循环模式")]
        public ABaseProperty.ELoopType loopType = ABaseProperty.ELoopType.Loop;
        [Framework.Plugin.PluginDisplay("循环次数")]
        public int loopCount = 1;

        [Framework.Plugin.PluginDisplay("自动播放")]
        public bool autoAwake = true;

        protected Renderer[] m_pRenders = null;

        protected int m_nPropertyNameID = 0;
        protected int m_nLoopCnt = 0;
        protected int m_nSign = 1;
        protected float m_fRunning = 0;
        protected float m_fDuration = 0;
        //------------------------------------------------------
        private void Awake()
        {
            Reset(0);
        }
        //------------------------------------------------------
        protected void Clear()
        {
            m_nSign = 1;
            m_fRunning = 0;
            m_fDuration = 0;
            m_nLoopCnt = 0;
        }
        //------------------------------------------------------
        public virtual void Reset(float fPlayTime = 0)
        {
            Clear();
            m_nPropertyNameID = MaterailBlockUtil.BuildPropertyID(strPropertyName);
            m_pRenders = GetComponentsInChildren<Renderer>();
            if (m_pRenders != null)
            {
                if (curve != null)
                {
                    m_fDuration = Framework.Core.CommonUtility.GetCurveMaxTime(curve);
                }
            }
            m_fRunning = fPlayTime;
        }
        //------------------------------------------------------
        private void Update()
        {
            if (!autoAwake) return;
            DoUpdate(Time.deltaTime);
        }
        //------------------------------------------------------
        public virtual void DoUpdate(float fFrameTime)
        {
            if (m_fDuration <= 0 || m_nPropertyNameID == 0) return;
            if (loopCount>=0 && m_nLoopCnt > loopCount) return;

            m_fRunning += fFrameTime * m_nSign;
            float fFactor = 0;
            if (curve != null) fFactor = curve.Evaluate(m_fRunning);
            else fFactor = Mathf.Clamp01(m_fRunning / m_fDuration);
            DoExcude(fFactor);

            bool bEnd = false;
            if ((m_nSign > 0 && m_fRunning >= m_fDuration) ||
                (m_nSign <= 0 && m_fRunning <= 0))
                bEnd = true;
            if (bEnd)
            {
                bool bCheckCnt = true;
                if (loopType == ABaseProperty.ELoopType.Pingpong)
                {
                    if (m_fRunning > 0) bCheckCnt = false;

                    m_nSign = -m_nSign;
                    if (m_nSign > 0) m_fRunning = 0;
                    else m_fRunning = m_fDuration;
                }
                else if(loopType == ABaseProperty.ELoopType.Loop)
                {
                    m_fRunning = 0;
                }

                if (loopCount > 0 && bCheckCnt)
                {
                    m_nLoopCnt++;
                }
            }

#if UNITY_EDITOR
            if (SceneView.lastActiveSceneView)
                SceneView.lastActiveSceneView.Repaint();
#endif
        }
        //------------------------------------------------------
        protected virtual void DoExcude(float fFactor)
        {

        }
    }
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(PropertyBehavior), true)]
    public class PropertyBehaviorEditor : UnityEditor.Editor
    {
        List<string> m_vMatrialPorpertyPops = new List<string>();
        List<string> m_vMatrialPorpertys = new List<string>();
        float m_fTestTime = 0;
        private void OnEnable()
        {
            PropertyBehavior controller = target as PropertyBehavior;
            RefreshMaterialPop(controller);
            m_fTestTime = 0;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            PropertyBehavior controller = target as PropertyBehavior;

            System.Reflection.FieldInfo[] fields = controller.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            int selIndex = -1;
            if(!string.IsNullOrEmpty(controller.strPropertyName))
            {
                selIndex = m_vMatrialPorpertys.IndexOf(controller.strPropertyName);
            }
            selIndex = EditorGUILayout.Popup("属性名", selIndex, m_vMatrialPorpertyPops.ToArray());
            if(selIndex >=0 && selIndex < m_vMatrialPorpertys.Count)
            {
                if(m_vMatrialPorpertys[selIndex]!= controller.strPropertyName)
                {
                    controller.strPropertyName = m_vMatrialPorpertys[selIndex];
                    controller.Reset(m_fTestTime);
                }
            }

            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i].IsNotSerialized) continue;
                string strDisplayName = fields[i].Name;
                if (fields[i].IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                {
                    strDisplayName = fields[i].GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
                }
                if (fields[i].IsDefined(typeof(Framework.Plugin.DisableGUIAttribute)))
                {
                    continue;
                }
                if(fields[i].FieldType == typeof(Color))
                {
                    Color color = (Color)fields[i].GetValue(controller);
                    color = EditorGUILayout.ColorField(new GUIContent(fields[i].Name), color, true, true, true);
                    fields[i].SetValue(controller, color);
                    continue;
                }
                UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty(fields[i].Name), new GUIContent(strDisplayName), true);
            }
            if(serializedObject.ApplyModifiedProperties())
            {
                controller.Reset(m_fTestTime);
                controller.DoUpdate(0);
            }

            EditorGUI.BeginChangeCheck();
            m_fTestTime = EditorGUILayout.Slider(m_fTestTime, 0, Framework.Core.CommonUtility.GetCurveMaxTime(controller.curve));
            if(EditorGUI.EndChangeCheck())
            {
                controller.Reset(m_fTestTime);
                controller.DoUpdate(0);
            }
            if (GUILayout.Button("刷新"))
            {
                controller.Reset(m_fTestTime);
                controller.DoUpdate(0);
                RefreshMaterialPop(controller);
                EditorUtility.SetDirty(controller);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        //------------------------------------------------------
        void RefreshMaterialPop(PropertyBehavior controller)
        {
            m_vMatrialPorpertyPops.Clear();
            m_vMatrialPorpertys.Clear();
            m_vMatrialPorpertys.Add("");
            m_vMatrialPorpertyPops.Add("None");
            Renderer[] renders = controller.GetComponentsInChildren<Renderer>();
            if (renders != null)
            {
                for(int i = 0; i < renders.Length; ++i)
                {
                    if(renders[i].sharedMaterial == null) continue;
                    MaterialProperty[]  propertys = UnityEditor.MaterialEditor.GetMaterialProperties(new Object[] { renders[i].sharedMaterial });
                    if(propertys != null)
                    {
                        for(int j = 0; j < propertys.Length; ++j)
                        {
                            if (m_vMatrialPorpertyPops.Contains(propertys[j].displayName) || m_vMatrialPorpertys.Contains(propertys[j].name))
                                continue;

                            m_vMatrialPorpertys.Add(propertys[j].name);
                            m_vMatrialPorpertyPops.Add(propertys[j].displayName);
                        }
                    }
                }
            }
        }
    }
#endif
}

