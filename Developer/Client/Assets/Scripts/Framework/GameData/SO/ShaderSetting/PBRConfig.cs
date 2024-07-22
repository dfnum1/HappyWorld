/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	全局参数设置
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.Data;
namespace TopGame.Data.PBRLab
{
    [System.Serializable]
    public class PBRConfig : AConfig
    {
        public class PropertyID
        {
            public int environment_texture;

            public int _EnvCubeMap;

            //: param auto environment_irrad_mat_red
            //public int irrad_mat_red;
            //: param auto environment_irrad_mat_green
            //public int irrad_mat_green;
            //: param auto environment_irrad_mat_blue
            //public int irrad_mat_blue;

            public int SHAr;
            public int SHAg;
            public int SHAb;
            public int SHBr;
            public int SHBg;
            public int SHBb;
            public int SHC;

            //: param auto environment_rotation
            //public int environment_rotation;
            //: param auto environment_exposure
            public int environment_exposure;

            public int maxLod;

//             // substance material required.
//             public int nbSamples;
//             public int horizonFade;
        }

        PropertyID propertyIDs = new PropertyID();

        public LightSHConfig SHConfig = new LightSHConfig();

//         [Header("Shader Properties")]
//         [Framework.Data.DisplayNameGUI("Substance 材质")]
//         public int nbSamples = 4;

        float maxLod;

//         [Range(0.0f, 2.0f)]
//         [Framework.Data.DisplayNameGUI("Substance 材质")]
//         public float horizonFade = 1.3f;

        //[Range(0.0f, 360.0f)]
        //public float environment_rotation;

        public float environment_exposure;

        #region Shader Properties Values

        #endregion

        /// <summary>
        /// Init Property Name to Property ID.
        /// </summary>
        public void Init(Framework.Module.AFrameworkBase pFramewok)
        {
            propertyIDs.environment_texture = Shader.PropertyToID("environment_texture");

            propertyIDs._EnvCubeMap = Shader.PropertyToID("_EnvCubeMap");

            //propertyIDs.irrad_mat_red			= Shader.PropertyToID("irrad_mat_red");
            //propertyIDs.irrad_mat_green			= Shader.PropertyToID("irrad_mat_green");
            //propertyIDs.irrad_mat_blue			= Shader.PropertyToID("irrad_mat_blue");

            propertyIDs.SHAr = Shader.PropertyToID("SHAr");
            propertyIDs.SHAg = Shader.PropertyToID("SHAg");
            propertyIDs.SHAb = Shader.PropertyToID("SHAb");

            propertyIDs.SHBr = Shader.PropertyToID("SHBr");
            propertyIDs.SHBg = Shader.PropertyToID("SHBg");
            propertyIDs.SHBb = Shader.PropertyToID("SHBb");

            propertyIDs.SHC = Shader.PropertyToID("SHC");

            //propertyIDs.environment_rotation	= Shader.PropertyToID("environment_rotation");
            propertyIDs.environment_exposure = Shader.PropertyToID("environment_exposure");

            propertyIDs.maxLod = Shader.PropertyToID("maxLod");

//             // substance material required.
//             propertyIDs.nbSamples				= Shader.PropertyToID("nbSamples");
//             propertyIDs.horizonFade			= Shader.PropertyToID("horizonFade");
        }

        /// <summary>
        /// Setting the values of the property in material.
        /// </summary>
        public void Apply()
        {
            //Shader.SetGlobalFloat(propertyIDs.environment_rotation, environment_rotation/360.0f);

            if (SHConfig != null)
            {
                //SHConfig.RotatorMat(environment_rotation);

                if (SHConfig.mCubeMap != null)
                    Shader.SetGlobalTexture(propertyIDs._EnvCubeMap, SHConfig.mCubeMap);

                if (SHConfig.mCube2D != null)
                    Shader.SetGlobalTexture(propertyIDs.environment_texture, SHConfig.mCube2D);

                //Shader.SetGlobalMatrix(propertyIDs.irrad_mat_red, SHConfig.matR);
                //Shader.SetGlobalMatrix(propertyIDs.irrad_mat_green, SHConfig.matG);
                //Shader.SetGlobalMatrix(propertyIDs.irrad_mat_blue, SHConfig.matB);

                if(SHConfig.SH9Value.SHParams!=null && SHConfig.SH9Value.SHParams.Length >= 6)
                {
                    Shader.SetGlobalVector(propertyIDs.SHAr, SHConfig.SH9Value.SHParams[0]);
                    Shader.SetGlobalVector(propertyIDs.SHAg, SHConfig.SH9Value.SHParams[1]);
                    Shader.SetGlobalVector(propertyIDs.SHAb, SHConfig.SH9Value.SHParams[2]);
                    Shader.SetGlobalVector(propertyIDs.SHBr, SHConfig.SH9Value.SHParams[3]);
                    Shader.SetGlobalVector(propertyIDs.SHBg, SHConfig.SH9Value.SHParams[4]);
                    Shader.SetGlobalVector(propertyIDs.SHBb, SHConfig.SH9Value.SHParams[5]);
                    Shader.SetGlobalVector(propertyIDs.SHC, SHConfig.SH9Value.SHParams[6]);
                }

                switch (SHConfig.buildType)
                {
                    case LightSHConfig.BuildType.CubeMap:
                        if(SHConfig.mCubeMap) Shader.SetGlobalFloat(propertyIDs.maxLod, SHConfig.mCubeMap.mipmapCount - 1);
                        break;
                    case LightSHConfig.BuildType.MatCap:
                        if (SHConfig.mCube2D) Shader.SetGlobalFloat(propertyIDs.maxLod, SHConfig.mCube2D.mipmapCount - 1);
                        break;
                }

                //Shader.SetGlobalFloat(propertyIDs.maxLod, maxLod);
            }


            Shader.SetGlobalFloat(propertyIDs.environment_exposure, environment_exposure);

//             Shader.SetGlobalInt(propertyIDs.nbSamples, nbSamples);
//             Shader.SetGlobalFloat(propertyIDs.horizonFade, horizonFade);
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public void OnInspector(System.Object param = null)
        {
            Framework.ED.HandleUtilityWrapper.DrawProperty(this, null);
        }
        public void Save() { }
#endif
    }
}
