using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    /// <summary>
    /// Класс игры
    /// </summary>
    public class Tetris
    {
        static Random rand = new Random(1);
        /// <summary>
        /// Количество ячеек в ширину
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Количество ячеек в высоту
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Тип ячейки
        /// </summary>
        int[,] cells;
        public int this[int x, int y]
        {
            get
            {
                return cells[y, x];
            }
            set
            {
                cells[y, x] = value;
            }
        }
        /// <summary>
        /// Скорость перемещения фигуры
        /// </summary>
        public float Velocity { get; set; }
        /// <summary>
        /// Увеличение скорости с каждым новым уровнем
        /// </summary>
        public static float VelocityK { get; set; } = 1.5F;
        /// <summary>
        /// Текущая фигура
        /// </summary>
        public Figure Figures { get; set; }
        public int Level { get; set; } = 1;
        public int Score { get; set; }
        /// <summary>
        /// Количество заполненных рядов для перехода на следующий уровень
        /// </summary>
        public int LevelRowCount { get; set; }
        /// <summary>
        /// Количество заполненных рядов на текущем уровне
        /// </summary>
        public int CurrentRowCount { get; set; }
        public Tetris(int w, int h, float v)
        {
            Width = w;
            Height = h;
            LevelRowCount = Height / 2;
            Velocity = v;
            cells = new int[h, w];
            ClearCells();
            CreateNewFigure();
        }
        // освободить все ячейки
        public void ClearCells()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j] = -1;
                }
            }
        }
        private bool IsCorrect()
        {
            return Figures.IsCorrect(cells);
        }
        public delegate void ModelChangedHandler();
        // изменение ячейки фигуры
        public event ModelChangedHandler FigureCellChanged;
        // сместить фигуру вниз
        public void ShiftDown()
        {
            // смещать до тех пор, пока на пути не будет преграды
            while (IsCorrect()) Figures.Y += Velocity;
            StopFigure();
            FigureCellChanged?.Invoke();
        }
        private void StopFigure()
        {
            // сместить фигуру назад
            Figures.Y -= 1;
            FigureToCells();
            CreateNewFigure();
        }
        public void NextFrame()
        {
            int prevY = Figures.YIndex;
            Figures.Y += Velocity;
            // если изменилась высота начальной ячейки фигуры
            if (Figures.YIndex != prevY)
            {
                // если фигура пересекла заполненные ячейки
                if (!IsCorrect())
                {
                    StopFigure();
                }
                FigureCellChanged?.Invoke();
            }
            CheckRows();
        }
        /// <summary>
        /// Проверить ряды на заполненность
        /// </summary>
        private void CheckRows()
        {
            // пройти по всем рядам снизу вверх
            for(int y = Height -1, fullCount = 0;y>=0;y--, fullCount++)
            {
                bool isFull = true;
                for(int x = 0;x<Width;x++)
                {
                    if (this[x,y] == -1)
                    {
                        isFull = false;
                        break;
                    }
                }
                // если предыдущий ряд заполнен, сместить вниз на 1 все, что находится выше него
                if (!isFull)
                { 
                    if (fullCount > 0)
                    {
                        DeleteRow(y + fullCount, fullCount);
                        y += fullCount; // оставить счетчик на месте
                    }
                    fullCount = -1;
                }
            }
        }
        /// <summary>
        /// Увеличить количество очков
        /// </summary>
        /// <param name="y">Ряд, для которого считаются очки</param>
        /// <param name="count">Количество рядов</param>
        public void RaiseScore(int y, int count)
        {
            // увеличить счетчик заполненных рядов
            CurrentRowCount += count;
            // если количество заполненных рядов достаточно для нового уровня
            if (CurrentRowCount>=LevelRowCount)
            {
                Level++;
                Velocity *= VelocityK;
                // записать избыток в счет нового уровня
                CurrentRowCount %= LevelRowCount;
            }
            int[] countT = new int[Figure.TypesCount];
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < countT.Length; i++) countT[i] = 0;
                for (int i = 0; i < Width; i++)
                {
                    countT[this[i, y - j]]++;
                }
                /* 
                 * увеличить количество очков пропорционально
                 * - count - количество заполненных рядов
                 * - countT[i] - количество одноцветных ячеек
                 * - Level - текущий уровень
                */
                for (int i = 0; i < countT.Length; i++) Score += count * countT[i] * countT[i] * Level;
            }
        }
        /// <summary>
        /// Удалить ряды
        /// </summary>
        /// <param name="y0"></param>
        /// <param name="count">Количество рядов для удаления</param>
        private void DeleteRow(int y0, int count)
        {
            RaiseScore(y0, count);
            for(int y = y0 - count;y>0;y--)
            {
                for(int x = 0;x<Width;x++)
                {
                    this[x, y + count] = this[x, y];
                }
            }
            for (int i = 0; i < count; i++)
            {
                // очистить верхние ряды
                for (int x = 0; x < Width; x++)
                {
                    this[x, i] = -1;
                }
            }
        }
        /// <summary>
        /// Повернуть фигуру, если это возможно
        /// </summary>
        public void RotateFigure()
        {
            Figures.RotateClockWise();
            // если фигура стала располагаться некорректно, повернуть обратно
            if (IsCorrect())
            {
                FigureCellChanged?.Invoke();
                return;
            }
            Figures.RotateCounterClockWise();
        }
        /// <summary>
        /// Переместить фигуру, если это возможно
        /// </summary>
        /// <param name="dir">Направление перемещения</param>
        public void Shift(Direction dir)
        {
            Figures.X += (int)dir;
            // если фигура стала располагаться некорректно, возвратить обратно
            if (IsCorrect())
            {
                FigureCellChanged?.Invoke();
                return;
            }
            Figures.X -= (int)dir;
        }
        // перевод фигуры в неподвижные ячейки
        private void FigureToCells()
        {
            int x0 = Figures.XIndex, y0 = Figures.YIndex;
            for(int y = 0;y<Figures.Height;y++)
            {
                for (int x = 0; x < Figures.Width; x++)
                {
                    if (Figures[x, y] > 0) cells[y + y0, x + x0] = Figures.Type;
                }
            }
        }
        private void CreateNewFigure()
        {
            Figures = new Figure(
                (FigureType)
                rand.Next(0, Figure.TypesCount)
                //Figure.TypesCount-1
                , Width / 2, 0);
        }
    }
    /// <summary>
    /// Направление смещения
    /// </summary>
    public enum Direction: int { Left = - 1, Right = 1}
}
