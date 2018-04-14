using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

            if (Games.Count == 0)
            {
                UpdateFiles();
            }

            base.OnNavigatedTo(e);
        }

        private async void UpdateFiles()
        {
            List<string> files = await UWPIO.listFiles(UWPIO.GAMEDIR);

            foreach (string file in files)
            {
                int l = file.Length;
                if (file.Substring(l-5) == ".json" || file.Substring(l-4) == ".agf")
                {
                    string fname = UWPIO.GAMEDIR + "\\" + file;
                    string contents = await UWPIO.readFile(fname);
                    AdventureGame ag = AdventureGame.loadFromString(contents);
                    Games.Add(new GameInfo(ag.title, ag.author, file,
                        await UWPIO.dateCreatedAsync(fname),
                        await UWPIO.dateModifiedAsync(fname)));
                }
            }
        }

        private void FitScreenToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }

        private void ImageGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //TODO: details
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
