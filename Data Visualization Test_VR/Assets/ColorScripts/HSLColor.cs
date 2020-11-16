using UnityEngine;
[System.Serializable]



public struct HSLColor
 {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        public float H;
        public float S;
        public float L;

        

    // lightness accessors
    public float x
    {
        get { return this.H; }
        set { this.H = value; }
    }


    public float y
    {
        get { return this.S; }
        set { this.S = value; }
    }


    public float z
    {
        get { return this.L; }
        set { this.L = value; }
    }
    // constructor - takes three floats for lightness and color-opponent dimensions
    public HSLColor(float x, float y, float z)
    {
        this.H = x;
        this.S = y;
        this.L = z;
    }

    // constructor - takes a Color
    public HSLColor(Color col)
    {
        HSLColor temp = FromColor(col);
        H = temp.x;
        S = temp.y;
        L = temp.z;
    }
    // static function for linear interpolation between two HSLColors
    public static HSLColor Lerp(HSLColor a, HSLColor b, float t)
    {
        return new HSLColor(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
    }

    // static function for interpolation between two Unity Colors through normalized colorspace
    public static Color Lerp(Color a, Color b, float t)
    {
        return (HSLColor.Lerp(HSLColor.FromColor(a), HSLColor.FromColor(b), t)).ToColor();
    }

    // static function for returning the color difference in a normalized colorspace (Delta-E)
    public static float Distance(HSLColor a, HSLColor b)
    {
        return Mathf.Sqrt(Mathf.Pow((a.x - b.x), 2f) + Mathf.Pow((a.y - b.y), 2f) + Mathf.Pow((a.z - b.z), 2f));
    }

    public static HSLColor FromColor(Color RGB)
    {
        float r = (RGB.r) / 225;
        float g = (RGB.g) / 225;
        float b = (RGB.b) / 225;

        HSLColor HSL = new HSLColor(0f, 0f, 0f);

        float Min = Mathf.Min(r, Mathf.Min(g, b));
        float Max = Mathf.Max(r, Mathf.Max(g, b));

        float D = Max - Min;
        HSL.z = (Max + Min) / 2;
        

        if (D == 0)//This is a gray, no chroma...
        {
            HSL.x = 0;
            HSL.y = 0;
        }
        else
        {
            if (HSL.z < 0.5)
            {
                HSL.y = D / (Max + Min);
            }
            else
            {
                HSL.y = D / (2 - Max - Min);
            }


            float R = (((Max - r) / 6) + (D / 2)) / D;
            float G = (((Max - g) / 6) + (D / 2)) / D;
            float B = (((Max - b) / 6) + (D / 2)) / D;

            if (r == Max)
            {
                HSL.x = B - G;
            }
            else if (g == Max)
            {
                HSL.x = (1 / 3) + R - B;

            }
            else if (b == Max) {

                HSL.x = (2 / 3) + G - R;
            }



            if (HSL.x < 0)
            {
                HSL.x += 1;
             }
            if (HSL.x > 1)
            {
                HSL.x -= 1;
            }


        }



        return HSL;
    }

    public static Color ToColor(HSLColor HSL)
    {
        Color RGB = new Color(0f, 0f, 0f);
        float var2;

        if (HSL.y == 0)
        {
            RGB.r = HSL.z * 255;
            RGB.g = HSL.z * 255;
            RGB.b = HSL.z * 255;
        }
        else
        {
            if (HSL.z < 0.5)
            {
               var2 = HSL.z * (1 + HSL.y);
            }
            else 
            {
                var2 = (HSL.z+ HSL.y) - (HSL.y * HSL.z);

            }

            float var1 = 2 * HSL.z - var2;


            RGB.r = 255 * Hue_2_RGB(var1, var2, HSL.x + (1 / 3));
            RGB.g = 255 * Hue_2_RGB(var1, var2, HSL.x);
            RGB.b = 255 * Hue_2_RGB(var1, var2, HSL.x - (1 / 3));

        }

        return RGB;
    }
    // function for converting an instance of HSLColor to Color
    public Color ToColor()
    {
        return HSLColor.ToColor(this);
    }

    // override for string
    public override string ToString()
    {
        return "H:" + x + " C:" + y + " L:" + z;
    }

    // are two HSLColors the same?
    public override bool Equals(System.Object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        return (this == (HSLColor)obj);
    }

    // override hashcode for a HSLColor
    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

    // Equality operator
    public static bool operator ==(HSLColor item1, HSLColor item2)
    {
        return (item1.x == item2.x && item1.y == item2.y && item1.z == item2.z);
    }

    // Inequality operator
    public static bool operator !=(HSLColor item1, HSLColor item2)
    {
        return (item1.x != item2.x || item1.y != item2.y || item1.z != item2.z);
    }

    static float Hue_2_RGB(float v1, float v2, float vH)//Function Hue_2_RGB
    {
        if (vH < 0)
        {
            vH += 1;
        }
        if (vH > 1)
        {
            vH -= 1;
        }

        if ((6 * vH) < 1)
        {

            return (v1 + (v2 - v1) * 6 * vH);
        }
        if ((2 * vH) < 1)
        {
            return (v2);

        }
        if ((3 * vH) < 2)
        {

            return (v1 + (v2 - v1) * ((2 / 3) - vH) * 6);
        }
        else
        {
            return (v1);
        }
    }
}


