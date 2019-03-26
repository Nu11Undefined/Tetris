using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Tetris
{
    public class RenderWindow : Form
    {
        BufferedGraphicsContext ctx;
        BufferedGraphics buf;
        Graphics g, gBuf;
        Bitmap bmp;
        Bitmap bmpBuf;
        Point MouseDownPoint { get; set; } = new Point(-1, -1);
        public RenderWindow(int w, int h)
        {
            Width = w;
            Height = h;
            bmpBuf = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            gBuf = Graphics.FromImage(bmpBuf);
            FormBorderStyle = FormBorderStyle.None;
            ctx = BufferedGraphicsManager.Current;
            g = CreateGraphics();
            buf = ctx.Allocate(g, ClientRectangle);
            buf.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            bmp = new Bitmap("2.jpg");
            // возможность перемещения окна
            MouseDown += (s, e) => MouseDownPoint = new Point(e.X, e.Y);
            MouseUp += (s, e) => MouseDownPoint = new Point(-1, -1);
            MouseMove += (s, e) =>
            {
                if (MouseDownPoint.X == -1) return;
                else Location = new Point(
                    Location.X + e.X - MouseDownPoint.X, 
                    Location.Y + e.Y - MouseDownPoint.Y);
            };
        }
        public void DrawTetris(Tetris s, byte a)
        {
            buf.Graphics.DrawImage(bmp, 0, 0, ClientSize.Width, ClientSize.Height);
            gBuf.Clear(Color.Transparent);
            gBuf.DrawTetris(s, ClientRectangle);
            //bmpBuf.SetAlphaChannel(a);
            buf.Graphics.DrawImage(bmpBuf, 0, 0, bmpBuf.Width, bmpBuf.Height);
            buf.Render();
        }
    }
}
