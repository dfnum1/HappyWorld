/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	LightSHConfig
作    者:	HappLI copy by rh
描    述:	
*********************************************************************/
using Framework.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace TopGame.Data.PBRLab
{
    [System.Serializable]
    public class LightSHConfig : AConfig
    {
        public enum BuildType
        {
            CubeMap,
            MatCap,
        }
        public BuildType buildType = BuildType.CubeMap;

        public Texture2D mCube2D;

        public Cubemap mCubeMap;
        public bool IsHDRToLDR = false;
        public float Blur = 1.0f;

        [HideInInspector]
        public int maxSamples;

        #region Mat4x4 Cal
        [HideInInspector]
        public int maxPower;

        [HideInInspector]
        public Vector3[] LightSH;

        [HideInInspector]
        public float rotatorAngle = 0.0f;

        [Framework.Data.DisplayNameGUI("是否生成GLSL SH9声明文本")]
        public bool IsBuildSHDataForGLSL;

        // Stop use this to make SH GI Datas.
        [HideInInspector]
        public Matrix4x4 matR = Matrix4x4.identity;
        [HideInInspector]
        public Matrix4x4 matG = Matrix4x4.identity;
        [HideInInspector]
        public Matrix4x4 matB = Matrix4x4.identity;

        public void Init(Framework.Module.AFrameworkBase pFramewok)
        {

        }

        public void Apply()
        {

        }

        public void BuildSHDataMat4x4()
        {
            if (maxSamples == 0 || maxPower == 0)
            {
                Debug.LogErrorFormat("Build Failed: maxSamples == {0}| maxPower == {1}", maxSamples, maxPower);
                return;
            }

            LightSH = new Vector3[maxPower];
            SHFunc.Init();

            switch (buildType)
            {
                case BuildType.CubeMap:
                    LightIntegratorCubeMap();
                    break;

                case BuildType.MatCap:
                    LightIntegratorMatCap();
                    break;
            }

            RotatorMat(rotatorAngle);
        }

        void LightIntegratorCubeMap()
        {
            if (mCubeMap == null)
                return;
#if UNITY_EDITOR
            // 检查纹理是否可读
            var assetReadable = false;
            var importer = UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(mCubeMap)) as UnityEditor.TextureImporter;
            if (importer != null)
            {
                assetReadable = importer.isReadable;
                if (!assetReadable) importer.isReadable = true;
                importer.SaveAndReimport();
            }
#endif

            Vector2[] Samples = MCIntegrator(maxSamples);

            float factor = 4 * Mathf.PI / maxSamples;

            Vector3[] result = new Vector3[maxPower];
            for (int i = 0; i < maxPower; i++)
                result[i] = Vector3.zero;

            for (int i = 0; i < maxSamples; i++)
            {
                Color sampleColor = SampleCubeMap(Samples[i].x, Samples[i].y);
                for (int j = 0; j < maxPower; j++)
                {
                    int l = 0;
                    int m = 0;
                    SHFunc.tranferK(j, ref l, ref m);
                    float y = SHFunc.SH(l, m, Samples[i].x, Samples[i].y);
                    result[j] += new Vector3(sampleColor.r * y, sampleColor.g * y, sampleColor.b * y);
                }
            }

            for (int i = 0; i < maxPower; i++)
            {
                LightSH[i] = factor * result[i];
            }

#if UNITY_EDITOR
            // 还原纹理可读性
            if (importer != null)
            {
                if (!assetReadable)
                    importer.isReadable = false;
                importer.SaveAndReimport();
            }
#endif
        }

        void LightIntegratorMatCap()

        {
            if (mCube2D == null)
                return;
#if UNITY_EDITOR
            // 检查纹理是否可读
            var assetReadable = false;
            var importer = UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(mCube2D)) as UnityEditor.TextureImporter;
            if (importer != null)
            {
                assetReadable = importer.isReadable;
                if (!assetReadable) importer.isReadable = true;
                importer.SaveAndReimport();
            }
#endif
            Vector2[] Samples = MCIntegrator(maxSamples);

            float factor = 4 * Mathf.PI / maxSamples;

            Vector3[] result = new Vector3[maxPower];
            for (int i = 0; i < maxPower; i++)
                result[i] = Vector3.zero;

            for (int i = 0; i < maxSamples; i++)
            {
                Color sampleColor = SampleCube2D(Samples[i].x, Samples[i].y);
                for (int j = 0; j < maxPower; j++)
                {
                    int l = 0;
                    int m = 0;
                    SHFunc.tranferK(j, ref l, ref m);
                    float y = SHFunc.SH(l, m, Samples[i].x, Samples[i].y);
                    result[j] += new Vector3(sampleColor.r * y, sampleColor.g * y, sampleColor.b * y);
                }
            }

            for (int i = 0; i < maxPower; i++)
            {
                LightSH[i] = factor * result[i];
            }

#if UNITY_EDITOR
            // 还原纹理可读性
            if (importer != null)
            {
                if (!assetReadable)
                    importer.isReadable = false;
                importer.SaveAndReimport();
            }
#endif
        }

        Vector2[] MCIntegrator(int maxCount)
        {
            Vector2[] res = new Vector2[maxCount];
            for (int i = 0; i < maxCount; i++)
            {
                res[i].x = Random.Range(0.0f, 1.0f);
                res[i].y = Random.Range(0.0f, 1.0f);
                res[i].x = 2 * Mathf.Acos(Mathf.Sqrt(1 - res[i].x));
                res[i].y = 2 * res[i].y * Mathf.PI;
            }
            return res;
        }

        Color SampleCubeMap(float theta, float phi)
        {
            if (mCubeMap == null)
                return Color.black;

            Vector3 CastVec = tranfer(theta, phi);

            CubemapFace HitFace = JudgeHitWhatFace(CastVec);

            Vector2 HitPixel = hitPoint(CastVec, HitFace);

            Color targetColor = mCubeMap.GetPixel(HitFace, (int)(HitPixel.x * mCubeMap.width + 0.5f), (int)(HitPixel.y * mCubeMap.height + 0.5f));

            //	Debug.Log (CastVec.ToString()+" "+HitFace.ToString()+" "+HitPixel.ToString());



            return targetColor;
        }

        Color SampleCube2D(float theta, float phi)
        {
            if (mCube2D == null)
                return Color.black;

            Color targetColor = mCube2D.GetPixel((int)(mCube2D.width * Mathf.Cos(theta) + 0.5f), (int)(mCube2D.height * Mathf.Sin(phi) + 0.5f));

            return targetColor;
        }

        CubemapFace JudgeHitWhatFace(Vector3 v)
        {
            float absX = Mathf.Abs(v.x);
            float absY = Mathf.Abs(v.y);
            float absZ = Mathf.Abs(v.z);

            if (absX > absY && absX > absZ && v.x > 0)
                return CubemapFace.PositiveX;
            if (absX > absY && absX > absZ && v.x < 0)
                return CubemapFace.NegativeX;
            if (absY > absX && absY > absZ && v.y > 0)
                return CubemapFace.PositiveY;
            if (absY > absX && absY > absZ && v.y < 0)
                return CubemapFace.NegativeY;
            if (absZ > absX && absZ > absY && v.z > 0)
                return CubemapFace.PositiveZ;
            if (absZ > absX && absZ > absY && v.z < 0)
                return CubemapFace.NegativeZ;

            return CubemapFace.Unknown;
        }

        Vector2 hitPoint(Vector3 Cast, CubemapFace face)
        {
            if (face == CubemapFace.PositiveX)
            {
                Vector3 vertical = new Vector3(1.0f, 0.0f, 0.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(hit.z + 0.5f, Mathf.Abs(hit.y - 0.5f));
            }
            if (face == CubemapFace.NegativeX)
            {
                Vector3 vertical = new Vector3(-1.0f, 0.0f, 0.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(Mathf.Abs(hit.z - 0.5f), Mathf.Abs(hit.y - 0.5f));
            }
            if (face == CubemapFace.PositiveY)
            {
                Vector3 vertical = new Vector3(0.0f, 1.0f, 0.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(hit.x + 0.5f, Mathf.Abs(hit.z - 0.5f));
            }
            if (face == CubemapFace.NegativeY)
            {
                Vector3 vertical = new Vector3(0.0f, -1.0f, 0.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(Mathf.Abs(hit.x + 0.5f), Mathf.Abs(hit.z + 0.5f));
            }
            if (face == CubemapFace.PositiveZ)
            {
                Vector3 vertical = new Vector3(0.0f, 0.0f, 1.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(Mathf.Abs(hit.x - 0.5f), Mathf.Abs(hit.y - 0.5f));
            }
            if (face == CubemapFace.NegativeZ)
            {
                Vector3 vertical = new Vector3(0.0f, 0.0f, -1.0f);
                float costheta = Vector3.Dot(Cast, vertical);
                float Length = 0.5f / costheta;
                Vector3 hit = Cast * Length;
                return new Vector2(Mathf.Abs(hit.x + 0.5f), Mathf.Abs(hit.y - 0.5f));
            }
            return new Vector2(0, 0);
        }

        Vector3 tranfer(float theta, float phi)
        {
            Vector3 normal = new Vector3(0, 0, 1);
            Vector3 tangent = new Vector3(0, 1, 0);
            Vector3 BiNormal = new Vector3(1, 0, 0);
            Vector3 result = normal * Mathf.Cos(theta) + tangent * Mathf.Sin(theta) * Mathf.Sin(phi) + BiNormal * Mathf.Sin(theta) * Mathf.Cos(phi);
            return result;
        }

        public void RotatorMat(float rot)
        {
            Matrix4x4 W2O = Matrix4x4.Rotate(Quaternion.Euler(0.0f, rot * Mathf.Deg2Rad, 0.0f)).inverse;

            Vector3[] rotLight = SHRotate.Rotate(LightSH, W2O);

            matR.m00 = rotLight[0].x;
            matR.m01 = rotLight[1].x;
            matR.m02 = rotLight[2].x;
            matR.m03 = rotLight[3].x;

            matR.m10 = rotLight[4].x;
            matR.m11 = rotLight[5].x;
            matR.m12 = rotLight[6].x;
            matR.m13 = rotLight[7].x;

            matR.m20 = rotLight[8].x;
            matR.m21 = rotLight[9].x;
            matR.m22 = rotLight[10].x;
            matR.m23 = rotLight[11].x;

            matR.m30 = rotLight[12].x;
            matR.m31 = rotLight[13].x;
            matR.m32 = rotLight[14].x;
            matR.m33 = rotLight[15].x;

            matG.m00 = rotLight[0].y;
            matG.m01 = rotLight[1].y;
            matG.m02 = rotLight[2].y;
            matG.m03 = rotLight[3].y;

            matG.m10 = rotLight[4].y;
            matG.m11 = rotLight[5].y;
            matG.m12 = rotLight[6].y;
            matG.m13 = rotLight[7].y;

            matG.m20 = rotLight[8].y;
            matG.m21 = rotLight[9].y;
            matG.m22 = rotLight[10].y;
            matG.m23 = rotLight[11].y;

            matG.m30 = rotLight[12].y;
            matG.m31 = rotLight[13].y;
            matG.m32 = rotLight[14].y;
            matG.m33 = rotLight[15].y;

            matB.m00 = rotLight[0].z;
            matB.m01 = rotLight[1].z;
            matB.m02 = rotLight[2].z;
            matB.m03 = rotLight[3].z;

            matB.m10 = rotLight[4].z;
            matB.m11 = rotLight[5].z;
            matB.m12 = rotLight[6].z;
            matB.m13 = rotLight[7].z;

            matB.m20 = rotLight[8].z;
            matB.m21 = rotLight[9].z;
            matB.m22 = rotLight[10].z;
            matB.m23 = rotLight[11].z;

            matB.m30 = rotLight[12].z;
            matB.m31 = rotLight[13].z;
            matB.m32 = rotLight[14].z;
            matB.m33 = rotLight[15].z;
        }

        #endregion


        #region SH9 Cal
        // 球谐数据结构
        [System.Serializable]
        public struct SHValue
        {
            // 球谐数据
            public Vector4[] SHParams;

            public SHValue(float brightness)
            {
                SHParams = new Vector4[9];
                for (int i = 0; i < 9; i++)
                {
                    SHParams[i] = new Vector4(brightness, brightness, brightness, brightness);
                }
#if UNITY_EDITOR
                bEpand = false;
#endif
            }

            // 构造方法
            public SHValue(SphericalHarmonicsL2 daySH, SphericalHarmonicsL2 nightSH)
            {
                var sh1 = new Vector4(nightSH[0, 0], nightSH[1, 0], nightSH[2, 0], daySH[0, 0] * 0.30f + daySH[1, 0] * 0.59f + daySH[2, 0] * 0.11f);
                var sh2 = new Vector4(nightSH[0, 1], nightSH[1, 1], nightSH[2, 1], daySH[0, 1] * 0.30f + daySH[1, 1] * 0.59f + daySH[2, 1] * 0.11f);
                var sh3 = new Vector4(nightSH[0, 2], nightSH[1, 2], nightSH[2, 2], daySH[0, 2] * 0.30f + daySH[1, 2] * 0.59f + daySH[2, 2] * 0.11f);
                var sh4 = new Vector4(nightSH[0, 3], nightSH[1, 3], nightSH[2, 3], daySH[0, 3] * 0.30f + daySH[1, 3] * 0.59f + daySH[2, 3] * 0.11f);
                var sh5 = new Vector4(nightSH[0, 4], nightSH[1, 4], nightSH[2, 4], daySH[0, 4] * 0.30f + daySH[1, 4] * 0.59f + daySH[2, 4] * 0.11f);
                var sh6 = new Vector4(nightSH[0, 5], nightSH[1, 5], nightSH[2, 5], daySH[0, 5] * 0.30f + daySH[1, 5] * 0.59f + daySH[2, 5] * 0.11f);
                var sh7 = new Vector4(nightSH[0, 6], nightSH[1, 6], nightSH[2, 6], daySH[0, 6] * 0.30f + daySH[1, 6] * 0.59f + daySH[2, 6] * 0.11f);
                var sh8 = new Vector4(nightSH[0, 7], nightSH[1, 7], nightSH[2, 7], daySH[0, 7] * 0.30f + daySH[1, 7] * 0.59f + daySH[2, 7] * 0.11f);
                var sh9 = new Vector4(nightSH[0, 8], nightSH[1, 8], nightSH[2, 8], daySH[0, 8] * 0.30f + daySH[1, 8] * 0.59f + daySH[2, 8] * 0.11f);

                var shAr = new Vector4(sh4.x, sh2.x, sh3.x, sh1.x - sh7.x);
                var shAg = new Vector4(sh4.y, sh2.y, sh3.y, sh1.y - sh7.y);
                var shAb = new Vector4(sh4.z, sh2.z, sh3.z, sh1.z - sh7.z);
                var shAa = new Vector4(sh4.w, sh2.w, sh3.w, sh1.w - sh7.w);
                var shBr = new Vector4(sh5.x, sh6.x, 3.0f * sh7.x, sh8.x);
                var shBg = new Vector4(sh5.y, sh6.y, 3.0f * sh7.y, sh8.y);
                var shBb = new Vector4(sh5.z, sh6.z, 3.0f * sh7.z, sh8.z);
                var shBa = new Vector4(sh5.w, sh6.w, 3.0f * sh7.w, sh8.w);
                var shC = sh9;

                SHParams = new Vector4[9]
                {
                    shAr, shAg, shAb, shAa,
                    shBr, shBg, shBb, shBa,
                    shC
                };
#if UNITY_EDITOR
                bEpand = false;
#endif
            }

            public SHValue(SHValue val1, SHValue val2, SHValue val3, Vector3 weights)
            {
                SHParams = new Vector4[9];
                for (int i = 0; i < 9; i++)
                {
                    SHParams[i] = val1.SHParams[i] * weights.x + val2.SHParams[i] * weights.y + val3.SHParams[i] * weights.z;
                }
#if UNITY_EDITOR
                bEpand = false;
#endif
            }
#if UNITY_EDITOR
            [System.NonSerialized]
            bool bEpand;
            public void OnInspector(System.Object param = null)
            {
                if (SHParams == null) return;
                bEpand = UnityEditor.EditorGUILayout.Foldout(bEpand, "SHParams");
                if (bEpand)
                {
                    for (int i = 0; i < SHParams.Length; ++i)
                        SHParams[i] = UnityEditor.EditorGUILayout.Vector4Field("SH-" + i, SHParams[i]);
                }
            }
#endif
        }

        // 球谐系数常量
        private static readonly float[] SHCoefficients = {
            0.28209479177387814347403972578039f,    // L0 M0          1 / (2*Sqrt(Pi))
			0.48860251190291992158638462283835f,    // L1 M-1   Sqrt(3) / (2*Sqrt(Pi))
			0.48860251190291992158638462283835f,    // L1 M0    Sqrt(3) / (2*Sqrt(Pi))
			0.48860251190291992158638462283835f,    // L1 M1    Sqrt(3) / (2*Sqrt(Pi))
			1.0925484305920790705433857058027f,     // L2 M-2  Sqrt(15) / (2*Sqrt(Pi))
			1.0925484305920790705433857058027f,     // L2 M-1  Sqrt(15) / (2*Sqrt(Pi))
			0.31539156525252000603089369029571f,    // L2 M0   Sqrtf(5) / (4*sqrt(Pi))
			1.0925484305920790705433857058027f,     // L2 M1   Sqrt(15) / (2*Sqrt(Pi))
			0.54627421529603953527169285290135f,    // L2 M2   Sqrt(15) / (4*Sqrt(Pi))
		};

        // 求Cube采样点世界坐标方向方法
        static Vector3 MapXYSToDir(float posX, float posY, float width, float height, int faceID, ref float weight)
        {
            var dir = Vector3.zero;
            var u = (posX + 0.5f) * 2.0f / width - 1.0f;
            var v = (posY + 0.5f) * 2.0f / height - 1.0f;
            switch (faceID)
            {
                case 0:
                    dir = new Vector3(1.0f, -v, -u);
                    break;
                case 1:
                    dir = new Vector3(-1.0f, -v, u);
                    break;
                case 2:
                    dir = new Vector3(u, 1.0f, v);
                    break;
                case 3:
                    dir = new Vector3(u, -1.0f, -v);
                    break;
                case 4:
                    dir = new Vector3(u, -v, 1.0f);
                    break;
                case 5:
                    dir = new Vector3(-u, -v, -1.0f);
                    break;
            }
            var mag = dir.magnitude;
            dir = dir / mag;
            weight = 4.0f / (mag * mag * mag);
            return dir;
        }

        static Vector3 MapXYSToDir(float posX, float posY, float width, float height, ref float weight)
        {
            var dir = Vector3.zero;

            float u = (posX + 0.5f) / width;
            float v = (posY + 0.5f) / height;

            float horAngle = (u - 0.5f) * Mathf.PI * 2.0f;
            float vecAngle = (0.5f - v) * Mathf.PI;

            dir = Quaternion.Euler(vecAngle * Mathf.Rad2Deg, horAngle * Mathf.Rad2Deg, 0.0f) * Vector3.forward;

            var mag = dir.magnitude;
            //var mag = Mathf.Sqrt( 1.0f / ( Mathf.Sin(vecAngle) * Mathf.Sin(vecAngle) + 1.0f / (Mathf.Sin(horAngle) * Mathf.Sin(horAngle)) ) );//dir.magnitude;

            dir /= mag;

            weight = 4.0f / (mag * mag * mag);

            //weight = 1.0f / (mag * mag * mag);

            return dir;
        }

        // 方向 颜色 权重求球谐系数方法
        static Vector3[] DirToSH9Col(Vector3 dir, Color col, float weight)
        {
            // 方向转SH系数
            var sh = new float[9];
            var dirX = dir.x;
            var dirY = dir.y;
            var dirZ = dir.z;
            sh[0] = SHCoefficients[0];
            sh[1] = SHCoefficients[1] * dirY;
            sh[2] = SHCoefficients[2] * dirZ;
            sh[3] = SHCoefficients[3] * dirX;
            sh[4] = SHCoefficients[4] * dirX * dirY;
            sh[5] = SHCoefficients[5] * dirY * dirZ;
            sh[6] = SHCoefficients[6] * (3.0f * dirZ * dirZ - 1.0f);
            sh[7] = SHCoefficients[7] * dirX * dirZ;
            sh[8] = SHCoefficients[8] * (dirX * dirX - dirY * dirY);
            // 混合SH系数 颜色 权重
            var colVec = new Vector3(col.r, col.g, col.b);
            Vector3[] shCol = new Vector3[9];
            for (int i = 0; i < 9; ++i)
            {
                shCol[i] = colVec * sh[i] * weight;
            }
            // 返回值
            return shCol;
        }

        public static Vector3 GetStandardParams(float[] values, int index)
        {
            if (index < 0 || index > 8) return Vector3.zero;
            return new Vector3(values[index * 3], values[index * 3 + 1], values[index * 3 + 2]);
        }

        // 设置SH标准参
        static void SetStandardParams(float[] values, Vector3 value, int index)
        {
            if (index < 0 || index > 8) return;
            values[index * 3] = value.x;
            values[index * 3 + 1] = value.y;
            values[index * 3 + 2] = value.z;
        }

        // 增量SH标准参
        static void AddStandardParam(float[] values, Vector3 value, int index)
        {
            if (index < 0 || index > 8)
                return;
            values[index * 3] += value.x;
            values[index * 3 + 1] += value.y;
            values[index * 3 + 2] += value.z;
        }

        static void ScaleValues(float[] values, float scale)
        {
            for (int i = 0; i < 27; i++)
            {
                values[i] *= scale;
            }
        }

        public static Color GammaToLinearSpace(Color sRGB)
        {
            Color rt = new Color();
            // Approximate version from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
            rt.r = sRGB.r * (sRGB.r * (sRGB.r * 0.305306011f + 0.682171111f) + 0.012522878f);
            rt.g = sRGB.g * (sRGB.g * (sRGB.g * 0.305306011f + 0.682171111f) + 0.012522878f);
            rt.b = sRGB.b * (sRGB.b * (sRGB.b * 0.305306011f + 0.682171111f) + 0.012522878f);
            return rt;
        }


        public static float[] CalculateCube(Cubemap cube, bool hdr2ldr, float blur = 1.0f)
        {
            // 保护
            if (cube == null)
                return null;

            var values = new float[27];

#if UNITY_EDITOR
            // 检查纹理是否可读
            var assetReadable = false;
            var importer = UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(cube)) as UnityEditor.TextureImporter;
            if (importer != null)
            {
                assetReadable = importer.isReadable;
                if (!assetReadable) importer.isReadable = true;
                importer.SaveAndReimport();
            }
#endif

            // 分析Mipmap尺寸
            var mipMax = cube.mipmapCount - 2;
            var mip = Mathf.FloorToInt(Mathf.Lerp(0, mipMax, blur));
            var width = (int)Mathf.Pow(2.0f, 1.0f + (mipMax - mip));
            var height = width;
            Debug.Log(mip);
            Debug.Log(height);
            // 采样球谐参数
            var weightSum = 0.0f;
            for (int face = 0; face < 6; ++face)
            {
                var cols = cube.GetPixels((CubemapFace)face, mip);
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        // 计算方向和权重
                        var weight = 0.0f;
                        var dir = MapXYSToDir(x, y, width, height, face, ref weight);
                        weightSum += weight;
                        // 计算球谐参数
                        var index = y * height + x;
                        var col = cols[index];
                        // 对HDR2LDR编码支持
                        if (hdr2ldr)
                        {
                            col = new Color(col.r * col.a * 5.0f, col.g * col.a * 5.0f, col.b * col.a * 5.0f, 0.0f);
                        }
                        else
                        {
                            //col = GammaToLinearSpace(col);
                        }

                        var dsh = DirToSH9Col(dir, col, weight);
                        for (int i = 0; i < 9; i++)
                        {
                            AddStandardParam(values, dsh[i], i);
                        }
                    }
                }
            }
            ScaleValues(values, (4.0f * 3.14159f) / weightSum);
            for (int i = 0; i < values.Length / 3; ++i)
            {
                values[3 * i] *= SHCoefficients[i];
                values[3 * i + 1] *= SHCoefficients[i];
                values[3 * i + 2] *= SHCoefficients[i];
            }

#if UNITY_EDITOR
            // 还原纹理可读性
            if (importer != null)
            {
                if (!assetReadable) importer.isReadable = false;
                importer.SaveAndReimport();
            }
#endif

            return values;
        }

        public static float[] CalculateCube2D(Texture2D cube2D, bool hdr2ldr, float blur = 1.0f)
        {
            // 保护
            if (cube2D == null)
                return null;

            var values = new float[27];

#if UNITY_EDITOR
            // 检查纹理是否可读
            var assetReadable = false;
            var importer = UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(cube2D)) as UnityEditor.TextureImporter;
            if (importer != null)
            {
                assetReadable = importer.isReadable;
                if (!assetReadable) importer.isReadable = true;
                importer.SaveAndReimport();
            }
#endif

            // 分析Mipmap尺寸
            var mipMax = cube2D.mipmapCount - 2;
            var mip = Mathf.FloorToInt(Mathf.Lerp(0, mipMax, blur));
            var width = (int)Mathf.Pow(2.0f, 1.0f + (mipMax - mip));
            var height = width;
            Debug.Log(mip);
            Debug.Log(height);
            // 采样球谐参数
            var weightSum = 0.0f;

            var pixels = cube2D.GetPixels(mip);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    // 计算方向和权重
                    var weight = 0.0f;
                    var dir = MapXYSToDir(x, y, width, height, ref weight);
                    weightSum += weight;

                    // 计算球谐参数
                    //var index = y * width + x;
                    Color col = pixels[y * width + x];//  matCap.GetPixel(x, y, mip);

                    // 对HDR2LDR编码支持
                    if (hdr2ldr)
                    {
                        col = new Color(col.r * col.a * 5.0f, col.g * col.a * 5.0f, col.b * col.a * 5.0f, 0.0f);
                    }
                    var dsh = DirToSH9Col(dir, col, weight);
                    for (int i = 0; i < 9; i++)
                    {
                        AddStandardParam(values, dsh[i], i);
                    }
                }
            }

            ScaleValues(values, (4.0f * 3.14159265f) / weightSum);

            for (int i = 0; i < values.Length / 3; ++i)
            {
                values[3 * i] *= SHCoefficients[i];
                values[3 * i + 1] *= SHCoefficients[i];
                values[3 * i + 2] *= SHCoefficients[i];
            }

