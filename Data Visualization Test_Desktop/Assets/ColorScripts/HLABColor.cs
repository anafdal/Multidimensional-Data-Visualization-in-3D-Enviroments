using UnityEngine;
[System.Serializable]

//Still in Development
public struct HLABColor
{

	// This script provides a Lab color space in addition to Unity's built in Red/Green/Blue colors.
	// Lab is based on CIE XYZ and is a color-opponent space with L for lightness and a and b for the color-opponent dimensions.
	// Lab color is designed to approximate human vision and so it aspires to perceptual uniformity.
	// The L component closely matches human perception of lightness.
	// Put HLABColor.cs in a 'Plugins' folder to ensure that it is accessible to other scripts.

	private float L;
	private float A;
	private float B;



	// lightness accessors
	public float l
	{
		get { return this.L; }
		set { this.L = value; }
	}

	// a color-opponent accessor
	public float a
	{
		get { return this.A; }
		set { this.A = value; }
	}

	// b color-opponent accessor
	public float b
	{
		get { return this.B; }
		set { this.B = value; }
	}

	// constructor - takes three floats for lightness and color-opponent dimensions
	public HLABColor(float x, float y, float z)
	{
		this.L = x;
		this.A = y;
		this.B = z;
	}

	// constructor - takes a Color
	public HLABColor(Color col)
	{
		HLABColor temp = FromColor(col);
		L = temp.l;
		A = temp.a;
		B = temp.b;
	}

	// static function for linear interpolation between two HLABColors
	public static HLABColor Lerp(HLABColor a, HLABColor b, float t)
	{
		return new HLABColor(Mathf.Lerp(a.l, b.l, t), Mathf.Lerp(a.a, b.a, t), Mathf.Lerp(a.b, b.b, t));
	}

	// static function for interpolation between two Unity Colors through normalized colorspace
	public static Color Lerp(Color a, Color b, float t)
	{
		return (HLABColor.Lerp(HLABColor.FromColor(a), HLABColor.FromColor(b), t)).ToColor();
	}

	// static function for returning the color difference in a normalized colorspace (Delta-E)
	public static float Distance(HLABColor a, HLABColor b)
	{
		return Mathf.Sqrt(Mathf.Pow((a.l - b.l), 2f) + Mathf.Pow((a.a - b.a), 2f) + Mathf.Pow((a.b - b.b), 2f));
	}

	// static function for converting from Color to HLABColor
	public static HLABColor FromColor(Color RGB)
	{
		
		HLABColor lab = new HLABColor(0f, 0f, 0f);

		float R = (RGB.r / 255);
		float G = (RGB.g / 255);
		float B = (RGB.b / 255);

		if (R > 0.04045f)
		{
			R = Mathf.Pow(((R + 0.055f) / 1.055f), 2.4f);

		}
		else
		{ 
			R = R / 12.92f;

		}

		if (G > 0.04045f)
		{
			G = Mathf.Pow(((G + 0.055f) / 1.055f), 2.4f);
		}
		else {
			G = G / 12.92f;
		}

		if (B > 0.04045f)
		{
			B = Mathf.Pow(((B + 0.055f) / 1.055f), 2.4f);
		}
		else
		{ 
			B = B / 12.92f;

		}

		R = R * 100;
		G = G * 100;
		B = B * 100;


		float X = R * 0.4124f + G * 0.3576f + B * 0.1805f;
		float Y = R * 0.2126f + G * 0.7152f + B * 0.0722f;
		float Z = R * 0.0193f + G * 0.1192f + B * 0.9505f;



		float Ka = (175.0f / 198.04f) * (Y + X);
		float Kb = (70.0f / 218.11f) * (Y + Z);

		lab.l = 100.0f * Mathf.Pow((Y / Y), 2);
		lab.a = Ka * (((X / X) - (Y / Y)) / Mathf.Sqrt(Y / Y));
		lab.b = Kb * (((Y / Y) - (Z / Z)) / Mathf.Sqrt(Y / Y));

		return lab;
	}

	// static function for converting from HLABColor to Color
	public static Color ToColor(HLABColor lab)
	{
		/*Color RGB = new Color();

		float Ka = (175.0f / 198.04f) * (Y + X);
		float Kb = (70.0f / 218.11f) * (Y + Z);
		*/

		return new Color(1, 1, 1);
	}

	// function for converting an instance of HLABColor to Color
	public Color ToColor()
	{
		return HLABColor.ToColor(this);
	}

	// override for string
	public override string ToString()
	{
		return "L:" + l + " A:" + a + " B:" + b;
	}

	// are two HLABColors the same?
	public override bool Equals(System.Object obj)
	{
		if (obj == null || GetType() != obj.GetType()) return false;
		return (this == (HLABColor)obj);
	}

	// override hashcode for a HLABColor
	public override int GetHashCode()
	{
		return l.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
	}

	// Equality operator
	public static bool operator ==(HLABColor item1, HLABColor item2)
	{
		return (item1.l == item2.l && item1.a == item2.a && item1.b == item2.b);
	}

	// Inequality operator
	public static bool operator !=(HLABColor item1, HLABColor item2)
	{
		return (item1.l != item2.l || item1.a != item2.a || item1.b != item2.b);
	}
}
