using Nikse.SubtitleEdit.Core.Common;
using System;
using System.Collections.Generic;
using Color = System.Drawing.Color;
using Gdk;
using Cairo;
using Lucas.SubtitleEdit.SysDrawExpansions;

namespace Nikse.SubtitleEdit.Core.ContainerFormats
{
    public class XSub
    {
        public TimeCode Start { get; set; }
        public TimeCode End { get; set; }
        public int Width { get; }
        public int Height { get; }

        private readonly byte[] _colorBuffer;
        private readonly byte[] _rleBuffer;

        public XSub(string timeCode, int width, int height, byte[] colors, byte[] rle)
        {
            Start = DecodeTimeCode(timeCode.Substring(0, 13));
            End = DecodeTimeCode(timeCode.Substring(13, 12));
            Width = width;
            Height = height;
            _colorBuffer = colors;
            _rleBuffer = rle;
        }

        private static TimeCode DecodeTimeCode(string timeCode)
        {
            var parts = timeCode.Split(new[] { ':', ';', '.', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            return new TimeCode(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
        }

        private static void GenerateBitmap(FastBitmap bmp, byte[] buf, List<Color> fourColors)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int nibbleOffset = 0;
            var nibbleEnd = buf.Length * 2;
            var x = 0;
            var y = 0;
            for (; ; )
            {
                if (nibbleOffset >= nibbleEnd)
                {
                    return;
                }

                var v = GetNibble(buf, nibbleOffset++);
                if (v < 0x4)
                {
                    v = (v << 4) | GetNibble(buf, nibbleOffset++);
                    if (v < 0x10)
                    {
                        v = (v << 4) | GetNibble(buf, nibbleOffset++);
                        if (v < 0x040)
                        {
                            v = (v << 4) | GetNibble(buf, nibbleOffset++);
                            if (v < 4)
                            {
                                v |= (w - x) << 2;
                            }
                        }
                    }
                }

                var len = v >> 2;
                if (len > w - x)
                {
                    len = w - x;
                }

                var color = v & 0x03;
                if (color > 0)
                {
                    var c = fourColors[color];
                    bmp.SetPixel(x, y, c, len);
                }

                x += len;
                if (x >= w)
                {
                    y++;
                    if (y >= h)
                    {
                        break;
                    }

                    x = 0;
                    nibbleOffset += (nibbleOffset & 1);
                }
            }
        }

        private static int GetNibble(byte[] buf, int nibbleOffset)
        {
            return (buf[nibbleOffset >> 1] >> ((1 - (nibbleOffset & 1)) << 2)) & 0xf;
        }

        public Pixbuf GetImage(Color background, Color pattern, Color emphasis1, Color emphasis2)
        {
            var fourColors = new List<Color> { background, pattern, emphasis1, emphasis2 };
            var bmp = new Pixbuf(Colorspace.Rgb, true, 8, Width, Height);
            if (fourColors[0] != Color.Transparent)
            {
                using (Surface surface = CairoHelper.SurfaceCreateFromPixbuf(bmp, 1, null))
                using (Context context = new Context(surface))
                {
                    context.SetSourceColor(fourColors[0].ToCairo());
                    context.Rectangle(new Cairo.Rectangle(0, 0, bmp.Width, bmp.Height));
                    context.Fill();
                }
            }
            var fastBmp = new FastBitmap(bmp);
            // fastBmp.LockImage();
            GenerateBitmap(fastBmp, _rleBuffer, fourColors);
            // fastBmp.UnlockImage();
            return bmp;
        }

        private Color GetColor(int start)
        {
            return Color.FromArgb(_colorBuffer[start], _colorBuffer[start + 1], _colorBuffer[start + 2]);
        }

        public Pixbuf GetImage()
        {
            return GetImage(Color.Transparent, GetColor(3), GetColor(6), GetColor(9));
        }
    }
}
