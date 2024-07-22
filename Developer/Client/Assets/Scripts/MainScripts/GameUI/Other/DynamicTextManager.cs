/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	BloodManager
作    者:	HappLI
描    述:	战斗飘血
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using Framework.Module;
using UnityEngine;
using UnityEngine.UI;
using Framework.Core;
using Framework.Logic;
using Framework.Base;
using TopGame.Base;
using Framework.BattlePlus;
using TopGame.Logic;
using TopGame.SvrData;

namespace TopGame.UI
{
    public class DynamicTextManager : RecycleAble<DynamicTextManager.DynamicText>
    {
        public class DynamicText
        {
            public AInstanceAble pInstance;
            public AWorldNode pActor;
            public Vector3 popPosition;
            public Vector3 endPosition;
            public string strText;
            public string imgPath;
            public float Vaule;
            public int CreateTime;
            public EPermanentAssetType EPermanentAssetType;
            public Color color;
            Logic.BloodTweenEffecter m_pEffector;

            bool bLoaded = false;

            public void OnSign(InstanceOperiaon pOp)
            {
                pOp.bUsed = true;
            }
            public void OnSpawnCallback(InstanceOperiaon pOp)
            {
                bLoaded = true;
                pInstance = pOp.pPoolAble;
                if (pInstance != null)
                {
                    UI.UIManager mgr = GameInstance.getInstance().uiManager;
                    Vector3 spawnPos = Vector3.zero;
                    mgr.ConvertWorldPosToUIPos(GameInstance.getInstance().GetCurveWorldPosition(popPosition), true, ref spawnPos);
                    m_pEffector = pInstance.GetComponent<Logic.BloodTweenEffecter>();

                    if (!pInstance.gameObject.activeSelf)
                        pInstance.gameObject.SetActive(true);

                    if (m_pEffector != null)
                    {
                        Vector3 uiScaler = Vector3.one;
                        UI.UIKits.ScaleWithScreenScale(ref uiScaler);

                        //m_pEffector.SetColor(color);
                        m_pEffector.SetText(strText);
                        m_pEffector.SetImg(imgPath);
                        Vector3 startOffset = Vector3.zero;
                        if (m_pEffector.bRandomStart)
                        {
                            startOffset = new Vector3(UnityEngine.Random.Range(-m_pEffector.nStartXRandomRange, m_pEffector.nStartXRandomRange) * uiScaler.x,
                               UnityEngine.Random.Range(0, m_pEffector.nStartYRandomRange) * uiScaler.y, 0);
                            if (m_pEffector.bLocal)
                            {
                                m_pEffector.SetRuntimeStartPos(startOffset);
                            }
                            else
                            {
                                m_pEffector.SetRuntimeStartPos(new Vector3(spawnPos.x + startOffset.x, spawnPos.y + startOffset.y, spawnPos.z + startOffset.z));
                            }
                        }
                        if (m_pEffector.bRandomEnd)
                        {
                            if(m_pEffector.bLocal)
                               m_pEffector.SetRuntimeEndPos(startOffset + new Vector3(UnityEngine.Random.Range(-m_pEffector.nRandomRange, m_pEffector.nRandomRange) * uiScaler.x, UnityEngine.Random.Range(0, m_pEffector.nRandomRange) * uiScaler.y ,0));
                            else
                               m_pEffector.SetRuntimeEndPos(startOffset + new Vector3(spawnPos.x + UnityEngine.Random.Range(-m_pEffector.nRandomRange, m_pEffector.nRandomRange) * uiScaler.x, spawnPos.y + UnityEngine.Random.Range(0, m_pEffector.nRandomRange) * uiScaler.y , spawnPos.z));
                        }
                        if (endPosition != Vector3.zero)
                        {
                            Vector3 endPos = Vector3.zero;
                            Vector3 screenPos = UIManager.GetInstance().ConvertUIPosToScreen(endPosition);//世界坐标转屏幕坐标

                            Vector2 local_temp = Vector2.zero;
                            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.GetInstance().GetRoot() as RectTransform, screenPos, UIManager.GetInstance().GetUICamera(), out local_temp))
                            {
                                endPos.x = local_temp.x;//屏幕坐标转本地坐标
                                endPos.y = local_temp.y;
                            }

                            m_pEffector.SetRuntimeEndPos(endPos);
                        }
                        m_pEffector.Play(spawnPos);
                    }


                    
                }
                else
                    m_pEffector = null;

            }