#if UNITY_EDITOR
            // 还原纹理可读性
            if (importer != null)
            {
                if (!assetReadable) importer.isReadable = false;
                importer.SaveAndReimport();
            }
#endif

            return values;
        }

        public static Vector4[] GetUnityParams(float[] values)
        {
            var shParams = new Vector4[7];

            shParams[0] = new Vector4(values[9], values[3], values[6], values[0] - values[18]);
            shParams[1] = new Vector4(values[10], values[4], values[7], values[1] - values[19]);
            shParams[2] = new Vector4(values[11], values[5], values[8], values[2] - values[20]);
            shParams[3] = new Vector4(values[12], values[15], 3.0f * values[18], values[21]);
            shParams[4] = new Vector4(values[13], values[16], 3.0f * values[19], values[22]);
            shParams[5] = new Vector4(values[14], values[17], 3.0f * values[20], values[23]);
            shParams[6] = new Vector4(values[24], values[25], values[26], 1.0f);

            return shParams;
        }

        // 场景球谐数据集
        public SHValue SH9Value = new SHValue();

        public void BuildSH(Cubemap cube, bool hdr2ldr, float blur = 1.0f)
        {
            var float27Array = CalculateCube(cube, hdr2ldr, blur);

            SH9Value.SHParams = GetUnityParams(float27Array);

        }

        public void BuildSH(Texture2D cube2D, bool hdr2ldr, float blur = 1.0f)
        {
            var float27Array = CalculateCube2D(cube2D, hdr2ldr, blur);
            SH9Value.SHParams = GetUnityParams(float27Array);
        }

        public void BuildSH9Data()
        {
            switch (buildType)
            {
                case BuildType.CubeMap:
                    {
                        if (mCubeMap == null)
                        {
                            Debug.LogError("Create SH Failed: mCubeMap == null");
                            return;
                        }
                        BuildSH(mCubeMap, IsHDRToLDR, Blur);
                        break;
                    }

                case BuildType.MatCap:
                    {
                        if (mCube2D == null)
                        {
                            Debug.LogError("Create SH Failed: mMatCap == null");
                            return;
                        }
                        BuildSH(mCube2D, IsHDRToLDR, Blur);
                        break;
                    }
            }

            if (IsBuildSHDataForGLSL)
            {
                string SH9DefineStr = "";
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHAr", SH9Value.SHParams[0].x, SH9Value.SHParams[0].y, SH9Value.SHParams[0].z, SH9Value.SHParams[0].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHAg", SH9Value.SHParams[1].x, SH9Value.SHParams[1].y, SH9Value.SHParams[1].z, SH9Value.SHParams[1].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHAb", SH9Value.SHParams[2].x, SH9Value.SHParams[2].y, SH9Value.SHParams[2].z, SH9Value.SHParams[2].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHBr", SH9Value.SHParams[3].x, SH9Value.SHParams[3].y, SH9Value.SHParams[3].z, SH9Value.SHParams[3].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHBg", SH9Value.SHParams[4].x, SH9Value.SHParams[4].y, SH9Value.SHParams[4].z, SH9Value.SHParams[4].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHBb", SH9Value.SHParams[5].x, SH9Value.SHParams[5].y, SH9Value.SHParams[5].z, SH9Value.SHParams[5].w);
                SH9DefineStr += string.Format("const vec4\t\t{0} = vec4({1},{2},{3},{4});\r\n", "SHC", SH9Value.SHParams[6].x, SH9Value.SHParams[6].y, SH9Value.SHParams[6].z, SH9Value.SHParams[6].w);

                System.IO.File.WriteAllText("SH9.txt", SH9DefineStr);

                Debug.LogFormat("{0}/SH9.txt has been refresh.", System.IO.Directory.GetCurrentDirectory());
            }
        }
        #endregion
#if UNITY_EDITOR
        //------------------------------------------------------
        public void OnInspector(System.Object param = null)
        {
            Framework.ED.HandleUtilityWrapper.DrawProperty(this, null);

            if (GUILayout.Button("Build SH9", GUILayout.Width(150)))
            {
                BuildSH9Data();
            }
        }
        public void Save() { }
#endif
    }
}
