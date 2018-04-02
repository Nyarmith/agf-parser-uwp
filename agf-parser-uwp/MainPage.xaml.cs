using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        public string currentPage = "Welcome to Adventure-Game UWP Client!";

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            this.InitializeComponent();
            updateTitle();
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
                currentPage = "Settings";
                updateTitle();
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                Navigate(item);
                updateTitle();
            }
        }

        private void updateTitle()
        {
            MainNavView.Header = currentPage;
        }

        private void Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Play":
                    currentPage = "Play";
                    ContentFrame.Navigate(typeof(PlayList));
                    break;

                case "Resume":
                    //List Games In Progress
                    currentPage = "In-Progress";
                    ContentFrame.Navigate(typeof(ResumePage));
                    break;

                case "Import":
                    currentPage = "Browsing Local Filesystem";
                    ContentFrame.Navigate(typeof(ImportPage));
                    break;

                case "BrowseNet":
                    currentPage = "Browsing Network Adventures";
                    ContentFrame.Navigate(typeof(BrowsePageNet));
                    break;

                case "BrowseLocal":
                    currentPage = "Browsing Adventure Collection";
                    ContentFrame.Navigate(typeof(GamesList));
                    break;

                case "Creator":
                    currentPage = "Adventure-Game Studio";
                    ContentFrame.Navigate(typeof(Creator));
                    //consider using base.OnNavigatedTo here
                    break;
            }

            //updateProperty(nameof(currentPage));
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
