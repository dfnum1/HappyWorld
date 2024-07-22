/********************************************************************
类    名:   CsvData_Projectile
作    者:	自动生成
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Core;
namespace TopGame.Data
{
    public partial class CsvData_Projectile : Data_Base
    {
		Dictionary<uint, ProjectileData> m_vData = new Dictionary<uint, ProjectileData>();
		//-------------------------------------------
		public Dictionary<uint, ProjectileData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Projectile()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public ProjectileData GetData(uint id)
		{
			ProjectileData outData;
			if(m_vData.TryGetValue(id, out outData))
				return outData;
			return null;
		}
        //-------------------------------------------
        public override bool LoadData(string strContext,CsvParser csv = null)
        {
			if(csv == null) csv = new CsvParser();
			if(!csv.LoadTableString(strContext))
				return false;
			ClearData();
#if UNITY_EDITOR
            m_CsvParser = csv;
#endif			
			
			int i = csv.GetTitleLine();
			if(i < 0) return false;
			
			int nLineCnt = csv.GetLineCount();
			for(i++; i < nLineCnt; i++)
			{
				if(!csv[i]["id"].IsValid()) continue;
				
				ProjectileData data = new ProjectileData();
				
				data.id = csv[i]["id"].Uint();
				data.desc = csv[i]["desc"].String();
				data.life_time = csv[i]["life_time"].Float();
				data.speedLows = csv[i]["speedLows"].Vec3Array();
				data.speedUppers = csv[i]["speedUppers"].Vec3Array();
				data.accelerations = csv[i]["accelerations"].Vec3Array();
				data.speedMaxs = csv[i]["speedMaxs"].Vec3Array();
				data.speedLerp = csv[i]["speedLerp"].Vec2();
				data.type = (EProjectileType)csv[i]["type"].Int();
				data.track_target_slot = csv[i]["track_target_slot"].StringArray();
				data.track_target_offset = csv[i]["track_target_offset"].Vec3();
				data.collisionType = (EActorCollisionType)csv[i]["collisionType"].Int();
				data.aabb_min = csv[i]["aabb_min"].Vec3();
				data.aabb_max = csv[i]["aabb_max"].Vec3();
				data.minRotate = csv[i]["minRotate"].Vec3();
				data.maxRotate = csv[i]["maxRotate"].Vec3();
				data.hit_count = csv[i]["hit_count"].Byte();
				data.launch_delay = csv[i]["launch_delay"].Float();
				data.hit_step = csv[i]["hit_step"].Float();
				data.hit_rate_base = csv[i]["hit_rate_base"].Float();
				data.max_oneframe_hit = csv[i]["max_oneframe_hit"].Byte();
				data.attack_type_filer = csv[i]["attack_type_filer"].Ushort();
				data.counteract = csv[i]["counteract"].Bool();
				data.unSceneTest = csv[i]["unSceneTest"].Bool();
				data.externLogicSpeed = csv[i]["externLogicSpeed"].Bool();
				data.damage = csv[i]["damage"].Uint();
				data.buff = csv[i]["buff"].Uint();
				data.penetrable = csv[i]["penetrable"].Bool();
				data.explode_range = csv[i]["explode_range"].Float();
				data.explode_damage_id = csv[i]["explode_damage_id"].Uint();
				data.explode_effect = csv[i]["explode_effect"].String();
				data.explode_effect_offset = csv[i]["explode_effect_offset"].Vec3();
				data.target_hit_flag = csv[i]["target_hit_flag"].Byte();
				data.bound_count = csv[i]["bound_count"].Int();
				data.bound_range = csv[i]["bound_range"].Float();
				data.bound_effect = csv[i]["bound_effect"].String();
				data.bound_hit_effect = csv[i]["bound_hit_effect"].String();
				data.bound_effectSpeed = csv[i]["bound_effectSpeed"].Float();
				data.bound_sound_launch = csv[i]["bound_sound_launch"].String();
				data.bound_hit_sound = csv[i]["bound_hit_sound"].String();
				data.bound_damage_id = csv[i]["bound_damage_id"].Int();
				data.bound_buffs = csv[i]["bound_buffs"].IntArray();
				data.bound_lock_type = (ELockHitType)csv[i]["bound_lock_type"].Int();
				{
					int[] temps = csv[i]["bound_lock_conditions"].IntArray();
					if(temps !=null)
					{
						data.bound_lock_conditions= new ELockHitCondition[temps.Length];
						for(int e = 0; e < temps.Length; ++e)
							data.bound_lock_conditions[e] = (ELockHitCondition)temps[e];
					}
				}
				data.bound_lock_num = csv[i]["bound_lock_num"].Byte();
				data.bound_lock_param1 = csv[i]["bound_lock_param1"].IntArray();
				data.bound_lock_param2 = csv[i]["bound_lock_param2"].IntArray();
				data.bound_lock_param3 = csv[i]["bound_lock_param3"].IntArray();
				data.bound_lock_rode = csv[i]["bound_lock_rode"].ShortArray();
				data.bound_lockHeight = csv[i]["bound_lockHeight"].Bool();
				data.bound_minLockHeight = csv[i]["bound_minLockHeight"].Float();
				data.bound_maxLockHeight = csv[i]["bound_maxLockHeight"].Float();
				data.bound_speed = csv[i]["bound_speed"].Vec3();
				data.target_action_hit_ground = (EActionStateType)csv[i]["target_action_hit_ground"].Int();
				data.target_action_hit_ground_back = (EActionStateType)csv[i]["target_action_hit_ground_back"].Int();
				data.target_action_hit_air = (EActionStateType)csv[i]["target_action_hit_air"].Int();
				data.target_action_hit_air_back = (EActionStateType)csv[i]["target_action_hit_air_back"].Int();
				data.target_property_hit_ground = csv[i]["target_property_hit_ground"].Uint();
				data.target_property_hit_ground_back = csv[i]["target_property_hit_ground_back"].Uint();
				data.target_property_hit_air = csv[i]["target_property_hit_air"].Uint();
				data.target_property_hit_air_back = csv[i]["target_property_hit_air_back"].Uint();
				data.target_duration_hit = csv[i]["target_duration_hit"].Float();
				data.target_effect_hit_offset = csv[i]["target_effect_hit_offset"].Vec3();
				data.stuck_time_hit = csv[i]["stuck_time_hit"].Float();
				data.effectSpeed = csv[i]["effectSpeed"].Float();
				data.effect = csv[i]["effect"].String();
				data.target_effect_hit = csv[i]["target_effect_hit"].String();
				data.target_effect_hit_scale = csv[i]["target_effect_hit_scale"].Float();
				data.waring_effect = csv[i]["waring_effect"].String();
				data.waring_duration = csv[i]["waring_duration"].Float();
				data.sound_launch = csv[i]["sound_launch"].String();
				data.sound_hit = csv[i]["sound_hit"].String();
				data.scale = csv[i]["scale"].Float();
				data.OverEventID = csv[i]["OverEventID"].Int();
				data.StepEventID = csv[i]["StepEventID"].Int();
				data.fEventStepGap = csv[i]["fEventStepGap"].Float();
				data.HitEventID = csv[i]["HitEventID"].Int();
				data.bImmedate = csv[i]["bImmedate"].Bool();
				data.AttackEventID = csv[i]["AttackEventID"].Int();
				data.classify = csv[i]["classify"].Uint();
				data.launch_flag = csv[i]["launch_flag"].Uint();
				data.sound_hit_id = csv[i]["sound_hit_id"].Uint();
				data.sound_launch_id = csv[i]["sound_launch_id"].Uint();
				data.bound_flag = csv[i]["bound_flag"].Ushort();
				data.bound_sound_launch_id = csv[i]["bound_sound_launch_id"].Uint();
				data.bound_hit_sound_id = csv[i]["bound_hit_sound_id"].Uint();
				data.effect_hit_slot = csv[i]["effect_hit_slot"].String();
				
				m_vData.Add(data.id, data);
				OnAddData(data);
			}
			OnLoadCompleted();
            return true;
        }

#if UNITY_EDITOR
        CsvParser m_CsvParser = null;
        //-------------------------------------------
        public override void Save(string strPath = null)
        {
            if (string.IsNullOrEmpty(strPath)) strPath = strFilePath;
            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);

            System.IO.FileStream fs = new System.IO.FileStream(strPath, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);

            int head = m_CsvParser.GetTitleLine();
            int col = m_CsvParser.GetMaxColumn();
            for (int i=0; i <= head; ++i)
            {
                for (int c = 0; c < col; ++c)
                {
                    string vla = m_CsvParser[i][c].String();
                    writer.Write(vla);
                    if (c < col - 1) writer.Write(',');
                }
                writer.Write('\n');
            }

            foreach(var db in m_vData)
            {
				writer.Write(CsvParser.TableItem.Uint(db.Value.id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.desc));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.life_time));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3Array(db.Value.speedLows));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3Array(db.Value.speedUppers));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3Array(db.Value.accelerations));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3Array(db.Value.speedMaxs));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec2(db.Value.speedLerp));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.type));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.StringArray(db.Value.track_target_slot));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.track_target_offset));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.collisionType));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.aabb_min));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.aabb_max));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.minRotate));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.maxRotate));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Byte(db.Value.hit_count));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.launch_delay));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.hit_step));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.hit_rate_base));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Byte(db.Value.max_oneframe_hit));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Ushort(db.Value.attack_type_filer));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.counteract));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.unSceneTest));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.externLogicSpeed));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.damage));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.buff));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.penetrable));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.explode_range));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.explode_damage_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.explode_effect));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.explode_effect_offset));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Byte(db.Value.target_hit_flag));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.bound_count));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.bound_range));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.bound_effect));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.bound_hit_effect));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.bound_effectSpeed));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.bound_sound_launch));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.bound_hit_sound));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.bound_damage_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.IntArray(db.Value.bound_buffs));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.bound_lock_type));
				writer.Write(",");
				{
					string strTableTemp="";
					if(db.Value.bound_lock_conditions!=null)
					{
						for(int e = 0; e < db.Value.bound_lock_conditions.Length; ++e)
						{
							strTableTemp +=((int)db.Value.bound_lock_conditions[e]).ToString();
							if(e < db.Value.bound_lock_conditions.Length-1)strTableTemp +="|";
						}
						writer.Write(CsvParser.TableItem.String(strTableTemp));
					}
				}
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Byte(db.Value.bound_lock_num));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.IntArray(db.Value.bound_lock_param1));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.IntArray(db.Value.bound_lock_param2));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.IntArray(db.Value.bound_lock_param3));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.ShortArray(db.Value.bound_lock_rode));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.bound_lockHeight));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.bound_minLockHeight));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.bound_maxLockHeight));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.bound_speed));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.target_action_hit_ground));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.target_action_hit_ground_back));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.target_action_hit_air));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int((int)db.Value.target_action_hit_air_back));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.target_property_hit_ground));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.target_property_hit_ground_back));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.target_property_hit_air));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.target_property_hit_air_back));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.target_duration_hit));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Vec3(db.Value.target_effect_hit_offset));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.stuck_time_hit));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.effectSpeed));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.effect));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.target_effect_hit));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.target_effect_hit_scale));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.waring_effect));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.waring_duration));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.sound_launch));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.sound_hit));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.scale));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.OverEventID));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.StepEventID));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Float(db.Value.fEventStepGap));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.HitEventID));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Bool(db.Value.bImmedate));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Int(db.Value.AttackEventID));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.classify));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.launch_flag));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.sound_hit_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.sound_launch_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Ushort(db.Value.bound_flag));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.bound_sound_launch_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.Uint(db.Value.bound_hit_sound_id));
				writer.Write(",");
				writer.Write(CsvParser.TableItem.String(db.Value.effect_hit_slot));
                writer.Write('\n');
            }
            writer.Close();
            fs.Close();
        }
#endif		
        //-------------------------------------------
        public override void ClearData()
        {
			m_vData.Clear();
			base.ClearData();
        }
    }
}
