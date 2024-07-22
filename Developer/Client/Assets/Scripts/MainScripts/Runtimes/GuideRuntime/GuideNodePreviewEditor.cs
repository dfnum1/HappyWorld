#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideNodePreviewEditor
作    者:	HappLI
描    述:	引导节点预览编辑
*********************************************************************/

using System;
using System.Collections.Generic;
using TopGame;
using UnityEngine;

namespace Framework.Plugin.Guide
{
    [GuideEditorPreview("OnEditorPreview")]
    public class GuideNodePreviewEditor
    {
        public static void OnEnable()
        {
            if (!GuideWrapper.bDoing)
            {
                OnVisible(false);
            }
        }
        //------------------------------------------------------
        public static void OnDisable()
        {
            if (!GuideWrapper.bDoing)
            {
                OnVisible(false);
            }
        }
        //------------------------------------------------------
        public static void OnVisible(bool bVisible)
        {
            if (GuideWrapper.bDoing)
                return;

            TopGame.UI.GuidePanel panel = GetGuidPanel();
            if (panel == null) return;
            if(bVisible) panel.Show();
            else panel.Hide();
        }
        //------------------------------------------------------
        static TopGame.UI.GuidePanel GetGuidPanel()
        {
            if (GameInstance.getInstance() == null) return null;
            if (GameInstance.getInstance().uiFramework == null) return null;
            return GameInstance.getInstance().uiFramework.CastGetUI<TopGame.UI.GuidePanel>((ushort)TopGame.UI.EUIType.GuidePanel);
        }
        //------------------------------------------------------
        public static void OnEditorPreview(BaseNode pNode)
        {
            if (GuideWrapper.bDoing)
                return;
            if (pNode == null) return;
            SeqNode stepNode = pNode as SeqNode;
            if (stepNode == null) return;
            TopGame.UI.GuidePanel panel = GetGuidPanel();
            if (panel == null) return;
            List<ArgvPort> vPorts = stepNode.GetArgvPorts();
            panel.bDoing = false;
            if (vPorts != null)
            {
                for (int i = 0; i < vPorts.Count; ++i)
                {
                    vPorts[i].Init();
                }
            }
            if (pNode is StepNode)
                GuideStepHandler.OnGuideNode(stepNode as StepNode);
            else if (pNode is ExcudeNode)
                GuideExcudesHandler.OnGuideNode(pNode as ExcudeNode);
        }
    }
}
#endif