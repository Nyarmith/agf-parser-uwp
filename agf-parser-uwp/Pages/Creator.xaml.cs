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

namespace agf_parser_uwp
{

    // TODO: add on-transiton function that lets you pass an activegame or adventuregame
    // TODO: add arrow to the right of the transitions so you can follow them
    // TODO: add visual indicator for when transition requirements not met and trying to transition(2nd click to force)
    // TODO: add import functionality for this editor
    // TODO: make save pop up a filepicker dialogue to create a file

    public sealed partial class Creator : Page
    {

        public Dictionary<string, Dictionary<string, Object>> myVars = new Dictionary<string, Dictionary<string, Object>>();
        public List<List<string>> binderTest = new List<List<string>>();
        public List<string> stateText = new List<string>();
        public string curTxt = "<b> main text my dude </b>";
        public Creator()
        {
            binderTest.Add(new List<string>());
            binderTest.Add(new List<string>());
            binderTest.Add(new List<string>());
            stateText.Add("state1");
            stateText.Add("state2");

            string a = "0";
            foreach (List<string> lst in binderTest)
            {
                lst.Add("1frst" + a);
                lst.Add("2snd" + a);
                lst.Add("3thrd" + a);
                lst.Add("4frth" + a);
                a += "1";
            }

            myVars["ayy"] = new Dictionary<string, Object>();
            myVars["ayy"]["lmao"] = 23;
            myVars["ayy"]["nowow"] = 44;

            myVars["wow"] = new Dictionary<string, Object>();
            myVars["wow"]["pls"] = 0;

            this.InitializeComponent();
        }

        private void addTransition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addState_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editProperties_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveAdventure_Click(object sender, RoutedEventArgs e)
        {

        }

        private void importEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void runTransition_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
