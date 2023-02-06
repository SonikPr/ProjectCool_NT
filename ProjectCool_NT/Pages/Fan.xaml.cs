using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;
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

namespace ProjectCool_NT.Pages
{
    /// <summary>
    /// Interaction logic for Fan.xaml
    /// </summary>
    public partial class Fan : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public Fan()
        {
            InitializeComponent();
            DispatcherTimer UpdateDataTimer = new DispatcherTimer();
            UpdateDataTimer.Interval = TimeSpan.FromMilliseconds(500);
            UpdateDataTimer.Tick += UpdateData;
            while (true)
            {
                ProjectCoolDevice.LoadDevice(); 
                if (ProjectCoolDevice.model_name.Contains("PC"))
                {
                    break;
                }
                Thread.Sleep(500);
            }
            UpdateDataTimer.Start();
            UpdateInfo();

        }
        void UpdateInfo()
        {
            FanConnection.Text = "Fans: " + ProjectCoolDevice.FanDescription;
            switch (ProjectCoolDevice.CurrentFanMode)
            {
                case 0:
                    Default.IsChecked = true;
                    break;
                case 1:
                    Cool.IsChecked = true;
                    break;
                case 2:
                    Quiet.IsChecked = true;
                    break;
                case 3:
                    FanOff.IsChecked = true;
                    break;
                case 4:
                    Manual.IsChecked = true;
                    break;
                case 5:
                    Fan100.IsChecked = true;
                    break;
                default:
                    break;
            }
            ManualSpeedSetting_slider.Value = ProjectCoolDevice.ManualFanSpeed;
            Hysteresis_slider.Value = ProjectCoolDevice.Hysteresis;
            UpdateModeInfo();
        }

        void UpdateModeInfo()
        {
            Selected_mode_label.Text = ProjectCoolDevice.FanModeName();
            Selected_mode_description_label.Text = ProjectCoolDevice.GetFanModeDescription();
            MinDeg.Text = ProjectCoolDevice.GetMinTemp().ToString();
            MaxDeg.Text = ProjectCoolDevice.GetMaxTemp().ToString();
        }

        void UpdateData(object sender, EventArgs e)
        {
            ProjectCoolDevice.RestoreSensor();
            int fan_speed = ProjectCoolDevice.ProgramFanSpeed;
            if (ProjectCoolDevice.model_name == "PC1.0")
            {
                double CFM = ProjectCoolDevice.GetRealCFM();
                double RPM = ProjectCoolDevice.GetRealRPM();
                TachoRPM.Value = map((int)RPM, 0, ProjectCoolDevice.MaxRPM, 0, 100);
                TachoRPM.Tag = RPM.ToString();          
                IntakeCFM.Value = map((int)(CFM * ProjectCoolDevice.IntakeFanCount), 0, (int)(ProjectCoolDevice.MaxCFM * ProjectCoolDevice.IntakeFanCount), 0, 100);
                IntakeCFM.Tag = (CFM * ProjectCoolDevice.IntakeFanCount).ToString();
                ExhaustCFM.Value = map((int)(CFM * ProjectCoolDevice.ExhaustFanCount), 0, (int)(ProjectCoolDevice.MaxCFM * ProjectCoolDevice.ExhaustFanCount), 0, 100);
                ExhaustCFM.Tag = (CFM * ProjectCoolDevice.ExhaustFanCount).ToString();
            }
            else
                if (ProjectCoolDevice.model_name == "PC1.1")
            {
                int maxRPM = ProjectCoolDevice.TachoFanSpeed;
                if (maxRPM > ProjectCoolDevice.MaxRPM)
                {
                    ProjectCoolDevice.MaxRPM = maxRPM;
                }

                TachoRPM.Value = map(ProjectCoolDevice.TachoFanSpeed, 0, ProjectCoolDevice.MaxRPM, 0, 100);
                TachoRPM.Tag = ProjectCoolDevice.TachoFanSpeed.ToString();

                double CFM = (double)ProjectCoolDevice.MaxCFM * ((double)ProjectCoolDevice.TachoFanSpeed / (double)ProjectCoolDevice.MaxRPM);
                CFM = Math.Round(CFM, 2);
                IntakeCFM.Value = map((int)(CFM * ProjectCoolDevice.IntakeFanCount), 0, (int)(ProjectCoolDevice.MaxCFM * ProjectCoolDevice.IntakeFanCount), 0, 100);
                IntakeCFM.Tag = Math.Round((CFM * ProjectCoolDevice.IntakeFanCount),2).ToString();
                ExhaustCFM.Value = map((int)(CFM * ProjectCoolDevice.ExhaustFanCount), 0, (int)(ProjectCoolDevice.MaxCFM * ProjectCoolDevice.ExhaustFanCount), 0, 100);
                ExhaustCFM.Tag = Math.Round((CFM * ProjectCoolDevice.ExhaustFanCount),2).ToString();
            }
            TargetRPM.Value = fan_speed;
            TargetRPM.Tag = fan_speed.ToString();
        }

        int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
