using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Asset_Editor
{
    public partial class Form1 : Form
    {
        bool unsavedChanges = false;
        byte workMode = 0;
        Face face;
        List<Image> heads;
        List<Image> hair;
        List<Image> eyesL;
        List<Image> eyesR;
        List<Image> noses;
        List<Image> mouths;

        public Form1()
        {
            InitializeComponent();
            face = new Face();
            pictureBox1.Image = new Bitmap(552, 589);

            heads = new List<Image>();
            hair = new List<Image>();
            eyesL = new List<Image>();
            eyesR = new List<Image>();
            noses = new List<Image>();
            mouths = new List<Image>();

            heads.Add(Image.FromFile("shapes/face1.png"));
            heads.Add(Image.FromFile("shapes/face2.png"));
            heads.Add(Image.FromFile("shapes/face3.png"));
            hair.Add(Image.FromFile("shapes/hair1.png"));
            hair.Add(Image.FromFile("shapes/hair2.png"));
            hair.Add(Image.FromFile("shapes/hair3.png"));
            eyesL.Add(Image.FromFile("shapes/eye1.png"));
            eyesL.Add(Image.FromFile("shapes/eye2.png"));
            eyesL.Add(Image.FromFile("shapes/eye3.png"));
            eyesR.Add(Image.FromFile("shapes/eye1r.png"));
            eyesR.Add(Image.FromFile("shapes/eye2r.png"));
            eyesR.Add(Image.FromFile("shapes/eye3r.png"));
            noses.Add(Image.FromFile("shapes/nose1.png"));
            noses.Add(Image.FromFile("shapes/nose2.png"));
            noses.Add(Image.FromFile("shapes/nose3.png"));
            mouths.Add(Image.FromFile("shapes/mouth1.png"));
            mouths.Add(Image.FromFile("shapes/mouth2.png"));
            mouths.Add(Image.FromFile("shapes/mouth3.png"));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (unsavedChanges) 
                return;
            else
            {
                switch (toolStripComboBox1.SelectedIndex)
                {
                    case 0:
                        {
                            newFace();
                            break;
                        }
                    case 1:
                        {
                            newGeom();
                            break;
                        }
                }
            }
        }

        #region save and load
        //export
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            switch (workMode)
            {
                case 0:
                    MessageBox.Show("No asset to save!");
                    return;
                case 1:
                    sfd.Filter = "Face file|*.face|Bitmap|*.bmp";
                    break;
                case 2:
                    sfd.Filter = "Geom file|*.geom|Bitmap|*.bmp";
                    break;
            }
            sfd.Title = "Export asset";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                try
                {
                    System.IO.FileStream fs =
                       (System.IO.FileStream)sfd.OpenFile();

                    switch (workMode)
                    {
                        case 1:
                            switch (sfd.FilterIndex)
                            {
                                case 1:
                                    BinaryFormatter bf = new BinaryFormatter();
                                    bf.Serialize(fs, face);
                                    fs.Close();
                                    break;
                                case 2:
                                    PictureBox picBox = Controls.Find("pictureBox" + (tabControl1.SelectedIndex + 1), true).First() as PictureBox;
                                    picBox.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                                    fs.Close();
                                    break;
                            }
                            break;

                        case 2:

                            break;
                    }

                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //import
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Asset file|*.face;*.geom";
            ofd.Title = "Import asset";
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                try
                {
                    System.IO.FileStream fs =
                       (System.IO.FileStream)ofd.OpenFile();

                    switch (Path.GetExtension(ofd.FileName))
                    {
                        case ".face":
                            BinaryFormatter bf = new BinaryFormatter();
                            Face f = (Face)bf.Deserialize(fs);
                            fs.Close();

                            face = f;
                            switchWorkMode(1);
                            break;

                        case ".geom":
                            switchWorkMode(2);
                            break;
                    }

                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawFace(tabControl1.SelectedIndex + 1);
        }

        private void switchWorkMode(int m)
        {
            switch (m)
            {
                case 1:
                    tabControl1.Visible = true;
                    tabControl2.Visible = false;
                    workMode = 1;
                    drawFace(tabControl1.SelectedIndex + 1);

                    trackBar1.Value = (face.headWidth - 350) / 10;
                    trackBar14.Value = (face.headHeight - 380) / 10;
                    trackBar2.Value = (face.eyeHeight - 40) / 5;
                    trackBar3.Value = (int) Math.Round((face.eyeSize - 0.65f) / 0.05f);
                    trackBar4.Value = face.eyeDist / 2;
                    trackBar5.Value = (face.noseHeight - 20) / 3;
                    trackBar6.Value = (int) Math.Round((face.noseSize - 0.65f) / 0.05f);
                    trackBar7.Value = (face.mouthHeight + 70) / 3;
                    trackBar8.Value = (int) Math.Round((face.mouthSize - 0.65f) / 0.05f);
                    trackBar13.Value = face.headType;
                    trackBar12.Value = face.hairType;
                    trackBar11.Value = face.eyeType;
                    trackBar10.Value = face.noseType;
                    trackBar9.Value = face.mouthType;
                    break;
                case 2:
                    tabControl2.Visible = true;
                    tabControl1.Visible = false;
                    workMode = 2;
                    break;
                default:
                    tabControl1.Visible = false;
                    tabControl2.Visible = false;
                    workMode = 0;
                    break;
            }
        }

        #region face editor
        private void newFace()
        {
            face.init();
            switchWorkMode(1);
        }

        private void drawFace(int index)
        {
            Image head = heads[face.headType];
            Image hair1 = hair[face.hairType];
            Image eyeL = eyesL[face.eyeType];
            Image eyeR = eyesR[face.eyeType];
            Image nose = noses[face.noseType];
            Image mouth = mouths[face.mouthType];

            PictureBox picBox = Controls.Find("pictureBox"+index, true).First() as PictureBox;
            Bitmap bmp = new Bitmap(picBox.Width, picBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            picBox.Image = bmp;

            using (Graphics g = Graphics.FromImage(picBox.Image))
            {
                g.DrawImage(head,
                    picBox.Image.Width / 2 - face.headWidth / 2,
                    picBox.Image.Height / 2 - face.headHeight / 2,
                    face.headWidth,
                    face.headHeight);
                g.DrawImage(eyeL,
                    picBox.Image.Width / 2 + face.eyeDist,
                    picBox.Image.Height / 2 - face.eyeHeight,
                    face.eyeSize * eyeL.Width,
                    face.eyeSize * eyeL.Height);
                g.DrawImage(eyeR,
                    picBox.Image.Width / 2 - face.eyeDist - face.eyeSize * eyeL.Width,
                    picBox.Image.Height / 2 - face.eyeHeight,
                    face.eyeSize * eyeL.Width,
                    face.eyeSize * eyeL.Height);
                g.DrawImage(hair1,
                    picBox.Image.Width / 2 - face.headWidth / 2,
                    picBox.Image.Height / 2 - face.headHeight / 2,
                    face.headWidth,
                    face.headHeight);
                g.DrawImage(mouth,
                    picBox.Image.Width / 2 - face.mouthSize * mouth.Width / 2,
                    picBox.Image.Height / 2 - face.mouthHeight,
                    face.mouthSize * mouth.Width,
                    face.mouthSize * mouth.Height);
                g.DrawImage(nose,
                    picBox.Image.Width / 2 - face.noseSize * nose.Width / 2,
                    picBox.Image.Height / 2 - face.noseHeight,
                    face.noseSize * nose.Width,
                    face.noseSize * nose.Height);
            }
        }

        private void updateFaceFromTrackBars(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                face.headWidth = 350 + trackBar1.Value * 10;
                trackBar27.Value = trackBar1.Value;

                face.headHeight = 380 + trackBar14.Value * 10;
                trackBar28.Value = trackBar14.Value;

                face.eyeHeight = 40 + trackBar2.Value * 5;
                trackBar26.Value = trackBar2.Value;

                face.eyeSize = 0.65f + trackBar3.Value * 0.05f;
                trackBar25.Value = trackBar3.Value;

                face.eyeDist = 2 * trackBar4.Value;
                trackBar24.Value = trackBar4.Value;

                face.noseHeight = 20 + trackBar5.Value * 3;
                trackBar23.Value = trackBar5.Value;

                face.noseSize = 0.65f + trackBar6.Value * 0.05f;
                trackBar22.Value = trackBar6.Value;

                face.mouthHeight = trackBar7.Value * 3 - 70;
                trackBar21.Value = trackBar7.Value;

                face.mouthSize = 0.65f + trackBar8.Value * 0.05f;
                trackBar20.Value = trackBar8.Value;

                face.headType = trackBar13.Value;
                trackBar15.Value = trackBar13.Value;

                face.hairType = trackBar12.Value;
                trackBar17.Value = trackBar12.Value;

                face.eyeType = trackBar11.Value;
                trackBar19.Value = trackBar11.Value;

                face.noseType = trackBar10.Value;
                trackBar18.Value = trackBar10.Value;

                face.mouthType = trackBar9.Value;
                trackBar16.Value = trackBar9.Value;
                drawFace(1);
            }
            else
            {
                face.headWidth = 350 + trackBar27.Value * 10;
                face.headHeight = 380 + trackBar28.Value * 10;
                face.eyeHeight = 40 + trackBar26.Value * 5;
                face.eyeSize = 0.65f + trackBar25.Value * 0.05f;
                face.eyeDist = 2 * trackBar24.Value;
                face.noseHeight = 20 + trackBar23.Value * 3;
                face.noseSize = 0.65f + trackBar22.Value * 0.05f;
                face.mouthHeight = trackBar21.Value * 3 - 70;
                face.mouthSize = 0.65f + trackBar20.Value * 0.05f;
                face.headType = trackBar15.Value;
                face.hairType = trackBar17.Value;
                face.eyeType = trackBar19.Value;
                face.noseType = trackBar18.Value;
                face.mouthType = trackBar16.Value;
                drawFace(2);
            }
        }
        #endregion

        #region geom editor
        byte moveShape = 0;
        Point lastMousePos;

        private void newGeom()
        {
            switchWorkMode(2);
        }

        #endregion

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveShape == 1)
            {
                Point p = this.PointToScreen(e.Location);
                pictureBox5.SetBounds(pictureBox5.Left + p.X - lastMousePos.X, pictureBox5.Top + p.Y - lastMousePos.Y, pictureBox5.Width, pictureBox5.Height);
                lastMousePos = p;
                //label29.Text = "" + p;
                //label29.Update();
                pictureBox5.Update();
            }
        }

        private void tabPage3_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox5.Bounds.Contains(e.Location))
            {
                moveShape = 1;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                label29.Text = "yo";
        }

    }
}
