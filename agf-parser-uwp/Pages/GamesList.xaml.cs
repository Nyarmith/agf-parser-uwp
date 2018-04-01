//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

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
    public sealed partial class GamesList : Page
    {
        public ObservableCollection<GameInfo> Games { get; } = new ObservableCollection<GameInfo>();

        public GamesList()
        {
            //this.InitializeComponent();
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
                    AdventureGame ag = AdventureGame.loadFromFile(folderpath + file);
                    System.IO.FileInfo finfo = new System.IO.FileInfo(folderpath + file);
                    Games.Add(new GameInfo(finfo.Name,finfo.CreationTime.ToShortDateString(),
                        finfo.CreationTime.ToLongDateString(),
                        finfo.LastAccessTime.ToShortDateString(),
                        ag.author));
                }
            }
        }

        private void FitScreenToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void ImageGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Frame.Navigate(typeof(DetailPage), e.ClickedItem);
        }
    }
}
