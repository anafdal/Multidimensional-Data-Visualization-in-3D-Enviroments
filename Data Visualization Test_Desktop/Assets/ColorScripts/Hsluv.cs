using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code manipulated from Mark Wonnacott, Alexei Boronine
/// </summary>

public class Hsluv : MonoBehaviour
{
	protected static float[][] M = new float[][]
		{
			new float[] {  3.240969941904521f, -1.537383177570093f, -0.498610760293f    },
			new float[] { -0.96924363628087f,   1.87596750150772f,   0.041555057407175f },
			new float[] {  0.055630079696993f, -0.20397695888897f,   1.056971514242878f },
		};

	protected static float[][] MInv = new float[][]
	{
			new float[] { 0.41239079926595f,  0.35758433938387f, 0.18048078840183f  },
			new float[] { 0.21263900587151f,  0.71516867876775f, 0.072192315360733f },
			new float[] { 0.019330818715591f, 0.11919477979462f, 0.95053215224966f  },
	};

	protected static float RefX = 0.95045592705167f;
	protected static float RefY = 1.0f;
	protected static float RefZ = 1.089057750759878f;

	protected static float RefU = 0.19783000664283f;
	protected static float RefV = 0.46831999493879f;

	protected static float Kappa = 903.2962962f;
	protected static float Epsilon = 0.0088564516f;

	protected static IList<float[]> GetBounds(float L)
	{
		var result = new List<float[]>();

		float sub1 = Mathf.Pow(L + 16, 3) / 1560896;
		float sub2 = sub1 > Epsilon ? sub1 : L / Kappa;

		for (int c = 0; c < 3; ++c)
		{
			var m1 = M[c][0];
			var m2 = M[c][1];
			var m3 = M[c][2];

			for (int t = 0; t < 2; ++t)
			{
				var top1 = (284517 * m1 - 94839 * m3) * sub2;
				var top2 = (838422 * m3 + 769860 * m2 + 731718 * m1) * L * sub2 - 769860 * t * L;
				var bottom = (632260 * m3 - 126452 * m2) * sub2 + 126452 * t;

				result.Add(new float[] { top1 / bottom, top2 / bottom });
			}
		}

		return result;
	}

	protected static float IntersectLineLine(IList<float> lineA,
		IList<float> lineB)
	{
		return (lineA[1] - lineB[1]) / (lineB[0] - lineA[0]);
	}

	protected static float DistanceFromPole(IList<float> point)
	{
		return Mathf.Sqrt(Mathf.Pow(point[0], 2) + Mathf.Pow(point[1], 2));
	}

	protected static bool LengthOfRayUntilIntersect(float theta,
		IList<float> line,
		out float length)
	{
		length = line[1] / (Mathf.Sin(theta) - line[0] * Mathf.Cos(theta));

		return length >= 0;
	}

	protected static float MaxSafeChromaForL(float L)
	{
		var bounds = GetBounds(L);
		float min = float.MaxValue;

		for (int i = 0; i < 2; ++i)
		{
			var m1 = bounds[i][0];
			var b1 = bounds[i][1];
			var line = new float[] { m1, b1 };

			float x = IntersectLineLine(line, new float[] { -1 / m1, 0 });
			float length = DistanceFromPole(new float[] { x, b1 + x * m1 });

			min = Mathf.Min(min, length);
		}

		return min;
	}

	protected static float MaxChromaForLH(float L, float H)
	{
		float hrad = H / 360 * Mathf.PI * 2;

		var bounds = GetBounds(L);
		float min = float.MaxValue;

		foreach (var bound in bounds)
		{
			float length;

			if (LengthOfRayUntilIntersect(hrad, bound, out length))
			{
				min = Mathf.Min(min, length);
			}
		}

		return min;
	}

	protected static float DotProduct(IList<float> a,
		IList<float> b)
	{
		float sum = 0;

		for (int i = 0; i < a.Count; ++i)
		{
			sum += a[i] * b[i];
		}

		return sum;
	}

	protected static float Round(float value, int places)
	{
		float n = Mathf.Pow(10, places);

		return Mathf.Round(value * n) / n;
	}

	protected static float FromLinear(float c)
	{
		if (c <= 0.0031308f)
		{
			return 12.92f * c;
		}
		else
		{
			return 1.055f * Mathf.Pow(c, 1 / 2.4f) - 0.055f;
		}
	}

