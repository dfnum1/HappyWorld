/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	VirtualActions
作    者:	HappLI
描    述:	虚拟动作控制器
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class VirtualActions : AInstanceAble
    {
        [System.Serializable]
        public class Action
        {
            public string action;
            public GameObject node;

            public AnimationClip clip;

            [HideInInspector]
            public AInstanceAble instanceAble;
        }
        GameObject m_pSimpleTarget = null;
        bool m_bSimpleClip = false;
        float m_fDuraiton = 0;
        float m_fPlayTime = 0;
        AnimationClip m_SimpleClip = null;
        public List<Action> Actions;
        public bool Play(string action)
        {
            if (Actions == null) return false;
            Action actionData;
            for (int i = 0; i < Actions.Count; ++i)
            {
                actionData = Actions[i];
                if (actionData.node)
                {
                    actionData.node.SetActive(action.CompareTo(actionData.action) == 0);
                }
            }
               
            for (int i = 0; i < Actions.Count; ++i)
            {
                actionData = Actions[i];
                if (action.CompareTo(actionData.action) == 0)
                {
                    m_pSimpleTarget = this.gameObject;
                    if (actionData.node)
                    {
                        m_pSimpleTarget = actionData.node;
                    }
                    if (actionData.clip)
                    {
                        m_bSimpleClip = true;
                        m_fPlayTime = 0;
                        m_fDuraiton = actionData.clip.length;
                        m_SimpleClip = actionData.clip;
                    }
                    if (actionData.instanceAble)
                    {
                        if(actionData.instanceAble is AParticleController)
                        {
                            AParticleController ctl = actionData.instanceAble as AParticleController;
                            if(ctl!=null)
                            {
                                ctl.Play();
                                return true;
                            }
                        }
                    }
                    break;
                }
            }
            return false;
        }
        //------------------------------------------------------
        protected override void LateUpdate()
        {
            base.LateUpdate();
            if(m_bSimpleClip)
            {
                if (m_fDuraiton > 0 && m_SimpleClip)
                {
                    m_fPlayTime += Time.deltaTime;
                    m_SimpleClip.SampleAnimation(m_pSimpleTarget, m_fPlayTime);
                    if (m_fPlayTime >= m_fDuraiton)
                    {
                        if (m_SimpleClip.wrapMode == WrapMode.Loop) m_fPlayTime = 0;
                    }
                }
                else
                    m_bSimpleClip = false;
            }
        }
    }
#if UNITY_EDITOR
    //------------------------------------------------------
    [CustomEditor(typeof(VirtualActions), true)]
    [CanEditMultipleObjects]
    public class VirtualActionsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VirtualActions ctl = target as VirtualActions;
            base.OnInspectorGUI();
            if(ctl.Actions!=null)
            {
                for (int i = 0; i < ctl.Actions.Count; ++i)
                {
                    if (ctl.Actions[i].node == null) continue;
                    ctl.Actions[i].instanceAble = ctl.Actions[i].node.GetComponent<AInstanceAble>();
                }
            }

        }
    }
#endif
}
