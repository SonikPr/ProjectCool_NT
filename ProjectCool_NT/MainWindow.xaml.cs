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
using System.Windows.Threading;
using System.IO;
using System.Threading;

namespace ProjectCool_NT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();
            foreach (UIElement Button in UpperMenuGrid.Children)
            {
                if (Button is Button)
                {
                    ((Button)Button).Click += TaskbarClick;
                }
            }

            foreach (UIElement Radiobutton in MenuButtonsContainer.Children)
            {
                if (Radiobutton is RadioButton)
                {
                    ((RadioButton)Radiobutton).Checked += MenuItem_Checked;
                }
            }   
            ProjectCoolDevice.CreateDevice();
            PageContainer.Navigate(new System.Uri("Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute));
        }
        

        private void TaskbarClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)e.OriginalSource).Name)
            {
                case "Minimize":
                    WindowState = WindowState.Minimized;
                    break;
                case "Close":
                    Shutdown();
                    break;
            }
        }

        private void Shutdown()//This will be used in future to secure port closing, saving all the setings and safe app shutdown
        {
            ProjectCoolDevice.DeleteDevice();
            App.Current.Shutdown();
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

        void timer_Tick(object sender, EventArgs e)
        {
            ProjectCoolDevice.GetSensors();
            
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
