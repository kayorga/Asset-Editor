using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Asset_Editor
{
    public partial class Form1 : Form
    {
        bool unsavedChanges = false;
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

        #region face editor
        private void newFace()
        {
            tabControl1.Visible = true;
            tabControl2.Visible = false;
            face.init();
            drawFace();
        }

        #region face parameter trackbars
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            face.headWidth = 350+trackBar1.Value*10;
            drawFace();
        }

        private void trackBar14_Scroll(object sender, EventArgs e)
        {
            face.headHeight = 380+trackBar14.Value*10;
            drawFace();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            face.eyeHeight = 40 + trackBar2.Value*5;
            drawFace();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            face.eyeSize = 0.65f + trackBar3.Value*0.05f;
            drawFace();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            face.eyeDist = 2*trackBar4.Value;
            drawFace();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            face.noseHeight = 20 + trackBar5.Value * 3;
            drawFace();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            face.noseSize = 0.65f + trackBar6.Value * 0.05f;
            drawFace();
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            face.mouthHeight = trackBar7.Value * 3 - 70;
            drawFace();
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            face.mouthSize = 0.65f + trackBar8.Value * 0.05f;
            drawFace();
        }

        private void trackBar13_Scroll(object sender, EventArgs e)
        {
            face.headType = trackBar13.Value;
            drawFace();
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            face.hairType = trackBar12.Value;
            drawFace();
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            face.eyeType = trackBar11.Value;
            drawFace();
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            face.noseType = trackBar10.Value;
            drawFace();
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            face.mouthType = trackBar9.Value;
            drawFace();
        }

        private void drawFace()
        {
            Image head = heads[face.headType];
            Image hair1 = hair[face.hairType];
            Image eyeL = eyesL[face.eyeType];
            Image eyeR = eyesR[face.eyeType];
            Image nose = noses[face.noseType];
            Image mouth = mouths[face.mouthType];

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Image = bmp;
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.DrawImage(head, 
                    pictureBox1.Image.Width / 2 - face.headWidth / 2, 
                    pictureBox1.Image.Height / 2 - face.headHeight / 2, 
                    face.headWidth, 
                    face.headHeight);
                g.DrawImage(eyeL, 
                    pictureBox1.Image.Width / 2 + face.eyeDist, 
                    pictureBox1.Image.Height / 2 - face.eyeHeight, 
                    face.eyeSize*eyeL.Width, 
                    face.eyeSize*eyeL.Height);
                g.DrawImage(eyeR, 
                    pictureBox1.Image.Width / 2 - face.eyeDist - face.eyeSize * eyeL.Width, 
                    pictureBox1.Image.Height / 2 - face.eyeHeight, 
                    face.eyeSize * eyeL.Width, 
                    face.eyeSize * eyeL.Height);
                g.DrawImage(hair1,
                    pictureBox1.Image.Width / 2 - face.headWidth / 2,
                    pictureBox1.Image.Height / 2 - face.headHeight / 2,
                    face.headWidth,
                    face.headHeight);
                g.DrawImage(mouth,
                    pictureBox1.Image.Width / 2 - face.mouthSize * mouth.Width / 2,
                    pictureBox1.Image.Height / 2 - face.mouthHeight,
                    face.mouthSize * mouth.Width,
                    face.mouthSize * mouth.Height);
                g.DrawImage(nose,
                    pictureBox1.Image.Width / 2 - face.noseSize * nose.Width / 2,
                    pictureBox1.Image.Height / 2 - face.noseHeight,
                    face.noseSize * nose.Width,
                    face.noseSize * nose.Height);
                

            }
            pictureBox1.Invalidate();
        }
        #endregion

        #endregion

        #region geom editor
        private void newGeom()
        {
            tabControl2.Visible = true;
            tabControl1.Visible = false;
        }

        #endregion

    }
}
