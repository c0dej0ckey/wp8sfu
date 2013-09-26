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
        private List<string> mBurnabyBuildings;
        private List<Room> mRooms;
        private string mSurreySelectedFloor;
        private Room mSurreySelectedRoom;
        private string mSelectedCampus;

        public MapsVM()
        {
            mRooms = new List<Room>();

            mBurnabyBuildings = new List<string>();

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
            

        }

        public ICommand GetRoomCommand
        {
            get { return new DelegateCommand(ExecuteGetRoom, CanExecuteGetRoom); }
        }

        private bool CanExecuteGetRoom(object parameter)
        {
            if(SurreySelectedRoom != null)
            {
                return true;
            }
            return false;
        }

        private void ExecuteGetRoom(object parameter)
        {
            PhoneApplicationService.Current.State["SelectedRoom"] = SurreySelectedRoom;
            if (mSelectedCampus == "Surrey Campus")
            {
                PhoneApplicationService.Current.State["SelectedEntity"] = SurreySelectedFloor;
            }
            else
            {
                PhoneApplicationService.Current.State["SelectedEntity"] = "Burnaby Campus";
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
                    else if (SelectedCampus.Equals("Burnaby Campus"))
                    {
                        return this.mBurnabyBuildings;
                    }
                }
                return null;
            }
            set 
            { 
                this.mSurreyFloors = value; 
            }
        }


        public string SurreySelectedFloor
        {
            get { return this.mSurreySelectedFloor; }
            set { this.mSurreySelectedFloor = value;
            OnPropertyChanged("Rooms");
            }
        }

        public Room SurreySelectedRoom
        {
            get { return this.mSurreySelectedRoom; }
            set 
            { 
                this.mSurreySelectedRoom = value;
                OnPropertyChanged("GetRoomCommand");
            }
        }

        public List<Room> Rooms
        {
            get 
            {
                OnPropertyChanged("GetRoomCommand");
                if(SurreySelectedFloor != null && SurreySelectedFloor != "")
                {
                    string floorNumber = SurreySelectedFloor.Split(' ')[1];
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
            }
        }

        public List<string> Campuses
        {
            get { return this.mCampuses; }
            set { this.mCampuses = value; }
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
