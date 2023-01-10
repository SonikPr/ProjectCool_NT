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

namespace ProjectCool_NT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach(UIElement Radiobutton in MenuButtonsContainer.Children)
            {
                if(Radiobutton is RadioButton)
                {
                    ((RadioButton)Radiobutton).Checked += MenuItem_Checked;
                }
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)e.OriginalSource).Name)
            {
                case "HomeButton":
                    PageContainer.Navigate(new System.Uri("Pages/Home.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "DashboardButton":
                    PageContainer.Navigate(new System.Uri("Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "LEDButton":
                    PageContainer.Navigate(new System.Uri("Pages/LED.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "FanButton":
                    PageContainer.Navigate(new System.Uri("Pages/Fan.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "SettingsButton":
                    PageContainer.Navigate(new System.Uri("Pages/Settings.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "AboutButton":
                    PageContainer.Navigate(new System.Uri("Pages/About.xaml", UriKind.RelativeOrAbsolute));
                    break;
            }
        }

        private void DragDropHandler (object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
