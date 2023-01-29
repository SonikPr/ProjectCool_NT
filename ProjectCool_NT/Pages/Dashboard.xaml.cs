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

namespace ProjectCool_NT.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DispatcherTimer UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            UpdateDataTimer.Tick += UpdateData;
            UpdateDataTimer.Start();
        }
        int FanSpeedValue;
        void UpdateData(object sender, EventArgs e)
        {

            RecieveSensorData();
            if (SensorsData.Count > 0)
            {
                int fan_rpm = Convert.ToInt32(SensorsData.Pop()); ;
                int fan_speed = Convert.ToInt32(SensorsData.Pop()) ;
                double humidity = Convert.ToDouble(SensorsData.Pop());
                double temp = Convert.ToDouble(SensorsData.Pop());

                humidity = humidity / 10;
                temp = temp / 10;

                FanRPM.Value = fan_rpm;
                FanRPM.Tag = map(fan_rpm,0,100,0,1100).ToString();
                FanSpeed.Value = fan_speed;
                FanSpeed.Tag = fan_speed.ToString();
                CaseHumidity.Value = (int)humidity;
                CaseHumidity.Tag = humidity.ToString();
                CaseTemp.Value = map((int)temp,20,50,0,100);
                CaseTemp.Tag = temp.ToString();   
                SensorsData.Clear();
            }
        }

        int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private Stack<string> SensorsData = new Stack<string>();
        void RecieveSensorData()
        {
            string file = "SensorData.sensors";
            if (File.Exists(file))
            {
                string NewData;
                using (StreamReader DatabaseFetcher = new StreamReader("SensorData.sensors"))
                {
                    while ((NewData = DatabaseFetcher.ReadLine()) != null)
                    {
                        SensorsData.Push(NewData);
                    }
                }
            }
        }
    }
}
