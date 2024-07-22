/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TerrainFootShadows
作    者:	HappLI
描    述:	地表贴地单位阴影
*********************************************************************/
using Framework.Core;
using Framework.URP;
using System.Collections.Generic;
using TopGame.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.Core
{
    [PostPass(EPostPassType.ForceCustomTransparent, 0)]
    public class TerrainFootShadows : APostLogic
    {
        CommandBuffer m_cmdBuff = null;
        Mesh m_pFootMesh = null;
        Material m_pFootShadaw = null;
        Matrix4x4[] m_arrNodes = null;
        int m_nShadowCnt = 0;
        int m_nNodeCount = 0;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_pFootMesh = new Mesh();
            m_pFootMesh.vertices = new Vector3[]
            {
                    new Vector3(-0.5f,0.05f,-0.5f),
                    new Vector3(-0.5f,0.05f,0.5f),
                    new Vector3(0.5f,0.05f,0.5f),
                    new Vector3(0.5f,0.05f,-0.5f)
            };
            m_pFootMesh.SetUVs(0, new Vector2[] {
                    new Vector2(0,0),
                    new Vector2(1,0),
                    new Vector2(1,1),
                    new Vector2(0,1)
                    });
            m_pFootMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            m_pFootShadaw = Data.GlobalDefaultResources.GetDefaultMaterial(5);
        }
        //------------------------------------------------------
        public override void Excude(APostRenderPass pass, CommandBuffer cmd, ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (m_nShadowCnt <= 0) return;

            if (m_pFootShadaw == null || m_pFootMesh == null)
                return;
            if (m_cmdBuff == null)
            {
                m_cmdBuff = new CommandBuffer();
                m_cmdBuff.name = "Terrain Foot Shadow";
            }
            m_cmdBuff.Clear();
            m_cmdBuff.DrawMeshInstanced(m_pFootMesh, 0, m_pFootShadaw, 0, m_arrNodes, m_nShadowCnt);
            if (m_cmdBuff != null)
            {
                context.ExecuteCommandBuffer(m_cmdBuff);
            }
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            m_nShadowCnt = 0;
            if (URPPostWorker.IsEnabledPass(EPostPassType.PlaneShadow))
            {
                return;
            }
            if (m_pFootShadaw == null) return;
            Framework.Module.AFramework mainFramework = Framework.Module.ModuleManager.GetMainFramework<Framework.Module.AFramework>();
            if (mainFramework == null) return;
            var world = mainFramework.world;
            if (world == null) return;

            if(m_nNodeCount>0)
            {
                if (m_nNodeCount >= 1024) m_nNodeCount = 1023;
                if (m_arrNodes == null || m_arrNodes.Length< m_nNodeCount)
                {
                    m_arrNodes = new Matrix4x4[m_nNodeCount];
                }
            }

            int nodeCnt =0;
            var pNode = world.GetRootNode();
            while(pNode !=null)
            {
                if(pNode is Actor)
                {
                    Actor actor = pNode as Actor;
                    if (actor != null && !actor.IsKilled() && !actor.IsDestroy() && (actor.GetActorType() == EActorType.Player || actor.GetActorType() == EActorType.Monster))
                    {
                        if (m_arrNodes == null)
                        {
                            m_nNodeCount = 10;
                            m_arrNodes = new Matrix4x4[10];
                        }

                        if(m_nShadowCnt < m_arrNodes.Length)
                        {
                            Vector3 curPos = Framework.Core.CommonUtility.GetPosition(m_arrNodes[m_nShadowCnt]);
                            Vector3 pos = Vector3.Lerp(curPos, actor.GetTerrain(), Time.fixedDeltaTime*50);
                            m_arrNodes[m_nShadowCnt] = Matrix4x4.TRS(pos, Quaternion.Euler(actor.GetEulerAngle()), actor.GetScale()*Mathf.Max(actor.GetFootShadowScale(), 1.2f));
                            m_nShadowCnt++;
                        }
                        nodeCnt++;
                    }
                }
                pNode = pNode.GetNext();
            }
            m_nNodeCount = Mathf.Max(nodeCnt, m_nNodeCount);
        }
    }
}