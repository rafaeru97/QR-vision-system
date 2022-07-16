using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Robotyka_1
{
    public partial class Form1 : Form
    {
        Rectangle rect;
        Point StartROI, EndROI;
        bool Selecting, MouseDown;
        Image<Bgr, Byte> PrimeImage;
        VideoCapture capture;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(Selecting)
            {
                MouseDown = true;
                StartROI = e.Location;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(Selecting)
            {
                int width = Math.Max(StartROI.X, e.X) - Math.Min(StartROI.X, e.X);
                int height = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
                rect = new Rectangle(Math.Min(StartROI.X, e.X), Math.Min(StartROI.Y, e.Y), width, height);
                Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if(MouseDown)
            {
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(Selecting)
            {
                Selecting = false;
                MouseDown = false;
            }
        }

        private void displayROI()
        {
            if (pictureBox1.Image == null)
                return;

            if (rect == Rectangle.Empty)
                return;

            var img = PrimeImage.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
            img.ROI = rect;

            var imgROI = img.Copy();
            img.ROI = Rectangle.Empty;

            pictureBox2.Image = imgROI.ToBitmap();
        }

        private void findROI()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            if (pictureBox1.Image == null)
                return;
            
            Image<Bgr, Byte> Image;

            Image = PrimeImage;

            //var TempImage = Image; 
            Image<Gray, Byte> TempImage;
            TempImage = Image.Convert<Gray, Byte>();
            //CvInvoke.Imshow("grey", TempImage);

            var corners = new Mat();

            QRCodeDetector qrCodeDetector = new QRCodeDetector();
            bool result = qrCodeDetector.DetectMulti(TempImage, corners);
            
            if (result)
            {
                var resultString = new VectorOfCvString();
                qrCodeDetector.DecodeMulti(TempImage, corners, resultString);

                if (resultString.Length / 32 != 4)
                {
                    MessageBox.Show("Not found 4 QR codes.");
                    return;
                }

                label3.Text = "QR Decode: " + resultString.ToArray()[0].ToString();
                if (resultString.ToArray()[0].ToString() != resultString.ToArray()[1].ToString())
                    label3.Text += ", " + resultString.ToArray()[1].ToString();
                if (resultString.ToArray()[0].ToString() != resultString.ToArray()[2].ToString())
                    label3.Text += ", " + resultString.ToArray()[2].ToString();
                if (resultString.ToArray()[0].ToString() != resultString.ToArray()[3].ToString())
                    label3.Text += ", " + resultString.ToArray()[3].ToString();

                var qr1_x1 = corners.GetData().GetValue(0, 0, 0);
                var qr1_y1 = corners.GetData().GetValue(0, 0, 1);
                var qr1_x2 = corners.GetData().GetValue(0, 2, 0);
                var qr1_y2 = corners.GetData().GetValue(0, 2, 1);

                var qr2_x1 = corners.GetData().GetValue(1, 0, 0);
                var qr2_y1 = corners.GetData().GetValue(1, 0, 1);
                var qr2_x2 = corners.GetData().GetValue(1, 2, 0);
                var qr2_y2 = corners.GetData().GetValue(1, 2, 1);

                var qr3_x1 = corners.GetData().GetValue(2, 0, 0);
                var qr3_y1 = corners.GetData().GetValue(2, 0, 1);
                var qr3_x2 = corners.GetData().GetValue(2, 2, 0);
                var qr3_y2 = corners.GetData().GetValue(2, 2, 1);

                var qr4_x1 = corners.GetData().GetValue(3, 0, 0);
                var qr4_y1 = corners.GetData().GetValue(3, 0, 1);
                var qr4_x2 = corners.GetData().GetValue(3, 2, 0);
                var qr4_y2 = corners.GetData().GetValue(3, 2, 1);

                int qr1_width = Math.Max(Convert.ToInt32(qr1_x1), Convert.ToInt32(qr1_x2)) - Math.Min(Convert.ToInt32(qr1_x1), Convert.ToInt32(qr1_x2));
                int qr1_height = Math.Max(Convert.ToInt32(qr1_y1), Convert.ToInt32(qr1_y2)) - Math.Min(Convert.ToInt32(qr1_y1), Convert.ToInt32(qr1_y2));

                int qr2_width = Math.Max(Convert.ToInt32(qr2_x1), Convert.ToInt32(qr2_x2)) - Math.Min(Convert.ToInt32(qr2_x1), Convert.ToInt32(qr2_x2));
                int qr2_height = Math.Max(Convert.ToInt32(qr2_y1), Convert.ToInt32(qr2_y2)) - Math.Min(Convert.ToInt32(qr2_y1), Convert.ToInt32(qr2_y2));

                int qr3_width = Math.Max(Convert.ToInt32(qr3_x1), Convert.ToInt32(qr3_x2)) - Math.Min(Convert.ToInt32(qr3_x1), Convert.ToInt32(qr3_x2));
                int qr3_height = Math.Max(Convert.ToInt32(qr3_y1), Convert.ToInt32(qr3_y2)) - Math.Min(Convert.ToInt32(qr3_y1), Convert.ToInt32(qr3_y2));

                int qr4_width = Math.Max(Convert.ToInt32(qr4_x1), Convert.ToInt32(qr4_x2)) - Math.Min(Convert.ToInt32(qr4_x1), Convert.ToInt32(qr4_x2));
                int qr4_height = Math.Max(Convert.ToInt32(qr4_y1), Convert.ToInt32(qr4_y2)) - Math.Min(Convert.ToInt32(qr4_y1), Convert.ToInt32(qr4_y2));

                Rectangle qr1_rect = new Rectangle(Math.Min(Convert.ToInt32(qr1_x1), Convert.ToInt32(qr1_x2)), Math.Min(Convert.ToInt32(qr1_y1), Convert.ToInt32(qr1_y2)), qr1_width, qr1_height);
                Rectangle qr2_rect = new Rectangle(Math.Min(Convert.ToInt32(qr2_x1), Convert.ToInt32(qr2_x2)), Math.Min(Convert.ToInt32(qr2_y1), Convert.ToInt32(qr2_y2)), qr2_width, qr2_height);
                Rectangle qr3_rect = new Rectangle(Math.Min(Convert.ToInt32(qr3_x1), Convert.ToInt32(qr3_x2)), Math.Min(Convert.ToInt32(qr3_y1), Convert.ToInt32(qr3_y2)), qr3_width, qr3_height);
                Rectangle qr4_rect = new Rectangle(Math.Min(Convert.ToInt32(qr4_x1), Convert.ToInt32(qr4_x2)), Math.Min(Convert.ToInt32(qr4_y1), Convert.ToInt32(qr4_y2)), qr2_width, qr4_height);

                Point qr1_center = new Point(((Convert.ToInt32(corners.GetData().GetValue(0, 2, 0)) - Convert.ToInt32(corners.GetData().GetValue(0, 0, 0))) / 2 + Convert.ToInt32(corners.GetData().GetValue(0, 0, 0))),
                                             ((Convert.ToInt32(corners.GetData().GetValue(0, 2, 1)) - Convert.ToInt32(corners.GetData().GetValue(0, 0, 1))) / 2 + Convert.ToInt32(corners.GetData().GetValue(0, 0, 1)))
                                             );

                Point qr2_center = new Point(((Convert.ToInt32(corners.GetData().GetValue(1, 2, 0)) - Convert.ToInt32(corners.GetData().GetValue(1, 0, 0))) / 2 + Convert.ToInt32(corners.GetData().GetValue(1, 0, 0))),
                                             ((Convert.ToInt32(corners.GetData().GetValue(1, 2, 1)) - Convert.ToInt32(corners.GetData().GetValue(1, 0, 1))) / 2 + Convert.ToInt32(corners.GetData().GetValue(1, 0, 1)))
                                             );

                Point qr3_center = new Point(((Convert.ToInt32(corners.GetData().GetValue(2, 2, 0)) - Convert.ToInt32(corners.GetData().GetValue(2, 0, 0))) / 2 + Convert.ToInt32(corners.GetData().GetValue(2, 0, 0))),
                                              ((Convert.ToInt32(corners.GetData().GetValue(2, 2, 1)) - Convert.ToInt32(corners.GetData().GetValue(2, 0, 1))) / 2 + Convert.ToInt32(corners.GetData().GetValue(2, 0, 1)))
                                              );

                Point qr4_center = new Point(((Convert.ToInt32(corners.GetData().GetValue(3, 2, 0)) - Convert.ToInt32(corners.GetData().GetValue(3, 0, 0))) / 2 + Convert.ToInt32(corners.GetData().GetValue(3, 0, 0))),
                                              ((Convert.ToInt32(corners.GetData().GetValue(3, 2, 1)) - Convert.ToInt32(corners.GetData().GetValue(3, 0, 1))) / 2 + Convert.ToInt32(corners.GetData().GetValue(3, 0, 1)))
                                              );

                int grROI_X_min = Math.Min(qr4_center.X, Math.Min(qr3_center.X, Math.Min(qr1_center.X, qr2_center.X)));
                int grROI_Y_min = Math.Min(qr4_center.Y, Math.Min(qr3_center.Y, Math.Min(qr1_center.Y, qr2_center.Y)));
                int grROI_X_max = Math.Max(qr4_center.X, Math.Max(qr3_center.X, Math.Max(qr1_center.X, qr2_center.X)));
                int grROI_Y_max = Math.Max(qr4_center.Y, Math.Max(qr3_center.Y, Math.Max(qr1_center.Y, qr2_center.Y)));

                int qrROI_width = grROI_X_max - grROI_X_min;
                int qrROI_height = grROI_Y_max - grROI_Y_min;

                Rectangle qrROI_rect = new Rectangle(grROI_X_min, grROI_Y_min, qrROI_width, qrROI_height);

                Pen ROI_pen = new Pen(Color.Orange, 3);
                pictureBox1.CreateGraphics().DrawRectangle(ROI_pen, qrROI_rect);

                MCvScalar QRcolor = new MCvScalar(0, 255, 255);
                MCvScalar QRcolorText = new MCvScalar(255, 0, 255);
                MCvScalar ROIcolor = new MCvScalar(200, 0, 200);
                Emgu.CV.CvEnum.FontFace Font = new Emgu.CV.CvEnum.FontFace();

                Image<Bgr, Byte> DrawImage;
                DrawImage = Image.Copy();

                CvInvoke.Rectangle(DrawImage, qr1_rect, QRcolor, 5);
                CvInvoke.Rectangle(DrawImage, qr2_rect, QRcolor, 5);
                CvInvoke.Rectangle(DrawImage, qr3_rect, QRcolor, 5);
                CvInvoke.Rectangle(DrawImage, qr4_rect, QRcolor, 5);
                CvInvoke.Rectangle(DrawImage, qrROI_rect, ROIcolor, 5);
                CvInvoke.PutText(DrawImage, "[1](" + qr1_center.X + "," + qr1_center.Y + ")", qr1_center, Font, 1, QRcolorText, 4);
                CvInvoke.PutText(DrawImage, "[2](" + qr2_center.X + "," + qr2_center.Y + ")", qr2_center, Font, 1, QRcolorText, 4);
                CvInvoke.PutText(DrawImage, "[3](" + qr3_center.X + "," + qr3_center.Y + ")", qr3_center, Font, 1, QRcolorText, 4);
                CvInvoke.PutText(DrawImage, "[4](" + qr4_center.X + "," + qr4_center.Y + ")", qr4_center, Font, 1, QRcolorText, 4);

                Image.ROI = qrROI_rect;

                var imgROI = Image.Copy();
                Image.ROI = Rectangle.Empty;

                pictureBox1.Image = DrawImage.ToBitmap();
                pictureBox2.Image = imgROI.ToBitmap();

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                MessageBox.Show("Execution time: " + elapsedMs + "ms");
            }
            else
                MessageBox.Show("Cannot detect QR code from this image.");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    if(capture != null)
                    {
                        capture.Stop();
                    }
                        

                    PrimeImage = new Image<Bgr, Byte>(ofd.FileName);
                    pictureBox1.Image = PrimeImage.ToBitmap();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void autoROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                findROI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void displayROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                displayROI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveROI()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK) {
                pictureBox2.Image.Save(sfd.FileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void saveROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveROI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture == null)
                    capture = new VideoCapture(0);

                capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
                capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);

                capture.ImageGrabbed += Capture_ImageGrabbed1;
                capture.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Capture_ImageGrabbed1(object? sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                PrimeImage = m.ToImage<Bgr, Byte>();
                var dispImage = PrimeImage.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                pictureBox1.Image = dispImage.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void manualROI()
        {
            Selecting = true;
        }

        private void manualROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                manualROI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}