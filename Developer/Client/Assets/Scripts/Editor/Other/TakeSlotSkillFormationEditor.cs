/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TakeSlotSkillFormation
作    者:	HappLI
描    述:	单飞技能点阵
*********************************************************************/
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
#endif
namespace TopGame.Data
{
    [CustomEditor(typeof(TakeSlotSkillFormation), true)]
    [CanEditMultipleObjects]
    public class TakeSlotSkillFormationEditor : Editor
    {
        public EActorType curType = EActorType.Player;
        public void OnEnable()
        {
            SceneView.duringSceneGui += this.OnSceneGUI;
        }
        private void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            TakeSlotSkillFormation assets = target as TakeSlotSkillFormation;
            if (assets.Slots == null) return;
            for (int t = 0; t < assets.Slots.Length; ++t)
            {
                if (!assets.Slots[t].isValid) continue;
                for (int i = 0; i < assets.Slots[t].format.Length; ++i)
                    GameObject.DestroyImmediate(assets.Slots[t].format[i].pInstance);
            }
        }
        GameObject m_pTset = null;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TakeSlotSkillFormation assets = target as TakeSlotSkillFormation;
            GameObject ppre = m_pTset;
            m_pTset = EditorGUILayout.ObjectField("测试", m_pTset, typeof(GameObject)) as GameObject;
            if(ppre != m_pTset)
            {
                if (assets.Slots!=null)
                {
                    for (int t = 0; t < assets.Slots.Length; ++t)
                    {
                        for (int i = 0; i < assets.Slots[t].format.Length; ++i)
                        {
                            if (assets.Slots[t].format[i].pInstance != null)
                                GameObject.DestroyImmediate(assets.Slots[t].format[i].pInstance);
                            if (m_pTset != null) assets.Slots[t].format[i].pInstance = GameObject.Instantiate(m_pTset);
                        }
                    }
                }
            }

            curType = (EActorType)Framework.ED.HandleUtilityWrapper.PopEnum("类型", curType, typeof(EActorType));

            int index = (int)curType;
            if (assets.Slots == null || (int)EActorType.Count != assets.Slots.Length)
            {
                List<TakeSlotSkillFormation.TypeData> formats = assets.Slots!=null?new List<TakeSlotSkillFormation.TypeData>(assets.Slots):new List<TakeSlotSkillFormation.TypeData>();
                for (int i = formats.Count; i <= (int)EActorType.Count && formats.Count < (int)EActorType.Count; ++i)
                {
                    TakeSlotSkillFormation.TypeData tp = new TakeSlotSkillFormation.TypeData();
                    tp.offset = Vector3.zero;
                    formats.Add(tp);
                }
                assets.Slots = formats.ToArray();
            }

            TakeSlotSkillFormation.TypeData tpData = assets.Slots[index];

            tpData.offset = EditorGUILayout.Vector3Field("位置偏移", tpData.offset);
            List<TakeSlotSkillFormation.Formation> vFormates = (tpData.format != null)? new List<TakeSlotSkillFormation.Formation>(tpData.format):new List<TakeSlotSkillFormation.Formation>();
            for(int i = 0; i < vFormates.Count; ++i)
            {
                TakeSlotSkillFormation.Formation foramt = vFormates[i];
                GUILayout.BeginHorizontal();
                EditorGUILayout.Foldout(true, "");
                if(GUILayout.Button("删除"))
                {
                    if(EditorUtility.DisplayDialog("提示", "是否确认删除", "删除", "取消"))
                    {
                        vFormates.RemoveAt(i);
                        break;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                foramt.slot = EditorGUILayout.Vector3Field("位置[" + (i+1)  + "]",foramt.slot);
                if(EditorGUI.EndChangeCheck())
                {
                    if (SceneView.lastActiveSceneView != null) SceneView.lastActiveSceneView.Repaint();
                }
                EditorGUI.indentLevel--;
                vFormates[i] = foramt;
            }
            if(GUILayout.Button("新建"))
            {
                vFormates.Add(new TakeSlotSkillFormation.Formation() { slot = Vector3.zero });
            }
            tpData.format = vFormates.ToArray();
            assets.Slots[index] = tpData;
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

        }
        //------------------------------------------------------
        private void OnSceneGUI(SceneView view)
        {
            TakeSlotSkillFormation assets = target as TakeSlotSkillFormation;
            int index = (int)curType;
            if (index >= assets.Slots.Length) return;

            TakeSlotSkillFormation.TypeData tpData = assets.Slots[index];

            tpData.offset = Handles.DoPositionHandle(tpData.offset, Quaternion.identity);
            if (tpData.format == null) return;
            for (int i = 0; i < tpData.format.Length; ++i)
            {
                TakeSlotSkillFormation.Formation foramt = tpData.format[i];
                foramt.slot = Handles.DoPositionHandle(foramt.slot + tpData.offset, Quaternion.identity) - tpData.offset;
                Handles.Label(foramt.slot + tpData.offset, "站位编号:" + (i+1).ToString());

                if(foramt.pInstance == null && m_pTset != null)
                {
                    if (m_pTset != null)
                    {
                        foramt.pInstance = GameObject.Instantiate(m_pTset);
                    }
                }
                if (foramt.pInstance)
                {
                    foramt.pInstance.hideFlags = HideFlags.HideAndDontSave;
                    foramt.pInstance.transform.position = foramt.slot + tpData.offset;
                    foramt.pInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
                tpData.format[i] = foramt;
            }
            assets.Slots[index] = tpData;
        }
    }
}
