using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace agf_parser_uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowsePageLocal : Page
    {
        private FileInfo chosenItem;
        public ObservableCollection<FileInfo> Files { get; } = new ObservableCollection<FileInfo>();

        public BrowsePageLocal()
        {
            this.InitializeComponent();
        }

        private void InitStorageFolder()
        {
            /*
            Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;

            System.IO.DirectoryInfo(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            */

        }
    }
}
