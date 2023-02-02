using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectCool_NT.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public Settings()
        {
            InitializeComponent();
            ProjectCoolDevice.LoadDevice();
            UpdateInfo();

        }
        void UpdateInfo()
        {
            PortSelect.ItemsSource = ProjectCoolDevice.AvailablePorts;
            FanDescription.Text = ProjectCoolDevice.FanDescription;
            IntakeFanCount.Text = ProjectCoolDevice.IntakeFanCount.ToString();
            ExhaustFanCount.Text = ProjectCoolDevice.ExhaustFanCount.ToString();
            MaxRPM.Text = ProjectCoolDevice.MaxRPM.ToString();
            MaxCFM.Text = ProjectCoolDevice.MaxCFM.ToString();
            LEDDescription.Text = ProjectCoolDevice.leds_description;
        }
    }
}
