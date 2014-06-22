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
                            pictureBox17.Visible = true;
                            pictureBox19.Visible = true;
                            pictureBox20.Visible = true;
                            pictureBox21.Visible = true;
                            pictureBox22.Visible = true;
                            pictureBox24.Visible = true;
                            pictureBox28.Visible = true;
                            pictureBox29.Visible = true;
                            pictureBox30.Visible = true;
                            pictureBox31.Visible = true;
                            pictureBox32.Visible = true;
                            pictureBox33.Visible = true;
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
                    toolStrip1.Visible = true;
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

                    trackBar27.Value = (face.headWidth - 350) / 10;
                    trackBar28.Value = (face.headHeight - 380) / 10;
                    trackBar26.Value = (face.eyeHeight - 40) / 5;
                    trackBar25.Value = (int) Math.Round((face.eyeSize - 0.65f) / 0.05f);
                    trackBar24.Value = face.eyeDist / 2;
                    trackBar23.Value = (face.noseHeight - 20) / 3;
                    trackBar22.Value = (int) Math.Round((face.noseSize - 0.65f) / 0.05f);
                    trackBar21.Value = (face.mouthHeight + 70) / 3;
                    trackBar20.Value = (int) Math.Round((face.mouthSize - 0.65f) / 0.05f);
                    trackBar15.Value = face.headType;
                    trackBar17.Value = face.hairType;
                    trackBar19.Value = face.eyeType;
                    trackBar18.Value = face.noseType;
                    trackBar16.Value = face.mouthType;

                    textBox10.Text = "" + face.headType;
                    textBox11.Text = "" + face.hairType;
                    textBox12.Text = "" + face.eyeType;
                    textBox13.Text = "" + face.noseType;
                    textBox14.Text = "" + face.mouthType;

                    label45.Visible = false;
                    label46.Visible = false;
                    label47.Visible = false;
                    label48.Visible = false;
                    button5.Visible = false;
                    button6.Visible = false;
                    button7.Visible = false;
                    break;
                case 2:
                    tabControl2.Visible = true;
                    tabControl1.Visible = false;
                    toolStrip1.Visible = true;
                    workMode = 2;

                    label45.Visible = false;
                    label46.Visible = false;
                    label47.Visible = false;
                    label48.Visible = false;
                    button5.Visible = false;
                    button6.Visible = false;
                    button7.Visible = false;
                    break;
                default:
                    tabControl1.Visible = false;
                    tabControl2.Visible = false;
                    toolStrip1.Visible = false;
                    workMode = 0;

                    label45.Visible = true;
                    label46.Visible = true;
                    label47.Visible = true;
                    label48.Visible = true;
                    button5.Visible = true;
                    button6.Visible = true;
                    button7.Visible = true;
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
                textBox1.Text = "" + face.headWidth;

                face.headHeight = 380 + trackBar14.Value * 10;
                trackBar28.Value = trackBar14.Value;
                textBox2.Text = "" + face.headHeight;

                face.eyeHeight = 40 + trackBar2.Value * 5;
                trackBar26.Value = trackBar2.Value;
                textBox3.Text = "" + face.eyeHeight;

                face.eyeSize = 0.65f + trackBar3.Value * 0.05f;
                trackBar25.Value = trackBar3.Value;
                textBox4.Text = "" + face.eyeSize * 100;

                face.eyeDist = 2 * trackBar4.Value;
                trackBar24.Value = trackBar4.Value;
                textBox5.Text = "" + face.eyeDist;

                face.noseHeight = 20 + trackBar5.Value * 3;
                trackBar23.Value = trackBar5.Value;
                textBox6.Text = "" + face.noseHeight;

                face.noseSize = 0.65f + trackBar6.Value * 0.05f;
                trackBar22.Value = trackBar6.Value;
                textBox7.Text = "" + face.noseSize * 100;

                face.mouthHeight = trackBar7.Value * 3 - 70;
                trackBar21.Value = trackBar7.Value;
                textBox8.Text = "" + (face.mouthHeight + 140);

                face.mouthSize = 0.65f + trackBar8.Value * 0.05f;
                trackBar20.Value = trackBar8.Value;
                textBox9.Text = "" + face.mouthSize * 100;

                face.headType = trackBar13.Value;
                trackBar15.Value = trackBar13.Value;
                textBox10.Text = "" + face.headType;

                face.hairType = trackBar12.Value;
                trackBar17.Value = trackBar12.Value;
                textBox11.Text = "" + face.hairType;

                face.eyeType = trackBar11.Value;
                trackBar19.Value = trackBar11.Value;
                textBox12.Text = "" + face.eyeType;

                face.noseType = trackBar10.Value;
                trackBar18.Value = trackBar10.Value;
                textBox13.Text = "" + face.noseType;

                face.mouthType = trackBar9.Value;
                trackBar16.Value = trackBar9.Value;
                textBox14.Text = "" + face.mouthType;
                drawFace(1);
            }
            else
            {
                face.headWidth = 350 + trackBar27.Value * 10;
                trackBar1.Value = trackBar27.Value;
                textBox1.Text = "" + face.headWidth;

                face.headHeight = 380 + trackBar28.Value * 10;
                trackBar14.Value = trackBar28.Value;
                textBox2.Text = "" + face.headHeight;

                face.eyeHeight = 40 + trackBar26.Value * 5;
                trackBar2.Value = trackBar26.Value;
                textBox3.Text = "" + face.eyeHeight;

                face.eyeSize = 0.65f + trackBar25.Value * 0.05f;
                trackBar3.Value = trackBar25.Value;
                textBox4.Text = "" + face.eyeSize * 100;

                face.eyeDist = 2 * trackBar24.Value;
                trackBar4.Value = trackBar24.Value;
                textBox5.Text = "" + face.eyeDist;

                face.noseHeight = 20 + trackBar23.Value * 3;
                trackBar5.Value = trackBar23.Value;
                textBox6.Text = "" + face.noseHeight;

                face.noseSize = 0.65f + trackBar22.Value * 0.05f;
                trackBar6.Value = trackBar22.Value;
                textBox7.Text = "" + face.noseSize * 100;

                face.mouthHeight = trackBar21.Value * 3 - 70;
                trackBar7.Value = trackBar21.Value;
                textBox8.Text = "" + (face.mouthHeight + 140);

                face.mouthSize = 0.65f + trackBar20.Value * 0.05f;
                trackBar8.Value = trackBar20.Value;
                textBox9.Text = "" + face.mouthSize * 100;

                face.headType = trackBar15.Value;
                trackBar13.Value = trackBar15.Value;
                textBox10.Text = "" + face.headType;

                face.hairType = trackBar17.Value;
                trackBar12.Value = trackBar17.Value;
                textBox11.Text = "" + face.hairType;

                face.eyeType = trackBar19.Value;
                trackBar11.Value = trackBar19.Value;
                textBox12.Text = "" + face.eyeType;

                face.noseType = trackBar18.Value;
                trackBar10.Value = trackBar18.Value;
                textBox13.Text = "" + face.noseType;

                face.mouthType = trackBar16.Value;
                trackBar9.Value = trackBar16.Value;
                textBox14.Text = "" + face.mouthType;

                drawFace(2);
            }
        }
        #endregion

        #region geom editor
        private void newGeom()
        {
            switchWorkMode(2);
        }

        #endregion

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            pictureBox17.Visible = false;
            pictureBox26.Visible = true;
            pictureBox23.Visible = true;
            pictureBox18.Visible = true;
            pictureBox27.Visible = true;
            pictureBox25.Visible = true;
            label29.Visible = true;
            label30.Visible = true;
            label31.Visible = true;
            label32.Visible = true;
            label33.Visible = true;
            label34.Visible = true;
            label35.Visible = true;
            label36.Visible = true;
            label37.Visible = true;
            label38.Visible = true;
            label39.Visible = true;
            label40.Visible = true;
            label41.Visible = true;
            label42.Visible = true;
            label43.Visible = true;
            label44.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            trackBar29.Visible = true;
            trackBar30.Visible = true;
            textBox15.Visible = true;
            textBox16.Visible = true;
            textBox17.Visible = true;
            textBox18.Visible = true;
            textBox19.Visible = true;
            textBox20.Visible = true;
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            newFace();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            newGeom();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                updateFaceFromTextBoxes();
            }
        }

        private void updateFaceFromTextBoxes()
        {
            int f = 1;
            if (int.TryParse(textBox1.Text, out f))
            {

                face.headWidth = f;
                trackBar1.Value = (Math.Min(Math.Max(350, f), 450) - 350) / 10;
                trackBar27.Value = (Math.Min(Math.Max(350, f), 450) - 350) / 10;
            }

            if (int.TryParse(textBox2.Text, out f))
            {
                face.headHeight = f;
                trackBar14.Value = (Math.Min(Math.Max(380, f), 480) - 380) / 10;
                trackBar28.Value = (Math.Min(Math.Max(380, f), 480) - 380) / 10;
            }

            if (int.TryParse(textBox3.Text, out f))
            {
                face.eyeHeight = f;
                trackBar2.Value = (Math.Min(Math.Max(40, f), 90) - 40) / 5;
                trackBar26.Value = (Math.Min(Math.Max(40, f), 90) - 40) / 5;
            }

            if (int.TryParse(textBox4.Text, out f))
            {
                face.eyeSize = f * .01f;
                trackBar3.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
                trackBar25.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
            }

            if (int.TryParse(textBox5.Text, out f))
            {
                face.eyeDist = f;
                trackBar4.Value = (Math.Min(Math.Max(0, f), 20) - 0) / 2;
                trackBar24.Value = (Math.Min(Math.Max(0, f), 20) - 0) / 2;
            }

            if (int.TryParse(textBox6.Text, out f))
            {
                face.noseHeight = f;
                trackBar5.Value = (Math.Min(Math.Max(20, f), 50) - 20) / 3;
                trackBar23.Value = (Math.Min(Math.Max(20, f), 50) - 20) / 3;
            }

            if (int.TryParse(textBox7.Text, out f))
            {
                face.noseSize = f * .01f;
                trackBar6.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
                trackBar22.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
            }

            if (int.TryParse(textBox8.Text, out f))
            {
                face.mouthHeight = f - 140;
                trackBar7.Value = (Math.Min(Math.Max(-70, f - 140), -40) + 70) / 3;
                trackBar21.Value = (Math.Min(Math.Max(-70, f - 140), -40) + 70) / 3;
            }

            if (int.TryParse(textBox9.Text, out f))
            {
                face.mouthSize = f * .01f;
                trackBar8.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
                trackBar20.Value = (Math.Min(Math.Max(65, f), 115) - 65) / 5;
            }

            if (int.TryParse(textBox10.Text, out f))
            {
                f = Math.Min(Math.Max(0, f), 2);
                face.headType = f;
                trackBar13.Value = f;
                trackBar15.Value = f;
                textBox10.Text = "" + f;
            }

            if (int.TryParse(textBox11.Text, out f))
            {
                f = Math.Min(Math.Max(0, f), 2);
                face.hairType = f;
                trackBar12.Value = f;
                trackBar17.Value = f;
                textBox11.Text = "" + f;
            }

            if (int.TryParse(textBox12.Text, out f))
            {
                f = Math.Min(Math.Max(0, f), 2);
                face.eyeType = f;
                trackBar11.Value = f;
                trackBar19.Value = f;
                textBox12.Text = "" + f;
            }

            if (int.TryParse(textBox13.Text, out f))
            {
                f = Math.Min(Math.Max(0, f), 2);
                face.noseType = f;
                trackBar10.Value = f;
                trackBar18.Value = f;
                textBox13.Text = "" + f;
            }

            if (int.TryParse(textBox14.Text, out f))
            {
                f = Math.Min(Math.Max(0, f), 2);
                face.mouthType = f;
                trackBar9.Value = f;
                trackBar16.Value = f;
                textBox14.Text = "" + f;
            }
            drawFace(tabControl1.SelectedIndex + 1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            textBox1.Text = "" + r.Next(350, 450);
            textBox2.Text = "" + r.Next(380, 480);
            textBox3.Text = "" + r.Next(40, 90);
            textBox4.Text = "" + r.Next(65, 115);
            textBox5.Text = "" + r.Next(0, 20);
            textBox6.Text = "" + r.Next(20, 50);
            textBox7.Text = "" + r.Next(65, 115);
            textBox8.Text = "" + r.Next(70, 100);
            textBox9.Text = "" + r.Next(65, 115);

            updateFaceFromTextBoxes();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            textBox10.Text = "" + r.Next(0, 3);
            textBox11.Text = "" + r.Next(0, 3);
            textBox12.Text = "" + r.Next(0, 3);
            textBox13.Text = "" + r.Next(0, 3);
            textBox14.Text = "" + r.Next(0, 3);

            updateFaceFromTextBoxes();
        }
    }
}
