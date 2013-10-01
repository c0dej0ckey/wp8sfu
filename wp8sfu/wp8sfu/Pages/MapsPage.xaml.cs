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
            ListPicker campusPicker = sender as ListPicker;
            ListPicker floorPicker = this.FindName("FloorBuildingPicker") as ListPicker;
            ListPicker roomPicker = this.FindName("RoomPicker") as ListPicker;
            ListPicker buildingPicker = this.FindName("BuildingPicker") as ListPicker;
            

            if (campusPicker.SelectedItem != null && campusPicker.SelectedItem.ToString() != "")
            {

                if (campusPicker.SelectedItem.Equals("Burnaby Campus"))
                {
                    roomPicker.Visibility = Visibility.Collapsed;
                    if (floorPicker.Visibility == Visibility.Collapsed)
                    {
                        floorPicker.Visibility = Visibility.Visible;
                    }
                    if (floorPicker.Visibility == Visibility.Visible)
                    {
                        floorPicker.Visibility = Visibility.Collapsed;
                        buildingPicker.Visibility = Visibility.Visible;
                    }

                }
                else if (campusPicker.SelectedItem.Equals("Surrey Campus"))
                {
                    floorPicker.Header = "Floor";
                    if (floorPicker.Visibility == Visibility.Collapsed)
                    {
                        floorPicker.Visibility = Visibility.Visible;
                        buildingPicker.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                roomPicker.Visibility = System.Windows.Visibility.Collapsed;
                floorPicker.Visibility = System.Windows.Visibility.Collapsed;
                buildingPicker.Visibility = Visibility.Collapsed;
            }

        }

        private void GetRoom_FloorSelection(object sender, SelectionChangedEventArgs e)
        {
            ListPicker floorPicker = sender as ListPicker;
            ListPicker campusPicker = this.FindName("CampusListPicker") as ListPicker;
            ListPicker roomPicker = this.FindName("RoomPicker") as ListPicker;
            
            if (campusPicker.SelectedItem != null && campusPicker.SelectedItem.ToString() != "")
            {

                if (campusPicker.SelectedItem.Equals("Surrey Campus"))
                {
                    if (floorPicker.SelectedItem != null && floorPicker.SelectedItem.ToString() != "")
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