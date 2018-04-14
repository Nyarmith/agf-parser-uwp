using System;
using System.ComponentModel;
using System.Collections.Generic;
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
using Windows.ApplicationModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
//TODO: make asynchronous, get permissions to access all filesystem, implement the actual import (make it shared)

namespace agf_parser_uwp
{


    public sealed partial class ImportPage : Page, INotifyPropertyChanged
    {
        public string currentPath = "";
        public AdventureGame loadedGame = null;
        public StorageFile curFile = null;
        public string fileContents = "";
        public string title="N/A";
        public string author="N/A";
        public string gamevars="N/A";
        public string start_state="N/A";

        public event PropertyChangedEventHandler PropertyChanged;

        public ImportPage()
        {
            this.InitializeComponent();
            currentPath = null;
            updateDisplay();
        }

        //make click handler to open file dialog

        public void updateDisplay()
        {
            if (curFile == null)
                return;

            ConfirmationButton.Visibility = Visibility.Visible;

            updateProperty(nameof(fileContents));
            updateProperty(nameof(title));
            updateProperty(nameof(author));
            updateProperty(nameof(gamevars));
            updateProperty(nameof(start_state));
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
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
                loadedGame = AdventureGame.loadFromString(contents);
            } catch(Exception x)
            {
                await new MessageDialog("Invalid Adventuregame Selected", "Error").ShowAsync();
                return;
            }

            title = loadedGame.title;
            author = loadedGame.author;
            gamevars = loadedGame.gamevars.ToString();
            start_state = loadedGame.start_state;

            fileContents = contents;
            currentPath = file.Path;
            curFile = file;

            updateDisplay();
        }

        private async void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            await UWPIO.createFile(UWPIO.GAMEDIR + "\\" + curFile.Name, fileContents);

            //do some notification to let the user know it happened
            ConfirmationButton.IsEnabled = false;
            ConfirmationButtonText.Text = "Success!";

            Regex rgx = new Regex(@"\b\w+\b");
            fileContents = rgx.Replace(fileContents, "Success!");

            title = "Success!";
            author = "Success!";
            gamevars = "Success!";
            start_state = "Success!";

            updateDisplay();
        }
    }
}
