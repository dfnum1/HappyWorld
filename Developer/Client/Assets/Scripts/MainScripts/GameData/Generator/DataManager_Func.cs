/********************************************************************
作    者:	自动生成
描    述:
*********************************************************************/
#if USE_SERVER
using ExternEngine;
#else
using UnityEngine;
#endif
using Framework.Data;
using Framework.Core;
namespace TopGame.Data
{
	public partial class DataManager : ADataManager
	{
		private CsvData_Audio	m_pAudio;
		public CsvData_Audio	Audio{
			get{
			#if UNITY_EDITOR
				if(m_pAudio== null) m_pAudio=DataEditorUtil.GetTable<CsvData_Audio>();
			#endif
				return m_pAudio;
			}
			private set{m_pAudio=value;}
		}
		private CsvData_BattleObject	m_pBattleObject;
		public CsvData_BattleObject	BattleObject{
			get{
			#if UNITY_EDITOR
				if(m_pBattleObject== null) m_pBattleObject=DataEditorUtil.GetTable<CsvData_BattleObject>();
			#endif
				return m_pBattleObject;
			}
			private set{m_pBattleObject=value;}
		}
		private CsvData_Buff	m_pBuff;
		public CsvData_Buff	Buff{
			get{
			#if UNITY_EDITOR
				if(m_pBuff== null) m_pBuff=DataEditorUtil.GetTable<CsvData_Buff>();
			#endif
				return m_pBuff;
			}
			private set{m_pBuff=value;}
		}
		private CsvData_DeviceUIAdapter	m_pDeviceUIAdapter;
		public CsvData_DeviceUIAdapter	DeviceUIAdapter{
			get{
			#if UNITY_EDITOR
				if(m_pDeviceUIAdapter== null) m_pDeviceUIAdapter=DataEditorUtil.GetTable<CsvData_DeviceUIAdapter>();
			#endif
				return m_pDeviceUIAdapter;
			}
			private set{m_pDeviceUIAdapter=value;}
		}
		private CsvData_Dialog	m_pDialog;
		[Framework.Plugin.AT.ATField("",null,"",1)]
		public CsvData_Dialog	Dialog{
			get{
			#if UNITY_EDITOR
				if(m_pDialog== null) m_pDialog=DataEditorUtil.GetTable<CsvData_Dialog>();
			#endif
				return m_pDialog;
			}
			private set{m_pDialog=value;}
		}
		private CsvData_DropReward	m_pDropReward;
		public CsvData_DropReward	DropReward{
			get{
			#if UNITY_EDITOR
				if(m_pDropReward== null) m_pDropReward=DataEditorUtil.GetTable<CsvData_DropReward>();
			#endif
				return m_pDropReward;
			}
			private set{m_pDropReward=value;}
		}
		private CsvData_DungonThemes	m_pDungonThemes;
		public CsvData_DungonThemes	DungonThemes{
			get{
			#if UNITY_EDITOR
				if(m_pDungonThemes== null) m_pDungonThemes=DataEditorUtil.GetTable<CsvData_DungonThemes>();
			#endif
				return m_pDungonThemes;
			}
			private set{m_pDungonThemes=value;}
		}
		private CsvData_EventDatas	m_pEventDatas;
		public CsvData_EventDatas	EventDatas{
			get{
			#if UNITY_EDITOR
				if(m_pEventDatas== null) m_pEventDatas=DataEditorUtil.GetTable<CsvData_EventDatas>();
			#endif
				return m_pEventDatas;
			}
			private set{m_pEventDatas=value;}
		}
		private CsvData_GoldDropEffect	m_pGoldDropEffect;
		public CsvData_GoldDropEffect	GoldDropEffect{
			get{
			#if UNITY_EDITOR
				if(m_pGoldDropEffect== null) m_pGoldDropEffect=DataEditorUtil.GetTable<CsvData_GoldDropEffect>();
			#endif
				return m_pGoldDropEffect;
			}
			private set{m_pGoldDropEffect=value;}
		}
		private CsvData_Item	m_pItem;
		public CsvData_Item	Item{
			get{
			#if UNITY_EDITOR
				if(m_pItem== null) m_pItem=DataEditorUtil.GetTable<CsvData_Item>();
			#endif
				return m_pItem;
			}
			private set{m_pItem=value;}
		}
		private CsvData_Level	m_pLevel;
		public CsvData_Level	Level{
			get{
			#if UNITY_EDITOR
				if(m_pLevel== null) m_pLevel=DataEditorUtil.GetTable<CsvData_Level>();
			#endif
				return m_pLevel;
			}
			private set{m_pLevel=value;}
		}
		private CsvData_Models	m_pModels;
		public CsvData_Models	Models{
			get{
			#if UNITY_EDITOR
				if(m_pModels== null) m_pModels=DataEditorUtil.GetTable<CsvData_Models>();
			#endif
				return m_pModels;
			}
			private set{m_pModels=value;}
		}
		private CsvData_Monster	m_pMonster;
		public CsvData_Monster	Monster{
			get{
			#if UNITY_EDITOR
				if(m_pMonster== null) m_pMonster=DataEditorUtil.GetTable<CsvData_Monster>();
			#endif
				return m_pMonster;
			}
			private set{m_pMonster=value;}
		}
		private CsvData_PayList	m_pPayList;
		public CsvData_PayList	PayList{
			get{
			#if UNITY_EDITOR
				if(m_pPayList== null) m_pPayList=DataEditorUtil.GetTable<CsvData_PayList>();
			#endif
				return m_pPayList;
			}
			private set{m_pPayList=value;}
		}
		private CsvData_Player	m_pPlayer;
		public CsvData_Player	Player{
			get{
			#if UNITY_EDITOR
				if(m_pPlayer== null) m_pPlayer=DataEditorUtil.GetTable<CsvData_Player>();
			#endif
				return m_pPlayer;
			}
			private set{m_pPlayer=value;}
		}
		private CsvData_PlayerLevel	m_pPlayerLevel;
		public CsvData_PlayerLevel	PlayerLevel{
			get{
			#if UNITY_EDITOR
				if(m_pPlayerLevel== null) m_pPlayerLevel=DataEditorUtil.GetTable<CsvData_PlayerLevel>();
			#endif
				return m_pPlayerLevel;
			}
			private set{m_pPlayerLevel=value;}
		}
		private CsvData_Projectile	m_pProjectile;
		public CsvData_Projectile	Projectile{
			get{
			#if UNITY_EDITOR
				if(m_pProjectile== null) m_pProjectile=DataEditorUtil.GetTable<CsvData_Projectile>();
			#endif
				return m_pProjectile;
			}
			private set{m_pProjectile=value;}
		}
		private CsvData_Scene	m_pScene;
		public CsvData_Scene	Scene{
			get{
			#if UNITY_EDITOR
				if(m_pScene== null) m_pScene=DataEditorUtil.GetTable<CsvData_Scene>();
			#endif
				return m_pScene;
			}
			private set{m_pScene=value;}
		}
		private CsvData_Skill	m_pSkill;
		public CsvData_Skill	Skill{
			get{
			#if UNITY_EDITOR
				if(m_pSkill== null) m_pSkill=DataEditorUtil.GetTable<CsvData_Skill>();
			#endif
				return m_pSkill;
			}
			private set{m_pSkill=value;}
		}
		private CsvData_SkillDamage	m_pSkillDamage;
		public CsvData_SkillDamage	SkillDamage{
			get{
			#if UNITY_EDITOR
				if(m_pSkillDamage== null) m_pSkillDamage=DataEditorUtil.GetTable<CsvData_SkillDamage>();
			#endif
				return m_pSkillDamage;
			}
			private set{m_pSkillDamage=value;}
		}
		private CsvData_Summon	m_pSummon;
		public CsvData_Summon	Summon{
			get{
			#if UNITY_EDITOR
				if(m_pSummon== null) m_pSummon=DataEditorUtil.GetTable<CsvData_Summon>();
			#endif
				return m_pSummon;
			}
			private set{m_pSummon=value;}
		}
		private CsvData_SystemConfig	m_pSystemConfig;
		public CsvData_SystemConfig	SystemConfig{
			get{
			#if UNITY_EDITOR
				if(m_pSystemConfig== null) m_pSystemConfig=DataEditorUtil.GetTable<CsvData_SystemConfig>();
			#endif
				return m_pSystemConfig;
			}
			private set{m_pSystemConfig=value;}
		}
		private CsvData_TargetPaths	m_pTargetPaths;
		public CsvData_TargetPaths	TargetPaths{
			get{
			#if UNITY_EDITOR
				if(m_pTargetPaths== null) m_pTargetPaths=DataEditorUtil.GetTable<CsvData_TargetPaths>();
			#endif
				return m_pTargetPaths;
			}
			private set{m_pTargetPaths=value;}
		}
		private CsvData_Text	m_pText;
		public CsvData_Text	Text{
			get{
			#if UNITY_EDITOR
				if(m_pText== null) m_pText=DataEditorUtil.GetTable<CsvData_Text>();
			#endif
				return m_pText;
			}
			private set{m_pText=value;}
		}
		//-------------------------------------------
		protected override Data_Base  Parser(CsvParser csvParser, int index, TextAsset pAsset, EDataType eType = EDataType.Binary)
		{
			Data_Base pCsv = null;
			switch(index)
			{
				case 1279699418:
				{
					 Audio = new CsvData_Audio();
					 pCsv = Audio; break;
				}
				case -146561370:
				{
					 BattleObject = new CsvData_BattleObject();
					 pCsv = BattleObject; break;
				}
				case -1863375723:
				{
					 Buff = new CsvData_Buff();
					 pCsv = Buff; break;
				}
				case -1031938645:
				{
					 DeviceUIAdapter = new CsvData_DeviceUIAdapter();
					 pCsv = DeviceUIAdapter; break;
				}
				case -1554595844:
				{
					 Dialog = new CsvData_Dialog();
					 pCsv = Dialog; break;
				}
				case 906640040:
				{
					 DropReward = new CsvData_DropReward();
					 pCsv = DropReward; break;
				}
				case -1829301839:
				{
					 DungonThemes = new CsvData_DungonThemes();
					 pCsv = DungonThemes; break;
				}
				case 1151311073:
				{
					 EventDatas = new CsvData_EventDatas();
					 pCsv = EventDatas; break;
				}
				case -220898432:
				{
					 GoldDropEffect = new CsvData_GoldDropEffect();
					 pCsv = GoldDropEffect; break;
				}
				case -86817816:
				{
					 Item = new CsvData_Item();
					 pCsv = Item; break;
				}
				case -825142436:
				{
					 Level = new CsvData_Level();
					 pCsv = Level; break;
				}
				case 48322455:
				{
					 Models = new CsvData_Models();
					 pCsv = Models; break;
				}
				case 856641488:
				{
					 Monster = new CsvData_Monster();
					 pCsv = Monster; break;
				}
				case 884211508:
				{
					 PayList = new CsvData_PayList();
					 pCsv = PayList; break;
				}
				case 2116951547:
				{
					 Player = new CsvData_Player();
					 pCsv = Player; break;
				}
				case -2079187569:
				{
					 PlayerLevel = new CsvData_PlayerLevel();
					 pCsv = PlayerLevel; break;
				}
				case -174920154:
				{
					 Projectile = new CsvData_Projectile();
					 pCsv = Projectile; break;
				}
				case -1925025643:
				{
					 Scene = new CsvData_Scene();
					 pCsv = Scene; break;
				}
				case 168196920:
				{
					 Skill = new CsvData_Skill();
					 pCsv = Skill; break;
				}
				case 1595923064:
				{
					 SkillDamage = new CsvData_SkillDamage();
					 pCsv = SkillDamage; break;
				}
				case 710777463:
				{
					 Summon = new CsvData_Summon();
					 pCsv = Summon; break;
				}
				case -963581922:
				{
					 SystemConfig = new CsvData_SystemConfig();
					 pCsv = SystemConfig; break;
				}
				case -96464625:
				{
					 TargetPaths = new CsvData_TargetPaths();
					 pCsv = TargetPaths; break;
				}
				case -565984975:
				{
					 Text = new CsvData_Text();
					 pCsv = Text; break;
				}
			}
			if(pCsv!=null)
			{
				if(eType == EDataType.Binary)
				{
					if(!pCsv.LoadBinary(BeginBinary(pAsset.bytes)))
						Framework.Plugin.Logger.Warning(pAsset.name + ".bytes: load failed ... " );
					else
					m_nLoadCnt++;
					EndBinary();
				}
				else if(eType == EDataType.Json)
				{
					if( !pCsv.LoadJson(  pAsset.text ) )
						Framework.Plugin.Logger.Warning(pAsset.name + ".json: load failed ... " );
					else
						m_nLoadCnt++;
				}
				else
				{
					if( !pCsv.LoadData(  pAsset.text,csvParser ) )
						Framework.Plugin.Logger.Warning(pAsset.name + ".csv: load failed ... " );
					else
						m_nLoadCnt++;
				}
#if UNITY_EDITOR
				pCsv.strFilePath = UnityEditor.AssetDatabase.GetAssetPath(pAsset);
#endif
			}
					return pCsv;
		}
		//-------------------------------------------
		protected override void Mapping()
		{
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
			{
				foreach(var db in TargetPaths.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			{
				foreach(var db in EventDatas.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			{
				foreach(var db in Summon.datas)
				{
					db.Value.Models_nModelID_data = Models.GetData(db.Value.nModelID);
					db.Value.Text_battleIcon_data = Text.GetData(db.Value.battleIcon);
					db.Value.Text_battleName_data = Text.GetData(db.Value.battleName);
					db.Value.Text_battleDesc_data = Text.GetData(db.Value.battleDesc);
				}
			}
			{
				foreach(var db in DungonThemes.datas)
				{
					db.Value.Mapping(GetFramework());
				}
			}
			{
				foreach(var db in Dialog.datas)
				{
					db.Value.Text_nContext_data = Text.GetData(db.Value.nContext);
					db.Value.Text_nSyaName_data = Text.GetData(db.Value.nSyaName);
					db.Value.Text_nDesName_data = Text.GetData(db.Value.nDesName);
					db.Value.Models_nModel_data = Models.GetData(db.Value.nModel);
				}
			}
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
			{
				foreach(var db in DropReward.datas)
				{
					db.Value.GoldDropEffect_goldDropEffect_data = GoldDropEffect.GetData(db.Value.goldDropEffect);
				}
			}
			{
				foreach(var db in PayList.datas)
				{
					db.Value.Text_name_data = Text.GetData(db.Value.name);
					db.Value.Text_desc_data = Text.GetData(db.Value.desc);
					db.Value.Text_icon_data = Text.GetData(db.Value.icon);
				}
			}
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
	}
}
