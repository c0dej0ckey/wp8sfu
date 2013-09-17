using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using wp8sfu.VMs;

namespace wp8sfu.Pages
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
            ListPicker picker = sender as ListPicker;
            ListPicker floorBuildingPicker = (ListPicker)picker.FindName("FloorBuildingPicker");
            ListPicker roomPicker = (ListPicker)picker.FindName("RoomPicker");

            if ( picker.SelectedItem != null && picker.SelectedItem.ToString() != ""  )
            {

                if(picker.SelectedItem.Equals("Burnaby Campus"))
                {
                    roomPicker.Visibility = Visibility.Collapsed;
                    if(floorBuildingPicker.Visibility == Visibility.Collapsed)
                    {
                        floorBuildingPicker.Visibility = Visibility.Visible;
                    }
                    floorBuildingPicker.Header = "Building";

                }
                else if(picker.SelectedItem.Equals("Surrey Campus"))
                {
                    floorBuildingPicker.Header = "Floor";
                    if(floorBuildingPicker.Visibility == Visibility.Collapsed)
                    {
                        floorBuildingPicker.Visibility = Visibility.Visible;
                    }
                    //roomPicker.Visibility = Visibility.Visible;
                }
            }
            else
            {
                roomPicker.Visibility = System.Windows.Visibility.Collapsed;
                floorBuildingPicker.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        private void GetRoom_FloorSelection(object sender, SelectionChangedEventArgs e)
        {
            ListPicker floorBuildingPicker = sender as ListPicker;
            ListPicker campusPicker = floorBuildingPicker.FindName("CampusListPicker") as ListPicker;
            ListPicker roomPicker = floorBuildingPicker.FindName("RoomPicker") as ListPicker;

            if (campusPicker.SelectedItem != null && campusPicker.SelectedItem.ToString() != "")
            {

                if (campusPicker.SelectedItem.Equals("Surrey Campus"))
                {
                    if (floorBuildingPicker.SelectedItem != null && floorBuildingPicker.SelectedItem.ToString() != "")
                    {
                        roomPicker.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        roomPicker.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}