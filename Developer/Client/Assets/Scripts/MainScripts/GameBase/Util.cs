/********************************************************************
生成日期:	17:9:2019   16:19
类    名: 	Util
作    者:	HappLI
描    述:	通用工具集
*********************************************************************/
using Framework.BattlePlus;
using System;
using System.Collections.Generic;
using TopGame.Data;

namespace TopGame.Base
{

    [Framework.Plugin.AT.ATExportMono("基础通用工具")]
    public static class Util
    {
        //-----------------------------------------------------
        public static System.Text.StringBuilder stringBuilder
        {
            get
            {
                return GlobalUtil.stringBuilder;
            }
        }
        //-----------------------------------------------------
        public static void ResetGameObject(UnityEngine.GameObject gameObject, EResetType type = EResetType.Local)
        {
            GlobalUtil.ResetGameObject(gameObject, type);
        }
        //-----------------------------------------------------
        public static void ResetGameObject(UnityEngine.Transform pTrans, EResetType type = EResetType.Local)
        {
            GlobalUtil.ResetGameObject(pTrans, type);
        }
        //-----------------------------------------------------
        public static void Desytroy(UnityEngine.Object pObj)
        {
            GlobalUtil.Desytroy(pObj);
        }
        //------------------------------------------------------
        public static void DesytroyDelay(UnityEngine.Object pObj, float fDelay)
        {
            GlobalUtil.DesytroyDelay(pObj, fDelay);
        }
        //------------------------------------------------------
        public static void DestroyImmediate(UnityEngine.Object pObj)
        {
            GlobalUtil.DestroyImmediate(pObj);
        }
        //------------------------------------------------------
        public static void DestroyChilds(UnityEngine.GameObject pObj)
        {
            GlobalUtil.DestroyChilds(pObj);
        }
        //------------------------------------------------------
        public static void DestroyChildsDelay(UnityEngine.GameObject pObj, float fDelay)
        {
            GlobalUtil.DestroyChildsDelay(pObj, fDelay);
        }
        //------------------------------------------------------
        public static void DestroyChildsImmediate(UnityEngine.GameObject pObj)
        {
            GlobalUtil.DestroyChildsImmediate(pObj);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public static void SetActive(UnityEngine.GameObject pObj, bool bActive)
        {
            GlobalUtil.SetActive(pObj, bActive);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示帮助")]
        public static bool ShowAIHelp(string enter)
        {
#if USE_AIHELP
            SDK.AIHelpUnity.ShowHelp(enter);
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static string FormBytes(long b)
        {
            return GlobalUtil.FormBytes(b);
        }
        //-----------------------------------------------------
        public static DG.Tweening.Tweener Timer(float duration, System.Action action)
        {
            float time = 10;
            DG.Tweening.Tweener tw = DG.Tweening.DOTween.To(() => time, a => time = a, 1, duration);
            tw.onComplete += () => { action(); };
            return tw;
        }
        //------------------------------------------------------
        public static uint[] GetRandomNoRepeatList(uint[] ids, uint[] weights,uint count)
        {
            if (ids == null || weights == null || ids.Length != weights.Length)
            {
                Framework.Plugin.Logger.Error("数据有误,无法计算随机数组");
                return null;
            }
            uint[] result = new uint[count];
            List<uint> randomIds = ListPool<uint>.Get();
            randomIds.AddRange(ids);
            List<uint> randomWeights = ListPool<uint>.Get();
            randomWeights.AddRange(weights);

            for (int i = 0; i < count; i++)
            {
                uint totalWeight = 0;
                for (int j = 0; j < randomIds.Count; j++)
                {
                    totalWeight+= randomWeights[j];
                }

                int randomWeight = UnityEngine.Random.Range(0, (int)totalWeight);
                int index = 0;
                for (int k = 0; k < randomWeights.Count; k++)
                {
                    randomWeight -= (int)randomWeights[k];
                    if (randomWeight < 0)
                    {
                        break;
                    }
                    index++;
                }

                if (index < randomIds.Count)
                {
                    result[i] = randomIds[index];
                    randomIds.RemoveAt(index);
                    randomWeights.RemoveAt(index);
                }
            }

            ListPool<uint>.Release(randomIds);
            ListPool<uint>.Release(randomWeights);
            return result;
        }
        //------------------------------------------------------
        public static int GetHitCount(BattleStats battleStats, long roleGUID)
        {
            if (roleGUID == 0 || battleStats == null)
            {
                return 0;
            }
            int count = 0;


            var statData = battleStats.GetStatDataByGUID(roleGUID);
            if (statData == null) return count;
            var hits = statData.GetCollisionObject();
            if (hits == null) return count;
            foreach (var item in hits)
            {
                var cfg = DataManager.getInstance().BattleObject.GetData((uint)item.Key);
                if (cfg == null)
                {
                    continue;
                }

                if (cfg.objectType == Framework.Base.EObstacleType.SmallWall || cfg.objectType == Framework.Base.EObstacleType.Wall ||
                    cfg.objectType == Framework.Base.EObstacleType.Sunkens || cfg.objectType == Framework.Base.EObstacleType.Trap||
                    cfg.objectType == Framework.Base.EObstacleType.BombBox)
                {
                    count += item.Value;
                }
            }

            return count;
        }
        //------------------------------------------------------
        public static int GetPickObjectCount(BattleStats battleStats, long roleGUID, int objectID)
        {
            if (battleStats == null)
            {
                return 0;
            }

            var statData = battleStats.GetStatDataByGUID(roleGUID);
            if (statData == null) return 0;
            return statData.GetStatIdPickAndHitCount((uint)objectID);
        }
        //------------------------------------------------------
        public static int RandomNumber(int lower, int upper, Random random = null)
        {
            if (lower == upper) return lower;
            int value;
            if (random !=null) value = random.Next(lower, upper);
            else value = UnityEngine.Random.Range(lower, upper);
            return value;
        }
        //------------------------------------------------------
        public static int RandomNumberA(int lower, int upper, Random random = null)
        {
            if (lower == upper) return lower;
            return RandomNumber(lower, upper+1, random);
        }
    }
}