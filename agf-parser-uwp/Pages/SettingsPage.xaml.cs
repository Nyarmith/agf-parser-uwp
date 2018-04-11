using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace agf_parser_uwp
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void VaToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch o = e.OriginalSource as ToggleSwitch;
            GlobSetting g = GlobSetting.getInstance();
            g.text_speech = o.IsOn;
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //do a thing
            ComboBox o = sender as ComboBox;
            ComboBoxItem n = o.SelectedItem as ComboBoxItem;
            // do something with n.Content;
        }
    }
}