	protected static float ToLinear(float c)
	{
		if (c > 0.04045f)
		{
			return Mathf.Pow((c + 0.055f) / (1 + 0.055f), 2.4f);
		}
		else
		{
			return c / 12.92f;
		}
	}

	protected static IList<int> RgbPrepare(IList<float> tuple)
	{

		for (int i = 0; i < tuple.Count; ++i)
		{
			tuple[i] = Round(tuple[i], 3);
		}

		for (int i = 0; i < tuple.Count; ++i)
		{
			float ch = tuple[i];

			if (ch < -0.0001f || ch > 1.0001f)
			{
				throw new System.Exception("Illegal rgb value: " + ch);
			}
		}

		var results = new int[tuple.Count];

		for (int i = 0; i < tuple.Count; ++i)
		{
			results[i] = (int)Mathf.Round(tuple[i] * 255);
		}

		return results;
	}

	public static IList<float> XyzToRgb(IList<float> tuple)
	{
		return new float[]
		{
				FromLinear(DotProduct(M[0], tuple)),
				FromLinear(DotProduct(M[1], tuple)),
				FromLinear(DotProduct(M[2], tuple)),
		};
	}

	public static IList<float> RgbToXyz(IList<float> tuple)
	{
		var rgbl = new float[]
		{
				ToLinear(tuple[0]),
				ToLinear(tuple[1]),
				ToLinear(tuple[2]),
		};

		return new float[]
		{
				DotProduct(MInv[0], rgbl),
				DotProduct(MInv[1], rgbl),
				DotProduct(MInv[2], rgbl),
		};
	}

	protected static float YToL(float Y)
	{
		if (Y <= Epsilon)
		{
			return (Y / RefY) * Kappa;
		}
		else
		{
			return 116 * Mathf.Pow(Y / RefY, 1.0f / 3.0f) - 16;
		}
	}

	protected static float LToY(float L)
	{
		if (L <= 8)
		{
			return RefY * L / Kappa;
		}
		else
		{
			return RefY * Mathf.Pow((L + 16) / 116, 3);
		}
	}

	public static IList<float> XyzToLuv(IList<float> tuple)
	{
		float X = tuple[0];
		float Y = tuple[1];
		float Z = tuple[2];

		float varU = (4 * X) / (X + (15 * Y) + (3 * Z));
		float varV = (9 * Y) / (X + (15 * Y) + (3 * Z));

		float L = YToL(Y);

		if (L == 0)
		{
			return new float[] { 0, 0, 0 };
		}

		var U = 13 * L * (varU - RefU);
		var V = 13 * L * (varV - RefV);

		return new float[] { L, U, V };
	}

	public static IList<float> LuvToXyz(IList<float> tuple)
	{
		float L = tuple[0];
		float U = tuple[1];
		float V = tuple[2];

		if (L == 0)
		{
			return new float[] { 0, 0, 0 };
		}

		float varU = U / (13 * L) + RefU;
		float varV = V / (13 * L) + RefV;

		float Y = LToY(L);
		float X = 0 - (9 * Y * varU) / ((varU - 4) * varV - varU * varV);
		float Z = (9 * Y - (15 * varV * Y) - (varV * X)) / (3 * varV);

		return new float[] { X, Y, Z };
	}

	public static IList<float> LuvToLch(IList<float> tuple)
	{
		float L = tuple[0];
		float U = tuple[1];
		float V = tuple[2];

		float C = Mathf.Pow(Mathf.Pow(U, 2) + Mathf.Pow(V, 2), 0.5f);
		float Hrad = Mathf.Atan2(V, U);

		float H = Hrad * 180.0f / Mathf.PI;

		if (H < 0)
		{
			H = 360 + H;
		}

		return new float[] { L, C, H };
	}

	public static IList<float> LchToLuv(IList<float> tuple)
	{
		float L = tuple[0];
		float C = tuple[1];
		float H = tuple[2];

		float Hrad = H / 360.0f * 2 * Mathf.PI;
		float U = Mathf.Cos(Hrad) * C;
		float V = Mathf.Sin(Hrad) * C;

		return new float[] { L, U, V };
	}

	public static IList<float> HsluvToLch(IList<float> tuple)
	{
		float H = tuple[0];
		float S = tuple[1];
		float L = tuple[2];

		if (L > 99.9999999f)
		{
			return new float[] { 100, 0, H };
		}

		if (L < 0.00000001f)
		{
			return new float[] { 0, 0, H };
		}

		float max = MaxChromaForLH(L, H);
		float C = max / 100 * S;

		return new float[] { L, C, H };
	}

