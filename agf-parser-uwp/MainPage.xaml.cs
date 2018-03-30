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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace agf_parser_uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {

            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in MainNavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "apps")
                {
                    MainNavView.SelectedItem = item;
                    break;
                }
            }
        }

        private void ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                Navigate(item as NavigationViewItem);

            }
        }

        private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                Navigate(item);
            }
        }

        private void Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Play":
                    ContentFrame.Navigate(typeof(PlayList));
                    break;

                case "Resume":
                    //List Games In Progress
                    ContentFrame.Navigate(typeof(ResumePage));
                    break;

                case "Import":
                    ContentFrame.Navigate(typeof(ImportPage));
                    break;

                case "BrowseNet":
                    ContentFrame.Navigate(typeof(BrowsePageNet));
                    break;

                case "BrowseLocal":
                    ContentFrame.Navigate(typeof(GamesList));
                    break;

                case "Creator":
                    ContentFrame.Navigate(typeof(Creator));
                    //consider using base.OnNavigatedTo here
                    ContentFrame.Navigate(typeof(Creator));
                    break;
            }
        }

    }
}
