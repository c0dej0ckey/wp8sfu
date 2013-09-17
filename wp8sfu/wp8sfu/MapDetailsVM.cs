using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace wp8sfu
{
    public class MapDetailsVM : INotifyPropertyChanged
    {
        private Room mSelectedRoom;
        private string mEntity;
        private WriteableBitmap mCampusImage;

        public MapDetailsVM()
        {
            mSelectedRoom = (Room)PhoneApplicationService.Current.State["SelectedRoom"];
            mEntity = PhoneApplicationService.Current.State["SelectedEntity"].ToString();
        }

        public string Entity
        {
            get { return this.mEntity; }
            set { this.mEntity = value; }
        }

        public void SetPinOnImage(MemoryStream stream, BitmapImage bitmap)
        {
            WriteableBitmap bmp = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
            bmp.SetSource(stream);
            int[] pixels = bmp.Pixels;

            var res = System.Windows.Application.GetResourceStream(new Uri(@"wp8sfu;component/Resources/Maps/pin_map.png", UriKind.Relative));
            Stream pinStream = res.Stream;
            byte[] data = new byte[pinStream.Length];
            pinStream.Read(data, 0, data.Length);
            pinStream.Close();

            MemoryStream ms = new MemoryStream(data);
            WriteableBitmap pinBitmap = new WriteableBitmap(48,48);
            pinBitmap.SetSource(ms);


            for(int i = 0; i < 48; i++)
            {
                for(int j = 0; j < 48; j++)
                {
                    bmp.SetPixel(mSelectedRoom.X + i - 24, mSelectedRoom.Y + j - 24,pinBitmap.GetPixel(i,j));
                }
            }

            CampusImage = bmp;
            OnPropertyChanged("CampusImage");


        }

        public WriteableBitmap CampusImage
        {
            get { return this.mCampusImage; }
            set { this.mCampusImage = value; }
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
