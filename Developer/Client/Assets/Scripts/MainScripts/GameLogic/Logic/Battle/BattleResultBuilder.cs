///********************************************************************
//生成日期:	4:11:2022  13:38
//类    名: 	BattleResultBuilder
//作    者:	HappLI
//描    述:	战斗结果数据构建
//*********************************************************************/

//using UnityEngine;
//using Framework.BattlePlus;
//using Framework.Core;
//using Framework.Logic;
//using Proto3;
//using System.Collections.Generic;
//using TopGame.Data;
//using TopGame.SvrData;

//namespace TopGame.Logic
//{
//    public class BattleResultBuilder
//    {
//        //------------------------------------------------------
//        public static ExitLevelRequest Create(User user, BattleWorld battleWorld, EBttleExitType state)
//        {
//            int passTime = 0;
//            int score = 0;
//            int gold = 0;
//            int distance = 0;
//            BattleStats stats = battleWorld.GetLogic<BattleStats>();
//            if (stats != null)
//            {
//                gold = stats.GetGold();
//                passTime = stats.GetTime();
//                score = stats.GetScore();
//                distance = stats.GetDistance();
//            }

//            BattleDB battleDb = user.GetBattleDB();

//            ExitLevelRequest request = new ExitLevelRequest();
//#if USE_SERVER
//            request.IsBattleServerCheck = true;
//#else
//            request.IsBattleServerCheck = false;
//#endif
//            request.LevelID = (int)battleDb.GetCurrentLevelID();
//            request.Status = (int)state;
//            request.StatisticTime = passTime;
//            request.StatisticMark = score;
//            if (stats != null)
//            {
//                request.TeamHP = stats.GetTeamHP();
//                request.RemainingPlayer = stats.GetLiveMemeber();//要确定在复活前计算存活人数
//            }

//            List<LevelMonsterInfo> monsterInfoReqs = new List<LevelMonsterInfo>();
//            Dictionary<uint, int> monsterHurts = stats.GetKillMonsterDamageStats();
//            Dictionary<uint, int> monsterKills = stats.GetKillMonsterStats();
//            foreach (var item in monsterHurts)
//            {
//                LevelMonsterInfo req = new LevelMonsterInfo();
//                req.Hurt = item.Value;
//                int killNum = 0;
//                monsterKills.TryGetValue(item.Key, out killNum);
//                req.KillNum = killNum;
//                req.MosterID = (int)item.Key;
//                monsterInfoReqs.Add(req);
//            }
//            request.MosterInfo.AddRange(monsterInfoReqs);
//#if UNITY_EDITOR
//            Framework.Plugin.Logger.Info("跑动距离：" + distance);
//#endif
//            request.Distance = distance;

//            request.GoldNum = gold;
//            Dictionary<uint, int> picked = stats.GetPickDropStats();
//            foreach (var child in picked)
//            {
//                if (child.Value > 0)
//                    request.PickItem.Add(new LevelItemInfo() { ItemID = (int)child.Key, ItemNum = (int)child.Value });
//            }

//            Dictionary<int, int> pickObjs = stats.GetPickObjectStats();
//            foreach (var child in pickObjs)
//            {
//                if (child.Value > 0)
//                    request.PickObject.Add(new LevelItemInfo() { ItemID = (int)child.Key, ItemNum = (int)child.Value });
//            }

//            Team team = user.GetCurrentTeam();
//            int ALLAttackerDamage = 0;
//            //英雄战斗数据
//            Dictionary<int, ActorBattleDetial> vOutputDetail = null;
//            BattleStats pStats = battleWorld.GetLogic<BattleStats>();
//            if (pStats != null) vOutputDetail = pStats.GetBattleOutputDetial();
//            foreach (var item in vOutputDetail)
//            {
//                if (!(item.Value.pConfig is CsvData_Player.PlayerData)) continue;
//                ALLAttackerDamage += item.Value.AttackerDamage;
//                HeroBattleData battleHeroData = new HeroBattleData(); //Id", "HeroID", "Level", "Attack", "Defense", "Recover

//                battleHeroData.Level = 1;
//                if (item.Value.svrData != null)
//                {
//                    battleHeroData.Level = item.Value.svrData.GetLevel();
//                }
//                battleHeroData.Id = item.Value.guid;
//                battleHeroData.HeroID = (int)item.Value.configID;
//                battleHeroData.Attack = item.Value.AttackerDamage;
//                battleHeroData.Defense = item.Value.HurtDamage;
//                battleHeroData.Recover = item.Value.RecHP;
//                battleHeroData.CurrentHp = item.Value.curHp;
//                battleHeroData.MaxHp = item.Value.totalHp;
//                battleHeroData.CurrentSp = item.Value.curSp;
//                battleHeroData.MaxSp = item.Value.totalSp;
//                if (team!=null) battleHeroData.OwnHero = !team.ExtraHeroId.Contains(battleHeroData.Id);
//                if (item.Value.attackGroup == 0)
//                        request.HeroBattleData.Add(battleHeroData);
//                else
//                    request.EnemyHeroBattleData.Add(battleHeroData);
//            }

//            if (battleDb.GetCurrentlevelType() == Proto3.LevelTypeCode.InfiniteParkour || battleDb.GetCurrentlevelType() == Proto3.LevelTypeCode.Roguelite)
//            {
//                Dictionary<int, int> hitObjs = stats.GetInfiniteParkoursHitObject();
//                foreach (var child in hitObjs)
//                {
//                    if (child.Value > 0)
//                        request.InfiniteParkourItem.Add(new LevelItemInfo() { ItemID = (int)child.Key, ItemNum = (int)child.Value });
//                }
//#if !USE_SERVER
//                //总积分
//            request.AllHurt = user.GetInfiniteDB().RoguelitePoint;
//#endif
//            }
//            else if (battleDb.GetCurrentlevelType() == Proto3.LevelTypeCode.Material)
//            {
//                request.Wave = battleWorld.GetLogic<BattleRegions>().GetCurWave();
//                var chapterConfig = Data.DataManager.getInstance().Chapter.GetData(battleDb.GetCurrentLevelID());
//                request.MaxWave = chapterConfig.mapRegionCount;
//            }
//            else if (battleDb.GetCurrentlevelType() == Proto3.LevelTypeCode.GuildBoss)
//            {
//                GuildDB guildDB = user.ProxyDB<GuildDB>(Data.EDBType.Guild);
//                request.MaxWave = (int)guildDB.bossStage;
//                request.Wave = (int)guildDB.guildId;
//                request.AllHurt = guildDB.isPractice ? 1 : 0;
//            }
//            else if (battleDb.GetCurrentlevelType() == Proto3.LevelTypeCode.UnlimitedBoss)
//            {
//                UnLimitedBossDB LimitedBossDB = user.ProxyDB<UnLimitedBossDB>(Data.EDBType.UnLimitedBoss); 
//                LimitedBossDB.HurtDamage = (int)ALLAttackerDamage;
//                if ((int)ALLAttackerDamage > LimitedBossDB.MaxDamage)
//                    LimitedBossDB.IsNewRecord = true;    //是否新记录
//                else
//                    LimitedBossDB.IsNewRecord = false;
//                //对boss造成伤害
//                request.AllHurt = (int)ALLAttackerDamage;
//            }

//            if (team != null)
//            {
//                request.Combat = team.GetFightPower();
//            }

//            //! 我方英雄信息

//            Framework.Plugin.Logger.Info("请求退出关卡,时间:" + passTime + ",分数:" + score);
//            return request;
//        }
//    }
//}
