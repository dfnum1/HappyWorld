using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Data.PBRLab
{
	public class SHFunc
	{

		public static int[] LM16;

		public static void tranferK(int k,ref int l,ref int m)
		{
			l = (int)Mathf.Sqrt (k);
			m = k - l*(l+1);
		}

		public static void Init()
		{
			LM16 = new int[32];
			LM16 [0] = 0;
			LM16 [16] = 0;

			LM16 [1] = 1;
			LM16 [17] = -1;
			LM16 [2] = 1;
			LM16 [18] = 0;
			LM16 [3] = 1;
			LM16 [19] = 1;

			LM16 [4] = 2;
			LM16 [20] = -2;
			LM16 [5] = 2;
			LM16 [21] = -1;
			LM16 [6] = 2;
			LM16 [22] = 0;
			LM16 [7] = 2;
			LM16 [23] = 1;
			LM16 [8] = 2;
			LM16 [24] = 2;

			LM16 [9] = 3;
			LM16 [25] = -3;
			LM16 [10] = 3;
			LM16 [26] = -2;
			LM16 [11] = 3;
			LM16 [27] = -1;
			LM16 [12] = 3;
			LM16 [28] = 0;
			LM16 [13] = 3;
			LM16 [29] = 1;
			LM16 [14] = 3;
			LM16 [30] = 2;
			LM16 [15] = 3;
			LM16 [31] = 3;

		}

		//Evaluate an Associated Legendre Polynomial P(l, m) at x
		private static float P(int l, int m, float x)
		{
			//First generate the value of P(m, m) at x
			float pmm=1.0f;

			if(m>0)
			{
				float sqrtOneMinusX2=Mathf.Sqrt(1.0f-x*x);

				float fact=1.0f;

				for(int i=1; i<=m; ++i)
				{
					pmm*=(-fact)*sqrtOneMinusX2;
					fact+=2.0f;
				}
			}

			//If l==m, P(l, m)==P(m, m)
			if(l==m)
				return pmm;

			//Use rule 3 to calculate P(m+1, m) from P(m, m)
			float pmp1m=x*(2.0f*m+1.0f)*pmm;

			//If l==m+1, P(l, m)==P(m+1, m)
			if(l==m+1)
				return pmp1m;

			//Otherwise, l>m+1.
			//Iterate rule 1 to get the result
			float plm=0.0f;

			for(int i=m+2; i<=l; ++i)
			{
				plm=((2.0f*i-1.0f)*x*pmp1m-(i+m-1.0f)*pmm)/(i-m);
				pmm=pmp1m;
				pmp1m=plm;
			}

			return plm;
		}

		private static float K(int l, int m)
		{
			float temp=((2.0f*l+1.0f)*Factorial(l-m))/((4.0f*Mathf.PI)*Factorial(l+m));

			return Mathf.Sqrt(temp);
		}

		//Sample a spherical harmonic basis function Y(l, m) at a point on the unit sphere
		public static float SH(int l, int m, float theta, float phi)
		{
			float sqrt2=Mathf.Sqrt(2.0f);

			if(m==0)
				return K(l, 0)*P(l, m, Mathf.Cos(theta));

			if(m>0)
				return sqrt2*K(l, m)*Mathf.Cos(m*phi)*P(l, m, Mathf.Cos(theta));

			//m<0
			return sqrt2*K(l,-m)*Mathf.Sin(-m*phi)*P(l, -m, Mathf.Cos(theta));
		}


		//Calculate n! (n>=0)
		private static int Factorial(int n)
		{
			if(n<=1)
				return 1;

			int result=n;

			while(--n > 1)
				result*=n;

			return result;
		}
	}

	public class PermutedMatrix
	{
		public PermutedMatrix(Matrix4x4 m)
		{
			mMat44 = m;
		}

		static int permute(int v)
		{
			if (v == 1)
				return 0;
			if (v == -1)
				return 1;
			if (v == 0)
				return 2;
			return 0;
		}

		public float GetByMN(int m, int n)
		{
			int row = permute(m);
			int column = permute(n);

			if (row == 0 && column == 0)
				return mMat44.m00;
			if (row == 0 && column == 1)
				return mMat44.m01;
			if (row == 0 && column == 2)
				return mMat44.m02;

			if (row == 1 && column == 0)
				return mMat44.m10;
			if (row == 1 && column == 1)
				return mMat44.m11;
			if (row == 1 && column == 2)
				return mMat44.m12;

			if (row == 2 && column == 0)
				return mMat44.m20;
			if (row == 2 && column == 1)
				return mMat44.m21;
			if (row == 2 && column == 2)
				return mMat44.m22;

			return -1;
		}

		private Matrix4x4 mMat44;
	}

	public class SHRotate
	{

		private static float delta(int m, int n)
		{
			return (m == n ? 1 : 0);
		}

		private static void uvw(int l, int m, int n, ref float u, ref float v, ref float w)
		{
			float d = delta(m, 0);
			int abs_m = Mathf.Abs(m);

			float denom;
			if (Mathf.Abs(n) == l)
				denom = (2 * l) * (2 * l - 1);

			else
				denom = (l + n) * (l - n);

			u = Mathf.Sqrt((l + m) * (l - m) / denom);
			v = 0.5f * Mathf.Sqrt((1 + d) * (l + abs_m - 1) * (l + abs_m) / denom) * (1 - 2 * d);
			w = -0.5f * Mathf.Sqrt((l - abs_m - 1) * (l - abs_m) / denom) * (1 - d);
		}

		private static float P(int i, int l, int a, int b, PermutedMatrix R, SHRotateMatrix M)
		{
			if (b == -l)
			{
				return (R.GetByMN(i, 1) * M.GetValueByBand(l - 1, a, -l + 1) + R.GetByMN(i, -1) * M.GetValueByBand(l - 1, a, l - 1));
			}
			else if (b == l)
			{
				return (R.GetByMN(i, 1) * M.GetValueByBand(l - 1, a, l - 1) - R.GetByMN(i, -1) * M.GetValueByBand(l - 1, a, -l + 1));
			}
			else
			{
				return (R.GetByMN(i, 0) * M.GetValueByBand(l - 1, a, b));
			}
		}

		private static float U(int l, int m, int n, PermutedMatrix R, SHRotateMatrix M)
		{
			if (m == 0)
				return (P(0, l, 0, n, R, M));

			return (P(0, l, m, n, R, M));
		}


		private static float V(int l, int m, int n, PermutedMatrix R, SHRotateMatrix M)
		{
			if (m == 0)
			{
				float p0 = P(1, l, 1, n, R, M);
				float p1 = P(-1, l, -1, n, R, M);
				return (p0 + p1);
			}
			else if (m > 0)
			{
				float d = delta(m, 1);
				float p0 = P(1, l, m - 1, n, R, M);
				float p1 = P(-1, l, -m + 1, n, R, M);
				return (p0 * Mathf.Sqrt(1 + d) - p1 * (1 - d));
			}
			else
			{
				float d = delta(m, -1);
				float p0 = P(1, l, m + 1, n, R, M);
				float p1 = P(-1, l, -m - 1, n, R, M);
				return (p0 * (1 - d) + p1 * Mathf.Sqrt(1 - d));
			}
		}


		private static float W(int l, int m, int n, PermutedMatrix R, SHRotateMatrix M)
		{
			if (m == 0)
			{
				return (0);
			}
			else if (m > 0)
			{
				float p0 = P(1, l, m + 1, n, R, M);
				float p1 = P(-1, l, -m - 1, n, R, M);
				return (p0 + p1);
			}
			else // m < 0
			{
				float p0 = P(1, l, m - 1, n, R, M);
				float p1 = P(-1, l, -m + 1, n, R, M);
				return (p0 - p1);
			}
		}


		private static float M(int l, int m, int n, PermutedMatrix R, SHRotateMatrix M)
		{
			// First get the scalars
			float u = 0.0f, v = 0.0f, w = 0.0f;
			uvw(l, m, n, ref u, ref v, ref w);

			// Scale by their functions
			if (u != 0.0f)
				u *= U(l, m, n, R, M);
			if (v != 0.0f)
				v *= V(l, m, n, R, M);
			if (w != 0.0f)
				w *= W(l, m, n, R, M);

			return (u + v + w);
		}


		public static Vector3[] Rotate(Vector3[] src, Matrix4x4 rot)
		{
			SHRotateMatrix shrm = transfer(rot, (int)Mathf.Sqrt(src.Length));
			Vector3[] dest = shrm.Transform(src);
			return dest;
		}

		public static SHRotateMatrix transfer(Matrix4x4 rot, int bands)
		{
			SHRotateMatrix result = new SHRotateMatrix(bands * bands);
			result.SetValue(0, 0, 1);

			PermutedMatrix pm = new PermutedMatrix(rot);

			for (int m = -1; m <= 1; m++)
				for (int n = -1; n <= 1; n++)
					result.SetValueByBand(1, m, n, pm.GetByMN(m, n));

			for (int band = 2; band < bands; band++)
			{
				for (int m = -band; m <= band; m++)
					for (int n = -band; n <= band; n++)
						result.SetValueByBand(band, m, n, M(band, m, n, pm, result));
			}

			return result;
		}

	}

	public class SHRotateMatrix
	{
		public Vector3[] Transform(Vector3[] src)
		{
			int bands = (int)Mathf.Sqrt(mDim);
			Vector3[] dest = new Vector3[src.Length];
			for (int i = 0; i < dest.Length; i++)
				dest[i] = Vector3.zero;
			for (int l = 0; l < bands; l++)
			{
				for (int mo = -l; mo <= l; mo++)
				{
					int outputIndex = GetIndexByLM(l, mo);
					Vector3 target = Vector3.zero;
					for (int mi = -l; mi <= l; mi++)
					{
						int inputIndex = GetIndexByLM(l, mi);
						float matValue = GetValueByBand(l, mo, mi);
						Vector3 source = src[inputIndex];
						target += source * matValue;
					}

					dest[outputIndex] = target;
				}
			}

			return dest;
		}

		public SHRotateMatrix(int dim)
		{
			mDim = dim;
			mMatrix = new float[mDim][];
			for (int i = 0; i < mDim; i++)
			{
				mMatrix[i] = new float[mDim];
				for (int j = 0; j < mDim; j++)
				{
					mMatrix[i][j] = 0.0f;
				}
			}
		}

		public void SetValue(int i, int j, float value)
		{
			mMatrix[i][j] = value;
		}

		public float GetValueByBand(int l, int a, int b)
		{
			int centre = (l + 1) * l;
			return mMatrix[centre + a][centre + b];
		}

		public void SetValueByBand(int l, int a, int b, float value)
		{
			int centre = (l + 1) * l;
			mMatrix[centre + a][centre + b] = value;
		}

		private int GetIndexByLM(int l, int m)
		{
			return (l + 1) * l + m;
		}

		public int mDim;
		private float[][] mMatrix;
	}
}
