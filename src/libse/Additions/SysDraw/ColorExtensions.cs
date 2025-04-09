using System;
using Color = System.Drawing.Color;

namespace Lucas.SubtitleEdit.SysDrawExpansions
{
    public static class ColorExtensions
    {
        public static uint ToUintRGBA(this Color color)
        {
            return (uint)color.R << 24 | (uint)color.G << 16 | (uint)color.B << 8 | color.A;
        }

        public static Cairo.Color ToCairo(this Color color)
        {
            double[] values = RGBA_0to1(color);
            return new Cairo.Color(values[0], values[1], values[2], values[3]);
        }

        public static Gdk.RGBA ToGdkRGBA(this Color color)
        {
            double[] values = RGBA_0to1(color);
            return new Gdk.RGBA() { Red = values[0], Green = values[1], Blue = values[2], Alpha = values[3] };
        }

        public static double[] RGBA_0to1(this Color color)
        {
            return new double[4]
            {
                Math.Round((double)color.R * byte.MaxValue),
                Math.Round((double)color.G * byte.MaxValue),
                Math.Round((double)color.B * byte.MaxValue),
                Math.Round((double)color.A * byte.MaxValue)
            };
        }

        public static Pango.Color ToPango(this Color color)
        {
            return new Pango.Color()
            {
                Red = (ushort)(color.R * 257),
                Green = (ushort)(color.G * 257),
                Blue = (ushort)(color.B * 257)
            };
        }
    }
}