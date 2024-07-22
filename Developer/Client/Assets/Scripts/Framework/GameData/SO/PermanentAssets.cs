/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	PermanentAssets
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.Core;
namespace TopGame.Data
{
   // [CreateAssetMenu]
    public class PermanentAssets : ScriptableObject
    {
        [System.Serializable]
        public struct PathAsset
        {
            public string asset;
            public bool permanent;
        }
        public UnityEngine.Object[] Assets = null;
        public PathAsset[] Paths = null;

        [System.Serializable]
        public struct BuffMaterial
        {
            public Material asset;
            public string propertyName;
        }
        public BuffMaterial[] BuffMaterials = null;

        static PermanentAssets ms_Instance = null;
        private void OnEnable()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
         //   ms_Instance = null;
        }
        //------------------------------------------------------
        public static PermanentAssets Instance
        {
            get { return ms_Instance; }
        }
    }
}
