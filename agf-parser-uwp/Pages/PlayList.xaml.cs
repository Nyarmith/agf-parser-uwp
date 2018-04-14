using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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

        private async void UpdateFiles()
        {

            List<string> files = await UWPIO.listFiles(UWPIO.GAMEDIR);
            foreach (string file in files)
            {
                // Limit to only json or agf files.
                int l = file.Length;
                if (file.Substring(l - 5) == ".json" || file.Substring(l - 4) == ".agf")
                {
                    string fname = UWPIO.GAMEDIR + "\\" + file;
                    AdventureGame ag = AdventureGame.loadFromString(await UWPIO.readFile(fname));
                    Games.Add(new GameInfo(ag.title, ag.author, file,
                        await UWPIO.dateCreatedAsync(fname),
                        await UWPIO.dateModifiedAsync(fname) ));
                }
            }
        }

        public static ActiveGame loadFromString(string json_str)
        {
            ActiveGame ag = JsonConvert.DeserializeObject<ActiveGame>(json_str);
            return ag;
        }

        public static ActiveGame loadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("Error, no file found at path: " + path);
            }
            string contents = "";
            contents = File.ReadAllText(path);

            return loadFromString(contents);
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
        }
    }

}
