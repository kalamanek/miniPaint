using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiniPaintWektorowo
{
    
    public abstract class FilledForm : Form
    {
        protected Color kolorWypelnienia;
        public FilledForm(Color kolorLinii, Int32 gruboscLinii, Point polozenie, Color kolorWypelnienia)
            : base(kolorLinii, gruboscLinii, polozenie)
        {
            this.kolorWypelnienia = kolorWypelnienia;
        }
    }
}