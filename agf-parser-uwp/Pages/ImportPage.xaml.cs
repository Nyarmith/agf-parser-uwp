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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
//TODO: make asynchronous, get permissions to access all filesystem, implement the actual import (make it shared)

namespace agf_parser_uwp
{
    //individual item being listed in the grid
    public class GridObj
    {
        public string name;
        public string icon;
        public GridObj(string name_, string icon_)
        {
            name = name_;
            icon = icon_;
        }
    }

    public sealed partial class ImportPage : Page, INotifyPropertyChanged
    {
        public string currentPath;
        public  ObservableCollection<GridObj> dirObjects = new ObservableCollection<GridObj>();

        private List<string> folders;
        private List<string> files;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImportPage()
        {
            this.InitializeComponent();
            currentPath = Package.Current.InstalledLocation.Path;
            updateFiles();
        }

        public void updateFiles()
        {
            files = System.IO.Directory.GetFiles(currentPath).Select(x => last(x)).ToList();
            folders = System.IO.Directory.GetDirectories(currentPath).Select(x => last(x)).ToList();
            files.Sort();
            folders.Sort();
            dirObjects = new ObservableCollection<GridObj>();

            dirObjects.Add(new GridObj("..", "MoveToFolder"));

            foreach (string folder in folders)
            {
                dirObjects.Add(new GridObj(folder, "Folder"));
            }

            folders.Add("..");

            foreach (string file in files)
            {
                dirObjects.Add(new GridObj(file, "Page"));
            }

            //PropertyChanged(this, new PropertyChangedEventArgs(nameof(dirObjects)));
            updateProperty(nameof(dirObjects));
        }

        public void chdir(string dirName)
        {
            if (dirName == "..")
            {
                currentPath = init(currentPath);
                updateProperty(nameof(currentPath));
                updateFiles();
            }
            else if (folders.Contains(dirName))
            {
                currentPath = currentPath + "\\" + dirName;
                updateProperty(nameof(currentPath));
                updateFiles();
            }
            else
                throw new Exception("non-dir passed to chdir");
        }

        //handle a click
        private void FileList_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridObj go = e.ClickedItem as GridObj;
            string item = go.name;
            if (folders.Contains(item))
                chdir(item);
            else if (files.Contains(item))
                openDetails(item);
            else
                throw new Exception("somehow got an item not in files or folders");
        }

        private void openDetails(string item)
        {
            //I'd like this to expand the thing to the right, with more details
        }

        private void updateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private static string init(string fpath)
        {
            string[] s = fpath.Split('\\');
            return String.Join("\\", s, 0, s.Length - 1);
        }
        private static string last(string fpath)
        {
            string[] s = fpath.Split('\\');
            return s[s.Length - 1];
        }
    }
}
