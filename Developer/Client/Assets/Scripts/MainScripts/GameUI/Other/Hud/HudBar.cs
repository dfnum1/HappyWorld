/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	HudBar
作    者:	HappLI
描    述:	对象头顶血条
*********************************************************************/

using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using Framework.Module;
using UnityEngine;
using UnityEngine.UI;
using Framework.Core;
using Framework.Base;
using TopGame.Logic;

namespace TopGame.UI
{
    public class HudBar : IBuffCallBack, VariablePoolAble
    {
        private bool m_bInited = false;

        private bool m_bFirshShow = false;
        private byte m_HudType = 0xff;
        private float m_fDuration = 0;
        Actor m_pActor;
        RectTransform m_pTrans;
        Text m_pName;
        private Text m_hpText;

        //HpMark m_hpMark;
        private SliderLerp m_SliderLerp;
        private Image m_EnergyFill;
        private Image m_Frame;
        DynamicLoader m_Loader;

        BuffUILogic m_pBuffLogic;
        UISerialized m_pUI;

        ObjectHudHp m_pHudHp;
        public InstanceOperiaon pInstanceOperiaon = null;

        //------------------------------------------------------
        public void Awake(ObjectHudHp hudHp)
        {
            m_bInited = true;
            if (m_pHudHp == hudHp) return;
            if (m_pHudHp != null) m_pHudHp.RecyleDestroy();
             m_pHudHp = hudHp;
            m_Loader = new DynamicLoader();
            m_pTrans = null;
            m_pUI = null;
            m_SliderLerp = null;
            m_EnergyFill = null;
            m_pName = null;
            if (m_pHudHp)
            {
                m_pTrans = m_pHudHp.GetTransorm() as RectTransform;
                m_pUI = m_pHudHp.GetBehaviour<UISerialized>();
                if (m_pUI)
                {
                    m_pName = m_pUI.GetWidget<Text>("name");
                    m_hpText = m_pUI.GetWidget<UnityEngine.UI.Text>("hpText");
                    m_SliderLerp = m_pUI.GetWidget<TopGame.UI.SliderLerp>("slider");
                    m_EnergyFill = m_pUI.GetWidget<UnityEngine.UI.Image>("EnergyFill");
                    m_Frame = m_pUI.GetWidget<UnityEngine.UI.Image>("Frame");
                    if (m_EnergyFill)
                    {
                        m_EnergyFill.fillAmount = 0;
                    }

                    ClearShieldValue();
                }
            }

            SetFrameColor();

            //初始化buff
            if (m_pBuffLogic == null)
            {
                m_pBuffLogic = new BuffUILogic();
                m_pBuffLogic.Init(m_pUI, m_pActor);
            }
        }
        //------------------------------------------------------
        public void SetActor(Actor pActor)
        {
            if (m_pActor == pActor) return;
            ClearActor();
            m_pActor = pActor;
            if (m_pActor != null)
            {
                m_pActor.GetActorParameter().AddAttrEvent(EAttrType.MaxSp, OnMpChange);
                m_pActor.GetActorParameter().AddAttrEvent(EAttrType.MaxHp, OnHpChange); 
            }

            ClearShieldValue();
            SetFrameColor();
        }
        //------------------------------------------------------
        public Actor GetActor()
        {
            return m_pActor;
        }
        //------------------------------------------------------
        public int GetInstanceID()
        {
            if (m_pActor == null) return 0;
            return m_pActor.GetInstanceID();
        }
        //------------------------------------------------------
        public bool isInited()
        {
            return m_bInited;
        }
        //------------------------------------------------------
        public void SetHudType(byte type)
        {
            m_HudType = type;
        }
        //------------------------------------------------------
        public byte GetHudType()
        {
            return m_HudType;
        }
        //------------------------------------------------------
        void SetFrameColor()
        {
            if (!m_Frame || m_pActor == null)
            {
                return;
            }

            var attack = 1;// pActor.GetActorParameter().GetBaseAttr(EAttrType.Attack);

            //获取当前最大血量
            var maxHp = m_pActor.GetActorParameter().GetRuntimeParam().max_hp;

            var ratio = maxHp / attack;
            Color color = ColorUtil.GreenColor;

            if (ratio > 5)
            {
                color = ColorUtil.RedColor;
            }
            else if (ratio > 3)
            {
                color = ColorUtil.YellowColor;
            }
            UIUtil.SetGraphicColor(m_Frame, color);
        }
        //------------------------------------------------------
        public void Show(float damage)
        {
            if (m_pHudHp == null) return;
            m_fDuration = m_pHudHp.fShowDuration;
            m_bFirshShow = true;
            if (m_pBuffLogic == null)
            {
                m_pBuffLogic = new BuffUILogic();
                m_pBuffLogic.Init(m_pUI, m_pActor);
            }
            if(damage<=0)
            {
                if (m_EnergyFill)
                {
                    m_EnergyFill.fillAmount = m_pActor.GetActorParameter().GetRuntimeParam().GetSpRate();
                }
            }

            if (m_pActor != null)
            {
                bool bShowName = true;
                if(m_pActor.GetContextData()!=null)
                {
                    EMonsterType mosterType = EMonsterType.Boss;
                    if(m_pActor.GetContextData() is Data.CsvData_Monster.MonsterData)
                    {
                        mosterType = (m_pActor.GetContextData() as Data.CsvData_Monster.MonsterData).monsterType;
                    }
                    if (mosterType != EMonsterType.Boss)
                        bShowName = false;
                }
                //if (bShowName && m_pName)
                //    m_pName.text = m_pActor.GetActorParameter().GetName();
                //else if(m_pName) m_pName.text = "";

                int maxHp = m_pActor.GetActorParameter().GetRuntimeParam().max_hp;
                int hp = m_pActor.GetActorParameter().GetRuntimeParam().hp;


                //UIUtil.SetLabel(m_pName, $"Lv.{m_pActor.GetActorParameter().GetLevel()}");
                UIUtil.SetLabel(m_pName, "");

                if (m_SliderLerp)//血条过渡
                {
                    float newValue = (float)hp / Mathf.Max(1, maxHp);
                    float oldValue = (float)(hp + damage) / Mathf.Max(1, maxHp);
                    if (damage == 0 && newValue == oldValue)//第一次显示的情况
                    {
                        oldValue = 1;
                    }
                    if (m_SliderLerp.GetIsActive() == false)
                    {
                        m_SliderLerp.SetValue(newValue, oldValue,true);
                    }
                    else
                    {
                        m_SliderLerp.SetValue(newValue, oldValue);
                    }
                    UIUtil.SetLabel(m_hpText, hp.ToString());
                }

                //更新护盾值
                if (m_pActor.GetActorParameter().IsShielded())
                {
                    float shieldValue = m_pActor.GetActorParameter().GetRuntimeParam().GetSheild();

                    
                    if (m_SliderLerp)
                    {
                        float shieldScale = 0;
                        bool isFull = false;
                        if ((hp + shieldValue) > maxHp)//当前血量加上护盾后,超过血条上限的情况
                        {
                            shieldScale = 1 - hp / (maxHp + shieldValue);//计算出血量占比,然后取反就是护盾比例
                            isFull = true;
                        }
                        else
                        {
                            shieldScale = shieldValue / maxHp;
                        }
                        
                        m_SliderLerp.SetShieldValue(shieldScale, isFull);
                        //UIUtil.SetLabel(m_hpText, (hp + shieldValue).ToString());//只显示血量,不显示加上护盾值数值
                    }

                }
                else
                {
                    ClearShieldValue();
                }
            }
        }
        //------------------------------------------------------
        void OnMpChange(ExternEngine.FFloat fValue, ExternEngine.FFloat fOriginal, bool bPlus)
        {
            if (m_pActor == null)
            {
                return;
            }
            var actorParameter = m_pActor.GetActorParameter();
            var runtimeParam = actorParameter.GetRuntimeParam();
            //能量
            if (m_EnergyFill)
            {
                m_EnergyFill.fillAmount = runtimeParam.GetSpRate();
            }
        }
        //------------------------------------------------------
        void OnHpChange(ExternEngine.FFloat fValue, ExternEngine.FFloat fOriginal, bool bPlus)
        {
            if (m_pActor == null)
            {
                return;
            }
            int hp = m_pActor.GetActorParameter().GetRuntimeParam().hp;
            UIUtil.SetLabel(m_hpText, hp.ToString());
        }
        //------------------------------------------------------
        public void Destroy()
        {
            ClearActor();
            m_pBuffLogic = null;
            if (m_pHudHp != null) m_pHudHp.RecyleDestroy();
            m_pHudHp = null;
            m_bInited = false;
            if (pInstanceOperiaon != null) pInstanceOperiaon.Earse();
            pInstanceOperiaon = null;
        }
        //------------------------------------------------------
        public void Clear()
        {
            ClearActor();
            if (m_pTrans) m_pTrans.localPosition = Vector3.one * 1000;

            if (m_SliderLerp)
            {
                m_SliderLerp.Clear();
            }

            if(m_Loader!=null) m_Loader.ClearLoaded();

            if (pInstanceOperiaon != null) pInstanceOperiaon.Earse();
            pInstanceOperiaon = null;

            ClearShieldValue();
        }

