using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Tetris
{
    public static class GraphicsExtension
    {
        static Font font;
        static SolidBrush ShadowBrush { get; set; } = new SolidBrush(Color.FromArgb(64, Color.White));
        public static void DrawTetris(this Graphics g, Tetris s, RectangleF rect)
        {
            if (font==null) font = new Font("Consolas", rect.Height/s.Height, GraphicsUnit.Pixel);
            float dx = rect.Width / s.Width, dy = rect.Height / s.Height;
            // тень фигуры
            g.FillRectangle(ShadowBrush, s.Figures.XIndex * dx, s.Figures.YIndex * dy, dx * s.Figures.Width, dy * (s.Height - s.Figures.YIndex));
            // фигура 
            for (int y = 0; y < s.Figures.Height; y++)
            {
                for (int x = 0; x < s.Figures.Width; x++)
                {
                    if (s.Figures[x, y] > 0)
                        g.DrawCell(s.Figures.Type, s.Figures.XIndex + x, s.Figures.YIndex + y, dx, dy);
                }
            }
            // ячейки
            for (int y = 0;y<s.Height;y++)
            {
                for(int x = 0;x<s.Width;x++)
                {
                    if (s[x, y] > -1)
                        g.DrawCell(s[x, y], x, y, dx, dy);
                    //g.DrawString($"{x}, {y}", font, Brushes.White, x * dx, y * dy + dy/2);
                }
            }
            // уровень, количество очков
            g.DrawString($"{s.Level} ({s.CurrentRowCount}/{s.LevelRowCount})\n{s.Score}", font, Brushes.White, 10, 10);
        }
        private static void DrawCell(this Graphics g, int color, float x, float y, float dx, float dy)
        {
            g.ScaleTransform(dx, dy);
            g.TranslateTransform(x, y);
            g.FillRectangle(Figure.Brushes[color, 4], 0, 0, 1, 1);
            for(int i = 0;i<Figure.Pathes.Length;i++)
            {
                g.FillPath(Figure.Brushes[color, i], Figure.Pathes[i]);
            }
            g.ResetTransform();
        }
        public static Color Interpolate(Color c1, Color c2, double k)
        {
            return Color.FromArgb(
                (byte)(c1.A + k * (c2.A - c1.A)),
                (byte)(c1.R + k * (c2.R - c1.R)),
                (byte)(c1.G + k * (c2.G - c1.G)),
                (byte)(c1.B + k * (c2.B - c1.B))
                );
        }
        public static void SetAlphaChannel(this Bitmap bmp, byte a)
        {
            BitmapData bd = bmp.LockBits(bmp.GetRect(), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int count = bd.Stride * bd.Height;
            for(int i = 3;i<count;i+=4)
            {
                Marshal.WriteByte(bd.Scan0, i, a);
            }
            bmp.UnlockBits(bd);
        }
        public static Rectangle GetRect(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }
    }
}
