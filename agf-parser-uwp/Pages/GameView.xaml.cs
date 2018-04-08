using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //load activeGame
            dynamic btn = e.Parameter;
            string file_path = btn.Tag;
            AdventureGame ag = AdventureGame.loadFromFile(file_path);
            game = new ActiveGame(ag);

            base.OnNavigatedTo(e); //idk what this does

            refresh();
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
