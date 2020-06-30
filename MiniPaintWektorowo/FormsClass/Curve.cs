using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiniPaintWektorowo
{
    public class Curve : UnfilledForm
    {
        private List<Point> pp;

        public Curve(Color kolorLinii, Int32 gruboscLinii, List<Point> pp)
            : base(kolorLinii, gruboscLinii, pp[0])
        {
            this.pp = new List<Point>(pp);
            this.pp.RemoveAt(0);
        }
        public override void Rysuj(Graphics g)
        {
            Pen pen = new Pen(kolorLinii, gruboscLinii)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            g.DrawLine(pen, polozenie, pp[0]);
            for (int i=0; i<pp.Count-1; i++)
            {
                g.DrawLine(pen, pp[i], pp[i+1]);
            }
        }
    }
}