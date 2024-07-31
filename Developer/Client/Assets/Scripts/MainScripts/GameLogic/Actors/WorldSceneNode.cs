/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	SubScene
作    者:	HappLI
描    述:	场景中的子场景节点
*********************************************************************/
using ExternEngine;
using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using UnityEngine;
namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/WorldSceneNode")]
    public class WorldSceneNode : AWorldNode
    {
        float m_fSceneSizeDepth = 0;
        public WorldSceneNode(AFrameworkModule pMoudle) : base(pMoudle) { }

        private InstanceOperiaon m_pCutSceneOperiaon = null;
        private AInstanceAble m_pCutScene = null;
        private Vector3 m_CutSceneOffsetPosition = Vector3.zero;
        private Vector3 m_CutSceneOffsetRotation = Vector3.zero;
        private Vector3 m_CutSceneScale = Vector3.one;

        public override uint GetConfigID()
        {
            return 0;
        }
        //------------------------------------------------------
        protected override void InnerCreated()
        {
            base.InnerCreated();
        }
        //------------------------------------------------------
        protected override void OnInnerSpawnObject()
        {
            m_BoundBox.Clear();
            if (m_pObjectAble == null) return;
            m_fSceneSizeDepth = 100;
            LevelScene levelScene = m_pObjectAble as LevelScene;
            if(levelScene!=null)
            {
                m_BoundBox.Set(new Vector3(-levelScene.BoxSize.x/2, -1.0f, 0.0f), new Vector3(levelScene.BoxSize.x/2, levelScene.BoxSize.y+1.0f, levelScene.BoxSize.z));
                //! create trigger
                if(levelScene.worldTriggers!=null)
                {
                    for(int i =0; i < levelScene.worldTriggers.Length; ++i)
                    {
                        levelScene.worldTriggers[i].Create(GetGameModule());
                        GetWorld().AddTrigger(this, levelScene.worldTriggers[i]);
                    }
                }
            }
        }
        //------------------------------------------------------
        public override uint GetElementFlags()
        {
            return 0xffffffff;
        }
        //------------------------------------------------------
        public override void SetElementFlags(uint flags)
        {
        }
        //------------------------------------------------------
        protected override void OnDirtyPosition()
        {
            if (IsDestroy()) return;
            if (m_pCutScene != null)
            {
                m_pCutScene.SetPosition(GetPosition() + m_CutSceneOffsetPosition);
            }
        }
        //------------------------------------------------------
        protected override void OnDirtyEulerAngle()
        {
            base.OnDirtyEulerAngle();
            if (m_pCutScene != null)
            {
                m_pCutScene.SetEulerAngle(GetEulerAngle() + m_CutSceneOffsetRotation);
            }
        }
        //------------------------------------------------------
        public override byte GetClassify()
        {
            return 0;
        }
        //------------------------------------------------------
        public bool CanUnload(Vector3 pos, Vector3 direction, float offsetDist = 0)
        {
            Vector3 newPos = GetPosition() + direction.normalized * m_fSceneSizeDepth * 0.5f;
            if (Vector3.Dot(newPos - pos, direction) < 0)
            {
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public bool IsInSide(AWorldNode pNode)
        {
            if (pNode == null) return false;
            if (pNode == this) return true;
            return IsInSide(pNode.GetPosition());
        }
        //------------------------------------------------------
        public bool IsInSide(Vector3 worldPos)
        {
            m_BoundBox.SetTransform(this.GetMatrix());
            return m_BoundBox.Contain(GetGameModule().shareParams.intersetionParam, worldPos);
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_pCutScene != null) m_pCutScene.Destroy();
            m_pCutScene = null;
        }
        //------------------------------------------------------
        public void SetCutScene(string strFile, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            if(string.IsNullOrEmpty(strFile))
            {
                if (m_pCutScene != null) m_pCutScene.Destroy();
                m_pCutScene = null;
                if (m_pCutSceneOperiaon != null)
                {
                    m_pCutSceneOperiaon.Earse();
                    m_pCutSceneOperiaon = null;
                }
                return;
            }
            if (m_pCutSceneOperiaon != null && strFile.CompareTo(m_pCutSceneOperiaon.strFile) == 0)
                return;
            if (m_pCutSceneOperiaon != null)
            {
                m_pCutSceneOperiaon.Earse();
                m_pCutSceneOperiaon = null;
            }
            m_CutSceneOffsetPosition = position;
            m_CutSceneOffsetRotation = rotation;
            m_CutSceneScale = scale;
            InstanceOperiaon callback = FileSystemUtil.SpawnInstance(strFile, true);
            if(callback!=null)
            {
                callback.OnCallback = OnCutSceneInstance;
                callback.OnSign = OnCutSceneSign;
                callback.pByParent = ARootsHandler.ScenesRoot;
            }
        }
        //------------------------------------------------------
        void OnCutSceneInstance(InstanceOperiaon callback)
        {
            if (m_pCutScene != null) m_pCutScene.Destroy();
             m_pCutScene = callback.pPoolAble;
            if(m_pCutScene!=null)
            {
                m_pCutScene.SetPosition(GetPosition() + m_CutSceneOffsetPosition);
                m_pCutScene.SetEulerAngle(GetEulerAngle() + m_CutSceneOffsetRotation);
                m_pCutScene.SetScale( m_CutSceneScale);
            }
            m_pCutSceneOperiaon = null;
        }
        //------------------------------------------------------
        void OnCutSceneSign(InstanceOperiaon callback)
        {
            callback.bUsed = !IsDestroy();
        }
    }
}
