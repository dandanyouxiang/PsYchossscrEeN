using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace PsYchossscrEeN {
    /// <summary>
    /// Interaction logic for ScreenArea.xaml
    /// </summary>
    public partial class ScreenArea {

        public string fileName = "Screen";
        public ImageFormat format = ImageFormat.Png; //Default is png
        public string extension = "png";
        public String fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public ScreenArea() {
            InitializeComponent();

            Focus();
            Topmost = true;
        }

        private void ScreenArea_OnKeyDown(object sender, KeyEventArgs e) {

            //Set window trasparent
            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
            Opacity = 0.00;

            //Take screenshot
            ScreenAreaCapture();
        }

        private void ScreenAreaCapture() {

            //Stop running window
            Close();

            //Create bitmap with this.Width, this.Height 
            var bmpScreenshot = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height), PixelFormat.Format32bppArgb);
            
            //New graphics from bitmap
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            //System.Drawing.Size with this dimensions
            var windowSize = new Size(Convert.ToInt32(Width), Convert.ToInt32(Height));

            //Take screenshot windows sized
            gfxScreenshot.CopyFromScreen(Convert.ToInt32(Left), Convert.ToInt32(Top), 0, 0, windowSize, CopyPixelOperation.SourceCopy);
            
            //Save screenshot
            bmpScreenshot.Save(fileDirectory + "\\" + fileName + "." + extension, format);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {

            //Move window when drag it
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
            Width += Width * e.Delta / 1000;
            Height += Height * e.Delta / 1000;
        }
    }
}
