using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PsYchossscrEeN {
    public partial class MainWindow {

        //Extensions struct
        public struct Extensions {
            public String name;
            public String ext;
        }

        //Variable definition
        ImageFormat format = ImageFormat.Png; //Default is png
        List<Extensions> extList = new List<Extensions>();
        String fileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //Notify in tray
        NotifyIcon notifyIcon;

        public MainWindow() {
            InitializeComponent();

            //Over all
            Topmost = true;

            //Move the window
            TitleBar.MouseMove += TitleBar_MouseMove;
            LabelTitle.MouseMove += TitleBar_MouseMove;

            //Fill list with extensions
            extList.Add(new Extensions { name = "Png", ext = "png" });
            extList.Add(new Extensions { name = "Jpge", ext = "jpg" });
            extList.Add(new Extensions { name = "Bmp", ext = "bmp" });
            extList.Add(new Extensions { name = "Gif", ext = "gif" });

            //Fill ComboBoxExt with extensions
            for (int i = 0; i < extList.Count; i++) ComboBoxExt.Items.Add(extList[i].name + " Image - " + extList[i].ext);

            //Fill TextBoxName
            TextBoxName.Text = "Screen";

            //Icon
            var smIcon = System.Drawing.Icon.FromHandle(Properties.Resources.SM.GetHicon());
            ImageSource smSource = Imaging.CreateBitmapSourceFromHIcon(smIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            Icon = smSource;
            
            //Initialize notify	
            notifyIcon = new NotifyIcon();
            var iconHandle = Properties.Resources.SM.GetHicon();
            notifyIcon.Icon = System.Drawing.Icon.FromHandle(iconHandle);
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        //Get the right file name
        public string GetFileName() {
            string fileName = TextBoxName.Text; 

            if (File.Exists(fileDirectory + "\\" + fileName + "." + extList[ComboBoxExt.SelectedIndex].ext))
                for (int i = 1; i < int.MaxValue; i++)
                    if (!File.Exists(fileDirectory + "\\" + fileName + i + "." + extList[ComboBoxExt.SelectedIndex].ext)) return fileName + i;
            return fileName;
        }

        //Set the directory
        public void SetFileDirectory() {
            var folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel) return;

            //Set fileDirectory with selected path
            fileDirectory = folderBrowserDialog.SelectedPath;
        }

        //Set image format
        public void SetFileExt() {
            switch (extList[ComboBoxExt.SelectedIndex].name) {
                case "Png":
                    format = ImageFormat.Png;
                    break;
                case "Jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case "Bmp":
                    format = ImageFormat.Bmp;
                    break;
                case "Gif":
                    format = ImageFormat.Gif;
                    break;
                case "Tiff":
                    format = ImageFormat.Tiff;
                    break;
            }
        }

        #region Notify icon
        private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            WindowState = WindowState.Normal;
        }
        private void ScreenMakerWindow_StateChanged(object sender, EventArgs e) {
            if (WindowState == WindowState.Minimized) {
                ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
            else if (WindowState == WindowState.Normal) {
                notifyIcon.Visible = false;
                ShowInTaskbar = true;
            }
        }
        #endregion

        #region Window events methods

        //Move window through the tilebar
        private void TitleBar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        //Bar buttons
        private void ButtonClose_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        //App buttons
        private void ButtonDirectory_Click(object sender, RoutedEventArgs e) {
            SetFileDirectory();
        }
        private void ScreenAreaButton_OnClick(object sender, RoutedEventArgs e) {

            //Minimize main window
            WindowState = WindowState.Minimized;

            //Open screen area window
            //(new ScreenArea()).Show();
            new ScreenArea {
                fileName = GetFileName(),
                format = format,
                extension = extList[ComboBoxExt.SelectedIndex].ext,
                fileDirectory = fileDirectory
            }.Show();      
        }

        //Combobox selection
        private void ComboBoxExt_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SetFileExt();
        }

        //Opacity when activated/deactivated
        private void ScreenMakerWindow_Activated(object sender, EventArgs e) {
            Opacity = 1.0;
        }
        private void ScreenMakerWindow_Deactivated(object sender, EventArgs e) {
            Opacity = 0.20;
        }

        #endregion
    }
}