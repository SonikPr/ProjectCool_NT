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
    /// Interaction logic for BreatheTweaks.xaml
    /// </summary>
    public partial class BreatheTweaks : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public BreatheTweaks()
        {
                InitializeComponent();
                ProjectCoolDevice.LoadDevice();
                UpdateInfo();

            }
            void UpdateInfo()
            {
                Breathe_speed.Value = ProjectCoolDevice.BreatheSpeed;
            }

        }
}