            public bool Update(float fFrame)
            {
                if (!bLoaded) return false;
                if (m_pEffector == null) return true;
                return m_pEffector.bEnd();
            }

            public void Destroy()
            {
                if (pInstance)
                {
                    UISerialized serize = pInstance.GetComponent<UISerialized>();
                    if (serize)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            RectTransform rect = serize.GetWidget<RectTransform>("element" + i);
                            if (rect)
                                rect.gameObject.SetActive(false);
                        }
                    }
                }

                if (pInstance)
                    FileSystemUtil.DeSpawnInstance(pInstance, 10);
                pInstance = null;
                pActor = null;
                strText = null;
                imgPath = null;
                m_pEffector = null;
                bLoaded = false;
                endPosition = Vector3.zero;
            }
        }
        List<DynamicText> m_vDynamics = null;
        public DynamicTextManager()
        {
            m_vDynamics = new List<DynamicText>(64);
        }
        //------------------------------------------------------
        public void Destroy()
        {
            for (int i = 0; i < m_vDynamics.Count; ++i)
                m_vDynamics[i].Destroy();
            m_vDynamics.Clear();
        }
        //------------------------------------------------------
        public void OnActorPopText(AWorldNode pActor, uint nContent, VariablePoolAble externVarial = null)
        {
            uint strId = nContent;
            string iconPath = null;
            if (externVarial != null && externVarial is BattleDamage)
            {
                BattleDamage battleDamage = (BattleDamage)externVarial;
                if (!battleDamage.CanPost(EDamagePostBit.PopText))
                    return;
            }

            GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PopText);
            InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
            if (pCb == null) return;
            pCb.pByParent = UI.UIManager.GetAutoUIRoot();

            DynamicText pDt = m_vRecycle.Get();
            pDt.Destroy();
            pDt.pActor = pActor;
            if (pActor.GetAttackGroup() != (byte)EActorType.Player && externVarial != null && externVarial is BattleDamage)
                pDt.popPosition = ((BattleDamage)externVarial).hitPosition;
            else
                pDt.popPosition = pActor.GetPosition() + Vector3.up * pActor.GetBounds().GetSize().y;
         
            pDt.strText = Core.LocalizationManager.ToLocalization(strId);
            pDt.imgPath = iconPath;
            pCb.OnCallback = pDt.OnSpawnCallback;
            pCb.OnSign = pDt.OnSign;

            m_vDynamics.Add(pDt);
        }
        //------------------------------------------------------
        public void OnActorPopBuffText(AWorldNode pActor, string nContent, EBuffType type)
        {
            EPermanentAssetType path = EPermanentAssetType.PopText;
            switch (type)
            {
                case EBuffType.None:
                    break;
                case EBuffType.Damage:
                    path = EPermanentAssetType.DamageBuffText;
                    break;
                case EBuffType.Cure:
                    path = EPermanentAssetType.CureBuffText;
                    break;
                case EBuffType.Buff:
                    path = EPermanentAssetType.GainBuffText;
                    break;
                case EBuffType.Debuff:
                    path = EPermanentAssetType.DebuffText;
                    break;
                case EBuffType.Shield:
                    path = EPermanentAssetType.ShieldBuffText;
                    break;
                default:
                    break;
            }
            GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(path);
            InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
            if (pCb == null) return;
            pCb.pByParent = UI.UIManager.GetAutoUIRoot();

            DynamicText pDt = m_vRecycle.Get();
            pDt.Destroy();
            pDt.pActor = pActor;
            if (pActor != null)
            {
                pDt.popPosition = pActor.GetPosition() + Vector3.up * (pActor.GetBounds().GetSize()).y;
            }
            pDt.strText = nContent;
            pCb.OnCallback = pDt.OnSpawnCallback;
            pCb.OnSign = pDt.OnSign;

            m_vDynamics.Add(pDt);
        }
        //------------------------------------------------------
        public void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, bool bPlus, VariablePoolAble externVarial = null)
        {
            EElementType elementType = 0;
            if (Mathf.Abs(fValue) <= 0.01f) return;

            bool bTeamSoul = false;
            ESkillType attackSkillType = ESkillType.Attack;
            if (externVarial != null && externVarial is BattleDamage)
            {
                BattleDamage battleDamage = ((BattleDamage)externVarial);
                attackSkillType = battleDamage.attackSkillType;
                if (!battleDamage.CanPost(EDamagePostBit.PopText))
                    return;

                if(battleDamage.pAttacker!=null && battleDamage.pAttacker.GetActorType() == EActorType.Mecha)
                {
                    bTeamSoul = true;
                }
            }
            if(type == EAttrType.MaxSp)
            {
                if(externVarial !=null && externVarial is Variable1)
                {
                    if (!((Variable1)externVarial).boolVal)
                        return;
                }
            }
            //if (pActor.GetActorType() == EActorType.s.Summon)
            //{
            //    uint id = pActor.GetConfigID();
            //    CsvData_Summon.SummonData data = DataManager.getInstance().Summon.GetData(id);
            //    if (data == null) return;
            //    if (data.hurtDisplayType == 0)
            //    {
            //        OnActorPopText(pActor, 10001002);
            //        return;
            //    }
            //}
            GameObject pAsset = null;
            EPermanentAssetType ePermanentAssetType = 0;
            string tempStrValue = "";
            switch(type)
            {
                case EAttrType.Crit:
                    {
                        tempStrValue = Base.Util.stringBuilder.Append("-").Append(fValue).ToString();
                        if (externVarial != null && externVarial is BattleDamage && ((BattleDamage)externVarial).bodyPart != null)
                        {
                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.WeakBloodCrit);
                        }
                        else
                        {
                            if (pActor.GetAttackGroup() == (byte)EActorType.Player)
                            {
                                if (bTeamSoul)
                                {
                                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.TeamSoulCrilBlood);
                                }
                                if (pAsset == null) pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PlayerCrit);
                            }
                            else
                            {
                                if (externVarial != null && externVarial is BattleDamage && IsElementRestraint(((BattleDamage)externVarial)))
                                {
                                    if (pAsset == null)
                                    {
                                        elementType = ((BattleDamage)externVarial).attackElementType;
                                        switch (elementType)
                                        {
                                            case EElementType.Water:
                                                pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintCritiDamageWater);
                                                break;
                                            case EElementType.Fire:
                                                pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintCritiDamageFire);
                                                break;
                                            case EElementType.Wood:
                                                pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintCritiDamageWood);
                                                break;
                                            case EElementType.Soil:
                                                pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintCritiDamageSoil);
                                                break;
                                            case EElementType.Thunder:
                                                pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintCritiDamageElectric);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bTeamSoul)
                                    {
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.TeamSoulCrilBlood);
                                    }
                                    if (pAsset == null)
                                    {
                                        if (attackSkillType != ESkillType.Attack)
                                        {
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SkillCritBlood);
                                        }
                                        else
                                        {
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.Crit);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    break;
                case EAttrType.MaxHp:
                    {
                        tempStrValue = bPlus ? Base.Util.stringBuilder.Append("+").Append(fValue).ToString() :
                    Base.Util.stringBuilder.Append("-").Append(fValue).ToString();

                        if (externVarial != null && externVarial is BattleDamage && ((BattleDamage)externVarial).bodyPart != null)
                        {
                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.WeakBlood);
                        }
                        else if (externVarial != null && externVarial is BattleDamage && IsElementRestraint(((BattleDamage)externVarial)))
                        {
                            if (pAsset == null)
                            {
                                elementType = ((BattleDamage)externVarial).attackElementType;
                                switch (elementType)
                                {
                                    case EElementType.Water:
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintDamageWater);
                                        break;
                                    case EElementType.Fire:
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintDamageFire);
                                        break;
                                    case EElementType.Wood:
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintDamageWood);
                                        break;
                                    case EElementType.Soil:
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintDamageSoil);
                                        break;
                                    case EElementType.Thunder:
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.RestraintDamageElectric);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (bTeamSoul)
                            {
                                if (bPlus) pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.TeamSoulCure);
                                else pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.TeamSoulBlood);
                            }
                            if (pActor.GetAttackGroup() == (byte)EActorType.Player)
                            {
                                if (bPlus)
                                {
                                    if (pAsset == null)
                                    {
                                    	ePermanentAssetType = EPermanentAssetType.PlayerCure;
                                        var dynamicText = GetDynamicTextByType(EPermanentAssetType.PlayerCure);
                                        if (dynamicText != null)
                                        {
                                            dynamicText.Vaule += fValue;
                                            dynamicText.strText = Base.Util.stringBuilder.Append("+").Append(dynamicText.Vaule)
                                                .ToString();
                                            return;
                                        }
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PlayerCure);
                                    }
                                }
                                else
                                {
                                    if (bTeamSoul)
                                    {
                                        pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.TeamSoulBlood);
                                    }
                                    if (pAsset == null)
                                    {
                                        if (attackSkillType == ESkillType.Attack)
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PlayerBlood);
                                        else
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PlayerSkillBlood);
                                    }
                                }
                            }
                            else
                            {
                                if (bPlus)
                                {
                                    if (pAsset == null) pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.Cure);
                                }
                                else
                                {
                                    if (pAsset == null)
                                    {
                                        if (attackSkillType == ESkillType.Attack)
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.Blood);
                                        else
                                            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SkillBlood);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EAttrType.Posture:
                    {
                        tempStrValue = Base.Util.stringBuilder.Append("-").Append(fValue).ToString();
                        pAsset = bPlus ? null : Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.PostureDamage);
                    }
                    break;
                case EAttrType.MaxSp:
                    {
                        if(externVarial !=null && externVarial is BattleDamage)
                        {
                            BattleDamage damage = (BattleDamage)externVarial ;
                            if(bPlus)
                            {
                                //! 只有击杀回能才表现
                                if (damage.pActor == null || !damage.bKilled) return;
                                if (pActor.GetAttackGroup() == (byte)EActorType.Player)
                                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SpRec);
                                else
                                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.EmmySpRec);

                                tempStrValue = Base.Util.stringBuilder.Append("+").Append(fValue).ToString();
                            }
                            else
                            {
                                if (pActor.GetAttackGroup() == (byte)EActorType.Player)
                                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SpDrop);
                                else
                                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.EmmySpDrop);
                                tempStrValue = Base.Util.stringBuilder.Append("-").Append(fValue).ToString();
                            }
                        }
                    }
                    break;
                case EAttrType.Dodge:
                    pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.Dodge);
                    break;
            }
            if (pAsset == null) return;

            InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
            if (pCb == null) return;
            pCb.pByParent = UI.UIManager.GetAutoUIRoot();

            DynamicText pDt = m_vRecycle.Get();
            pDt.Destroy();
            //pDt.color = Color.white;

            if (pActor.GetAttackGroup() != (byte)EActorType.Player && externVarial != null && externVarial is BattleDamage)
                pDt.popPosition = ((BattleDamage)externVarial).hitPosition;
            else
                pDt.popPosition = pActor.GetPosition() + Vector3.up * pActor.GetModelHeight();

            if (!CameraKit.IsInView(pDt.popPosition,-0.1f))
            {
                if (CameraKit.MainCamera)
                {
                    Vector3 screen = CameraKit.MainCamera.WorldToScreenPoint(pDt.popPosition);
                    pDt.popPosition = CameraKit.MainCamera.ScreenToWorldPoint(new Vector3(screen.x * 0.9f, screen.y * 0.9f, screen.z * 0.9f));
                }
            }

            pDt.pActor = pActor;
            pDt.strText = tempStrValue;
            pDt.Vaule = fValue;
            pDt.EPermanentAssetType = ePermanentAssetType;
            pDt.CreateTime = DateTime.Now.Second;
            pCb.OnCallback = pDt.OnSpawnCallback;
            pCb.OnSign = pDt.OnSign;

            m_vDynamics.Add(pDt);
        }
        //------------------------------------------------------
        public void OnSkillActionState(Actor pActor, Skill skill, ActionState state, bool bBegin)
        {
            if (bBegin && skill  != null)
            {
                var skillDamage = (CsvData_Skill.SkillData)skill.skillData;
                if (skillDamage != null && skillDamage.isFloating)
                {
                    GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SkillText);
                    InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
                    if (pCb == null) return;
                    pCb.pByParent = UI.UIManager.GetAutoUIRoot();

                    DynamicText pDt = m_vRecycle.Get();
                    pDt.Destroy();
                    pDt.pActor = pActor;
                    if (pActor != null)
                    {
                        pDt.popPosition = pActor.GetPosition() + Vector3.up * (pActor.GetBounds().GetSize()).y;
                    }
                    pDt.strText = Core.LocalizationManager.ToLocalization(skillDamage.name);
                    pDt.imgPath = skillDamage.icon;
                    pCb.OnCallback = pDt.OnSpawnCallback;
                    pCb.OnSign = pDt.OnSign;

                    m_vDynamics.Add(pDt);
                }
            }
        }
        //------------------------------------------------------
        public void OnAddSkill(Actor pActor, CsvData_Skill.SkillData skill)
        {
            if (skill != null)
            {
                var skillDamage = skill;
                if (skillDamage != null && skillDamage.isFloating)
                {
                    GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.SkillText);
                    InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
                    if (pCb == null) return;
                    pCb.pByParent = UI.UIManager.GetAutoUIRoot();

                    DynamicText pDt = m_vRecycle.Get();
                    pDt.Destroy();
                    pDt.pActor = pActor;
                    if (pActor != null)
                    {
                        pDt.popPosition = pActor.GetPosition() + Vector3.up * (pActor.GetBounds().GetSize()).y;
                    }
                    pDt.strText = Core.LocalizationManager.ToLocalization(skillDamage.skillFloating);
                    pDt.imgPath = skillDamage.icon;
                    pCb.OnCallback = pDt.OnSpawnCallback;
                    pCb.OnSign = pDt.OnSign;

                    m_vDynamics.Add(pDt);
                }
            }
        }
        //------------------------------------------------------
        public void OnKillActor(Actor pActor)
        {
            if (pActor == null)
            {
                return;
            }

            GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.ScoreText);

            InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(pAsset);
            if (pCb == null) return;
            pCb.pByParent = UI.UIManager.GetAutoUIRoot();

            DynamicText pDt = m_vRecycle.Get();
            pDt.Destroy();
            pDt.pActor = pActor;
            int score = 0;
            if (pActor != null)
            {
                pDt.popPosition = pActor.GetPosition() + Vector3.up * (pActor.GetBounds().GetSize()).y;
            }
            pDt.strText = score.ToString();//分数 = 击杀怪物分数 * 分数加成
            //pDt.imgPath = skillDamage.icon;
            pCb.OnCallback = pDt.OnSpawnCallback;
            pCb.OnSign = pDt.OnSign;

            m_vDynamics.Add(pDt);
        }
        //------------------------------------------------------
        public bool IsElementRestraint(BattleDamage damage)
        {
         //   if(damage.targetElementType != Base.EElementType.None && damage.attackElementType != Base.EElementType.None) return true;

            return false;
        }

        //------------------------------------------------------
        public void Update(float fFrameTime)
        {
            for (int i = 0; i < m_vDynamics.Count;)
            {
                if (m_vDynamics[i].Update(fFrameTime))
                {
                    m_vDynamics[i].Destroy();
                    m_vRecycle.Release(m_vDynamics[i]);
                    m_vDynamics.RemoveAt(i);
                }
                else
                    ++i;
            }
        }
        //------------------------------------------------------
        private DynamicText GetDynamicTextByType(EPermanentAssetType ePermanentAssetType)
        {
            foreach (DynamicText dynamicText in m_vDynamics)
            {
                if (dynamicText.EPermanentAssetType == ePermanentAssetType && dynamicText.CreateTime == DateTime.Now.Second )
                {
                    return dynamicText;
                }
            }
            return null;
        }
    }
}
