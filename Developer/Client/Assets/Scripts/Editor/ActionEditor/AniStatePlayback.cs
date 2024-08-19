/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AniStatePlayback
作    者:	HappLI
描    述:	动作编辑器
*********************************************************************/
using ExternEngine;
using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TopGame.ED
{
    public class AniStatePlayback
    {
        class ParObj
        {
            public GameObject target;
            public ParticleSystem[] partiles;
            public float runing;
            public float maxTime;
        }
        GameObject m_pTarget;
        Animator m_pAnimator;
        ActionState m_pState;
        ActionState m_pBaseState = null;
        AnimationClip m_pSimpleClip = null;

        Dictionary<string, Motion> m_vClips = new Dictionary<string, Motion>();

        public bool bUseGraphPlayable = false;
        public bool bForceAnimatorUpdate = false;

        public int nTotalFrame = 0;
        public float RuningTime = 0f;
        public bool bPlay = false;
        public AniStatePlayback()
        {
        }
        //------------------------------------
        public void Clear()
        {
            m_vClips.Clear();
            m_pSimpleClip = null;
        }
        //------------------------------------
        public void SetCurState(ActionState state)
        {
            m_pState = state;
        }
        //------------------------------------
        public void SetTarget(GameObject target, ActionState state, ActionState baseState = null)
        {
            Clear();
            RuningTime = 0;
            m_pSimpleClip = null;
            bPlay = false;
            m_pTarget = target;
            m_pBaseState = baseState;
            m_pState = state;
            if (target)
            {
                m_pAnimator = target.GetComponent<Animator>();
                if (m_pAnimator == null) m_pAnimator = target.GetComponentInChildren<Animator>();
            }
            nTotalFrame = 0;
            if(bUseGraphPlayable)
            {
                if (m_pAnimator != null) m_pAnimator.enabled = false;
                string sourcePath = null;
                if (m_pAnimator != null && m_pAnimator.runtimeAnimatorController != null)
                {
                    sourcePath = AssetDatabase.GetAssetPath(m_pAnimator.runtimeAnimatorController);
                }
				if (!string.IsNullOrEmpty(sourcePath))
				{
					if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations"))
					{
						sourcePath = null;
					}
				}
                if (string.IsNullOrEmpty(sourcePath))
                {
                    var renderers = target.GetComponentsInChildren<Renderer>();
                    for (int i = 0; i < renderers.Length; ++i)
                    {
                        if (renderers[i] is MeshRenderer)
                        {
                            sourcePath = AssetDatabase.GetAssetPath((renderers[i] as MeshRenderer).additionalVertexStreams);
                            break;
                        }
                        if (renderers[i] is SkinnedMeshRenderer)
                        {
                            sourcePath = AssetDatabase.GetAssetPath((renderers[i] as SkinnedMeshRenderer).sharedMesh);
                            break;
                        }
                    }
					if (!string.IsNullOrEmpty(sourcePath))
					{
						if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations"))
						{
							sourcePath = null;
						}
					}					
                }
                if(!string.IsNullOrEmpty(sourcePath))
                {
                    string name = target.name.Replace("(Clone)", "");
                    string prefabNameRef = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
                    sourcePath = System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations";
                    string[] clips = AssetDatabase.FindAssets("t:AnimationClip", new string[] { sourcePath });
                    for (int i = 0; i < clips.Length; ++i)
                    {
                        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(clips[i]));
                        if (clip != null /*&& (clip.name.Contains(name) || clip.name.Contains(prefabNameRef))*/)
                        {
                            m_vClips[clip.name.Replace(name + "_", "").Replace(prefabNameRef+"_","")] = clip;
                        }
                    }
                }
            }
            else
            {
                if (m_pAnimator != null)
                {
                    m_pAnimator.enabled = true;
                    AnimatorController controller = (AnimatorController)m_pAnimator.runtimeAnimatorController;
                    if (controller != null)
                    {
                        for (int i = 0; i < controller.layers.Length; ++i)
                        {
                            List<AnimatorState> states;
                            if (controller.layers[i].syncedLayerIndex >= 0 && controller.layers[i].syncedLayerIndex < controller.layers.Length)
                            {
                                states = EditorKits.GetStatesRecursive(controller.layers[controller.layers[i].syncedLayerIndex].stateMachine, true);
                            }
                            else
                                states = EditorKits.GetStatesRecursive(controller.layers[i].stateMachine, true);
                            foreach (var st in states)
                            {
                                if (st == null || st.motion == null) continue;
                                if (st.motion != null)
                                    m_vClips[st.name] = st.motion;
                            }
                        }
                    }
                }
            }
            
            if (m_pState!=null && !m_pState.GetCore().isNullAnimation())
            {
                for (int j = 0; j < m_pState.GetCore().animations.Length; ++j)
                {
                    AnimationSeq seq = m_pState.GetCore().animations[j];
                    if(seq.state == null) continue;
                    ModelImporter import = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(seq.state.motion)) as ModelImporter;
                    if (import != null)
                    {
                        if (import.clipAnimations == null)
                            import.clipAnimations = import.defaultClipAnimations;
                        for (int i = 0; i < import.clipAnimations.Length; ++i)
                        {
                            if (import.clipAnimations[i].name.CompareTo(seq.state.motion.name) == 0)
                            {
                                nTotalFrame += (int)(import.clipAnimations[i].lastFrame - import.clipAnimations[i].firstFrame);
                                break;
                            }
                        }
                    }
                }

            }
        }
        //------------------------------------
        public void SetPlayTime(float curTime)
        {
            if (bPlay) return;
            RuningTime = curTime;
        }
        //------------------------------------
        public int GetPlayFrame()
        {
            if (m_pState == null) return 0;
            return (int)(RuningTime* nTotalFrame);
        }
        //------------------------------------
        public string GetStateName()
        {
            if (m_pState == null || m_pState.GetCore().isNullAnimation()) return "";
            string name = "";
            for(int i = 0; i < m_pState.GetCore().animations.Length; ++i)
            {
                name += m_pState.GetCore().animations[i].animation;
                if (i < m_pState.GetCore().animations.Length - 1) name += "->";
            }
            return name;
        }
        //------------------------------------
        public void Play(bool bPlay)
        {
            RuningTime = 0;
            this.bPlay = bPlay;
            if (m_pState == null) return;
            if (bPlay)
            {
                m_pState.SetDelta(RuningTime);
                if (!m_pState.IsRunning()) m_pState.Begin(0, 1, 0, 0, false);
            }
            else m_pState.End();
            if (bUseGraphPlayable)
            {
                string animKey = null;
                Motion motion;
                if(m_pBaseState!=null)
                {
                    animKey = m_pBaseState.GetCore().GetAnimation(m_pState.GetIndex());
                }
                else
                {
                    animKey = m_pState.GetCore().GetAnimation(m_pState.GetIndex());
                }
                if (!string.IsNullOrEmpty(animKey) && m_vClips.TryGetValue(animKey, out motion))
                {
                    m_pSimpleClip = motion as AnimationClip;
                }
                if (m_pSimpleClip != null)
                {
                    m_pSimpleClip.SampleAnimation(m_pTarget, 0);

                    if(m_pAnimator.runtimeAnimatorController == null)
                    {
                        AnimatorController ctl = new AnimatorController();
                        ctl.AddLayer("BaseLayer");
                        m_pAnimator.runtimeAnimatorController = ctl;
                    }
                    var animatorCtl = m_pAnimator.runtimeAnimatorController as AnimatorController;
                    if(animatorCtl == null)
                    {
                        AnimatorOverrideController overideAnimatorCtl = m_pAnimator.runtimeAnimatorController as AnimatorOverrideController;
                        if (overideAnimatorCtl != null)
                        {
                            animatorCtl = overideAnimatorCtl.runtimeAnimatorController as AnimatorController;
                        }
                    }
                    if(animatorCtl == null)
                    {
                        AnimatorController ctl = new AnimatorController();
                        ctl.AddLayer("BaseLayer");
                        m_pAnimator.runtimeAnimatorController = ctl;
                        animatorCtl = ctl;
                    }
                    if(animatorCtl!=null)
                    {
                        AnimatorState state = EditorKits.FindStatesRecursive(animatorCtl, m_pState.GetAnimation(m_pState.GetIndex()));
                        if (state == null)
                        {
                            state = animatorCtl.AddMotion(m_pSimpleClip, 0);
                            state.name = m_pState.GetAnimation(m_pState.GetIndex());
                        }
                    }
                }
            }
            else
            {
                if (m_pAnimator != null && m_pState != null && m_pState.GetCore().isNullAnimation())
                {
                    if (m_pBaseState != null)
                        m_pAnimator.PlayInFixedTime(m_pBaseState.GetCore().GetAnimation(m_pState.GetIndex()), m_pBaseState.GetLayer(m_pState.GetIndex()), 0);
                    m_pAnimator.PlayInFixedTime(m_pState.GetCore().GetAnimation(m_pState.GetIndex()), m_pState.GetLayer(m_pState.GetIndex()), 0);
                }
                if (!bPlay && m_pAnimator != null) m_pAnimator.StartPlayback();
            }

        }
        //------------------------------------
        public void Update(float fFrame)
        {
         //   fFrame = 0.03333f;
            if (m_pState == null) return;
            if (bPlay)
            {
                m_pState.ForceUpdate(fFrame);
                RuningTime = m_pState.GetDelta();
            }
            else
            {
                m_pState.SetDelta(RuningTime);
                m_pState.ForceUpdate(0);
            }

            float time = m_pState.GetClampPlayDelta(m_pState.GetIndex());
//             if (bUseGraphPlayable)
//             {
//                 if (m_pAnimator != null) m_pAnimator.enabled = false;
//                 if (m_pSimpleClip != null)
//                 {
//                     GameObject targetGo = m_pTarget;
//                     if (m_pAnimator != null) targetGo = m_pAnimator.gameObject;
//                     m_pSimpleClip.SampleAnimation(targetGo, time);
//                 }
//             }
//             else
            {
                if(m_pAnimator != null)
                {
                    int layer = 0;// Mathf.Max(0, m_pState.GetLayer(m_pState.GetIndex()));
                    if (m_pAnimator.HasState(layer,m_pState.GetAnimationHash(m_pState.GetIndex())))
                    {
                        if (m_pState.GetDuration() > 0)
                            m_pAnimator.PlayInFixedTime(m_pState.GetAnimation(m_pState.GetIndex()), layer, time);
                        else
                            m_pAnimator.PlayInFixedTime(m_pState.GetAnimation(m_pState.GetIndex()), layer, time);

                    }

                    m_pAnimator.Update(0);
                }
            }


            if (m_pState.GetCore().loop == 0)
            {
                if (RuningTime >= m_pState.GetDuration() + fFrame)
                {
                    RuningTime = 0;
                    m_pState.SetDelta(0);
                }
            }
            else
            {
                if (RuningTime >= m_pState.GetDuration())
                    RuningTime = m_pState.GetDuration();
            }
        }
    }
}