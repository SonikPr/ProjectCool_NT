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
using System.Windows.Threading;
using System.IO;
using System.Threading;

namespace ProjectCool_NT.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public Dashboard()
        {
            InitializeComponent();
            DispatcherTimer UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(500);
            UpdateDataTimer.Tick += UpdateData;
            while (true)
            {
                ProjectCoolDevice.LoadDevice();
                Thread.Sleep(700);
                if (ProjectCoolDevice.model_name.Contains("PC"))
                {
                    break;
                }
            }
            UpdateDataTimer.Start();
            UpdateInfo();

        }
        void UpdateInfo()
        {
            SensorType.Text = "SensorType: " + ProjectCoolDevice.temp_Sensor;
            FanConnection.Text = "Fans: " + ProjectCoolDevice.FanDescription;
            LedDescription.Text = "LEDs: "+ ProjectCoolDevice.leds_description;
            dashboardFanMode.Text = ProjectCoolDevice.FanModeName();
            dashboardActiveLEDMode.Text = ProjectCoolDevice.LedModeName();
        }
        void UpdateData(object sender, EventArgs e)
        {
                ProjectCoolDevice.RestoreSensor(); 
                int fan_speed = ProjectCoolDevice.ProgramFanSpeed;
                double humidity = ProjectCoolDevice.chassis_humidity;
                double temp = ProjectCoolDevice.chassis_temp;
                double CFM = ProjectCoolDevice.GetRealCFM();
                double RPM = ProjectCoolDevice.GetRealRPM();
                int brightness = ProjectCoolDevice.Brightness; 

                humidity = humidity / 10;
                temp = temp / 10;

                FanRPM.Value = map((int)RPM,0,ProjectCoolDevice.MaxRPM,0,100);
                FanRPM.Tag = RPM.ToString();
                FanCFM.Value = map((int)CFM,0,(int)ProjectCoolDevice.MaxCFM,0,100);
                FanCFM.Tag = CFM.ToString();
                FanSpeed.Value = fan_speed;
                FanSpeed.Tag = fan_speed.ToString();
                CaseHumidity.Value = (int)humidity;
                CaseHumidity.Tag = humidity.ToString();
                CaseTemp.Value = map((int)temp,20,50,0,100);
                CaseTemp.Tag = temp.ToString();
                Brightness.Value = brightness;
                Brightness.Tag = brightness.ToString(); 
        }

        int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }
}
