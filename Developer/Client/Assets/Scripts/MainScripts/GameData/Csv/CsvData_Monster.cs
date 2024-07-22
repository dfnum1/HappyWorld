/********************************************************************
类    名:   CsvData_Monster
作    者:	自动生成
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Core;
using Framework.Base;
using Framework.Plugin.AT;

namespace TopGame.Data
{
    public partial class CsvData_Monster : Data_Base
    {
        Dictionary<long, MonsterData> m_vSceneThemes = null;
        Dictionary<long, List<MonsterData>> m_vSubTypeDatas = null;
#if UNITY_EDITOR
        Dictionary<int, List<MonsterData>> m_vEditorSubTypeDatas = new Dictionary<int, List<MonsterData>>();
        List<string> m_vPopSubTypeName = new List<string>();
        List<byte> m_vPopSubType = new List<byte>();
        public List<string> PopSubTypeNames { get { return m_vPopSubTypeName; } }
        public List<byte> PopSubTypes { get { return m_vPopSubType; } }
        public int DrawSubTypes(string label, int subType, float labelWdith = 50, GUILayoutOption[] layOp=null)
        {
            int index = PopSubTypes.IndexOf((byte)subType);
            if (string.IsNullOrEmpty(label))
                index = UnityEditor.EditorGUILayout.Popup( index, PopSubTypeNames.ToArray(), layOp);
            else
            {
                float labW = UnityEditor.EditorGUIUtility.labelWidth;
                UnityEditor.EditorGUIUtility.labelWidth = labelWdith;
                index = UnityEditor.EditorGUILayout.Popup(label, index, PopSubTypeNames.ToArray(), layOp);
                UnityEditor.EditorGUIUtility.labelWidth = labW;
            }
            if (index >= 0 && index < PopSubTypes.Count)
                subType = PopSubTypes[index];
            return subType;
        }
#endif
        protected override void OnClearData()
        {
            base.OnClearData();
            if (m_vSceneThemes != null) m_vSceneThemes.Clear();
            if (m_vSubTypeDatas != null) m_vSubTypeDatas.Clear();
#if UNITY_EDITOR
            m_vEditorSubTypeDatas.Clear();
            m_vPopSubTypeName.Clear();
            m_vPopSubType.Clear();
#endif
        }
        //------------------------------------------------------
        protected override void OnAddData(IUserData baseData)
        {
            base.OnAddData(baseData);
            MonsterData objData = baseData as MonsterData;

            Variable2 key = new Variable2();
            if (objData.sceneThemes != null && objData.sceneThemes.Length > 0)
            {
                if (m_vSubTypeDatas == null) m_vSubTypeDatas = new Dictionary<long, List<MonsterData>>();
                for (int i = 0; i < objData.sceneThemes.Length; ++i)
                {
                    key.intVal0 = (int)objData.sceneThemes[i];
                    key.intVal1 = (int)objData.subType;

                    List<MonsterData> vObs;
                    if (!m_vSubTypeDatas.TryGetValue(key.longValue, out vObs))
                    {
                        vObs = new List<MonsterData>();
                        m_vSubTypeDatas.Add(key.longValue, vObs);
                    }
                    vObs.Add(objData);

#if UNITY_EDITOR
                    if (!m_vEditorSubTypeDatas.TryGetValue(objData.subType, out vObs))
                    {
                        vObs = new List<MonsterData>();
                        m_vEditorSubTypeDatas.Add(objData.subType, vObs);
                    }
                    vObs.Add(objData);
#endif
                }
            }

#if UNITY_EDITOR
            if (!m_vPopSubType.Contains(objData.subType))
            {
                m_vPopSubTypeName.Add(objData.subType.ToString());
                m_vPopSubType.Add(objData.subType);
            }
#endif
            if (objData.sceneThemes == null || objData.sceneThemes.Length <=0)
            {
                return;
            }
            if (m_vSceneThemes == null) m_vSceneThemes = new Dictionary<long, MonsterData>();
            for(int i =0; i< objData.sceneThemes.Length; ++i)
            {
                key.intVal0 = (int)objData.sceneThemes[i];
                key.intVal1 = (int)objData.id;
                m_vSceneThemes[key.longValue] = objData;
            }
        }
        //------------------------------------------------------
        protected override void OnLoadCompleted()
        {
            if (m_vSubTypeDatas == null) m_vSubTypeDatas = new Dictionary<long, List<MonsterData>>();
            foreach (var db in m_vData)
            {
                if (db.Value.subType != 0) continue;
                if(db.Value.sceneThemes == null || db.Value.sceneThemes.Length<=0)
                {
                    foreach (var subDb in m_vSubTypeDatas)
                    {
                        var list = subDb.Value;
                        list.Add(db.Value);
                    }
#if UNITY_EDITOR
                    foreach (var subDb in m_vEditorSubTypeDatas)
                    {
                        var list = subDb.Value;
                        list.Add(db.Value);
                    }
#endif
                }
            }
#if UNITY_EDITOR
            string descFile = Application.dataPath + "/../EditorData/Runnings/monsterSubTypes.txt";
            if (System.IO.File.Exists(descFile))
            {
                string[] lines = System.IO.File.ReadAllLines(descFile);
                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] keyVals = lines[i].Split('=');
                    if (keyVals.Length == 2)
                    {
                        int index = m_vPopSubTypeName.IndexOf(keyVals[0].Trim());
                        if (index >= 0) m_vPopSubTypeName[index] = m_vPopSubTypeName[index] + "-" + keyVals[1];
                    }
                }
            }
            if (m_vPopSubType.Contains(0)) return;
            m_vPopSubTypeName.Insert(0,"无");
            m_vPopSubType.Insert(0,0);
#endif
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public List<MonsterData> GetEditorSubTypeDatas(byte subType)
        {
            List<MonsterData> vObs;
            if (m_vEditorSubTypeDatas.TryGetValue(subType, out vObs))
            {
                return vObs;
            }
            return null;
        }
#endif
        //------------------------------------------------------
        public List<MonsterData> GetSubTypeDatas(byte subType, uint sceneTheme)
        {
            Variable2 key = new Variable2();
            key.intVal0 = (int)sceneTheme;
            key.intVal1 = (int)subType;
            List<MonsterData> vObs;
            if (m_vSubTypeDatas.TryGetValue(key.longValue, out vObs))
            {
                return vObs;
            }
            return null;
        }
        //------------------------------------------------------
        public MonsterData GetData(uint sceneTheme, uint id)
        {
            MonsterData objData;
            if (sceneTheme != 0 && m_vSceneThemes != null)
            {
                Variable2 key = new Variable2();
                key.intVal0 = (int)sceneTheme;
                key.intVal1 = (int)id;
                if (m_vSceneThemes.TryGetValue(key.longValue, out objData))
                    return objData;
            }
            return GetData(id);
        }
    }
}
