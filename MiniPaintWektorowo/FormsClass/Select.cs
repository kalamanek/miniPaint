using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPaintWektorowo
{
    public class Select : UnfilledForm
    {
        Point p;
        public Select(Color kolorLinii, int gruboscLinii, Point p1, Point p2) : base(kolorLinii, gruboscLinii, p1)
        {
            this.p = p2;
        }

        public override void Rysuj(Graphics g)
        {
            Pen pen = new Pen(kolorLinii, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(pen , Math.Min(polozenie.X, p.X),
                                                               Math.Min(polozenie.Y, p.Y),
                                                               Math.Abs(polozenie.X - p.X),
                                                               Math.Abs(polozenie.Y - p.Y));
        }
    }
}
