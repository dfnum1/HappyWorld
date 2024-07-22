#if UNITY_EDITOR
//auto generator code
using Framework.Core;
using System.Collections.Generic;
using Framework.Data;
using TopGame.Data;
namespace TopGame.ED
{
	public partial class CsvDataFormer
	{
		static Framework.Module.AFramework GetFramework(){ return null; }
		[TableMapping("mappingdata")]
		public static void MappingData(Data_Base table)
		{
			if(table == null) return;
			int hash = UnityEngine.Animator.StringToHash(table.GetType().Name.ToLower());
			switch(hash)
			{
			case -146561370:
			{
				CsvData_BattleObject BattleObject = (CsvData_BattleObject)table;
				CsvData_Models Models=DataEditorUtil.GetTable<CsvData_Models>();
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_DropReward DropReward=DataEditorUtil.GetTable<CsvData_DropReward>();
				CsvData_Buff Buff=DataEditorUtil.GetTable<CsvData_Buff>();
				CsvData_SkillDamage SkillDamage=DataEditorUtil.GetTable<CsvData_SkillDamage>();
				if(Models!=null&& Text!=null&& DropReward!=null&& Buff!=null&& SkillDamage!=null)
			{
				foreach(var db in BattleObject.datas)
				{
					db.Value.Models_modelId_data = Models.GetData(db.Value.modelId);
					db.Value.Text_nameText_data = Text.GetData(db.Value.nameText);
					if(db.Value.dropReward!=null)
					{
						db.Value.DropReward_dropReward_data = new CsvData_DropReward.DropRewardData[db.Value.dropReward.Length];
						for(int i=0; i < db.Value.dropReward.Length; ++i)
						{
							db.Value.DropReward_dropReward_data[i] = DropReward.GetData(db.Value.dropReward[i]);
						}
					}
					db.Value.Buff_buffId_data = Buff.GetData(db.Value.buffId);
					db.Value.SkillDamage_skillDamageID_data = SkillDamage.GetData(db.Value.skillDamageID);
				}
			}
			}
			break;
			case 856641488:
			{
				CsvData_Monster Monster = (CsvData_Monster)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_Models Models=DataEditorUtil.GetTable<CsvData_Models>();
				CsvData_Buff Buff=DataEditorUtil.GetTable<CsvData_Buff>();
				CsvData_SkillDamage SkillDamage=DataEditorUtil.GetTable<CsvData_SkillDamage>();
				CsvData_DropReward DropReward=DataEditorUtil.GetTable<CsvData_DropReward>();
				if(Text!=null&& Models!=null&& Buff!=null&& SkillDamage!=null&& DropReward!=null)
			{
				foreach(var db in Monster.datas)
				{
					db.Value.Text_nameText_data = Text.GetData(db.Value.nameText);
					db.Value.Text_descText_data = Text.GetData(db.Value.descText);
					db.Value.Models_modelId_data = Models.GetData(db.Value.modelId);
					db.Value.Buff_buffId_data = Buff.GetData(db.Value.buffId);
					db.Value.SkillDamage_damageId_data = SkillDamage.GetData(db.Value.damageId);
					if(db.Value.hitDropReward!=null)
					{
						db.Value.DropReward_hitDropReward_data = new CsvData_DropReward.DropRewardData[db.Value.hitDropReward.Length];
						for(int i=0; i < db.Value.hitDropReward.Length; ++i)
						{
							db.Value.DropReward_hitDropReward_data[i] = DropReward.GetData(db.Value.hitDropReward[i]);
						}
					}
					if(db.Value.dropReward!=null)
					{
						db.Value.DropReward_dropReward_data = new CsvData_DropReward.DropRewardData[db.Value.dropReward.Length];
						for(int i=0; i < db.Value.dropReward.Length; ++i)
						{
							db.Value.DropReward_dropReward_data[i] = DropReward.GetData(db.Value.dropReward[i]);
						}
					}
				}
			}
			}
			break;
			case 2116951547:
			{
				CsvData_Player Player = (CsvData_Player)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_Models Models=DataEditorUtil.GetTable<CsvData_Models>();
				CsvData_Buff Buff=DataEditorUtil.GetTable<CsvData_Buff>();
				if(Text!=null&& Models!=null&& Buff!=null)
			{
				foreach(var db in Player.datas)
				{
					db.Value.Text_quality_data = Text.GetData(db.Value.quality);
					db.Value.Models_nModelID_data = Models.GetData(db.Value.nModelID);
					db.Value.Models_hModelID_data = Models.GetData(db.Value.hModelID);
					db.Value.Text_nameText_data = Text.GetData(db.Value.nameText);
					db.Value.Text_descText_data = Text.GetData(db.Value.descText);
					db.Value.Text_spDescText_data = Text.GetData(db.Value.spDescText);
					db.Value.Text_icon_data = Text.GetData(db.Value.icon);
					db.Value.Text_playerIcon_data = Text.GetData(db.Value.playerIcon);
					db.Value.Text_storyText_data = Text.GetData(db.Value.storyText);
					db.Value.Text_nameResource_data = Text.GetData(db.Value.nameResource);
					if(db.Value.resurrectionBuffs!=null)
					{
						db.Value.Buff_resurrectionBuffs_data = new CsvData_Buff.BuffData[db.Value.resurrectionBuffs.Length];
						for(int i=0; i < db.Value.resurrectionBuffs.Length; ++i)
						{
							db.Value.Buff_resurrectionBuffs_data[i] = Buff.GetData(db.Value.resurrectionBuffs[i]);
						}
					}
				}
			}
			}
			break;
			case 168196920:
			{
				CsvData_Skill Skill = (CsvData_Skill)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_SkillDamage SkillDamage=DataEditorUtil.GetTable<CsvData_SkillDamage>();
				if(Text!=null&& SkillDamage!=null)
			{
				foreach(var db in Skill.datas)
				{
					for(int a =0; a< db.Value.Count; ++a)
					{
						db.Value[a].Text_name_data = Text.GetData(db.Value[a].name);
						db.Value[a].Text_upgradeDescID_data = Text.GetData(db.Value[a].upgradeDescID);
						db.Value[a].SkillDamage_skillDamageID_data = SkillDamage.GetData(db.Value[a].skillDamageID);
						db.Value[a].Text_skillFloating_data = Text.GetData(db.Value[a].skillFloating);
					}
				}
			}
			}
			break;
			case -1863375723:
			{
				CsvData_Buff Buff = (CsvData_Buff)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_SkillDamage SkillDamage=DataEditorUtil.GetTable<CsvData_SkillDamage>();
				if(Text!=null&& SkillDamage!=null)
			{
				foreach(var db in Buff.datas)
				{
					db.Value.Text_qualityFrame_data = Text.GetData(db.Value.qualityFrame);
					db.Value.SkillDamage_damage_data = SkillDamage.GetData(db.Value.damage);
					db.Value.Text_nameId_data = Text.GetData(db.Value.nameId);
					db.Value.Text_desc_data = Text.GetData(db.Value.desc);
					db.Value.Text_floatNameId_data = Text.GetData(db.Value.floatNameId);
				}
			}
			}
			break;
			case -96464625:
			{
				CsvData_TargetPaths TargetPaths = (CsvData_TargetPaths)table;
			{
				foreach(var db in TargetPaths.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			}
			break;
			case 1151311073:
			{
				CsvData_EventDatas EventDatas = (CsvData_EventDatas)table;
			{
				foreach(var db in EventDatas.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			}
			break;
			case 710777463:
			{
				CsvData_Summon Summon = (CsvData_Summon)table;
				CsvData_Models Models=DataEditorUtil.GetTable<CsvData_Models>();
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				if(Models!=null&& Text!=null)
			{
				foreach(var db in Summon.datas)
				{
					db.Value.Models_nModelID_data = Models.GetData(db.Value.nModelID);
					db.Value.Text_battleIcon_data = Text.GetData(db.Value.battleIcon);
					db.Value.Text_battleName_data = Text.GetData(db.Value.battleName);
					db.Value.Text_battleDesc_data = Text.GetData(db.Value.battleDesc);
				}
			}
			}
			break;
			case -1829301839:
			{
				CsvData_DungonThemes DungonThemes = (CsvData_DungonThemes)table;
			{
				foreach(var db in DungonThemes.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			}
			break;
			case -1554595844:
			{
				CsvData_Dialog Dialog = (CsvData_Dialog)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				CsvData_Models Models=DataEditorUtil.GetTable<CsvData_Models>();
				if(Text!=null&& Models!=null)
			{
				foreach(var db in Dialog.datas)
				{
					db.Value.Text_nContext_data = Text.GetData(db.Value.nContext);
					db.Value.Text_nSyaName_data = Text.GetData(db.Value.nSyaName);
					db.Value.Text_nDesName_data = Text.GetData(db.Value.nDesName);
					db.Value.Models_nModel_data = Models.GetData(db.Value.nModel);
				}
			}
			}
			break;
			case 1595923064:
			{
				CsvData_SkillDamage SkillDamage = (CsvData_SkillDamage)table;
				CsvData_Buff Buff=DataEditorUtil.GetTable<CsvData_Buff>();
				if(Buff!=null)
			{
				foreach(var db in SkillDamage.datas)
				{
					if(db.Value.searchTypeGroupID!=null)
					{
						db.Value.Buff_searchTypeGroupID_data = new CsvData_Buff.BuffData[db.Value.searchTypeGroupID.Length];
						for(int i=0; i < db.Value.searchTypeGroupID.Length; ++i)
						{
							db.Value.Buff_searchTypeGroupID_data[i] = Buff.GetData(db.Value.searchTypeGroupID[i]);
						}
					}
					if(db.Value.extraBuffId!=null)
					{
						db.Value.Buff_extraBuffId_data = new CsvData_Buff.BuffData[db.Value.extraBuffId.Length];
						for(int i=0; i < db.Value.extraBuffId.Length; ++i)
						{
							db.Value.Buff_extraBuffId_data[i] = Buff.GetData(db.Value.extraBuffId[i]);
						}
					}
				}
			}
			}
			break;
			case 906640040:
			{
				CsvData_DropReward DropReward = (CsvData_DropReward)table;
				CsvData_GoldDropEffect GoldDropEffect=DataEditorUtil.GetTable<CsvData_GoldDropEffect>();
				if(GoldDropEffect!=null)
			{
				foreach(var db in DropReward.datas)
				{
					db.Value.GoldDropEffect_goldDropEffect_data = GoldDropEffect.GetData(db.Value.goldDropEffect);
				}
			}
			}
			break;
			case 884211508:
			{
				CsvData_PayList PayList = (CsvData_PayList)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				if(Text!=null)
			{
				foreach(var db in PayList.datas)
				{
					db.Value.Text_name_data = Text.GetData(db.Value.name);
					db.Value.Text_desc_data = Text.GetData(db.Value.desc);
					db.Value.Text_icon_data = Text.GetData(db.Value.icon);
				}
			}
			}
			break;
			case -86817816:
			{
				CsvData_Item Item = (CsvData_Item)table;
				CsvData_Text Text=DataEditorUtil.GetTable<CsvData_Text>();
				if(Text!=null)
			{
				foreach(var db in Item.datas)
				{
					db.Value.Text_name_data = Text.GetData(db.Value.name);
					db.Value.Text_des_data = Text.GetData(db.Value.des);
					db.Value.Text_qualityFrame_data = Text.GetData(db.Value.qualityFrame);
					db.Value.Text_icon_data = Text.GetData(db.Value.icon);
					db.Value.Text_sourceText_data = Text.GetData(db.Value.sourceText);
				}
			}
			}
			break;
			}
		}
	}
}
#endif
