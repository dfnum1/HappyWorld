/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Scene
作    者:	HappLI
描    述:	关卡场景
*********************************************************************/
using UnityEngine;
using UnityEngine.AI;
using Framework.Core;

namespace TopGame.Core
{
    public class LevelScene : AInstanceAble
    {
        [HideInInspector]
        public WorldTriggerParamter[] worldTriggers;

        public NavMeshData navMesh;

        public Vector3 BoxSize = new Vector3(50.0f,10.0f, 50.0f);
        //------------------------------------------------------
        public override void OnRecyle()
        {
            base.OnRecyle();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
//         //------------------------------------------------------
//         private void Update()
//         {
//             if (Mirror)
//             {
//                 int dir = Vector3.Dot(CameraKit.MainCameraDirection, Vector3.forward) > 0 ? 1 : -1;
//                 if (dir != m_nDir)
//                 {
//                     m_nDir = dir;
//                     Vector3 scale = m_pTransform.localScale;
//                     scale.z = Mathf.Abs(scale.z) * m_nDir;
//                     m_pTransform.localScale = scale;
//                 }
//             }
//         }
    }

}
