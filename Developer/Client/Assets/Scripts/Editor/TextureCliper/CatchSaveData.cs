
using System.Collections.Generic;
using System.IO;
using TopGame.Data;
using UnityEngine;

namespace TopGame.ED
{
    [System.Serializable]
    public class TextureCliperGroupCatch
    {
        [System.Serializable]
        public class GroupData
        {
            [Framework.Plugin.DisableGUI]
            public string group;
            [Framework.Data.SelectDirGUI]
            [Framework.Data.DisplayNameGUI("导出目录")]
            public string saveDir;
        }
        [System.Serializable]
        public class CatchData
        {
            public GroupData[] groups;
            public string lastEditGroup = null;
            public string lastEditClip = null;
            public Color zoomColor = new Color(1, 0, 0, 1);
        }

        CatchData m_CatchData = new CatchData();
        [System.NonSerialized]
        Dictionary<string, GroupData> m_vGroupDatas = new Dictionary<string, GroupData>();
        public GroupData GetGroupCatch(string group)
        {
            if (string.IsNullOrEmpty(group)) return new GroupData() { group = group };
            GroupData outGroup;
            if (m_vGroupDatas.TryGetValue(group, out outGroup))
                return outGroup;
            return new GroupData() { group = group };
        }
        //------------------------------------------------------
        public CatchData GetCatch()
        {
            return m_CatchData;
        }
        //------------------------------------------------------
        public void Load(string catchFile)
        {
            if (!File.Exists(catchFile)) return;
            m_CatchData = JsonUtility.FromJson<CatchData>(File.ReadAllText(catchFile));
            m_vGroupDatas.Clear();
            if (m_CatchData.groups!=null)
            {
                for (int i = 0; i < m_CatchData.groups.Length; ++i)
                {
                    if(string.IsNullOrEmpty(m_CatchData.groups[i].group)) continue;
                    m_vGroupDatas[m_CatchData.groups[i].group] = m_CatchData.groups[i];
                }
            }
        }
        //------------------------------------------------------
        public void SaveData(List<TextureCliperAssets.ClipGroup> vGroups)
        {
            m_vGroupDatas.Clear();
            for(int i = 0; i < vGroups.Count; ++i)
            {
                if(vGroups[i].catchData == null) continue;
                GroupData gp =(GroupData)vGroups[i].catchData;
                if(string.IsNullOrEmpty(vGroups[i].name)) continue;
                gp.group = vGroups[i].name;
                m_vGroupDatas[gp.group] = gp;
            }
        }
        //------------------------------------------------------
        public void Save(List<TextureCliperAssets.ClipGroup> vGroup, string catchFile)
        {
            SaveData(vGroup);
            if (File.Exists(catchFile)) File.Delete(catchFile);
            List<GroupData> vData = new List<GroupData>();
            foreach (var db in m_vGroupDatas)
            {
                vData.Add(db.Value);
            }
            m_CatchData.groups = vData.ToArray();

            File.WriteAllText(catchFile, JsonUtility.ToJson(m_CatchData, true));
        }
    }
}