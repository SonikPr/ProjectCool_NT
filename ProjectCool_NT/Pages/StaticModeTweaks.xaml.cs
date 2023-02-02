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
    /// Interaction logic for StaticModeTweaks.xaml
    /// </summary>
    public partial class StaticModeTweaks : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public StaticModeTweaks()
        {
            InitializeComponent();
            ProjectCoolDevice.LoadDevice();
            UpdateInfo();

        }
        void UpdateInfo()
        {
            LED_hue.Value = ProjectCoolDevice.Hue;
            LED_sat.Value = ProjectCoolDevice.Sat;
        }
    }
}
