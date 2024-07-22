using Framework.Core;
using TopGame.Core;
using TopGame.Data;
using TopGame.Base;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace TopGame.UI
{
    public class BuffUILogic
    {
        struct SingleBuff
        {
            public GameObject BuffItem;
            public RawImage BuffRawImg;
            public Image BuffMask;
            public Text BuffTime;
            public CanvasGroup canvasGroup;
            public bool bAddAlpha;
        }

        List<SingleBuff> m_Buffs = new List<SingleBuff>();

        //Dictionary<BufferState, int> m_UIBuffMap = new Dictionary<BufferState, int>();
        List<BufferState> m_BuffData = null;
        DynamicLoader m_Loader;
        Actor m_pActor;
        float m_nThreshold = 3f;
        float m_nRatio = 5f;
        //------------------------------------------------------
        public void Init(UISerialized ui, Actor actor)
        {
            if (ui == null || actor == null) return;
            m_pActor = actor;
            m_Loader = new DynamicLoader();
            m_Buffs.Clear();
            for (int i = 1; i <= 5; i++)
            {
                SingleBuff singleBuff = new SingleBuff();
                UISerialized buffUI = ui.GetWidget<UISerialized>("Buff" + i);
                if (buffUI == null)
                {
                    continue;
                }

                singleBuff.BuffItem = buffUI.gameObject;
                buffUI.gameObject.SetActive(false);
                singleBuff.BuffRawImg = buffUI.GetWidget<RawImage>("Icon");
                singleBuff.BuffMask = buffUI.GetWidget<Image>("Mask");
                singleBuff.BuffTime = buffUI.GetWidget<Text>("Time");
                singleBuff.canvasGroup = buffUI.GetWidget<UnityEngine.CanvasGroup>("Buff" + i);
                SetCanvasGroupAlphaValue(singleBuff.canvasGroup, 1);

                m_Buffs.Add(singleBuff);
            }
            m_BuffData = new List<BufferState>();
        }
        //------------------------------------------------------
        public void BuffFilter()
        {
            if (m_pActor == null) return;
            ActorAgent agent = m_pActor.GetActorAgent();
            if (agent == null) return;
            Dictionary<uint, BufferState> actorBuffs = agent.GetBufferData();
            if (actorBuffs == null || m_BuffData == null) return;

            m_BuffData.Clear();
            foreach (var item in actorBuffs)
            {
                CsvData_Buff.BuffData buffData = item.Value.data as CsvData_Buff.BuffData;
                
                if (buffData.isShow)
                {
                    //Debug.Log(buffData.id);
                    m_BuffData.Add(item.Value);
                }
            }
        }
        //------------------------------------------------------
        public void RefreshBuffIconData()
        {
            if (m_BuffData == null) return;
            //m_UIBuffMap.Clear();

            SortBuff();


            if (m_pActor == null || m_pActor.IsFlag(EWorldNodeFlag.Killed)) m_BuffData.Clear();

            for (int i = 0; i < m_Buffs.Count; i++)
            {
                if (!m_Buffs[i].BuffItem) continue;

                if (i < m_BuffData.Count)
                {
                    if (m_BuffData[i].running_delta < m_BuffData[i].running_duration)
                    {
                        m_Buffs[i].BuffItem.SetActive(true);
                        //m_UIBuffMap.Add(m_BuffData[i], i);
                        FillBuffUIData(m_BuffData[i], i);
                    }
                    else if (m_Buffs[i].BuffItem)
                    {
                        m_Buffs[i].BuffItem.SetActive(false);
                        SetCanvasGroupAlphaValue(m_Buffs[i].canvasGroup, 1);
                    }

                }
                else
                {
                    m_Buffs[i].BuffItem.SetActive(false);
                    SetCanvasGroupAlphaValue(m_Buffs[i].canvasGroup, 1);
                }
            }
        }
        //------------------------------------------------------
        public void Update()
        {
            if (m_BuffData == null || m_BuffData.Count == 0) return;
            for (int i = 0; i < m_BuffData.Count; i++)
            {
                UpdateMaskUI(m_BuffData[i], i);
            }
            //foreach (var curValue in m_UIBuffMap)
            //{
            //    int index = curValue.Value;
            //    BufferState buff = curValue.Key;
            //    UpdateMaskUI(buff, index);
            //}
        }
        //------------------------------------------------------
        public void UpdateMaskUI(BufferState state, int index)
        {
            if (index >= m_Buffs.Count)
            {
                return;
            }
            SingleBuff item = m_Buffs[index];
            Image mask = item.BuffMask;
            mask.fillAmount = state.running_delta / state.running_duration;
            Text layerTime = item.BuffTime;
            UIUtil.SetLabel(layerTime, state.GetLayer().ToString());//不走多语言+tostring()
            //buff持续时间低于3s则开始闪烁,闪烁速率逐步加快直至消失
            float time = state.running_duration - state.running_delta;
            float alpha = item.canvasGroup.alpha;
            float value = (1 - Mathf.Clamp01(time / m_nThreshold)) * m_nRatio * Time.deltaTime;
            if (item.bAddAlpha)
            {
                alpha += value;
            }
            else
            {
                alpha -= value;
            }
            if (alpha < 0)
            {
                alpha = 0;
                item.bAddAlpha = true;
            }
            if (alpha > 1)
            {
                alpha = 1;
                item.bAddAlpha = false;
            }
            m_Buffs[index] = item;
            SetCanvasGroupAlphaValue(item.canvasGroup, alpha);
        }
        //------------------------------------------------------
        void SetCanvasGroupAlphaValue(CanvasGroup canvasGroup, float alpha)
        {
            if (canvasGroup)
            {
                canvasGroup.alpha = alpha;
            }
        }
        //------------------------------------------------------
        public void OnBuffStateChange()
        {
            BuffFilter();
            RefreshBuffIconData();
        }
        //------------------------------------------------------
        public void FillBuffUIData(BufferState state, int index)
        {
            RawImage icon = m_Buffs[index].BuffRawImg;
            string iconPath = null;
            if (state.data != null && state.data is CsvData_Buff.BuffData)
            {
                string path = (state.data as CsvData_Buff.BuffData).icon;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    iconPath = Util.stringBuilder.Append(path).Append(".png").ToString();
                }
            }

            if (icon)
            {
                icon.gameObject.SetActive(!string.IsNullOrWhiteSpace(iconPath));
                if (string.IsNullOrWhiteSpace(iconPath))
                {
                    Framework.Plugin.Logger.Warning("buff:" + state.id + ",配置buff图标路径为空,不显示buff");
                }
                else
                {
                    m_Loader.LoadObjectAsset(icon, iconPath);
                }
            }

            Image mask = m_Buffs[index].BuffMask;
            mask.fillAmount = 0;
            Text layerTime = m_Buffs[index].BuffTime;
            layerTime.text = "1";
        }
        //------------------------------------------------------
        static int CompareBuffStatePositionWeight(BufferState oth1, BufferState oth2)
        {
            if (oth1.data == null || oth2.data == null) return 0;
            return (int)((oth1.data as CsvData_Buff.BuffData).positionWeight - (oth2.data as CsvData_Buff.BuffData).positionWeight);
        }
        //------------------------------------------------------
        void SortBuff()
        {
            Framework.Plugin.SortUtility.QuickSort<BufferState>(ref m_BuffData, CompareBuffStatePositionWeight);
        }
        //------------------------------------------------------
        public void OnBeginBuff(Actor pActor, BufferState buffer)
        {
            if (buffer == null)
            {
                return;
            }
            CsvData_Buff.BuffData data = buffer.data as CsvData_Buff.BuffData;
            if (data == null || !data.isShow) return;

            //根据buff配置类型飘字
            GameInstance.getInstance().dynamicTexter.OnActorPopBuffText(pActor, Core.LocalizationManager.ToLocalization(data.floatNameId), data.type);
        }
        //------------------------------------------------------
        public void Destroy()
        {
            m_Buffs.Clear();
            //m_UIBuffMap.Clear();
            m_BuffData.Clear();
            m_Loader = null;
            m_pActor = null;
        }
    }
}
