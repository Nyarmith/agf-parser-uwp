using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace agf_parser_uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Creator : Page
    {

        public List<List<string>> binderTest = new List<List<string>>();
        public List<string> stateText = new List<string>();
        public string curTxt = "<b> main text my dude</b>";
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
            this.InitializeComponent();
        }

        private void addTransition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addState_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
