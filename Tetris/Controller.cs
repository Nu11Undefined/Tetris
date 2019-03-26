using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    /// <summary>
    /// Компонент управления игрой через события в окне
    /// </summary>
    public class Controller
    {
        public delegate void GameEventHandler();
        /// <summary>
        /// Смещение влево
        /// </summary>
        public event GameEventHandler Left;
        /// <summary>
        /// Смещение вправо
        /// </summary>
        public event GameEventHandler Right;
        /// <summary>
        /// Смещение вниз (ускоренное падение)
        /// </summary>
        public event GameEventHandler Down;
        public event GameEventHandler Rotate;
        public event GameEventHandler Pause;
        public event GameEventHandler Close;
        /// <summary>
        /// Экземпляр контроллера
        /// </summary>
        /// <param name="win">Окно, события которого будут прослушиваться</param>
        public Controller(Form win)
        {
            win.KeyDown += (s, e) =>
            {
                switch(e.KeyCode)
                {
                    case Keys.A:
                    case Keys.Left:
                        Left?.Invoke();
                        break;
                    case Keys.D:
                    case Keys.Right:
                        Right?.Invoke();
                        break;
                    case Keys.S:
                    case Keys.Down:
                        Down?.Invoke();
                        break;
                    case Keys.Space:
                        Pause?.Invoke();
                        break;
                    case Keys.Escape:
                        Close?.Invoke();
                        break;

                }
            };
            win.MouseDown += (s, e) =>
            {
                switch(e.Button)
                {
                    case MouseButtons.Right:
                        Rotate?.Invoke();
                        break;
                }
            };
        }
    }
}
