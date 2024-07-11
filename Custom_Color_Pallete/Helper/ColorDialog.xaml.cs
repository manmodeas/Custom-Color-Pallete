using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Custom_Color_Pallete.Helper
{
    /// <summary>
    /// Interaction logic for ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window
    {
        private bool isForeground = false;
        public bool IsUpdate { get; set; }
        public Color SelectedColor { get; set; }
        bool isMouse = false;
        byte[] pixels;
        public ColorDialog(bool isForeground)
        {
            InitializeComponent();
            IsUpdate = false;
            this.isForeground = isForeground;
            if (isForeground)
            {
                textBoxA.Text = "255";
                textBoxR.Text = "0";
                textBoxG.Text = "0";
                textBoxB.Text = "0";
                SelectedColor = MakeColorFromRGB();
                UpdateHEXValue();
                previewColor.Fill = new SolidColorBrush(SelectedColor);
            }
            else
            {
                textBoxA.Text = "255";
                textBoxR.Text = "255";
                textBoxG.Text = "255";
                textBoxB.Text = "255";
                SelectedColor = MakeColorFromRGB();
                UpdateHEXValue();
                previewColor.Fill = new SolidColorBrush(SelectedColor);
            }
            colorAlpha.Value = 255;
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.colorList.ItemsSource = typeof(Brushes).GetProperties();
        }

        private void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Brush brushColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            previewColor.Fill = brushColor;
            SolidColorBrush solidColor = (SolidColorBrush)brushColor;
            SelectedColor = Color.FromArgb(solidColor.Color.A,
                                        solidColor.Color.R,
                                        solidColor.Color.G,
                                        solidColor.Color.B);
            UpdateARGB();
            UpdateHEXValue();
            UpdateAlphaSliderValue();
        }

        #region ARGB
        private void ARGB_PreviewTextInput(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            if (!regex.IsMatch((sender as TextBox).Text))
            {
                (sender as TextBox).Text = "0";
            }
            SelectedColor = MakeColorFromRGB();
            previewColor.Fill = new SolidColorBrush(SelectedColor);
            UpdateHEXValue();
            UpdateAlphaSliderValue();
        }
        private void HEX_Color_PreviewTextInput(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
            if (!regex.IsMatch((sender as TextBox).Text))
            {
                if (isForeground)
                    (sender as TextBox).Text = "#000000";
                else
                    (sender as TextBox).Text = "#FFFFFF";
            }
            if (!MakeColorFromHex((sender as TextBox).Text))
            {
                if (isForeground)
                    (sender as TextBox).Text = "#000000";
                else
                    (sender as TextBox).Text = "#FFFFFF";

                MakeColorFromHex((sender as TextBox).Text);
            }
            previewColor.Fill = new SolidColorBrush(SelectedColor);
            UpdateARGB();
            UpdateAlphaSliderValue();
        }

        #endregion

        #region Set
        private void UpdateARGB()
        {
            textBoxA.Text = SelectedColor.A.ToString();
            textBoxR.Text = SelectedColor.R.ToString();
            textBoxG.Text = SelectedColor.G.ToString();
            textBoxB.Text = SelectedColor.B.ToString();
        }
        private void UpdateHEXValue()
        {
            hexColor.Text = $"#{SelectedColor.R:X2}{SelectedColor.G:X2}{SelectedColor.B:X2}";
        }
        private void UpdateAlphaSliderValue()
        {
            colorAlpha.Value = SelectedColor.A;
        }
        #endregion

        #region Get
        private bool MakeColorFromHex(string colorText)
        {
            try
            {
                ColorConverter cc = new ColorConverter();
                SelectedColor = (Color)cc.ConvertFrom(colorText);

            }
            catch
            {
                return false;
            }
            return true;
        }
        private Color MakeColorFromRGB()
        {
            byte abyteValue = Convert.ToByte(textBoxA.Text);
            byte rbyteValue = Convert.ToByte(textBoxR.Text);
            byte gbyteValue = Convert.ToByte(textBoxG.Text);
            byte bbyteValue = Convert.ToByte(textBoxB.Text);
            Color rgbColor =
                 Color.FromArgb(
                     abyteValue,
                     rbyteValue,
                     gbyteValue,
                     bbyteValue);
            return rgbColor;
        }
        private void colorAlpha_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBoxA.Text = ((int)e.NewValue).ToString();
            SelectedColor = MakeColorFromRGB();
            previewColor.Fill = new SolidColorBrush(SelectedColor);
            UpdateHEXValue();
        }

        #endregion

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            IsUpdate = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsUpdate = false;
            this.Close();
        }
        #region Advance Color Pallete
        
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouse)
            {
                try
                {
                    var cb = new CroppedBitmap((BitmapSource)(((Image)e.Source).Source),
                        new Int32Rect((int)Mouse.GetPosition(e.Source as Image).X,
                        (int)Mouse.GetPosition(e.Source as Image).Y, 1, 1));
                    pixels = new byte[4];
                    cb.CopyPixels(pixels, 4, 0);
                    SelectedColor = Color.FromRgb(pixels[2], pixels[1], pixels[0]);                    
                    previewColor.Fill = new SolidColorBrush(SelectedColor);
                    advanceColorPallleteBG.Background = new SolidColorBrush(SelectedColor);
                    UpdateARGB();
                    UpdateHEXValue();
                }
                catch (Exception)
                {
                }
            }
        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouse = false;
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouse = true;
        }
        #endregion

        public void AppClose()
        {
            this.Close();
        }
    }
}
