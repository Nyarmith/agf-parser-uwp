using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public class SaveGameInfo {
        public string title;
        public string author;
        public string saveDate;
        public ActiveGame gameRef; //reference to game for GameView passing
        public SaveGameInfo() { }
        public SaveGameInfo(string title_, string author_, string saveDate_, ActiveGame gameref_)
        {
            title    = title_;
            author   = author_;
            saveDate = saveDate_;
            gameRef = gameref_;
        }
    }

    public sealed partial class ResumePage : Page, INotifyPropertyChanged
    {
        //data source and refresh-trigger
        public ObservableCollection<SaveGameInfo> Games = new ObservableCollection<SaveGameInfo>();
        public event PropertyChangedEventHandler PropertyChanged;

        public ResumePage()
        {
            this.InitializeComponent();
            updateGames();
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

        async void updateGames()
        {
            Games = new ObservableCollection<SaveGameInfo>();

            List<string> fileList = await UWPIO.listFiles(UWPIO.SAVEDIR);

            foreach (string f in fileList)
            {
                int l = f.Length;
                if (f.Substring(l - 5) == ".json" || f.Substring(l-4) == ".agf")
                {
                    string fname = UWPIO.SAVEDIR + "\\" + f;
                    string content = await UWPIO.readFile(fname);
                    ActiveGame ag = loadFromString(content);

                    Games.Add(new SaveGameInfo(ag.getTitle(), ag.getAuthor(),
                        await UWPIO.dateModifiedAsync(fname), ag ));
                }
            }

            updateProperty(nameof(Games));
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void ResumeGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveGameInfo g = e.ClickedItem as SaveGameInfo;
            this.Frame.Navigate(typeof(GameView), g.gameRef);
        }
    }
}
