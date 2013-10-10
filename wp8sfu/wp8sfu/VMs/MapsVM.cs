using Kent.Boogaart.KBCsv;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Resources;
using Windows.Storage.Streams;
using wp8sfu.Entities;
using wp8sfu.Services;
using wp8sfu.UI;

namespace wp8sfu.VMs
{
    public class MapsVM : INotifyPropertyChanged
    {
        private List<string> mCampuses = new List<string>() { string.Empty, "Burnaby Campus", "Surrey Campus" };
        private List<string> mSurreyFloors = new List<string>() { string.Empty, "Galleria 3", "Galleria 4", "Galleria 5", "Podium 2" };
        private List<Room> mBuildings;
        private Room mSelectedBuilding;
        private List<Room> mRooms;
        private string mSelectedFloor;
        private Room mSelectedRoom;
        private string mSelectedCampus;

        public MapsVM()
        {
            mRooms = new List<Room>();

            mBuildings = new List<Room>();

            string dir = Directory.GetCurrentDirectory();
            StreamReader reader = null;

            var path = @"wp8sfu;component/Assets/Maps/surrey-campus-list.csv";
            StreamResourceInfo res = System.Windows.Application.GetResourceStream(new Uri(path, UriKind.Relative));

            reader = new StreamReader(res.Stream);
               
            string line = null;
            while((line = reader.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                Room room = new Room(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]));
                mRooms.Add(room);
            }

            path = @"wp8sfu;component/Assets/Maps/burnaby-campus-list.csv";
            res = System.Windows.Application.GetResourceStream(new Uri(path, UriKind.Relative));
            reader = new StreamReader(res.Stream);
            while((line = reader.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                Room room = new Room(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]));
                mBuildings.Add(room);
            }

        }

        public ICommand GetRoomCommand
        {
            get { return new DelegateCommand(ExecuteGetRoom, CanExecuteGetRoom); }
        }

        private bool CanExecuteGetRoom(object parameter)
        {
            if(SelectedRoom != null)
            {
                return true;
            }
            else if(SelectedBuilding != null)
            {
                return true;
            }
            return false;
        }

        private void ExecuteGetRoom(object parameter)
        {
            PhoneApplicationService.Current.State["SelectedRoom"] = SelectedRoom;
            if (mSelectedCampus == "Surrey Campus")
            {
                PhoneApplicationService.Current.State["SelectedEntity"] = SelectedFloor;
            }
            else
            {
                PhoneApplicationService.Current.State["SelectedEntity"] = "Burnaby Campus";
                PhoneApplicationService.Current.State["SelectedRoom"] = SelectedBuilding;
            }
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/MapDetailsPage.xaml", UriKind.Relative));
        }

        public List<string> Floors
        {
            get
            {
                if (SelectedCampus != null)
                {
                    if (SelectedCampus.Equals("Surrey Campus"))
                    {
                        return this.mSurreyFloors;
                    }
                }
                return null;
            }
            set 
            { 
                this.mSurreyFloors = value; 
            }
        }


        public string SelectedFloor
        {
            get { return this.mSelectedFloor; }
            set { this.mSelectedFloor = value;
            OnPropertyChanged("Rooms");
            }
        }

        public Room SelectedRoom
        {
            get { return this.mSelectedRoom; }
            set 
            { 
                this.mSelectedRoom = value;
                OnPropertyChanged("GetRoomCommand");
            }
        }

        public List<Room> Rooms
        {
            get 
            {
                OnPropertyChanged("GetRoomCommand");
                if(SelectedFloor != null && SelectedFloor != "")
                {
                    string floorNumber = SelectedFloor.Split(' ')[1];
                    return mRooms.Where(e => e.Number.First() == floorNumber[0]).ToList<Room>();
                }

                return null; 

            }
            set { this.mRooms = value; }
        }

        public string SelectedCampus
        {
            get { return this.mSelectedCampus; }
            set 
            {
                this.mSelectedCampus = value;
                OnPropertyChanged("Floors");
                OnPropertyChanged("GetRoomCommand");
            }
        }

        public List<string> Campuses
        {
            get { return this.mCampuses; }
            set { this.mCampuses = value; }
        }

        public List<Room> Buildings
        {
            get { return this.mBuildings; }
            set { this.mBuildings = value;
            OnPropertyChanged("GetRoomCommand");
            }
        }

        public Room SelectedBuilding
        {
            get { return this.mSelectedBuilding; }
            set { this.mSelectedBuilding = value;
            OnPropertyChanged("GetRoomCommand");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    
    
}
