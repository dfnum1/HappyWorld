/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TakeSlotSkillFormation
作    者:	HappLI
描    述:	单飞技能点阵
*********************************************************************/
using Framework.Core;
using UnityEngine;
namespace TopGame.Data
{
 //   [CreateAssetMenu]
    public class TakeSlotSkillFormation : ScriptableObject
    {
        [System.Serializable]
        public struct Formation
        {
            public Vector3 slot;
            [System.NonSerialized]
            public bool bTaking;

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
                get { return format != null; }
            }
        }


        public TypeData[] Slots = new TypeData[(int)EActorType.Count];

        static TakeSlotSkillFormation ms_Instance = null;
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
        public static bool IsTaking(EActorType type, byte index)
        {
            if (index <= 0 || ms_Instance == null || (int)type >= ms_Instance.Slots.Length) return true;
            int typeIndex = (int)type;
            index -= 1;
            if (!ms_Instance.Slots[typeIndex].isValid || index >= ms_Instance.Slots[typeIndex].format.Length) return true;
            return ms_Instance.Slots[typeIndex].format[index].bTaking;
        }
        //------------------------------------------------------
        public static bool TakeSlot(EActorType type, byte index, ref Vector3 anchorPos)
        {
            if (index<=0 || ms_Instance == null || (int)type >= ms_Instance.Slots.Length) return false;
            int typeIndex = (int)type;
            index -= 1;
            if (!ms_Instance.Slots[typeIndex].isValid || index >= ms_Instance.Slots[typeIndex].format.Length) return true;
            if (ms_Instance.Slots[typeIndex].format[index].bTaking) return false;
            ms_Instance.Slots[typeIndex].format[index].bTaking = true;
            anchorPos = ms_Instance.Slots[typeIndex].format[index].slot + ms_Instance.Slots[typeIndex].offset;
            return true;
        }
        //------------------------------------------------------
        public static void UnTakeSlot(EActorType type, byte index)
        {
            if (index <= 0 || ms_Instance == null || (int)type >= ms_Instance.Slots.Length) return;
            int typeIndex = (int)type;
            index -= 1;
            if (!ms_Instance.Slots[typeIndex].isValid || index >= ms_Instance.Slots[typeIndex].format.Length) return;
            ms_Instance.Slots[typeIndex].format[index].bTaking = false;
        }
        //------------------------------------------------------
        public static void ClearTakingState()
        {
            if (ms_Instance == null || ms_Instance.Slots == null) return;
            for(int t = 0; t < ms_Instance.Slots.Length; ++t)
            {
                if (!ms_Instance.Slots[t].isValid) continue;
                for (int i = 0; i < ms_Instance.Slots[t].format.Length; ++i)
                    ms_Instance.Slots[t].format[i].bTaking = false;
            }
        }
    }
}
