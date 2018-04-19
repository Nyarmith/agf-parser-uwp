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
        private AdventureGame ag;

        public class StateSelector : DataTemplateSelector
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
            }
        }

        public Creator()
        {
            this.InitializeComponent();
        }
    }
}
