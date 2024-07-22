/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	APostRenderPass
作    者:	HappLI
描    述:	URP
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.URP
{
    public partial class URPPostWorker
    {
        //------------------------------------------------------
        void RegisterLogics(APostRenderPass pass)
        {
            RenderPost renderPost = m_vPosts[(int)pass.GetPostPassType()];
            if(renderPost == null)
            {
                renderPost = new RenderPost();
                m_vPosts[(int)pass.GetPostPassType()] = renderPost;


                switch (pass.GetPostPassType())
                {
                    case Framework.URP.EPostPassType.Screen:
                        {
                            if (renderPost.posts == null) renderPost.posts = new List<APostLogic>(2);
                            ScreenFadeLogic logic = new ScreenFadeLogic();
                            logic.SetCode(1);
                            logic.SetPass(pass);
                            logic.Awake();
                            renderPost.posts.Add(logic);
                        }
                        break;
                    case Framework.URP.EPostPassType.ForceCustomOpaque:
                        {
                            if (renderPost.posts == null) renderPost.posts = new List<APostLogic>(2);

                            Core.TerrainRenderFoliages logic = new Core.TerrainRenderFoliages();
                            logic.SetCode(2);
                            logic.SetPass(pass);
                            logic.Awake();
                            renderPost.posts.Add(logic);
                        }
                        break;
                    case Framework.URP.EPostPassType.ForceCustomTransparent:
                        {
                            if (renderPost.posts == null) renderPost.posts = new List<APostLogic>(2);
                            Core.TerrainFootShadows footShadow = new Core.TerrainFootShadows();
                            footShadow.SetCode(3);
                            footShadow.SetPass(pass);
                            footShadow.Awake();
                            renderPost.posts.Add(footShadow);
                        }
                        break;
                }
            }
        }
    }
}
