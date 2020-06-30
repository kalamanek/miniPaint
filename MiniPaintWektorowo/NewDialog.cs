using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniPaintWektorowo {
    public partial class NewDialog : System.Windows.Forms.Form {
        public NewDialog() {
            InitializeComponent();
        }
        private void colorBox_Click(object sender, EventArgs e) {

            ColorDialog kolorOkno = new ColorDialog();
            if (kolorOkno.ShowDialog() == DialogResult.OK) {
                colorBox.BackColor = kolorOkno.Color;
            }
        }

        public Size ImageSize {
            get { return new Size((int)widthBox.Value, (int)heightBox.Value); }
        }


        public Color imageBackColor {
            get { return colorBox.BackColor; }
        }
    }
}
