using Kent.Boogaart.KBCsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using Windows.Storage.Streams;

namespace wp8sfu
{
    public class MapsVM
    {
        private List<string> mSurreyFloors;
        private List<string> mSurreyRooms;
        private List<Room> mRooms;

        public MapsVM()
        {
            mSurreyFloors = new List<string>();
            mSurreyFloors.Add("Galleria 3");
            mSurreyFloors.Add("Galleria 4");
            mSurreyFloors.Add("Galleria 5");
            mSurreyFloors.Add("Podium 2");
            mSurreyRooms = new List<string>();
            string dir = Directory.GetCurrentDirectory();
            StreamReader reader = null;

            var path = @"wp8sfu;component/Resources/Maps/surrey-campus-list.csv";
            StreamResourceInfo res = System.Windows.Application.GetResourceStream(new Uri(path, UriKind.Relative));

            reader = new StreamReader(res.Stream);
               
            string line = null;
            while((line = reader.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                Room room = new Room(data[0], int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
                mRooms.Add(room);
            }
            

        }

        public List<string> SurreyFloors
        {
            get { return this.mSurreyFloors; }
            set { this.mSurreyFloors = value; }
        }

        public List<string> SurreyRooms
        {
            get { return this.mSurreyRooms; }
            set { this.mSurreyRooms = value; }
        }

        public List<Room> Rooms
        {
            get { return this.mRooms; }
            set { this.mRooms = value; }
        }

        
    }

    
    
}
