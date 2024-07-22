/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	APostRenderPass
作    者:	HappLI
描    述:	URP
*********************************************************************/
using Framework.URP;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.URP
{

    public interface IPostPassRender : IBasePostPassRender
    {
        void OnFrameCleanup(CommandBuffer cmd);
        void OnSetup(RenderTargetIdentifier source);
        void OnRenderPassExecute(CommandBuffer cmd, ScriptableRenderContext context, ref RenderingData renderingData);
    }

    public partial class URPPostWorker : AURPPostWorker
    {
        class RenderPost
        {
            public bool isActived = false;
            public List<APostLogic> posts = null;

            public List<IPostPassRender> renderPass = null;
            public void ClearReset()
            {
                if (posts == null) return;
                for (int j = 0; j < posts.Count; ++j)
                    posts[j].ClearReset();
            }
        }
        RenderPost[] m_vPosts = new RenderPost[(int)EPostPassType.Count];
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            for (int i = 0; i < m_vPosts.Length; ++i)
            {
                if (m_vPosts[i] == null) continue;
                if (m_vPosts[i].posts == null) continue;
                for (int j = 0; j < m_vPosts[i].posts.Count; ++j)
                    m_vPosts[i].posts[j].Destroy();
                m_vPosts[i].posts.Clear();
            }
            base.OnDestroy();
        }
        //------------------------------------------------------
        protected override void OnTogglePass(EPostPassType passType, bool enable)
        {
            if (passType == EPostPassType.Grab)
                GrabPassFeature.OnTogglePass(enable);
            else if (passType == EPostPassType.DepthPass)
                DepthPassFeature.OnTogglePass(enable);
        }
        //------------------------------------------------------
        public static void OnCreateRenderPass(APostRenderPass pass)
        {
            if (pass == null) return;
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;
            urpWork.RegisterLogics(pass);
        }
        //------------------------------------------------------
        public static void OnRenderPassExecude(APostRenderPass pass, CommandBuffer cmd, ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (pass == null) return;
            EPostPassType postType = pass.GetPostPassType();
            if (postType >= EPostPassType.Count) return;
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;
            RenderPost renderPost = urpWork.m_vPosts[(int)postType];
            if (renderPost == null) return;
            renderPost.isActived = pass.IsActived();

            if(renderPost.renderPass!=null)
            {
                for(int i =0; i < renderPost.renderPass.Count; ++i)
                {
                    renderPost.renderPass[i].OnRenderPassExecute(cmd, context, ref renderingData);
                }
            }

            if (renderPost.posts == null) return;
            for(int i = 0; i < renderPost.posts.Count; ++i)
            {
                renderPost.posts[i].Excude(pass, cmd, context, ref renderingData);
            }
        }
        //------------------------------------------------------
        public static void OnRenderPassSetup(APostRenderPass pass, RenderTargetIdentifier source)
        {
            if (pass == null) return;
            EPostPassType postType = pass.GetPostPassType();
            if (postType >= EPostPassType.Count) return;
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;

            RenderPost renderPost = urpWork.m_vPosts[(int)postType];
            if (renderPost == null) return;
            renderPost.isActived = pass.IsActived();

            if (renderPost.renderPass != null)
            {
                for (int i = 0; i < renderPost.renderPass.Count; ++i)
                {
                    renderPost.renderPass[i].OnSetup(source);
                }
            }

            if (renderPost.posts == null) return;
            for (int i = 0; i < renderPost.posts.Count; ++i)
            {
                renderPost.posts[i].Setup(pass, source);
            }
        }
        //------------------------------------------------------
        public static void OnRenderPassFrameCleanup(APostRenderPass pass, CommandBuffer cmd)
        {
            if (pass == null) return;
            EPostPassType postType = pass.GetPostPassType();
            if (postType >= EPostPassType.Count) return;
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;

            RenderPost renderPost = urpWork.m_vPosts[(int)postType];
            if (renderPost == null) return;
            renderPost.isActived = pass.IsActived();

            if (renderPost.renderPass != null)
            {
                for (int i = 0; i < renderPost.renderPass.Count; ++i)
                {
                    renderPost.renderPass[i].OnFrameCleanup(cmd);
                }
            }

            if (renderPost.posts == null) return;
            for (int i = 0; i < renderPost.posts.Count; ++i)
            {
                renderPost.posts[i].FrameCleanup(pass, cmd);
            }
        }
        //------------------------------------------------------
        public T GetPostPass<T>(EPostPassType type) where T : APostLogic
        {
            if (type >= EPostPassType.Count) return null;
            RenderPost renderPost = m_vPosts[(int)type];
            if (renderPost == null || renderPost.posts == null) return null;
            T logic;
            for (int i = 0; i < renderPost.posts.Count; ++i)
            {
                logic = renderPost.posts[i] as T;
                if (logic != null) return logic;
            }
            return null;
        }
        //------------------------------------------------------
        public static T CastPostPass<T>(EPostPassType type) where T : APostLogic
        {
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return null;

            return urpWork.GetPostPass<T>(type);
        }
        //------------------------------------------------------
        protected override void OnRegisterPostRenderPass(IBasePostPassRender postPass)
        {
            int index = (int)postPass.GetPassType();
            if (index < 0 || index >= m_vPosts.Length || !(postPass is IPostPassRender)) return;
            IPostPassRender post = postPass as IPostPassRender;
            if (m_vPosts[index] == null) m_vPosts[index] = new RenderPost();
            if (m_vPosts[index].renderPass == null)
                m_vPosts[index].renderPass = new List<IPostPassRender>(2);
            if (m_vPosts[index].renderPass.Contains(post)) return;
            m_vPosts[index].renderPass.Add(post);
        }
        //------------------------------------------------------
        protected override void OnUnRegisterPostRenderPass(IBasePostPassRender postPass)
        {
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;
            int index = (int)postPass.GetPassType();
            if (index < 0 || index >= m_vPosts.Length || !(postPass is IPostPassRender)) return;
            if (m_vPosts[index] == null) return;
            if (urpWork.m_vPosts[index].renderPass == null) return;
            IPostPassRender post = postPass as IPostPassRender;
            urpWork.m_vPosts[index].renderPass.Remove(post);
        }
        //------------------------------------------------------
        public static void ClearReset()
        {
            URPPostWorker urpWork = getInstance() as URPPostWorker;
            if (urpWork == null) return;
            RenderPost renderPost;
            for (int i = 0; i < urpWork.m_vPosts.Length; ++i)
            {
                renderPost = urpWork.m_vPosts[i];
                if (renderPost == null) continue;
                renderPost.ClearReset();
            }
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            RenderPost renderPost;
            for (int i = 0; i < m_vPosts.Length; ++i)
            {
                renderPost = m_vPosts[i];
                if (renderPost == null || renderPost.posts == null) continue;
                for (int j = 0; j < renderPost.posts.Count; ++j)
                    renderPost.posts[j].Update(fFrame);
            }
        }
    }
}