	public static IList<float> LchToHsluv(IList<float> tuple)
	{
		float L = tuple[0];
		float C = tuple[1];
		float H = tuple[2];

		if (L > 99.9999999f)
		{
			return new float[] { H, 0, 100 };
		}

		if (L < 0.00000001f)
		{
			return new float[] { H, 0, 0 };
		}

		float max = MaxChromaForLH(L, H);
		float S = C / max * 100;

		return new float[] { H, S, L };
	}

	public static IList<float> HpluvToLch(IList<float> tuple)
	{
		float H = tuple[0];
		float S = tuple[1];
		float L = tuple[2];

		if (L > 99.9999999f)
		{
			return new float[] { 100, 0, H };
		}

		if (L < 0.00000001f)
		{
			return new float[] { 0, 0, H };
		}

		float max = MaxSafeChromaForL(L);
		float C = max / 100 * S;

		return new float[] { L, C, H };
	}

	public static IList<float> LchToHpluv(IList<float> tuple)
	{
		float L = tuple[0];
		float C = tuple[1];
		float H = tuple[2];

		if (L > 99.9999999f)
		{
			return new float[] { H, 0, 100 };
		}

		if (L < 0.00000001f)
		{
			return new float[] { H, 0, 0 };
		}

		float max = MaxSafeChromaForL(L);
		float S = C / max * 100;

		return new float[] { H, S, L };
	}

	public static string RgbToHex(IList<float> tuple)
	{
		IList<int> prepared = RgbPrepare(tuple);

		return string.Format("#{0}{1}{2}",
			prepared[0].ToString("x2"),
			prepared[1].ToString("x2"),
			prepared[2].ToString("x2"));
	}

	public static IList<float> HexToRgb(string hex)
	{
		return new float[]
		{
				int.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255,
				int.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255,
				int.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255,
		};
	}

	public static IList<float> LchToRgb(IList<float> tuple)
	{
		return XyzToRgb(LuvToXyz(LchToLuv(tuple)));
	}

	public static IList<float> RgbToLch(IList<float> tuple)
	{
		return LuvToLch(XyzToLuv(RgbToXyz(tuple)));
	}

	// Rgb <--> Hsluv(p)

	public static IList<float> HsluvToRgb(IList<float> tuple)
	{
		return LchToRgb(HsluvToLch(tuple));
	}

	public static IList<float> RgbToHsluv(IList<float> tuple)
	{
		return LchToHsluv(RgbToLch(tuple));
	}

	public static IList<float> HpluvToRgb(IList<float> tuple)
	{
		return LchToRgb(HpluvToLch(tuple));
	}

	public static IList<float> RgbToHpluv(IList<float> tuple)
	{
		return LchToHpluv(RgbToLch(tuple));
	}

	// Hex

	public static string HsluvToHex(IList<float> tuple)
	{
		return RgbToHex(HsluvToRgb(tuple));
	}

	public static string HpluvToHex(IList<float> tuple)
	{
		return RgbToHex(HpluvToRgb(tuple));
	}

	public static IList<float> HexToHsluv(string s)
	{
		return RgbToHsluv(HexToRgb(s));
	}

	public static IList<float> HexToHpluv(string s)
	{
		return RgbToHpluv(HexToRgb(s));
	}
	public static IList<float> HextoLch(string s)
	{
		return RgbToLch(HexToRgb(s));
	}

	public static string LchToHex(IList<float> tuple)
	{

		return RgbToHex(LchToRgb(tuple));

	}

	public static IList<float> Lerp(IList<float> tuple1, IList<float> tuple2, float t)//Lerp, can be used for different color spaces
	{
		return new float[] {Mathf.Lerp(tuple1[0], tuple2[0], t), Mathf.Lerp(tuple1[1], tuple2[1], t), Mathf.Lerp(tuple1[2], tuple2[2], t)};
	}

	public static Color ConvertToColor(IList<float> tuple)//Convert from RGB to Color
	{

		float r = tuple[0];
		float g = tuple[1];
		float b = tuple[2];

		

		return new Color(r/255.0f, g/255.0f, b/255.0f);
    }

	public static IList<float> ConvertToRGB(Color c)//Convert from Color to RGB
	{

		float r = c.r;
		float g = c.g;
		float b = c.b;



		return new float[] { r, g, b };
	}


}
