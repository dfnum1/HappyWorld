/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	FrameworkMainEditor
作    者:	HappLI
描    述:	编辑模式入口
*********************************************************************/

using UnityEngine;
using TopGame.Data;
using Framework.Core;
using System.Collections;

namespace TopGame
{
    public class FrameworkMainEditor : FrameworkMain
    {
        public UI.EditorWidgetConfig editorWidgetConfig;
        //------------------------------------------------------
        public override bool IsEditor()
        {
            return true;
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            if(editorWidgetConfig) UI.EditorWidgetMgr.getInstance().Init(editorWidgetConfig.Widgets);
            m_pStartUp = Logic.FrameworkStartUp.getInstance();
            m_pStartUp.Init(this);
            m_pStartUp.SetSection(Logic.EStartUpSection.AppAwake);
        }
        //------------------------------------------------------
        protected override void Start()
        {
        }
    }

}

