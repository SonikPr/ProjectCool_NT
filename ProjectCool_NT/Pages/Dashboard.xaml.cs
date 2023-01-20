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
            Brightness.Value = Convert.ToInt32(SensorsData.Pop());
            FanRPM.Value = Convert.ToInt32(SensorsData.Pop());
            FanCFM.Value = Convert.ToInt32(SensorsData.Pop());
            FanSpeed.Value = Convert.ToInt32(SensorsData.Pop());
            CaseHumidity.Value = Convert.ToInt32(SensorsData.Pop());
            CaseTemp.Value = Convert.ToInt32(SensorsData.Pop());
        }

        private Stack<string> SensorsData = new Stack<string>();
        void RecieveSensorData()
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
