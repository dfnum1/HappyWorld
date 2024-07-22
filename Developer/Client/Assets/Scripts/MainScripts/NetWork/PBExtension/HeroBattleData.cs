/********************************************************************
生成日期:	10:7:2019   12:06
类    名: 	HeroBattleData
作    者:	HappLI
描    述:	HeroBattleData 的分类，实现自定义数据和方法
*********************************************************************/
using Framework.Core;
using Framework.Data;
using System.Collections.Generic;

namespace Proto3
{
    public partial class HeroBattleData : TopGame.SvrData.ISvrHero
    {
        public override byte GetCareerID()
        {
            return 0;
        }

        public override uint GetConfigID()
        {
            return (uint)HeroID;
        }

        public override IContextData GetContextData()
        {
            return TopGame.Data.DataManager.getInstance().Player.GetData(GetConfigID());
        }

        protected override void OnExternAttrsParams(IContextData configData = null)
        {
        }

        public override long GetGUID()
        {
            return Id;
        }

        public override ushort GetLevel()
        {
            return (ushort)(Level + GetExternLevel());
        }

        public override List<SvrSkillData> GetSkills()
        {
            return null;
        }
    }
}
