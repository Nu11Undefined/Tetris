using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Tetris
{
    public class Figure
    {
        static readonly Color[] Colors = new Color[]
            {
                Color.Maroon,
                Color.Brown,
                Color.Purple,
                Color.Silver,
                Color.Navy,
                Color.DarkGreen,
                Color.Teal,

                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.DeepPink,
                Color.LightBlue
            };
        // кисти для отрисовки фигур
        public static readonly SolidBrush[,] Brushes;
        // контуры заливок внутри фигуры
        public static readonly GraphicsPath[] Pathes;
        static readonly double sideK = 0.33;
        static Figure()
        {
            Brushes = new SolidBrush[Colors.Length,5];
            Pathes = new GraphicsPath[4];
            double angle = Math.PI / 2;
            double side1 = Math.Sqrt(2)/2, side2 = Math.Sqrt(2) * (1 - sideK)/2;
            for(int i = 0;i<4;i++, angle += Math.PI/2)
            {
                Pathes[i] = new GraphicsPath();
                Pathes[i].AddPolygon(new PointF[]
                    {
                        new PointF((float)(0.5 + side2 * Math.Cos(angle - Math.PI / 4)), (float)(0.5 + side2 * Math.Sin(angle - Math.PI / 4))),
                        new PointF((float)(0.5 + side1 * Math.Cos(angle - Math.PI / 4)), (float)(0.5 + side1 * Math.Sin(angle - Math.PI / 4))),
                        new PointF((float)(0.5 + side1 * Math.Cos(angle + Math.PI / 4)), (float)(0.5F + side1 * Math.Sin(angle + Math.PI / 4))),
                        new PointF((float)(0.5 + side2 * Math.Cos(angle + Math.PI / 4)), (float)(0.5F + side2 * Math.Sin(angle + Math.PI / 4)))
                    });
            }
            for(int i = 0;i<Colors.Length;i++)
            {
                Brushes[i, 0] = new SolidBrush(Colors[i]);
                Brushes[i, 2] = new SolidBrush(Ext.Interpolate(Colors[i], Color.Black, 0.05));
                Brushes[i, 1] = new SolidBrush(Ext.Interpolate(Colors[i], Color.White, 0.15));
                Brushes[i, 4] = new SolidBrush(Ext.Interpolate(Colors[i], Color.Black, 0.2));
                Brushes[i, 3] = new SolidBrush(Ext.Interpolate(Colors[i], Color.Black, 0.35));
            }
        }
        public static int TypesCount
        {
            get
            {
                return F.Count;
            }
        }
        static readonly List<int[,]> F = new List<int[,]>()
        {
            // I
            new int [,]
            {
                {1,1,1,1 }
            },
            // T
            new int [,]
            {
                {1,1,1 },
                {0,1,0 }
            },
            // L
            new int [,]
            {
                {1,0,0 },
                {1,1,1 }
            },
            // J
            new int [,]
            {
                {0,0,1 },
                {1,1,1 }
            },
            // O
            new int [,]
            {
                {1,1 },
                {1,1 }
            },
            // S
            new int [,]
            {
                {0,1,1 },
                {1,1,0 }
            },
            // Z
            new int [,]
            {
                {1,1,0 },
                {0,1,1 }
            },

            new int[,]
            {
                {1 }
            },
            new int [,]
            {
                {1,1 }
            },
            new int [,]
            {
                {1,1,1 }
            },
            new int [,]
            {
                {1,0 },
                {1,1 }
            },
            new int [,]
            {
                {0,1 },
                {1,1 } },
        };
        public double X { get; set; }
        public double Y { get; set; }
        public int XIndex
        {
            get
            {
                return (int)Math.Floor(X);
            }
        }
        public int YIndex
        {
            get
            {
                return (int)Math.Floor(Y);
            }
        }
        public int Type { get; set; }
        private int[,] _blocks;
        public int Width
        {
            get
            {
                return _blocks.GetLength(1);
            }
        }
        public int Height
        {
            get
            {
                return _blocks.GetLength(0);
            }
        }
        public int this[int x, int y]
        {
            get
            {
                return _blocks[y, x];
            }
        }
        public Figure(FigureType type, double x, double y)
        {
            Type = (int)type;
            X = x;
            Y = y;
            _blocks = new int[F[Type].GetLength(0), F[Type].GetLength(1)];
            Array.Copy(F[Type], _blocks, F[Type].Length);
        }
        /// <summary>
        /// Повернуть фигуру по часовой стрелке
        /// </summary>
        public void RotateClockWise()
        {
            _blocks = _blocks.RotateClockWise();
        }
        /// <summary>
        /// Повернуть фигуру против часовой стрелки
        /// </summary>
        public void RotateCounterClockWise()
        {
            _blocks = _blocks.RotateCounterClockWise();
        }
        /// <summary>
        /// Корректно ли расположена фигура
        /// </summary>
        /// <param name="cells">Данные о ячейках</param>
        public bool IsCorrect(int [,] cells)
        {
            int x0 = (int)X, y0 = YIndex;
            // проверка на выход за границы
            if (x0 < 0 || x0 + Width > cells.GetLength(1) || y0 + Height > cells.GetLength(0)) return false;
            // проверка на коллизии
            for(int i = 0;i<Height;i++)
            {
                for(int j = 0;j<Width;j++)
                {
                    // если ячейка занята и также в ней есть блок фигуры
                    if (cells[i + y0, j + x0] > -1 && this[j,i]==1) return false;
                }
            }
            return true;
        }
    }
    public enum FigureType
    {
        I,
        T,
        L,
        J,
        O,
        S,
        Z,

        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10
    }
}
