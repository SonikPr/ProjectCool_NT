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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {

        Class.Device ProjectCoolDevice = new Class.Device();
        public About()
        {
            InitializeComponent();
            ProjectCoolDevice.LoadDevice();
            UpdateAll();
        }

        private void UpdateAll()
        {
            HardwareCodename.Text = "ProjectCool Dash";
            HardwareVersion.Text = ProjectCoolDevice.model_name;
            SoftwareCodename.Text = ProjectCoolDevice.software_version;
            FanConnction.Text = ProjectCoolDevice.Fan_Connection;
            MaximumFanSupport.Text = ProjectCoolDevice.Max_Fan_Support;
            ConnectedFans.Text = ProjectCoolDevice.FanDescription + ", " + (ProjectCoolDevice.IntakeFanCount + ProjectCoolDevice.ExhaustFanCount).ToString() + "pcs.";
            FanTachoType.Text = ProjectCoolDevice.Fan_Tacho_Type;
            TempSensorType.Text = ProjectCoolDevice.temp_Sensor;
            FanPWMRate.Text = ProjectCoolDevice.PWM_Rate;
            ConnectedLEDs.Text = ProjectCoolDevice.leds_description;
            LEDtype.Text = ProjectCoolDevice.LED_Type;

            PCver.Text = "2.0";
            Codename.Text = "ProjectCool_NewTechnology";
            SupportedProducts.Text = "ProjectCool1.0,SW5.0";
            Credits.Text = "Made by SonikPr. SonikPr 2023, all rights reserved";

        }
    }
}
