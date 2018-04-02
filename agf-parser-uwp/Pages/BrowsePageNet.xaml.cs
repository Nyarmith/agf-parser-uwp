using System;
using System.ComponentModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace agf_parser_uwp
{
    public sealed partial class BrowsePageNet : Page, INotifyPropertyChanged
    {
        private WebAPI netapi;
        public BrowsePageNet()
        {
            this.InitializeComponent();
            netapi = new WebAPI();
            refreshCurrentGames();
        }

        //get the freshest games on the web
        public void refreshCurrentGames()
        {
            List<string>  netfiles = netapi.listFiles();
            foreach(string nf in netfiles)
            {
                Games.Add(new GameInfo(nf,"network-author",nf,"today?","today!"));
            }
        }

        public ObservableCollection<GameInfo> Games { get; } = new ObservableCollection<GameInfo>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)Games).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)Games).PropertyChanged -= value;
            }
        }
    }
}
