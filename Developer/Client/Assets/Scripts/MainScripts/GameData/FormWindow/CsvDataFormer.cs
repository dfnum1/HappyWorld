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
			return vData;
		}
	}
}
#endif
