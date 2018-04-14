using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace agf_parser_uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameView : Page, INotifyPropertyChanged
    {
        private ActiveGame game;
        public List<string> choices=new List<string>();
        public string currentText="init";

        public event PropertyChangedEventHandler PropertyChanged;

        public GameView()
        {
            this.InitializeComponent();
        }

        //option click handler
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //depending on who entered, we do different things
            //load activeGame
            dynamic btn = e.Parameter;
            string file_path = btn.Tag;
            AdventureGame ag = AdventureGame.loadFromString(await UWPIO.readFile(UWPIO.GAMEDIR + "\\" + file_path));
            game = new ActiveGame(ag);
            game.start();

            base.OnNavigatedTo(e); //idk what this does

            refresh();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            //auto-save if game not finished
            if (!game.isEnd())
            {
                string fullName = UWPIO.SAVEDIR + "\\" + game.getTitle() + game.getAuthor() + ".json";
                await UWPIO.createFile(fullName, saveToString(game));
            }
            base.OnNavigatedFrom(e);
        }

        public static string saveToString(ActiveGame adv_obj)
        {
            string ser = JsonConvert.SerializeObject(adv_obj);
            return ser;
        }


        private void Option_Click(object sender, RoutedEventArgs e)
        {
            dynamic o = e.OriginalSource;//.Content.Text;
            string opt = o.Content.Text;
            int i = choices.FindIndex(a => a == opt);

            game.choose(i);
            refresh();
        }

        private void refresh()
        {
            //populates elements I guess
            //this.GameChoices;
            choices=new List<string>();
            foreach (string opt in game.getChoices())
            {
                choices.Add(opt);
            }
            updateProperty(nameof(choices));
            //update dialog
            currentText = game.getText();
            updateProperty(nameof(currentText));
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        //menu click handler
    }
}
