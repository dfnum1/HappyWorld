#if UNITY_EDITOR
//auto generator code
using Framework.Core;
using System.Collections.Generic;
using Framework.Data;
namespace TopGame.ED
{
	[TableMapping]
	public partial class CsvDataFormer
	{
		public static System.Type GetDataType(System.Type type)
		{
			if(type == typeof(TopGame.Data.CsvData_BattleObject)) return typeof(TopGame.Data.CsvData_BattleObject.BattleObjectData);
			if(type == typeof(TopGame.Data.CsvData_Buff)) return typeof(TopGame.Data.CsvData_Buff.BuffData);
			if(type == typeof(TopGame.Data.CsvData_DropReward)) return typeof(TopGame.Data.CsvData_DropReward.DropRewardData);
			if(type == typeof(TopGame.Data.CsvData_Monster)) return typeof(TopGame.Data.CsvData_Monster.MonsterData);
			if(type == typeof(TopGame.Data.CsvData_PayList)) return typeof(TopGame.Data.CsvData_PayList.PayListData);
			if(type == typeof(TopGame.Data.CsvData_Player)) return typeof(TopGame.Data.CsvData_Player.PlayerData);
			if(type == typeof(TopGame.Data.CsvData_Skill)) return typeof(TopGame.Data.CsvData_Skill.SkillData);
			if(type == typeof(TopGame.Data.CsvData_Summon)) return typeof(TopGame.Data.CsvData_Summon.SummonData);
			if(type == typeof(TopGame.Data.CsvData_SystemConfig)) return typeof(TopGame.Data.CsvData_SystemConfig.SystemConfigData);
			if(type == typeof(TopGame.Data.CsvData_Audio)) return typeof(TopGame.Data.CsvData_Audio.AudioData);
			if(type == typeof(TopGame.Data.CsvData_DeviceUIAdapter)) return typeof(TopGame.Data.CsvData_DeviceUIAdapter.DeviceUIAdapterData);
			if(type == typeof(TopGame.Data.CsvData_Dialog)) return typeof(TopGame.Data.CsvData_Dialog.DialogData);
			if(type == typeof(TopGame.Data.CsvData_DungonThemes)) return typeof(TopGame.Data.DungonTheme);
			if(type == typeof(TopGame.Data.CsvData_EventDatas)) return typeof(TopGame.Data.EventData);
			if(type == typeof(TopGame.Data.CsvData_GoldDropEffect)) return typeof(TopGame.Data.CsvData_GoldDropEffect.GoldDropEffectData);
			if(type == typeof(TopGame.Data.CsvData_Item)) return typeof(TopGame.Data.CsvData_Item.ItemData);
			if(type == typeof(TopGame.Data.CsvData_Level)) return typeof(TopGame.Data.CsvData_Level.LevelData);
			if(type == typeof(TopGame.Data.CsvData_Models)) return typeof(TopGame.Data.CsvData_Models.ModelsData);
			if(type == typeof(TopGame.Data.CsvData_PlayerLevel)) return typeof(TopGame.Data.CsvData_PlayerLevel.PlayerLevelData);
			if(type == typeof(TopGame.Data.CsvData_Projectile)) return typeof(Framework.Core.ProjectileData);
			if(type == typeof(TopGame.Data.CsvData_Scene)) return typeof(TopGame.Data.CsvData_Scene.SceneData);
			if(type == typeof(TopGame.Data.CsvData_SkillDamage)) return typeof(TopGame.Data.CsvData_SkillDamage.SkillDamageData);
			if(type == typeof(TopGame.Data.CsvData_TargetPaths)) return typeof(TopGame.Data.TargetPathData);
			if(type == typeof(TopGame.Data.CsvData_Text)) return typeof(TopGame.Data.CsvData_Text.TextData);
			return null;
		}
		[TableMapping("tabletype")]
		public static System.Type GetTableType(string tableName)
		{
			tableName = tableName.ToLower();
			if (tableName.StartsWith("data.")) tableName = tableName.Replace("data.", "");
			if (!tableName.StartsWith("csvdata_")) tableName = "csvdata_" + tableName;
			int hash = UnityEngine.Animator.StringToHash(tableName);
			switch(hash)
			{
				case -146561370: return typeof(TopGame.Data.CsvData_BattleObject);
				case -1863375723: return typeof(TopGame.Data.CsvData_Buff);
				case 906640040: return typeof(TopGame.Data.CsvData_DropReward);
				case 856641488: return typeof(TopGame.Data.CsvData_Monster);
				case 884211508: return typeof(TopGame.Data.CsvData_PayList);
				case 2116951547: return typeof(TopGame.Data.CsvData_Player);
				case 168196920: return typeof(TopGame.Data.CsvData_Skill);
				case 710777463: return typeof(TopGame.Data.CsvData_Summon);
				case -963581922: return typeof(TopGame.Data.CsvData_SystemConfig);
				case 1279699418: return typeof(TopGame.Data.CsvData_Audio);
				case -1031938645: return typeof(TopGame.Data.CsvData_DeviceUIAdapter);
				case -1554595844: return typeof(TopGame.Data.CsvData_Dialog);
				case -1829301839: return typeof(TopGame.Data.CsvData_DungonThemes);
				case 1151311073: return typeof(TopGame.Data.CsvData_EventDatas);
				case -220898432: return typeof(TopGame.Data.CsvData_GoldDropEffect);
				case -86817816: return typeof(TopGame.Data.CsvData_Item);
				case -825142436: return typeof(TopGame.Data.CsvData_Level);
				case 48322455: return typeof(TopGame.Data.CsvData_Models);
				case -2079187569: return typeof(TopGame.Data.CsvData_PlayerLevel);
				case -174920154: return typeof(TopGame.Data.CsvData_Projectile);
				case -1925025643: return typeof(TopGame.Data.CsvData_Scene);
				case 1595923064: return typeof(TopGame.Data.CsvData_SkillDamage);
				case -96464625: return typeof(TopGame.Data.CsvData_TargetPaths);
				case -565984975: return typeof(TopGame.Data.CsvData_Text);
			}
			return null;
		}
		public static List<object> FormTree(Data_Base dataBase)
		{
			System.Type type = dataBase.GetType();
			List<object> vData = new List<object>();
			if(type == typeof(TopGame.Data.CsvData_BattleObject))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_BattleObject).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Buff))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Buff).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_DropReward))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_DropReward).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Monster))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Monster).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_PayList))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_PayList).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Player))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Player).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Skill))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Skill).datas)
					foreach (var sub in db.Value)
						vData.Add(sub);
			}
			if(type == typeof(TopGame.Data.CsvData_Summon))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Summon).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_SystemConfig))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_SystemConfig).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Audio))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Audio).datas)
					foreach (var sub in db.Value)
						vData.Add(sub);
			}
			if(type == typeof(TopGame.Data.CsvData_DeviceUIAdapter))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_DeviceUIAdapter).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Dialog))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Dialog).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_DungonThemes))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_DungonThemes).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_EventDatas))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_EventDatas).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_GoldDropEffect))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_GoldDropEffect).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Item))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Item).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Level))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Level).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Models))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Models).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_PlayerLevel))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_PlayerLevel).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Projectile))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Projectile).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Scene))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Scene).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_SkillDamage))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_SkillDamage).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_TargetPaths))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_TargetPaths).datas)
					vData.Add(db.Value);
			}
			if(type == typeof(TopGame.Data.CsvData_Text))
			{
				foreach(var db in (dataBase as TopGame.Data.CsvData_Text).datas)
					vData.Add(db.Value);
			}
			return vData;
		}
	}
}
#endif