        //------------------------------------------------------
        public void ClearActor()
        {
            if (m_pActor != null)
            {
                m_pActor.GetActorParameter().DelAttrEvent(EAttrType.MaxSp, OnMpChange);
                m_pActor.GetActorParameter().DelAttrEvent(EAttrType.MaxHp, OnHpChange);
            }
            m_pActor = null;
        }
        //------------------------------------------------------
        void ClearShieldValue()
        {
            if (m_SliderLerp)
            {
                m_SliderLerp.SetShieldValue(0,false);
            }
        }
        //------------------------------------------------------
        public bool IsEnd()
        {
            //1.满血不显示,只要不是满血就一直显示,生命恢复至100%后,延迟1s隐藏
            //2.身上有buff状态,需要显示,buff消失后若是满血,则延迟1s隐藏
            return (m_fDuration <= 0 && IsFullHP() && !HaveBuff()) || m_pActor == null || m_pActor.IsFlag(EWorldNodeFlag.Killed) || m_pActor.IsDestroy();
        }
        //------------------------------------------------------
        public EActorType GetBindActorType()
        {
            if (m_pActor == null) return EActorType.None;
            return m_pActor.GetActorType();
        }
        //------------------------------------------------------
        public byte GetAttackGroup()
        {
            if (m_pActor == null) return 0xff;
            return m_pActor.GetAttackGroup();
        }
        //------------------------------------------------------
        public void Update()
        {
            if (!m_bInited) return;
            if (m_pActor == null || CameraController.getInstance() == null) return;
            m_fDuration -= Time.deltaTime;
            if (m_fDuration>0)
            {
                if (m_pTrans && !m_pActor.IsFlag(EWorldNodeFlag.Killed) && !m_pActor.IsDestroy())
                {
                    GameInstance world = GameInstance.getInstance();
                    Vector3 pos = m_pActor.GetPosition();
                    var actionStateGraph = m_pActor.GetActionStateGraph();
                    if (actionStateGraph != null)
                    {
                        var slot = actionStateGraph.FindSlot("F_hp");
                        if (slot.transform != null)
                        {
                            pos = slot.transform.position;
                        }
                    }

                    Vector3 worldPos = pos;//+ Vector3.up * m_pActor.GetModelHeight();
                    if (m_pActor.GetCurrentActionState() != null) worldPos += m_pActor.GetCurrentActionState().GetCore().actor_name_offset;
                    if(!(m_pActor.GetActorType() == EActorType.Player) && !world.cameraController.IsInView(worldPos, 0))
                    {
                        m_pTrans.localPosition = Framework.Core.CommonUtility.INVAILD_POS;
                        m_fDuration = 0;
                        ClearActor();
                        m_bFirshShow = true;
                        return;
                    }

                    if (!m_pActor.IsVisible() || !UI.UIUtil.CanShowDynamicMarker())
                    {
                        m_pTrans.localPosition = Framework.Core.CommonUtility.INVAILD_POS;
                        m_bFirshShow = true;
                    }
                    else
                    {
                        Vector3 uiguiPos = Vector3.zero;
                        world.uiManager.ConvertWorldPosToUIPos(world.GetCurveWorldPosition(worldPos), true, ref uiguiPos);
                        if(m_bFirshShow)
                            m_pTrans.localPosition = uiguiPos;
                        else
                            m_pTrans.localPosition = Vector3.Lerp(m_pTrans.localPosition, uiguiPos, Time.fixedDeltaTime * 10);
                        m_bFirshShow = false;
                    }
                }
                else
                {
                    if(m_pTrans)
                        m_pTrans.localPosition = Framework.Core.CommonUtility.INVAILD_POS;
                    m_fDuration = 0;
                    ClearActor();
                    m_bFirshShow = true;
                }

                if (m_pBuffLogic != null)
                {
                    m_pBuffLogic.Update();
                }
            }
            else
            {
                m_bFirshShow = true;
                m_fDuration = 0;
                ClearActor();
                if(m_pTrans) m_pTrans.localPosition = Vector3.one * 1000;
            }
        }
        
        #region buff
        //------------------------------------------------------
        public void OnBeginBuff(Actor pActor, BufferState buff)
        {
            if (m_pBuffLogic != null)
            {
                m_pBuffLogic.OnBeginBuff(pActor, buff);
                m_pBuffLogic.OnBuffStateChange();
            }
        }
        //------------------------------------------------------
        public void OnEndBuff(Actor pActor, BufferState buff)
        {
            if (m_pBuffLogic != null)
            {
                m_pBuffLogic.OnBuffStateChange();
            }
        }
        #endregion
        //------------------------------------------------------
        bool IsFullHP()
        {
            if (m_pActor == null)
            {
                return false;
            }
            return m_pActor.GetActorParameter().GetRuntimeParam().GetHpRate() >= 1;
        }
        //------------------------------------------------------
        bool HaveBuff()
        {
            if (m_pActor == null)
            {
                return false;
            }
            var agent = m_pActor.GetActorAgent();
            if (agent == null)
            {
                return false;
            }

            var buffDatas = agent.GetBufferData();
            if (buffDatas == null || buffDatas.Count == 0) return false;
            return true;
        }
    }
}
