using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Tetris
{
    public static class Ext
    {
        public static double NextD(this Random rand, double min, double max)
        {
            return min + (max - min) * rand.NextDouble();
        }
        /// <summary>
        /// Возвращает массив, повернутый по часовой стрелке
        /// </summary>
        public static T[,] RotateClockWise<T>(this T[,] a)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            T[,] b = new T[w, h];
            for(int i = 0;i<h;i++)
            {
                for(int j = 0;j<w;j++)
                {
                    b[j, h - i - 1] = a[i, j];
                }
            }
            return b;
        }
        /// <summary>
        /// Возвращает массив, повернутый против часовой стрелки
        /// </summary>
        public static T[,] RotateCounterClockWise<T>(this T[,] a)
        {
            int h = a.GetLength(0), w = a.GetLength(1);
            T[,] b = new T[w, h];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    b[w - j - 1, i] = a[i, j];
                }
            }
            return b;
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
    }
}
