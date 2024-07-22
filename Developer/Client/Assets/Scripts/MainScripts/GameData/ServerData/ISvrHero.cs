using Framework.Data;
using System.Collections.Generic;
using TopGame.Data;

namespace TopGame.SvrData
{
    public abstract class ISvrHero : Framework.Data.ISvrActorData
    {
        //! runtime 
        ushort m_nExternLevel = 0;

        private List<IAttrParam> m_vExternAttrParams = null;

        public List<IAttrParam> GetAllSystemAttrParams(Framework.Core.IContextData configData = null)
        {
            if (m_vExternAttrParams == null) m_vExternAttrParams = new List<IAttrParam>();
            m_vExternAttrParams.Clear();
            List<SvrSkillData>  skills = GetSkills();
            if(skills!=null)
            {
                CsvData_Skill.SkillData skillData;
                for (int i = 0; i < skills.Count; ++i)
                {
                    skillData = skills[i].configData as CsvData_Skill.SkillData;
                    if (skillData == null) continue;
                    if (skillData.attrBuffs != null)
                    {
                        for (int j = 0; j < skillData.attrBuffs.Length; ++j)
                        {
                            CsvData_Buff.BuffData buffData = DataManager.getInstance().Buff.GetData(skillData.attrBuffs[j]);
                            if (buffData != null)
                            {
                                ExternAttrParam externAttr = new ExternAttrParam(buffData,buffData.attrTypes, buffData.valueTypes, buffData.values);
#if UNITY_EDITOR
                                externAttr.strLabel = "Skills";
#endif
                                AddExternAttr(externAttr);
                            }
                        }
                    }
                }
            }
            OnExternAttrsParams(configData);
            return m_vExternAttrParams;
        }
        //------------------------------------------------------
        public void AddExternAttr(IAttrParam externAttr)
        {
            if (externAttr.IsValid())
            {
                if (m_vExternAttrParams == null)
                    return;
                m_vExternAttrParams.Add(externAttr);
            }
        }
        //------------------------------------------------------
        public void AddExternAttrs(List<IAttrParam> externAttr)
        {
            if (externAttr == null || m_vExternAttrParams == null) return;
            for(int i =0; i < externAttr.Count; ++i)
            {
                if (externAttr[i].IsValid())
                    m_vExternAttrParams.Add(externAttr[i]);
            }
        }
        //------------------------------------------------------
        protected abstract void OnExternAttrsParams(Framework.Core.IContextData configData = null);
        public abstract List<SvrSkillData> GetSkills();
        public abstract ushort GetLevel();
        public abstract uint GetConfigID();
        public abstract long GetGUID();
        public abstract byte GetCareerID();
        public abstract Framework.Core.IContextData GetContextData();

        public ushort GetExternLevel() { return m_nExternLevel; }
        public void SetExternLevel(ushort level, bool bAmount = true)
        {
            if (bAmount) m_nExternLevel += level;
            else m_nExternLevel = level;
        }
        //------------------------------------------------------
        public virtual void Destroy()
        {
            m_nExternLevel = 0;
            if (m_vExternAttrParams != null)
                m_vExternAttrParams.Clear();
        }
        //------------------------------------------------------

        public virtual int GetWeight()
        {
            return 0;
        }
        //------------------------------------------------------
        public virtual int GetBWeight()
        {
            return 0;
        }
        //------------------------------------------------------
        public virtual Framework.Core.SlotAvatar[] GetSlotAvatars()
        {
            return null;
        }
        //------------------------------------------------------
        public virtual Framework.Core.SlotAvatar[] GetSlotAvatarsByType(int type)
        {
            return null;
        }
    }
}
