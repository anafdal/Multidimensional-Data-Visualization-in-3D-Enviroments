using UnityEngine;
[System.Serializable]



public struct HSLColor
 {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        public float hue;
        public float saturation;
        public float luminosity;

        private const float scale = 240.0f;

        public float Hue
        {
            get { return hue * scale; }
            set { hue = CheckRange(value / scale); }
        }
        public float Saturation
        {
            get { return saturation * scale; }
            set { saturation = CheckRange(value / scale); }
        }
        public float Luminosity
        {
            get { return luminosity * scale; }
            set { luminosity = CheckRange(value / scale); }
        }

      public HSLColor(float hue, float saturation, float luminosity)
      {
        this.hue = hue;
        this.saturation = saturation;
        this.luminosity = luminosity;

      }

    private float CheckRange(float value)
        {
            if (value < 0.0)
                value = 0.0f;
            else if (value > 1.0)
                value = 1.0f;
            return value;
        }


        public  HSLColor SetRGB(float red, float green, float blue)
        {
            HSLColor hslColor = new HSLColor (red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;

            return hslColor;
        }
        public void GetHSLColor(Color color)
        {
            SetRGB(color.r, color.g, color.b);
        }
     
        

 }


