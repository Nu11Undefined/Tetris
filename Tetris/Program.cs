using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            int k = 2;
            Game game = new Game(18, 32, new Size(1080 / k, 1920 / k), 255);
        }
    }
}
