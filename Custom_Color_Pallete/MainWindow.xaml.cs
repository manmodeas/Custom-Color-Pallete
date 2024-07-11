using Custom_Color_Pallete.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Custom_Color_Pallete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ColorDialog colorDialog;
        public MainWindow()
        {
            InitializeComponent();
            btnColor.Background = Brushes.Black;
            colorDialog = new ColorDialog(true);
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            if (colorDialog.IsVisible)
            {
                if (colorDialog.WindowState == WindowState.Minimized)
                    colorDialog.WindowState = WindowState.Normal;
                colorDialog.Activate();
                return;
            }
            colorDialog = new ColorDialog(true);
            colorDialog.ShowDialog();
            if (colorDialog.IsUpdate)
                ((Button)sender).Background = new SolidColorBrush(colorDialog.SelectedColor);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            colorDialog?.AppClose();
        }
    }
}
