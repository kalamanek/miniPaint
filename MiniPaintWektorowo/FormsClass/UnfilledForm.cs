using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiniPaintWektorowo
{
    
    public abstract class UnfilledForm : Form
    {
        public UnfilledForm(Color kolorLinii, Int32 gruboscLinii, Point polozenie)
            : base(kolorLinii, gruboscLinii, polozenie)
        {
        }
    }
}