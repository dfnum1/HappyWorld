/********************************************************************
生成日期:	2020-6-16
类    名: 	UnLockListener
作    者:	zdq
描    述:	挂载在功能按钮上的脚本
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Base;
using Framework.Core;
using TopGame.UI;
using DG.Tweening;

namespace TopGame.Logic
{
    [System.Serializable]
    public struct UnLockInfo
    {
        public uint id;
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.UI.EUIType")]
#endif
        public int binderUI;
    }


    /// <summary>
    /// 锁定状态
    /// </summary>
    public enum EModuleLockState
    {
        None,
        Lock,
        UnlockWaitShow,
        Unlock,
    }
    public class UnLockListener : MonoBehaviour, VariablePoolAble
    {
        public UnLockInfo info;
        public List<GameObject> controlGo;
        public Color grayColor = Color.white;

        public DOTweenAnimation unlockTween;

        [System.NonSerialized]
        public EModuleLockState moduleLockState = EModuleLockState.None;

        private bool m_bPlayUnlock = false;
        Framework.Core.IContextData m_cfg;
        public Framework.Core.IContextData cfg
        {
            get
            {
                if (m_cfg == null)
                {
                    AUnLockManager unlockMgr = getUnlockMgr();
                    if (unlockMgr != null) m_cfg = unlockMgr.GetUnlockData(info.id);
                }
                return m_cfg;
            }
        }
        //------------------------------------------------------
        AUnLockManager getUnlockMgr()
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return null;
            Core.GameFramework pGameFramework = Framework.Module.ModuleManager.mainFramework as Core.GameFramework;
            if (pGameFramework == null) return null;
            return pGameFramework.unlockMgr;
        }
        //------------------------------------------------------
        void Awake()
        {
            AUnLockManager unlockMgr = getUnlockMgr();
            if (unlockMgr == null) return;
            if (unlockTween)
            {
                unlockTween.autoPlay = false;
                unlockTween.autoKill = false;
            }
            unlockMgr.AddUnLockUIItem(this);
            RefreshState();
            if (!unlockMgr.IsEnable())
                m_bPlayUnlock = true;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            //为了防止切换账号导致的缓存或者没有添加到manager中的情况,这边在显示的时候再次添加,
            Awake();
        }
        //------------------------------------------------------
        //private void Update()
        //{
        //    //监听不到 GameInstance.getInstance().GetState(),需要外部调用
        //}
        //------------------------------------------------------
        private void OnDestroy()
        {
            AUnLockManager unlockMgr = getUnlockMgr();
            if (unlockMgr == null) return;
            unlockMgr.RemoveUnlockListener(this);
        }
        //------------------------------------------------------
        public void Destroy()
        {

        }
        //------------------------------------------------------
        public bool OnUnlock()
        {
            AUnLockManager unlockMgr = getUnlockMgr();
            if (unlockMgr == null) return false;

            var preState = moduleLockState;

            if (moduleLockState == EModuleLockState.Lock || moduleLockState == EModuleLockState.None)
                moduleLockState = EModuleLockState.UnlockWaitShow;

            //判断是否第一次解锁
            if (moduleLockState == EModuleLockState.UnlockWaitShow)//上一个状态是锁定状态
            {
                //处理建筑解锁的特效展示
                if (info.binderUI!= 0)
                {
                    if (UI.UIKits.IsShowUI(info.binderUI))
                    {
                        if (Framework.Plugin.Guide.GuideSystem.getInstance().bDoing == false)//引导状态时,不展示功能解锁弹窗
                        {
                            unlockMgr.PopUnlockTipPanel(this);
                        }
                        moduleLockState = EModuleLockState.Unlock;
                        
                        //Debug.Log("tween Play:" + Time.realtimeSinceStartup);
                    }
                }
                //else
                //{
                //    if(info.binderUI != 0)
                //        return false;
                //}
            }
            else
            {
                moduleLockState = EModuleLockState.Unlock;//刷新解锁状态,这边可能存在状态为none的情况,所以需要设置
            }
            if(preState!= moduleLockState)
                m_bPlayUnlock = true;
            else
            {
                if (unlockTween)
                {
                    if (unlockTween.tween == null)
                        unlockTween.CreateTween();
                    if(unlockTween.tween.Duration()>0.01f)
                        unlockTween.tween.fullPosition = unlockTween.tween.Duration()- 0.01f;
                    unlockTween.DOPlay();
                }
            }
            RefreshState();
            return true;
        }
        //------------------------------------------------------
        public void OnLock()
        {
            moduleLockState = EModuleLockState.Lock;

            if (controlGo == null || cfg == null)
            {
                return;
            }

            //锁定状态时
            //原本隐藏 保持,要置灰就置灰
            //原本显示 要隐藏就隐藏,要置灰就置灰
            RefreshState();
        }
        //------------------------------------------------------
        void RefreshState()
        {
            AUnLockManager unlockMgr = getUnlockMgr();
            if (unlockMgr == null) return;
            unlockMgr.RefreshState(this);
        }
        //------------------------------------------------------
        public bool IsContain(GameObject pGO)
        {
            if (controlGo == null || pGO == null) return  false;
            return controlGo.IndexOf(pGO) >= 0;
        }
        //------------------------------------------------------
        public bool OnClickBtn()
        {
            if (cfg == null) return true;
            AUnLockManager unlockMgr = getUnlockMgr();
            if (unlockMgr == null) return false;
            return !unlockMgr.IsLockAndTip(info.id, cfg);
        }
        //------------------------------------------------------
        public void OnClear()
        {
            moduleLockState = EModuleLockState.None;
        }
        //------------------------------------------------------
        void LateUpdate()
        {
            if (m_bPlayUnlock)
            {
                AUnLockManager unlockMgr = getUnlockMgr();
                if (unlockMgr == null || !unlockMgr.CanShowTweenEffect()) return;

                m_bPlayUnlock = false;
                //目前解锁直接播放解锁动画,否则建筑会消失
                if (unlockTween)
                {
                    if (unlockTween.tween == null)
                        unlockTween.CreateTween();

                    unlockTween.DORestart();
                }
            }

        }

        
    }
}

