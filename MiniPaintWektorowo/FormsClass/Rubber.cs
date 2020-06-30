using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPaintWektorowo
{
    public class Rubber : UnfilledForm
    {
        private List<Point> pp;

        public Rubber(Color kolorLinii, Int32 gruboscLinii, List<Point> pp)
            : base(kolorLinii, gruboscLinii, pp[0])
        {
            this.pp = new List<Point>(pp);
            this.pp.RemoveAt(0);
        }
        public override void Rysuj(Graphics g)
        {
            Pen pen = new Pen(kolorLinii, gruboscLinii)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Square,
                EndCap = System.Drawing.Drawing2D.LineCap.Square
            };
            g.DrawLine(pen, polozenie, pp[0]);
            for (int i = 0; i < pp.Count - 1; i++)
            {
                g.DrawLine(pen, pp[i], pp[i + 1]);
            }
        }
    }
}
