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
