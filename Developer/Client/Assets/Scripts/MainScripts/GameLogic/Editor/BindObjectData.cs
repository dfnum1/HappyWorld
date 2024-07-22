//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TopGame.Data;
//using TopGame.Logic;
//using UnityEditor;
//using Framework.Core;
//namespace TopGame.ED
//{
//	//public class SceneEditObject : MonoBehaviour
//	//{
//	//	[HideInInspector]public TopGame.Data.RunDungonData.Part sharePart;
//	//	[HideInInspector]public TopGame.Data.RunDungonPartData.Item item;
// //    //   [HideInInspector] public SceneNode sceneNode;
//	//	[HideInInspector]public int groupId;

// //       public void Update()
// //       {
// //           LogicUpdate();
// //       }
// //       protected virtual void LogicUpdate() { }

// //       public virtual DungoneBaseElement GetElement()
// //       {
// //           return null;
// //       }

// //       public virtual uint GetConfigID()
// //       {
// //           return 0;
// //       }
// //   }
	
////    [ExecuteInEditMode]
////    public class BindObjectData : SceneEditObject
////    {
////        public int configId;
////        public DungonElement.Element element;
////        [System.NonSerialized]
////        public Vector2 scroll;
////        public override DungoneBaseElement GetElement()
////        {
////            return element;
////        }
////        public override uint GetConfigID()
////        {
////            if (element != null) return element.nID;
////            return 0;
////        }
////        protected override void LogicUpdate()
////        {
////            if (element != null)
////            {
//////                 if (sceneNode != null)
//////                 {
//////                     element.eulerAngle = transform.eulerAngles;
////// 
//////                 }
////            }
////        }
////        //------------------------------------------------------
////        private void OnDrawGizmos()
////        {
////            if (element ==null || element.battleObjectData == null) return;
////            Vector3 vC0 = (element.battleObjectData.aabb_max + element.battleObjectData.aabb_min) * 0.5f;
////            Vector3 vH0 = element.battleObjectData.aabb_max - vC0;
////            Framework.Core.CommonUtility.DrawBoundingBox(vC0, vH0, transform.localToWorldMatrix, Color.yellow, true);
////            if (element.battleObjectData.desc != null)
////                UnityEditor.Handles.Label(transform.position, element.battleObjectData.desc);
////        }
////    }

////    [ExecuteInEditMode]
////    public class MonsterEditorData : SceneEditObject
////    {
////        public DungonMonster.Element Data;

////        public override DungoneBaseElement GetElement()
////        {
////            return Data;
////        }

////        public override uint GetConfigID()
////        {
////            if (Data != null) return Data.nID;
////            return 0;
////        }
////        protected override void LogicUpdate()
////        {
////            if (Data != null)
////            {
////                Data.direction = transform.forward;
////            }
////        }
////        //------------------------------------------------------
////        private void OnDrawGizmos()
////        {
////            if (Data == null || Data.monsterData == null) return;
////            UnityEditor.Handles.Label(transform.position, Data.monsterData.desc);
////        }
////    }
//    //[ExecuteInEditMode]
//    //public class BindTransferSplineData : SceneEditObject
//    //{
//    //    public TransferSpline element;
//    //    [System.NonSerialized]
//    //    public Vector2 scroll;
//    //}
//}
//#endif