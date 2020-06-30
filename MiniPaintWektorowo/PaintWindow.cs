using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniPaintWektorowo
{
    public partial class PaintWindow : System.Windows.Forms.Form
    {
        private string imageFileDirectory = null;
        Graphics g;
        Graphics gp;
        List<Point> workingPoints = new List<Point>();
        Painting painting;
        Size baseSize;
        Font font;
        public PaintWindow()
        {
            InitializeComponent();
            pircureBoxFrame.Image = new Bitmap(pircureBoxFrame.Width, pircureBoxFrame.Height);
            pictureBoxView.Image = new Bitmap(pictureBoxView.Width, pictureBoxView.Height);
            baseSize = pircureBoxFrame.Size;
            pictureBoxView.BackColor = Color.White;
            g = Graphics.FromImage(pircureBoxFrame.Image);
            font = new Font("Arial", 16);

            painting = new Painting(pircureBoxFrame.Width, pircureBoxFrame.Height, Color.White);
            painting.Paint(g);
            pircureBoxFrame.Refresh();

        }

        #region mouseClicks
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                workingPoints.Clear();
                workingPoints.Add(e.Location);

                if (radioButtonTekst.Checked) {
                    groupBoxText.Visible = true;
                    groupBoxShape.Enabled = false;
                    groupBoxText.Location = new Point(e.X, e.Y);
                    Form figura = new Text(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints.First(), textBoxTekst.Text, font);
                    painting.Add(figura);
                } else if (radioButtonWiadro.Checked) {
                    FloodFill(new Bitmap(pircureBoxFrame.Image), e.Location);
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabelCursorPosition.Text = $"X: {e.X} Y: {e.Y}";

            if (e.Button == MouseButtons.Left)
            {
                workingPoints.Add(e.Location);

                Form form = null;
                pictureBoxView.Image = new Bitmap(pircureBoxFrame.Image);
                gp = Graphics.FromImage(pictureBoxView.Image);

                if (radioButtonStraight.Checked)
                {
                    form = new Straight(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints.First(), workingPoints.Last());
                }
                else if (radioButtonCurve.Checked)
                {
                    form = new Curve(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints);
                }
                else if (radioButtonRectangle.Checked)
                {
                    form = new Rectangle(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, buttonKolorWypelnienia.BackColor, workingPoints.First(), workingPoints.Last());
                }
                else if (radioButtonEllipse.Checked)
                {
                    form = new Ellipse(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, buttonKolorWypelnienia.BackColor, workingPoints.First(), workingPoints.Last());
                }
                else if (radioButtonRubber.Checked)
                {
                    form = new Rubber(Color.White, (int)numericUpDownLineThickness.Value, workingPoints);
                }
                else if (radioButtonSelect.Checked)
                {
                    form = new Select(Color.Red, (int)numericUpDownLineThickness.Value, workingPoints.First(), workingPoints.Last());
                }
                if (form != null)
                {
                    form.Rysuj(gp);
                    pictureBoxView.Refresh();
                }
            }
        }

        private void pictureBoxRamka_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                workingPoints.Add(e.Location);


                if (radioButtonStraight.Checked)
                {
                    painting.Add(new Straight(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints.First(), workingPoints.Last()));
                }
                else if (radioButtonCurve.Checked)
                {
                    painting.Add(new Curve(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints));
                }
                else if (radioButtonRectangle.Checked)
                {
                    painting.Add(new Rectangle(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, buttonKolorWypelnienia.BackColor, workingPoints.First(), workingPoints.Last()));
                }
                else if (radioButtonEllipse.Checked)
                {
                    painting.Add(new Ellipse(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, buttonKolorWypelnienia.BackColor, workingPoints.First(), workingPoints.Last()));
                }
                else if (radioButtonRubber.Checked)
                {
                    painting.Add(new Rubber(Color.White, (int)numericUpDownLineThickness.Value, workingPoints));
                }
                else if (radioButtonSelect.Checked)
                {
                    EnableCopyCutMenuItems(true);
                }
                else if (radioButtonKolor.Checked)
                {
                    buttonLoneColor.BackColor = new Bitmap(pircureBoxFrame.Image).GetPixel(e.Location.X, e.Location.Y);
                }

                painting.Paint(g);

                pircureBoxFrame.Refresh();
            }else if (e.Button == MouseButtons.Right) {
                if (radioButtonKolor.Checked) {
                    buttonKolorWypelnienia.BackColor = new Bitmap(pircureBoxFrame.Image).GetPixel(e.Location.X, e.Location.Y);
                }
            }
        }
        #endregion
        #region fileOptions

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e) {
            NewDialog dialog = new NewDialog();

            if (dialog.ShowDialog(this) == DialogResult.OK) {
                baseSize = dialog.ImageSize;
                pircureBoxFrame.Image = new Bitmap(baseSize.Width, baseSize.Height);
                pictureBoxView.Image = new Bitmap(baseSize.Width, baseSize.Height);
                g = Graphics.FromImage(pircureBoxFrame.Image);
                gp = Graphics.FromImage(pictureBoxView.Image);

                painting = new Painting(baseSize.Width, baseSize.Height, dialog.imageBackColor);
                painting.Paint(g);
                painting.Paint(gp);
                pircureBoxFrame.Refresh();
                pictureBoxView.Refresh();
                Text = "MiniPaint wektorowy - niezapisane*";
                imageFileDirectory = null;
            }
        }

        private void fileSaveMenuItem_Click(object sender, EventArgs e) {
            if (imageFileDirectory != null)
                pircureBoxFrame.Image.Save(imageFileDirectory, System.Drawing.Imaging.ImageFormat.Bmp);
            else
                fileSaveAsMenuItem_Click(sender, e);
        }

        private void fileSaveAsMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Bitmap (*.bmp)|*.bmp";
            if (saveDlg.ShowDialog() == DialogResult.OK) {
                pircureBoxFrame.Image.Save(saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                Text = "MiniPaint wektorowy - " + System.IO.Path.GetFileName(saveDlg.FileName);
                imageFileDirectory = saveDlg.FileName;
            }
        }
        private void fileOpenMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Image Files .BMP .JPG .GIF .Png|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openDlg.ShowDialog() == DialogResult.OK) {

                using (FileStream stream = new FileStream(openDlg.FileName, FileMode.Open)) {
                    newBackground(Image.FromStream(stream));
                    baseSize = pictureBoxView.Image.Size;
                }

                Text = "MiniPaint wektorowy - " + openDlg.SafeFileName;
                trackBar1.Value = 2;
                imageFileDirectory = openDlg.FileName;

            }
            else {
                MessageBox.Show("Error");
            }
        }

        private void closeMenuItem_Click(object sender, EventArgs e) {
            Close();
        }
        private void printMenuItem_Click(object sender, EventArgs e) {
            PrintDocument myPrintDocument1 = new PrintDocument();
            myPrintDocument1.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            if (printDialog1.ShowDialog() == DialogResult.OK) {
                myPrintDocument1.Print();
            }
        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e) {
            e.Graphics.DrawImage(pircureBoxFrame.Image, 0, 0);
        }

        #endregion
        #region edition
        private void copyToClipboard(Point p1, Point p2)
        {

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                                Math.Min(p1.X, p2.X),
                                Math.Min(p1.Y, p2.Y),
                                Math.Abs(p1.X - p2.X),
                                Math.Abs(p1.Y - p2.Y));
            Bitmap bm = new Bitmap(rect.Width, rect.Height);

            using (Graphics gr = Graphics.FromImage(bm))
            {
                System.Drawing.Rectangle dest_rect =
                    new System.Drawing.Rectangle(0, 0, rect.Width, rect.Height);
                gr.DrawImage(pircureBoxFrame.Image, dest_rect, rect,
                    GraphicsUnit.Pixel);
            }

            Clipboard.SetImage(bm);
        }

        private void copyMenuItem_Click(object sender, EventArgs e) {
            copyToClipboard(workingPoints.First(), workingPoints.Last());
            hideSelect();
        }

        private void cutMenuItem_Click(object sender, EventArgs e) {
            if (workingPoints.Count > 2) {
                copyToClipboard(workingPoints.First(), workingPoints.Last());
                painting.Add(new Rectangle(Color.White, 1, Color.White, workingPoints.First(), workingPoints.Last()));
                painting.Paint(g);
                hideSelect();
            }
        }
        private void hideSelect() {
            EnableCopyCutMenuItems(false);
            pictureBoxView.Image = new Bitmap(pircureBoxFrame.Image);
            gp = Graphics.FromImage(pictureBoxView.Image);
            pictureBoxView.Refresh();
        }

        private void pasteMenuItem_Click(object sender, EventArgs e) {
            if (!Clipboard.ContainsImage()) return;

            Image clipboard_image = Clipboard.GetImage();

            painting.Add(new Picture(Color.White, 1, workingPoints.First(), clipboard_image));
            painting.Paint(g);
            pictureBoxView.Image = new Bitmap(pircureBoxFrame.Image);
            gp = Graphics.FromImage(pictureBoxView.Image);
            pictureBoxView.Refresh();
        }
        private void undoMenuItem_Click(object sender, EventArgs e) {
            painting.Usun(g);
            painting.Paint(gp);
            pictureBoxView.Refresh();
        }

        private void redoMenuItem_Click(object sender, EventArgs e) {
            painting.Ponow(g);
            painting.Paint(gp);
            pictureBoxView.Refresh();
        }
        private void turnMenuItem_Click(object sender, EventArgs e) {
            Image image = pircureBoxFrame.Image;
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            newBackground(image);
        }
        private void EnableCopyCutMenuItems(bool b) {
            kopiujToolStripMenuItem.Enabled = b;
            wytnijToolStripMenuItem.Enabled = b;
        }

        #endregion
        #region color
        private void buttonLineColor_Click(object sender, EventArgs e)
        {
            ColorDialog kolorOkno = new ColorDialog();
            if (kolorOkno.ShowDialog() == DialogResult.OK)
            {
                buttonLoneColor.BackColor = kolorOkno.Color;
            }
        }

        private void buttonFillColor_Click(object sender, EventArgs e)
        {
            ColorDialog kolorOkno = new ColorDialog();
            if (kolorOkno.ShowDialog() == DialogResult.OK)
            {
                buttonKolorWypelnienia.BackColor = kolorOkno.Color;
            }
        }

        #endregion
        #region help
        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 AboutMe = new AboutBox1();
            AboutMe.Show();
        }

        #endregion
        #region textAdd
        private void textBox_TextChanged(object sender, EventArgs e) {
            painting.Usun(gp);

            Form figura = new Text(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints.First(), textBoxTekst.Text, font);
            painting.Add(figura);
            gp = Graphics.FromImage(pictureBoxView.Image);
            figura.Rysuj(gp);
            pictureBoxView.Refresh();
        }

        private void textButton_Click(object sender, EventArgs e) {
            painting.Add(new Text(buttonLoneColor.BackColor, (int)numericUpDownLineThickness.Value, workingPoints.First(), textBoxTekst.Text, font));
            painting.Paint(g);
            pircureBoxFrame.Refresh();
            textBoxTekst.Text = "";
            groupBoxShape.Enabled = true;
            groupBoxText.Visible = false;
        }

        private void fontButton_Click(object sender, EventArgs e) {
            if (fontDialog1.ShowDialog() == DialogResult.OK) {
                font = fontDialog1.Font;
            }
        }
        #endregion
        #region scale
        private void trackBar1_ValueChanged(object sender, EventArgs e) {
            double scale = Math.Pow(2.0, trackBar1.Value - 2);
            changeScale(scale);
        }

        private void changeScale(double scale) {
            newBackground(resize((Bitmap)pircureBoxFrame.Image, scale));

        }
        #endregion
        #region pomocnicze
        private void newBackground(Image image)
        {

            pircureBoxFrame.Image = new Bitmap(image);
            pictureBoxView.Image = new Bitmap(image);


            g = Graphics.FromImage(pircureBoxFrame.Image);
            painting = new Painting(image);
            painting.Paint(g);
            pircureBoxFrame.Refresh();
        }


        private Image resize(Bitmap obraz, double skala)
        {
            return (Image)new Bitmap(obraz, new Size(Convert.ToInt32(baseSize.Width * skala), Convert.ToInt32(baseSize.Height * skala)));
        }
        #endregion
        #region floodFillWypełnianie
        private void FloodFill(Bitmap bmp, Point pt) {
            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = bmp.GetPixel(pt.X, pt.Y);
            Color replacementColor = buttonLoneColor.BackColor;
            if (targetColor == replacementColor)
                return;

            pixels.Push(pt);

            while (pixels.Count > 0) {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 &&
                        a.Y < bmp.Height && a.Y > 0) {

                    if (bmp.GetPixel(a.X, a.Y) == targetColor) {
                        bmp.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
            newBackground((Image) bmp);
        }
        #endregion
    }
}            
