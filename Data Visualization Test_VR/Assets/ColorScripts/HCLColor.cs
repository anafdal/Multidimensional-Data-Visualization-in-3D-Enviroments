using UnityEngine;
[System.Serializable]

public struct HCLColor
{

	// This script provides a HCL color space in addition to Unity's built in Red/Green/Blue colors.
	// HCL is based on CIE XYZ and is a color-opponent space with L for lightness and a and b for the color-opponent dimensions.
	// HCL color is designed to approximate human vision and so it aspires to perceptual uniformity.
	// The L component closely matches human perception of lightness.
	// Put HCLColor.cs in a 'Plugins' folder to ensure that it is accessible to other scripts.

	private float H;
	private float C;
	private float L;



	// lightness accessors
	public float x
	{
		get { return this.H; }
		set { this.H = value; }
	}

	
	public float y
	{
		get { return this.C; }
		set { this.C = value; }
	}

	
	public float z
	{
		get { return this.L; }
		set { this.L = value; }
	}

	// constructor - takes three floats for lightness and color-opponent dimensions
	public HCLColor(float x, float y, float z)
	{
		this.H = x;
		this.C = y;
		this.L = z;
	}

	// constructor - takes a Color
	public HCLColor(Color col)
	{
		HCLColor temp = FromColor(col);
		H = temp.x;
		C = temp.y;
		L = temp.z;
	}

	// static function for linear interpolation between two HCLColors
	public static HCLColor Lerp(HCLColor a, HCLColor b, float t)
	{
		return new HCLColor(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
	}

	// static function for interpolation between two Unity Colors through normalized colorspace
	public static Color Lerp(Color a, Color b, float t)
	{
		return (HCLColor.Lerp(HCLColor.FromColor(a), HCLColor.FromColor(b), t)).ToColor();
	}

	// static function for returning the color difference in a normalized colorspace (Delta-E)
	public static float Distance(HCLColor a, HCLColor b)
	{
		return Mathf.Sqrt(Mathf.Pow((a.x - b.x), 2f) + Mathf.Pow((a.y - b.y), 2f) + Mathf.Pow((a.z - b.z), 2f));
	}

	// static function for converting from Color to HCLColor
	public static HCLColor FromColor(Color RGB)
	{
		float HCLgamma = 3;
		float HCLy0 = 100;
		float HCLmaxL = 0.530454533953517f;

		HCLColor HCL = new HCLColor(0f, 0f, 0f);

		float H = 0;
		float U = Mathf.Min(RGB.r, Mathf.Min(RGB.g, RGB.b));
		float V = Mathf.Max(RGB.r, Mathf.Max(RGB.g, RGB.b));

		float Q = HCLgamma / HCLy0;

		HCL.y = V - U;
		if (HCL.y != 0)
		{
			H = Mathf.Atan2(RGB.g - RGB.b, RGB.r - RGB.g) / Mathf.PI;
			Q *= U / V;
		}
		Q = Mathf.Exp(Q);

		HCL.x = (H / 2 - Mathf.Min(((H)%1), ((-H)%1)) / 6) % 1;
		HCL.y *= Q;
		HCL.z = Mathf.Lerp(-U, V, Q) / (HCLmaxL * 2);

		return HCL;
	}


	public static Color ToColor(HCLColor HCL)
    {
		float HCLgamma = 3;
		float HCLy0 = 100;
		float HCLmaxL = Mathf.Exp(HCLgamma / HCLy0) - 0.5f;

		Color RGB = new Color(0f, 0f, 0f);

		if (HCL.z != 0)
			{
				float H = HCL.x;
				float C = HCL.y;
				float L = HCL.z * HCLmaxL;

				float Q = Mathf.Exp((1 - C / (2 * L)) * (HCLgamma / HCLy0));
				float U = (2 * L - C) / (2 * Q - 1);
				float V = C / Q;
				float A = (H + Mathf.Min(((2 * H)%1) / 4, ((-2 * H)%1) / 8)) * Mathf.PI * 2;
				float T;

				H *= 6;
				if (H <= 0.999)
				{
					T = Mathf.Tan(A);
					RGB.r = 1;
					RGB.g = T / (1 + T);
				}
				else if (H <= 1.001)
				{
					RGB.r = 1;
					RGB.g = 1;
				}
				else if (H <= 2)
				{
					T = Mathf.Tan(A);
					RGB.r = (1 + T) / T;
					RGB.g = 1;
				}
				else if (H <= 3)
				{
					T = Mathf.Tan(A);
					RGB.g = 1;
					RGB.b = 1 + T;
				}
				else if (H <= 3.999)
				{
					T = Mathf.Tan(A);
					RGB.g = 1 / (1 + T);
					RGB.b = 1;
				}
				else if (H <= 4.001)
				{
					RGB.g = 0;
					RGB.b = 1;
				}
				else if (H <= 5)
				{
					T = Mathf.Tan(A);
					RGB.r = -1 / T;
					RGB.b = 1;
				}
				else
				{
					T = Mathf.Tan(A);
					RGB.r = 1;
					RGB.b = -T;
				}
				RGB = RGB * V +new Color(U,U,U);
			}
			return RGB;
		}
	



		// function for converting an instance of HCLColor to Color
		public Color ToColor()
	 {
		return HCLColor.ToColor(this);
	 }

	// override for string
	public override string ToString()
	{
		return "H:" + x + " C:" + y + " L:" + z;
	}

	// are two HCLColors the same?
	public override bool Equals(System.Object obj)
	{
		if (obj == null || GetType() != obj.GetType()) return false;
		return (this == (HCLColor)obj);
	}

	// override hashcode for a HCLColor
	public override int GetHashCode()
	{
		return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
	}

	// Equality operator
	public static bool operator ==(HCLColor item1, HCLColor item2)
	{
		return (item1.x == item2.x && item1.y == item2.y && item1.z == item2.z);
	}

	// Inequality operator
	public static bool operator !=(HCLColor item1, HCLColor item2)
	{
		return (item1.x != item2.x || item1.y != item2.y || item1.z != item2.z);
	}
}
