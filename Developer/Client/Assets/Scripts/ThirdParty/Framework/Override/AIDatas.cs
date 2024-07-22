using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Plugin.AI
{
    [System.Serializable]
    public struct AIServerDatas
    {
        public List<AIVariable> globalVariables;
        public AIData[] datas;

        public void Fill(AIDatas data)
        {
            data.globalVariables = globalVariables;
            data.Init(datas);
        }
    }
    //------------------------------------------------------
    // [CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/AIDatas.json", false, typeof(AIServerDatas))]
    public class AIDatas : AAIDatas
    {
        static AIDatas ms_Instacne =null;
        protected override void OnInnerEnable()
        {
            ms_Instacne = this;
        }
        //------------------------------------------------------
        public static Dictionary<int, AIData> AllDatas
        {
            get
            {
                if (ms_Instacne != null) return ms_Instacne.allDatas;
                return null;
            }
        }
        //------------------------------------------------------
        public static List<AIVariable> GlobalVariables
        {
            get
            {
                if (ms_Instacne != null) return ms_Instacne.globalVariables;
                return null;
            }
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public override void CommitServer()
        {
            Refresh(this);
            List<string> files = new List<string>();
            files.Add(saveToPath);
            files.Add(UnityEditor.AssetDatabase.GetAssetPath(this));
            UnitySVN.SVNCommit(files.ToArray());
        }
#endif
    }
}
