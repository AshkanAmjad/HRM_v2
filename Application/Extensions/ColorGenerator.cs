using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class ColorGenerator
    {
        private static Random random = new Random();

        public static string GenerateRandomColorHex()
        {
            Color color;
            do
            {
                int hue = random.Next(0, 361);
                int saturation = random.Next(50, 101);
                int lightness = random.Next(50, 101);

                color = HslToRgb(hue, saturation, lightness);
            }
            while (color.R == 255 && color.G == 255 && color.B == 255); 

            string hexColor = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            return hexColor;
        }

        public static Color HslToRgb(int hue, int saturation, int lightness)
        {
            float h = hue / 360f;
            float s = saturation / 100f;
            float l = lightness / 100f;

            float q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            float p = 2 * l - q;

            float r = CalculateColorComponent(p, q, h + 1 / 3f);
            float g = CalculateColorComponent(p, q, h);
            float b = CalculateColorComponent(p, q, h - 1 / 3f);

            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static float CalculateColorComponent(float p, float q, float t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1 / 6f) return p + (q - p) * 6 * t;
            if (t < 1 / 2f) return q;
            if (t < 2 / 3f) return p + (q - p) * 6 * (2 / 3f - t);
            return p;
        }

        public static List<string> GenerateMultipleRandomColorHex(int count)
        {
            List<string> colors = new();

            for (int i = 0; i < count; i++)
            {
                colors.Add(GenerateRandomColorHex());
            }

            return colors;
        }
    }
}
