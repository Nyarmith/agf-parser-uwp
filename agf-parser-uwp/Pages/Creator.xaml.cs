using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Popups;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace agf_parser_uwp
{

    // TODO: add on-transiton function that lets you pass an activegame or adventuregame
    // TODO: add arrow to the right of the transitions so you can follow them
    // TODO: add visual indicator for when transition requirements not met and trying to transition(2nd click to force)
    // TODO: add import functionality for this editor
    // TODO: make save pop up a filepicker dialogue to create a file

    public sealed partial class Creator : Page, INotifyPropertyChanged
    {

        public const string DEF_GAME_TXT = @"{
  ""title"":""basic addition"",
  ""author"":""sergey"",
  ""gamevars"" : { ""inventory"":{ ""baseball"":true, ""bat"":true}, ""user"":{ } },
  ""win_states"" : [""right_result""],
  ""start_state"" : ""question"",
  ""states"" : {

    ""question"" : {""text"":""do you know what 2+2 is?"", ""options"":[
      ["""","""",""right_result"",""Yes, 4""],
     ["""","""",""wrong_result"",""Yes, 3""],
      ["""",""user::error=True"",""answer"",""No Just Tell Me""]
    ]},

    ""right_result"" : {
      ""text"":""you are correct! <cond expr='user::error'>Good job fixing your error!</cond> "",
      ""options"":[]
},

    ""wrong_result"" : {""text"":""you are wrong!"", ""options"":[
        ["""",""user::error=true"",""question"",""Oops!""]]},

    ""answer"":{""text"":""it is 4!"", ""options"":[
        ["""","""",""question"",""Ok let’s try again""]
      ]}
    }
}";

        ActiveGame game;
        public ObservableDictionary<string, ObservableDictionary<string, int>> varsObservable = new ObservableDictionary<string, ObservableDictionary<string, int>>();
        public ObservableCollection<ObservableCollection<string>> choicesObservable = new ObservableCollection<ObservableCollection<string>>();
        public ObservableCollection<string> statesObservable = new ObservableCollection<string>();



        public State currentState = null;
        public event PropertyChangedEventHandler PropertyChanged;

        public Creator()
        {

            game = new ActiveGame(AdventureGame.loadFromString(DEF_GAME_TXT));
            game.start();

            currentState = game.data.states[game.position];

            this.InitializeComponent();
            refresh();
        }

        private void addTransition_Click(object sender, RoutedEventArgs e)
        {
            List<string> transitionTuple = new List<string>() {"","","destState","choice dialog"};
            game.data.states[game.position].options.Add(transitionTuple);
            refresh();
        }

        private void addState_Click(object sender, RoutedEventArgs e)
        {
            string defaultKey = "newState";
            while (game.data.states.ContainsKey(defaultKey))
            {
                defaultKey += "0";
            }

            makeDefaultState(defaultKey);

        }

        private void makeDefaultState(string stateName)
        {
            State defaultState = new State();
            defaultState.text = "new state placeholder text";
            defaultState.options = new List<List<string>>();
            defaultState.options.Add(new List<string>() { "", "", "destState", "choice dialog" } );
            game.data.states.Add(stateName, defaultState);
            refresh();
        }


        private void editProperties_Click(object sender, RoutedEventArgs e)
        {
            Flyout flyout = new Flyout();
            flyout.OverlayInputPassThroughElement = editProperties;

            flyout.ShowAt(sender as FrameworkElement);
            refresh();
        }

        private async void saveAdventure_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            //dropdown of file types the user can create
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".json", ".agf" });
            savePicker.SuggestedFileName = "newAdv.json";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, AdventureGame.saveToString(game.data));

                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if( status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //success
                }
                else
                {
                    await new MessageDialog("Error Saving Adventure Game!", "Error").ShowAsync();
                    return;
                }
            }
        }

        private async void importEdit_Click(object sender, RoutedEventArgs e)
        {
            //open up windows dialog to pick file
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            picker.FileTypeFilter.Add(".agf");
            picker.ViewMode = PickerViewMode.List;

            StorageFile file = await picker.PickSingleFileAsync();

            if (file == null)
                return;


            string contents = "";
            try
            {
                contents = await UWPIO.storageFileToString(file);
                game = new ActiveGame(AdventureGame.loadFromString(contents));
            } catch(Exception x)
            {
                await new MessageDialog("Invalid Adventuregame Selected", "Error").ShowAsync();
                return;
            }

            refresh();
        }

        //right click on state to delete ( or other detailed mouse actions )
        private void stateBtn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var p = e.GetCurrentPoint((UIElement)sender);
                if (p.Properties.IsRightButtonPressed)
                {
                    dynamic statebtn = sender;
                    string state_to_del = statebtn.Content.Text;
                    delState(state_to_del);
                }
            }
            refresh();
        }

        private void stateBtn_Click(object sender, RoutedEventArgs e)
        {
            dynamic statebtn = sender;
            string nextState = statebtn.Content.Text;

            game.forceState(nextState);
            currentState = game.data.states[game.position];
            refresh();
        }

        private void delState(string stateName)
        {
            //remove from our statesObservable, then our state dict
            statesObservable.Remove(stateName);
            game.data.states.Remove(stateName);

            //do I have any states?
            //if (game.data.states.Count == 0)
            //makeDefaultState("newState");

            refresh();
        }

        private void runTransition_Click(object sender, RoutedEventArgs e)
        {
            dynamic transition = e.OriginalSource;
            ObservableCollection<string> p = transition.DataContext as ObservableCollection<string>;
            //now we can search our transitions
            int i = choicesObservable.IndexOf(p); //List.IndexOf(currentState.options, p);

            game.choose(i);
            currentState = game.data.states[game.position];
            refresh();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dynamic g = e.Parameter;

            if (g is ActiveGame)
            {
                game = g;
            } else if (g is AdventureGame)
            {
                game = new ActiveGame(g);
            }

            base.OnNavigatedTo(e);
            refresh();
        }

        private void currentStateBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //manually update this state's key, and game's position
            TextBox tb = sender as TextBox;
            string newStr = tb.Text;

            if (newStr == "" || newStr == game.position)
                return; //ignore this case

            game.data.states.Remove(game.position);
            game.data.states.Add(newStr, currentState);

            foreach ( KeyValuePair<string,State> entry in game.data.states)
            {
                foreach( List<string> choice in entry.Value.options)
                {
                    if (choice[2] == game.position)
                        choice[2] = newStr;
                }
            }
            game.position = newStr;
            refresh();
        }

        void refresh()
        {

            varsObservable = new ObservableDictionary<string, ObservableDictionary<string, int>>();
            choicesObservable = new ObservableCollection<ObservableCollection<string>>();
            statesObservable = new ObservableCollection<string>();

            foreach (var e in game.states){
                varsObservable.Add(e.Key,new ObservableDictionary<string, int>());
                foreach(var r in e.Value)
                {
                    varsObservable[e.Key][r.Key] = r.Value;
                }
            }

            foreach (var e in currentState.options){
                choicesObservable.Add(new ObservableCollection<string>() { e[0], e[1], e[2], e[3] });
            }

            foreach (var e in game.data.states)
            {
                statesObservable.Add(e.Key);
            }


            updateProperty(nameof(varsObservable));
            updateProperty(nameof(choicesObservable));
            updateProperty(nameof(statesObservable));
            updateProperty(nameof(currentState.options));
            updateProperty(nameof(currentState.text));
            updateProperty(nameof(currentState));
            updateProperty(nameof(game.data.states));
            updateProperty(nameof(game.data));
            updateProperty(nameof(game.position));
            updateProperty(nameof(game.states));
            updateProperty(nameof(game));
            updateProperty("[0]");
            updateProperty("[1]");
            updateProperty("[2]");
            updateProperty("[3]");
            updateProperty("Value");
            updateProperty("Key");
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs r)
        {
            currentState.options = new List<List<string>>();
            foreach (var e in choicesObservable){
                currentState.options.Add(new List<string>{ e[0], e[1], e[2], e[3] });
            }
        }

    }
}
