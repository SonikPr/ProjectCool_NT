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
    /// Interaction logic for DynamicBrightnessTweaks.xaml
    /// </summary>
    public partial class DynamicBrightnessTweaks : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public DynamicBrightnessTweaks()
        {
            InitializeComponent();
            ProjectCoolDevice.LoadDevice();
            UpdateInfo();
        }
        void UpdateInfo()
        {
            switch (ProjectCoolDevice.variable_brightness_mode)
            {
                case 0:
                    DynamicColor.IsChecked = true;
                    break;
                case 1:
                    FlameColor.IsChecked = true;
                    break;
                default:
                    break;
            }
            switch (ProjectCoolDevice.variable_brightness_value)
            {
                case 0:
                    DisplayTemp.IsChecked = true;
                    break;
                case 1:
                    DisplayFanSpeed.IsChecked = true;
                    break;
                case 2:
                    DisplayHumidity.IsChecked = true;
                    break;
                default:
                    break;
            }
        }


    }
}
