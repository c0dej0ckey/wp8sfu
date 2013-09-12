using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace wp8sfu
{
    public partial class MapsPage : PhoneApplicationPage
    {
        public MapsPage()
        {
            InitializeComponent();
            this.DataContext = new MapsVM();
        }

        private void GetCampus_ForSelection(object sender, SelectionChangedEventArgs e)
        {
            ListPicker picker = (ListPicker)sender as ListPicker;
            string campus = picker.SelectedItem.ToString();
        }
    }
}