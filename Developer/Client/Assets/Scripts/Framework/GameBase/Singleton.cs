/********************************************************************
生成日期:	10:7:2019   12:11
类    名: 	Singleton
作    者:	HappLI
描    述:	全局单例
*********************************************************************/

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TopGame.Base
{
    public abstract class Singleton<T> where T : new()
    {
        protected static T sm_pInstance;

        static object sm_pLock = new object();
        //------------------------------------------------------
        public static T getInstance()
        {
            if (sm_pInstance == null)
            {
                lock(sm_pLock)
                {
                    if(sm_pInstance == null)
                        sm_pInstance = new T();
                }
                return sm_pInstance;
            }

            return sm_pInstance;
        }
    }
    //------------------------------------------------------
    //! SingletonNoNew
    //------------------------------------------------------
    //public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    //{
    //    protected static T sm_pInstance;
    //    //------------------------------------------------------
    //    private void Awake()
    //    {
    //        sm_pInstance = this as T;
    //        OnAwake();
    //    }
    //    //------------------------------------------------------
    //    public static T getInstance()
    //    {
    //        return sm_pInstance;
    //    }
    //    //------------------------------------------------------
    //    protected virtual void OnAwake() { }
    //}
}