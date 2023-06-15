using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Image_Editing_WPF_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected List<string> lstAllowedKeys;
        protected string fileRenderName = "";
        public MainWindow()
        {
            InitializeComponent();
            GetAllowedKeys();//
        }//ctor main
        private void GetAllowedKeys()
        {
            lstAllowedKeys = new List<string>();
            for (int i = 0; i < 10; i++)
                lstAllowedKeys.Add($"NumPad{i.ToString()}");
            lstAllowedKeys.AddRange(new string[] { "Back", "Delete" });
        }//GetKeys
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpenFile = (OpenFileDialog)GetDialog();

            if (dlgOpenFile.ShowDialog().HasValue)
            {
                //Remove existing image on the canvas
                inkCanvasImage.Children.Clear();
                inkCanvasImage.Strokes.Clear();

                AddImageToCanvas(dlgOpenFile.FileName);
            }//end if
        }//btnBrowse_Click
        private void AddImageToCanvas(string fileName)
        {
            //Create an image file to add to the canvas
            Image img = new Image()
            {
                Width = inkCanvasImage.ActualWidth,
                Height = inkCanvasImage.ActualHeight,
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute))
            };
            inkCanvasImage.Children.Add(img);
        }//AddImageToCanvas
        private void cmboEditingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inkCanvasImage == null)
                return;
            switch (cmboEditingMode.SelectedIndex)
            {
                case 0:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.None;
                    break;
                case 1:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case 2:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case 3:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case 4:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
                case 5:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.GestureOnly;
                    break;
                case 6:
                    inkCanvasImage.EditingMode = InkCanvasEditingMode.InkAndGesture;
                    break;
            }//end Switch
        }//cmboEditingMode_SelectionChanged
        private void txtPencilSize_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int index = lstAllowedKeys.IndexOf(e.Key.ToString());
            if (index == -1)
            {
                //Mark event as handled to stop Routing Tunneling event(stop it from entering non numeric values in the textbox)
                e.Handled = true;
                MessageBox.Show("Enter numeric values between 0 and 100", "INPUT ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }//end if 
        }//txtPencilSize_PreviewKeyDown
        private void txtPencilSize_KeyDown(object sender, KeyEventArgs e)
        {
            //Determine if the backspace or the delete have been pressed
            bool isNumber = int.TryParse(txtPencilSize.Text + e.Key.ToString().LastOrDefault(), out int iSize);
            if (!isNumber)
                return;
            if (iSize > 100 || iSize <= 0)
            {
                //Mark event as handled to stop Routing Tunneling event(stop it from writting a value in the textbox)
                e.Handled = true;
                MessageBox.Show("Size must be between 1 and 100 pixels", "SIZE ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            inkCanvasImage.DefaultDrawingAttributes.Width = iSize;
            inkCanvasImage.DefaultDrawingAttributes.Height = iSize;
        }//txtPencilSize_KeyDown
        private void btnCleanCanvas_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasImage.Children.Clear();
            inkCanvasImage.Strokes.Clear();
        }//btnCleanCanvas_Click
        private void ColorPicker_SelectedBrushChanged(object sender, RoutedEventArgs e)
        {
            //For opening the application for the first time
            if (inkCanvasImage == null)
                return;
            SolidColorBrush brush = cpbtnPencil.SelectedBrush;
            inkCanvasImage.DefaultDrawingAttributes.Color = new Color() { A = brush.Color.A, B = brush.Color.B, G = brush.Color.G, R = brush.Color.R };
        }//ColorPicker_SelectedBrushChanged
        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgSaveFile = (SaveFileDialog)GetDialog("Save");
            if (dlgSaveFile.ShowDialog().HasValue)
            {
                fileRenderName = dlgSaveFile.FileName;
                CRenderImage.SaveImageToFile(dlgSaveFile.FileName, inkCanvasImage);

                //Photo can be reviewed
                if(MessageBox.Show("Open image now ?", "OPEN", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    Process.Start(dlgSaveFile.FileName);

            }//end if 
        }//btnSaveImage_Click
        private FileDialog GetDialog(string dialog = null)
        {
            FileDialog dlgFile;
            if (dialog != null)
                dlgFile = new SaveFileDialog();
            else
                dlgFile = new OpenFileDialog();

            dlgFile.InitialDirectory = @"Downloads\";
            dlgFile.RestoreDirectory = true;
            dlgFile.Title = "Browse Image Files";
            dlgFile.DefaultExt = "png";
            dlgFile.Filter = "All |*.jpg;*.jpeg;*.png;*.gif;*.tif;*.webp;*.bmp | Bitmap Image(.bmp)| *.bmp | Gif Image(.gif) | *.gif | JPEG Image(.jpeg) | *.jpeg | Png Image(.png) | *.png | Tiff Image(.tiff) | *.tiff | Wmf Image(.wmf) | *.wmf | Web Image(.webp)| *.webp";
            return dlgFile;
        }//GetDialog
        private void githubProfile_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }//githubProfile_Click
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }//btnClose_Click
    }//Class
}//Namaespace
