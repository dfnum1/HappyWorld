#if UNITY_EDITOR
using Framework.Logic;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using Framework.BattlePlus;
using ExternEngine;
#endif
namespace TopGame.Data
{
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(GlobalSetting), true)]
    public class GlobalSettingEditor : UnityEditor.Editor
    {
        bool m_bExpandLockScatting = false;
        Logic.EMode m_FocusCatterType = Logic.EMode.Count;
        List<LookFocusScatter> m_vPrepareLookFocusScatters = new List<LookFocusScatter>();
        List<LookFocusScatter> m_vLookFocusScatters = new List<LookFocusScatter>();
        void OnEnable()
        {
            GlobalSetting controller = target as GlobalSetting;
            if(controller.PrepareLookFocusScatters!=null) m_vPrepareLookFocusScatters.AddRange(controller.PrepareLookFocusScatters);
            if (controller.LookFocusScatters != null) m_vLookFocusScatters.AddRange(controller.LookFocusScatters);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GlobalSetting controller = target as GlobalSetting;

            UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty("Lods"), new GUIContent("LOD配置"), true);
            controller = (GlobalSetting)Framework.ED.HandleUtilityWrapper.DrawProperty(controller, null);

           // m_bExpandLockScatting = UnityEditor.EditorGUILayout.Foldout(m_bExpandLockScatting, "相机视点分散抖动");
           // if(m_bExpandLockScatting)
            {
                //UnityEditor.EditorGUI.indentLevel++;
                //GUILayout.BeginHorizontal();
                //m_FocusCatterType = (Logic.EMode)UnityEditor.EditorGUILayout.EnumPopup("类型", m_FocusCatterType);
                //if (m_FocusCatterType< Logic.EMode.Count && GUILayout.Button("添加"))
                //{
                //    controller.PrepareLookFocusScatters = m_vPrepareLookFocusScatters.ToArray();
                //    controller.LookFocusScatters = m_vLookFocusScatters.ToArray();
                //}
                //if (GUILayout.Button("移除"))
                //{
                //    for(int i =0; i < m_vPrepareLookFocusScatters.Count; ++i)
                //    {
                //        if(m_vPrepareLookFocusScatters[i].lableID == (int)m_FocusCatterType)
                //        {
                //            m_vPrepareLookFocusScatters.RemoveAt(i);
                //            break;
                //        }
                //    }
                //    for (int i = 0; i < m_vLookFocusScatters.Count; ++i)
                //    {
                //        if (m_vLookFocusScatters[i].lableID == (int)m_FocusCatterType)
                //        {
                //            m_vLookFocusScatters.RemoveAt(i);
                //            break;
                //        }
                //    }
                //}
                //GUILayout.EndHorizontal();
                //DrawLookFocusScatters("备战", m_vPrepareLookFocusScatters);
                //DrawLookFocusScatters("战斗", m_vLookFocusScatters);
                //UnityEditor.EditorGUI.indentLevel--;
            }


            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            }
            bool bChange = serializedObject.ApplyModifiedProperties();

            GameInstance pMainModule = null;
            if(Framework.Module.ModuleManager.mainModule != null)
            {
                pMainModule = Framework.Module.ModuleManager.mainModule as GameInstance;
            }
            if (pMainModule == null) return;
            if(pMainModule.dropManager != null)
                pMainModule.dropManager.SetDropSpeed(controller.fDropSpeed);

            Framework.Base.ConfigUtil.timeHorizon = Data.GlobalSetting.fRVOTimeHorizon;
            Framework.Base.ConfigUtil.timeHorizonObst = Data.GlobalSetting.fRVOTimeHorizonObst;

            if (pMainModule.lodMgr != null)
                pMainModule.lodMgr.SetLODS(controller.Lods);

            if(pMainModule.cameraController!=null && pMainModule.cameraController.GetCurrentMode()!=null && m_FocusCatterType!= Logic.EMode.Count)
            {
                Logic.AbsMode absMode = Logic.AState.CastCurrentMode<Logic.AbsMode>();
                if (absMode != null && absMode.GetMode() == m_FocusCatterType)
                {
                    if (BattleKits.IsStartingAndActive(pMainModule))
                    {
                        LookFocusScatter scatter = controller.LookFocusScatters[(int)m_FocusCatterType];
                        pMainModule.cameraController.GetCurrentMode().SetLookFocusScatter(scatter.LookFocusScatterParam, scatter.LookFocusScatterIntensity, scatter.LookFocusScatterFrequency);
                    }
                    else
                    {
                        LookFocusScatter scatter = controller.PrepareLookFocusScatters[(int)m_FocusCatterType];
                        pMainModule.cameraController.GetCurrentMode().SetLookFocusScatter(scatter.LookFocusScatterParam, scatter.LookFocusScatterIntensity, scatter.LookFocusScatterFrequency);
                    }
                }
            }
        }
        //------------------------------------------------------
        void DrawLookFocusScatters(string label, List<LookFocusScatter> scatterings)
        {
            UnityEditor.EditorGUILayout.LabelField(label);
            if (m_FocusCatterType >= Logic.EMode.Count) return;
            int index = -1;
            LookFocusScatter scatter = LookFocusScatter.DEFAULT;
            for (int i =0; i < scatterings.Count; ++i)
            {
                if(scatterings[i].lableID == (int)m_FocusCatterType)
                {
                    index = i;
                    scatter = scatterings[i];
                    break;
                }
            }
            if(index<0)
            {
                index = scatterings.Count;
                scatter.lableID = (int)m_FocusCatterType;
                scatterings.Add(scatter);
            }
            scatter = (LookFocusScatter)Framework.ED.HandleUtilityWrapper.DrawProperty(scatter, null);
            scatterings[index] = scatter;
        }
    }
}
#endif