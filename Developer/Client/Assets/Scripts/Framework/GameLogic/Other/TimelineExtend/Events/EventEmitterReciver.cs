using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    public class EventEmitterReciver : MonoBehaviour//, INotificationReceiver, IInstanceSpawner
    {
        //List<AInstanceAble> m_vInstances = new List<AInstanceAble>();
        //public void OnNotify(Playable origin, INotification notification, object context)
        //{
        //    if(notification is EventEmitter)
        //    {
        //        EventEmitter emitter = notification as EventEmitter;
        //        emitter.Emitter(this.gameObject, this);
        //    }
        //}
        ////------------------------------------------------------
        //public void Spawn(string strFile, bool bAbs, Vector3 offset, Vector3 euler, Transform pParent = null)
        //{
        //    InstanceOperiaon pCallback =FileSystemUtil.SpawnInstance(strFile, true);
        //    if(pCallback!=null)
        //    {
        //        pCallback.OnCallback = OnSpawnCallback;
        //        if(bAbs)
        //        {
        //            pCallback.pByParent = RootsHandler.ScenesRoot;
        //            pCallback.userData0 = new Variable3() { floatVal0 = offset.x, floatVal1 = offset.y, floatVal2 = offset.z };
        //            pCallback.userData1 = new Variable3() { floatVal0 = euler.x, floatVal1 = euler.y, floatVal2 = euler.z };
        //            pCallback.userData2 = new Variable1() { boolVal = bAbs };
        //        }
        //        else
        //        {
        //            pCallback.pByParent = pParent;
        //            pCallback.userData0 = new Variable3() { floatVal0 = offset.x, floatVal1 = offset.y, floatVal2 = offset.z };
        //            pCallback.userData1 = new Variable3() { floatVal0 = euler.x, floatVal1 = euler.y, floatVal2 = euler.z };
        //            pCallback.userData2 = new Variable1() { boolVal = bAbs };
        //        }
        //    }
        //}
        ////------------------------------------------------------
        //void OnSpawnCallback(InstanceOperiaon pCallback)
        //{
        //    if(pCallback.pPoolAble != null)
        //    {
        //        Vector3 pos = ((Variable3)pCallback.userData0).ToVector3();
        //        Vector3 euler = ((Variable3)pCallback.userData1).ToVector3();
        //        bool bAbs = ((Variable1)pCallback.userData2).boolVal;
        //        pCallback.pPoolAble.SetEulerAngle(pos, bAbs);
        //        pCallback.pPoolAble.SetEulerAngle(euler, bAbs);
        //        m_vInstances.Add(pCallback.pPoolAble);
        //    }
        //}
        ////------------------------------------------------------
        //private void OnDestroy()
        //{
        //    for(int i = 0; i < m_vInstances.Count; ++i)
        //    {
        //        m_vInstances[i].RecyleDestroy();
        //    }
        //    m_vInstances.Clear();
        //}
    }
}