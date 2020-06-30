using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPaintWektorowo
{
    public class Text : UnfilledForm
    {
        string text;
        Font font;
        public Text(Color kolorLinii, int gruboscLinii, Point polozenie, string text, Font font) : base(kolorLinii, gruboscLinii, polozenie)
        {
            this.text = text;
            this.font = font;
        }

        public override void Rysuj(Graphics g)
        {
            SolidBrush drawBrush = new SolidBrush(kolorLinii);
            StringFormat drawFormat = new StringFormat();
            g.DrawString(text, font, drawBrush, polozenie.X, polozenie.Y, drawFormat);
            drawBrush.Dispose();
        }
    }
}
