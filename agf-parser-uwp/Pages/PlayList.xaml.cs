using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace agf_parser_uwp
{
    public class PlayListSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate SelectedItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(Object item, DependencyObject container)
        {
            if (container is GridViewItem c)
            {
                if (c.Tag != null && long.TryParse(c.Tag.ToString(), out var tkn))
                {
                    c.UnregisterPropertyChangedCallback(GridViewItem.IsSelectedProperty, tkn);
                }

                c.Tag = c.RegisterPropertyChangedCallback(GridViewItem.IsSelectedProperty, (s, e) =>
                    { c.ContentTemplateSelector = null; c.ContentTemplateSelector = this; });

                if (c.IsSelected)
                    return SelectedItemTemplate;

            }
            return DefaultTemplate;
            /*
            var cont = container as GridViewItem;
            if (cont != null)
            {
                if (cont.IsSelected)
                    return SelectedItemTemplate;
            }
            return DefaultTemplate;
            */
        }
    }

    public sealed partial class PlayList : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<GameInfo> Games { get; } = new ObservableCollection<GameInfo>();

        //private GameInfo selectedItem;

        public PlayList()
        {
            this.InitializeComponent();
            UpdateFiles();
        }


        //when a dude is clicked on a play button should appear, and when they're unclicked it should disappear

        private void UpdateFiles()
        {
            // https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.image#Windows_UI_Xaml_Controls_Image_Source
            // See "Using a stream source to show images from the Pictures library".
            // This code is modified to get images from the app folder.

            // Get the app folder where the images are stored.
            string folderpath = Package.Current.InstalledLocation.Path + "\\Assets\\Adventures\\";

            string[] files = System.IO.Directory.GetFiles(folderpath);
            foreach (string file in files)
            {
                // Limit to only json or agf files.
                int l = file.Length;
                if (file.Substring(l - 5) == ".json" || file.Substring(l - 4) == ".agf")
                {
                    //get file info from making an ag object
                    AdventureGame ag = AdventureGame.loadFromFile(file);
                    System.IO.FileInfo finfo = new System.IO.FileInfo(file);
                    Games.Add(new GameInfo(ag.title, ag.author, file,
                        finfo.CreationTime.ToLongDateString(),
                        finfo.LastAccessTime.ToShortDateString()));
                }
            }
        }

        //undo current selection, bring that item's display back to normal
        void deselect()
        {

        }

        void select(GameInfo game)
        {

        }

        private void ImageGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView)
                gvhandle((GridView)sender);
        }

        private void gvhandle(GridView gv)
        {
            var curitem = gv.SelectedItem;
            //do things to it
        }

        //play the linked game
        private async void playHandler(object sender, RoutedEventArgs e)
        {
            var obj = e.OriginalSource;

            //compromise: we're going to navigate in the same view, but minimize the side-bar
            base.Frame.Navigate(typeof(GameView), e.OriginalSource);
            //this.Frame.Navigate(typeof(GameView), e.OriginalSource);

            //change the view to GameView w/ panel
            //base.Frame.Navigate(typeof(GameView), e.OriginalSource);

            /*
            CoreApplicationView newView = CoreApplication.CreateNewView();

            int newViewId = 0;
            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(GameView), null);
                Window.Current.Content = frame;
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            */

            // one of the following two might suffice:
            // - base.OnNavigatedTo
            // - this.Frame.Navigate(typeof(DetailPage), e.ClickedItem);
        }
    }

}
