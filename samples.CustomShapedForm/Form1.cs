using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace samples.CustomShapedForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var bitmap = new Bitmap("ninja.png");
            Region = GetRegion(bitmap);
        }

        private Region GetRegion(Bitmap _img)
        {
            var rgn = new Region();
            rgn.MakeEmpty();
            var rc = new Rectangle(0, 0, 0, 0);
            bool inimage = false;
            for (int y = 0; y < _img.Height; y++)
            {
                for (int x = 0; x < _img.Width; x++)
                {
                    if (!inimage)
                    {
                        // if pixel is not transparent
                        if (_img.GetPixel(x, y).A != 0)
                        {
                            inimage = true;
                            rc.X = x;
                            rc.Y = y;
                            rc.Height = 1;
                        }
                    }
                    else
                    {
                        // if pixel is transparent
                        if (_img.GetPixel(x, y).A == 0)
                        {
                            inimage = false;
                            rc.Width = x - rc.X;
                            rgn.Union(rc);
                        }
                    }
                }
                if (inimage)
                {
                    inimage = false;
                    rc.Width = _img.Width - rc.X;
                    rgn.Union(rc);
                }
            }
            return rgn;
        }

        private bool _mouseDown;
        private Point _startPoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _startPoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var p1 = new Point(e.X, e.Y);
                var p2 = PointToScreen(p1);
                var p3 = new Point(p2.X - _startPoint.X,
                                     p2.Y - _startPoint.Y);
                Location = p3;
            }
        }

        private void uxClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
