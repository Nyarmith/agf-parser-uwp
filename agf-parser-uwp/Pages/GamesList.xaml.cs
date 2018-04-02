using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//initializecomponent https://stackoverflow.com/questions/6925584/the-name-initializecomponent-does-not-exist-in-the-current-context

namespace agf_parser_uwp
{
    public sealed partial class GamesList : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<GameInfo> Games { get; } = new ObservableCollection<GameInfo>();

        public GamesList()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;

            // Remove this when replaced with XAML bindings
            // Replaced with XAML binding
            // ImageGridView.ItemsSource = Images;

            if (Games.Count == 0)
            {
                UpdateFiles();
            }

            base.OnNavigatedTo(e);
        }

        /*
        //TODO: Implement this
        // Called by the Loaded event of the ImageGridView.
        private async void StartConnectedAnimationForBackNavigation()
        {
            // Run the connected animation for navigation back to the main page from the detail page.
            if (persistedItem != null)
            {
                ImageGridView.ScrollIntoView(persistedItem);
                ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("backAnimation");
                if (animation != null)
                {
                    await ImageGridView.TryStartConnectedAnimationAsync(animation, persistedItem, "ItemImage");
                }
            }
        }
        */

        private void UpdateFiles()
        {
            // https://docs.microsoft.com/uwp/api/windows.ui.xaml.controls.image#Windows_UI_Xaml_Controls_Image_Source
            // See "Using a stream source to show images from the Pictures library".
            // This code is modified to get images from the app folder.

            // Get the app folder where the images are stored.
            string folderpath = Package.Current.InstalledLocation.Path + "\\Assets\\Adventures\\";

            string [] files = System.IO.Directory.GetFiles(folderpath);
            foreach (string file in files)
            {
                // Limit to only json or agf files.
                int l = file.Length;
                if (file.Substring(l-5) == ".json" || file.Substring(l-4) == ".agf")
                {
                    //get file info from making an ag object
                    AdventureGame ag = AdventureGame.loadFromFile(file);
                    System.IO.FileInfo finfo = new System.IO.FileInfo(file);
                    Games.Add(new GameInfo(ag.title, ag.author, finfo.CreationTime.ToShortDateString(),
                        finfo.CreationTime.ToLongDateString(),
                        finfo.LastAccessTime.ToShortDateString()));
                }
            }
        }

        private void FitScreenToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void ImageGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Frame.Navigate(typeof(DetailPage), e.ClickedItem);
            //do a thing
        }

        private void DetermineItemSize()
        {
            if (FitScreenToggle != null
                && FitScreenToggle.IsOn == true
                && ImageGridView != null
                && ZoomSlider != null)
            {
                // The 'margins' value represents the total of the margins around the
                // image in the grid item. 8 from the ItemTemplate root grid + 8 from
                // the ItemContainerStyle * (Right + Left). If those values change, 
                // this value needs to be updated to match.
                int margins = (int)this.Resources["LargeItemMarginValue"] * 4;
                double gridWidth = ImageGridView.ActualWidth - (int)this.Resources["DesktopWindowSidePaddingValue"];

                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile" &&
                    UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Touch)
                {
                    margins = (int)this.Resources["SmallItemMarginValue"] * 4;
                    gridWidth = ImageGridView.ActualWidth - (int)this.Resources["MobileWindowSidePaddingValue"];
                }

                double ItemWidth = ZoomSlider.Value + margins;
                // We need at least 1 column.
                int columns = (int)Math.Max(gridWidth / ItemWidth, 1);

                // Adjust the available grid width to account for margins around each item.
                double adjustedGridWidth = gridWidth - (columns * margins);

                ItemSize = (adjustedGridWidth / columns);
            }
            else
            {
                ItemSize = ZoomSlider.Value;
            }
        }

        public double ItemSize
        {
            get => _itemSize;
            set
            {
                if (_itemSize != value)
                {
                    _itemSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemSize)));
                }
            }
        }

        private double _itemSize;


    }
}
