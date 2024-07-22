/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SummonSlotFormation
作    者:	HappLI
描    述:	召唤槽阵列
*********************************************************************/
using Framework.Core;
using UnityEngine;
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
#endif
namespace TopGame.Data
{
    //[CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/SummonSlotFormation.asset")]
    public class SummonSlotFormation : ScriptableObject
    {
        [System.Serializable]
        public struct Formation
        {
            public Vector3 slot;

#if UNITY_EDITOR
            [System.NonSerialized]
            public GameObject pInstance;
#endif
        }
        [System.Serializable]
        public struct TypeData
        {
            public Vector3 offset;
            public Formation[] format;
            public  bool isValid
            {
                get { return format != null && format.Length>0; }
            }

            public static TypeData DEF = new TypeData() { format = null };
        }


        public TypeData[] Slots = new TypeData[(int)EActorType.Count];

        static SummonSlotFormation ms_Instance = null;
        private void OnEnable()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static TypeData GetFormation(EActorType type)
        {
            if (ms_Instance == null || ms_Instance.Slots == null || (int)type >= ms_Instance.Slots.Length) return TypeData.DEF;
            return ms_Instance.Slots[(int)type];
        }
    }
}
