using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Multimedia;

namespace Tetris
{
    /// <summary>
    /// Совокупность модели, ее управления и отображения
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Модель объектов
        /// </summary>
        public Tetris Model { get; set; }
        /// <summary>
        /// Прием информации
        /// </summary>
        public Controller Controller { get; set; }
        /// <summary>
        /// Отображение модели
        /// </summary>
        public RenderWindow Renderer { get; set; }

        Timer timer = new Timer();
        public int FPS { get; set; } = 5;
        byte Alpha { get; set; }
        /// <param name="w">Количество ячеек по ширине</param>
        /// <param name="h">Количество ячеек по высоте</param>
        public Game(int w, int h, Size window, byte alpha)
        {
            Alpha = alpha;
            Model = new Tetris(w, h, 1F/FPS);
            Renderer = new RenderWindow(window.Width, window.Height);
            Controller = new Controller(Renderer);
            InitConnection();
            InitPlay();
        }
        /// <summary>
        /// Инициализировать обмен данными между контроллером и моделью
        /// </summary>
        public void InitConnection()
        {
            Controller.Left += () => Model.Shift(Direction.Left);
            Controller.Right += () => Model.Shift(Direction.Right);
            Controller.Down += () => Model.ShiftDown();
            Controller.Rotate += () => Model.RotateFigure();
            Controller.Pause += () =>
            {
                if (timer.IsRunning) timer.Stop();
                else timer.Start();
            };
            // Отрисовать, если изменилась координата фигуры
            Model.FigureCellChanged += () => Render();
            Controller.Close += () => Close();
        }
        private void Close()
        {
            timer.Dispose();
            Renderer.Close();
        }
        private void Render()
        {
            Renderer.DrawTetris(Model, Alpha);
        }
        public void InitPlay()
        {
            timer.Period = 1000 / FPS;
            timer.Tick += (s, e) => Renderer.BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
            {
                Model.NextFrame();
            }));
            timer.Start();
            Renderer.ShowDialog();
        }
    }
}
