using Color = System.Drawing.Color;
using Gdk;
using System.Runtime.InteropServices;

namespace Lucas.SubtitleEdit.GdkExpansions.PixbufExtensions
{
    // TODO: Use FastBitmap to better perfomace?
    public static class PixbufExtensions
    {
        public static byte[] GetPixelData(this Pixbuf pixbuf, int x, int y)
        {
            byte[] pixels = pixbuf.ReadPixelBytes().Data;
            byte index0 = pixels[(x * 4) + (y * pixbuf.Width * 4)];

            return new byte[4] // pixiel format can be RGBA or RGB. this extensions only support RGBA
            {
                pixels[index0],
                pixels[index0 + 1],
                pixels[index0 + 2],
                pixels[index0 + 3]
            };
        }

        public static Color GetPixel(this Pixbuf pixbuf, int x, int y)
        {
            return GetPixelColor(pixbuf, x, y);
        }

        public static Color GetPixelColor(this Pixbuf pixbuf, int x, int y)
        {
            byte[] pixel = GetPixelData(pixbuf, x, y);
            return Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
        }
 
        public static void MakeTransparent(this Pixbuf pixbuf)
        {
            MakeTransparent(pixbuf, GetPixelColor(pixbuf, 0, pixbuf.Height - 1));
            // Note: on mono runtime is the first pixel instead last left down pixel
        }

        public static void MakeTransparent(this Pixbuf pixbuf, Color color)
        {
            MakeTransparent(pixbuf, color.R, color.G, color.B, color.A);
        }

        public static void MakeTransparent(this Pixbuf pixbuf, byte[] data)
        {
            MakeTransparent(pixbuf, data[0], data[1], data[2], data[3]);
        }

        public static void MakeTransparent(this Pixbuf pixbuf, byte r, byte g, byte b, byte a)
        {
            byte[] pixels = pixbuf.ReadPixelBytes().Data;
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i++] == r && pixels[i++] == g && pixels[i++] == b && pixels[i] == a)
                {
                    pixels[i] = 0;
                }
            }

            Marshal.Copy(pixels, 0, pixbuf.Pixels, pixels.Length);
        }
    }
}
