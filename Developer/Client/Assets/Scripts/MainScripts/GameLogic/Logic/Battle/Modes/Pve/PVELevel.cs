/********************************************************************
生成日期:	3:23:2024  20:39
类    名: 	PVELevel
作    者:	HappLI
描    述:	PVE 关卡
*********************************************************************/
using ExternEngine;
using Framework.Core;
using TopGame.Core;
using UnityEngine;

namespace TopGame.Logic
{
    [ModeLogic(EMode.PVE)]
    public class PVELevel : AModeLogic
    {
        AInstanceAble m_pScene;
        protected override void OnPreStart()
        {
            base.OnPreStart();

            m_pScene = null;
            var opCall = FileSystemUtil.SpawnInstance("Assets/Datas/Objects/Scenes/Test/TestScene.prefab");
            if (opCall != null)
            {
                opCall.pByParent = ARootsHandler.ScenesRoot;
                opCall.OnSign += OnSpawnSign;
                opCall.OnCallback += OnSpawnScene;
            }
        }
        //------------------------------------------------------
        void OnSpawnScene(InstanceOperiaon callback)
        {
            m_pScene = callback.pPoolAble;
            if (m_pScene != null)
            {
                m_pScene.SetPosition(Vector3.zero);
                m_pScene.SetScale(Vector3.one);
                m_pScene.SetEulerAngle(Vector3.zero);
            }
        }
        //------------------------------------------------------
        void OnSpawnSign(InstanceOperiaon callback)
        {
            callback.bUsed = this.isActived();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            base.InnerUpdate(fFrame);

#if UNITY_EDITOR
            if (CameraKit.IsEditorMode)
                return;
#endif
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            base.OnClear();
            if (m_pScene != null) m_pScene.Destroy();
            m_pScene = null;
        }
    }
}