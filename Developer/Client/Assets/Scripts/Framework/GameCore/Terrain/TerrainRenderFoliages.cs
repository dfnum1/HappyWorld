/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TerrainRenderFoliages
作    者:	HappLI
描    述:	植被渲染
*********************************************************************/
using Framework.URP;
using System.Collections.Generic;
using TopGame.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.Core
{
    public struct BrushCmdStack
    {
        public Brush.Brush brush;
        public int stackCount;
        public int asyncStackCount;
        public Matrix4x4[] vMatrixs;
        public Vector4[] vExterns;
        public Vector4[] vParams;
        public float distanceSqr;
        public BrushCmdStack(Brush.Brush brush)
        {
            stackCount = 0;
            asyncStackCount = 0;
            vExterns = new Vector4[1023];
            vParams = new Vector4[1023];
            vMatrixs = new Matrix4x4[1023];
            this.brush = brush;
            this.distanceSqr = -1;
        }

        public Brush.FoliageLodMesh.Lod getMesh(float distance=-1)
        {
            if (distance < 0) distance = distanceSqr;
            return brush.GetShareMesh(distance);
        }
        public bool IsValid
        {
            get { return brush != null; }
        }
        public static BrushCmdStack DEF = new BrushCmdStack() { brush = null, vMatrixs = null, vExterns = null, vParams = null, distanceSqr = 0 };
    }
    [PostPass(EPostPassType.ForceCustomOpaque, 0)]
    public class TerrainRenderFoliages : APostLogic
    {
        CommandBuffer m_cmdBuff = null;
        static int ms_ExternFactorID = 0;
        static int ms_ExternParamID = 0;
        static MaterialPropertyBlock ms_ComdProperty = null;

        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            ms_ExternFactorID = Framework.Core.MaterailBlockUtil.BuildPropertyID("_ExternFactor");
            ms_ExternParamID = Framework.Core.MaterailBlockUtil.BuildPropertyID("_ExternParam");
        }
        //------------------------------------------------------
        public override void Excude(APostRenderPass pass, CommandBuffer cmd, ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (Data.GameQuality.bInstanceBrush && m_cmdBuff != null)
            {
                context.ExecuteCommandBuffer(m_cmdBuff);
            }
        }
        //------------------------------------------------------
        public bool UpdateFoliages(List<Core.BrushCmdStack> vStacks, bool bInstancing, Material material)
        {
            if (m_cmdBuff == null)
            {
                m_cmdBuff = new CommandBuffer();
                m_cmdBuff.name = "Terrain Foliage AfterForwardOpaque";
            }
            m_cmdBuff.Clear();
            bool bBreakDirty = false;
            int runtimeBrushCnt = vStacks.Count;
            if (bInstancing)
            {
                if (ms_ComdProperty == null)
                    ms_ComdProperty = new MaterialPropertyBlock();

                Core.BrushCmdStack stack;
                for (int i = 0; i < runtimeBrushCnt; ++i)
                {
                    if (i >= vStacks.Count)
                    {
                        bBreakDirty = true;
                        break;
                    }
                    stack = vStacks[i];
                    if (stack.stackCount <= 0) continue;
                    Brush.FoliageLodMesh.Lod lod = stack.getMesh();
                    if (lod == null || lod.mesh == null) continue;
                    ms_ComdProperty.SetVectorArray(ms_ExternFactorID, stack.vExterns);
                    ms_ComdProperty.SetVectorArray(ms_ExternParamID, stack.vParams);
                    m_cmdBuff.DrawMeshInstanced(lod.mesh, 0, material, 0, stack.vMatrixs, stack.stackCount, ms_ComdProperty);
                    //         Graphics.DrawMeshInstanced(stack.getMesh(), 0, Brush.TerrainFoliageDatas.FoliageMatrial, stack.vMatrixs,stack.stackCount, ms_ComdProperty, UnityEngine.Rendering.ShadowCastingMode.Off, false, m_nLayer, camera);
                }
            }
            else
            {
                Core.BrushCmdStack stack;
                for (int i = 0; i < runtimeBrushCnt; ++i)
                {
                    if (i >= vStacks.Count)
                    {
                        bBreakDirty = true;
                        break;
                    }
                    stack = vStacks[i];
                    if (stack.stackCount <= 0) continue;
                    Brush.FoliageLodMesh.Lod lod = stack.getMesh(100000);
                    if (lod == null || lod.mesh == null) continue;
                    for (int j = 0; j < stack.stackCount; j+=5)
                        m_cmdBuff.DrawMesh(lod.mesh, stack.vMatrixs[j], material);
                }
            }
            return bBreakDirty;
        }
    }
}