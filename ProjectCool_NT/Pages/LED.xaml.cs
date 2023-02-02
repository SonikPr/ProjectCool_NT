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
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : Page
    {
        Class.Device ProjectCoolDevice = new Class.Device();
        public LED()
        {
            InitializeComponent();

            foreach (UIElement Radiobutton in SwitchGrid.Children)
            {
                if (Radiobutton is RadioButton)
                {
                    ((RadioButton)Radiobutton).Checked += MenuItem_Checked;
                }
            }
            ProjectCoolDevice.LoadDevice();
            UpdateInfo();

        }
        void UpdateInfo()
        {
            LedDescription.Text = "LEDs: " + ProjectCoolDevice.leds_description;
            LEDBrightness.Value = ProjectCoolDevice.Brightness;
            switch (ProjectCoolDevice.Mode)
            {
                case 0:
                    StaticMode.IsChecked = true;
                    break;
                case 1:
                    SpectreMode.IsChecked = true;
                    break;
                case 2:
                    RainbowMode.IsChecked = true;
                    break;
                case 3:
                    BreatheMode.IsChecked = true;
                    break;
                case 4:
                    FlameMode.IsChecked = true;
                    break;
                case 5:
                    FSGMode.IsChecked = true;
                    break;
                case 6:
                    RunningLineMode.IsChecked = true;
                    break;
                case 7:
                    DybBrMode.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)e.OriginalSource).Name)
            {
                case "StaticMode":
                    TweakContainer.Navigate(new System.Uri("Pages/StaticModeTweaks.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "SpectreMode":
                    TweakContainer.Navigate(new System.Uri("Pages/SpectreTweaks.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "BreatheMode":
                    TweakContainer.Navigate(new System.Uri("Pages/BreatheTweaks.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "DybBrMode":
                    TweakContainer.Navigate(new System.Uri("Pages/DynamicBrightnessTweaks.xaml", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    TweakContainer.Navigate(null);
                    break;
            }
        }
    }
}
