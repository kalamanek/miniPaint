using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPaintWektorowo
{
    class Painting
    {
        private Int32 wysokosc;
        private Int32 szerokosc;
        private List<Form> figury;
        private List<Form> figuryUsuniete;
        private Color kolorTla;
        private Image imageFile = null;

        public Painting(int szerokosc, int wysokosc, Color kolorTla)
        {
            this.szerokosc = szerokosc;
            this.wysokosc = wysokosc;
            this.kolorTla = kolorTla;
            figury = new List<Form>();
            figuryUsuniete = new List<Form>();
        }
        public Painting(Image imageFile) {
            this.szerokosc = imageFile.Width;
            this.wysokosc = imageFile.Height;
            this.imageFile = imageFile;
            figury = new List<Form>();
            figuryUsuniete = new List<Form>();
        }

        public void Paint(Graphics g)
        {
            if (g != null)
            {
                g.Clear(kolorTla);

                if (imageFile != null) {
                    g.DrawImage(imageFile, new Point(0,0));
                }
                

                foreach (Form f in figury)
                {
                    f.Rysuj(g);
                }
            }
        }

        internal void Add(Form f)
        {
            figury.Add(f);
            figuryUsuniete.Clear();
        }

        internal void Usun(Graphics g)
        {
            if (figury.Any())
            {
                figuryUsuniete.Add(figury.Last());
                figury.RemoveAt(figury.Count - 1);
            }
            Paint(g);
        }

        internal void Ponow(Graphics g)
        {
            if (figuryUsuniete.Any())
            {
                figury.Add(figuryUsuniete.Last());
                figuryUsuniete.RemoveAt(figuryUsuniete.Count - 1);
            }
            Paint(g);
        }


    }
}
