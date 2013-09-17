using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using wp8sfu.Entities;


namespace wp8sfu.VMs
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

        public WriteableBitmap GetBitmapForCampus()
        {
            
            string path = @"wp8sfu;component/Assets/Maps/";
            switch(Entity)
            {
                case "Galleria 3":
                    path = path + "Campus_Guide_Galleria_3.png";
                    break;
                case "Galleria 4":
                    path = path + "Campus_Guide_Galleria_4.png";
                    break;
                case "Galleria 5":
                    path = path + "Campus_Guide_Galleria_5.png";
                    break;
                case "Podium 2":
                    path = path + "Campus_Guide_Podium_2.png";
                    break;
                case "Burnaby Campus":
                    path = path + "Campus_Guide_Galleria_3.png";
                    break;
                case "Burnaby Lots":
                    path = path + "Campus_Guide_Galleria_3.png";
                    break;
            }

            StreamResourceInfo res = System.Windows.Application.GetResourceStream(new Uri(path, UriKind.Relative));
            Stream isfs = res.Stream;
            byte[]  data = new byte[isfs.Length];
            isfs.Read(data, 0, data.Length);
            isfs.Close();
            MemoryStream ms = new MemoryStream(data);
            BitmapImage bi = new BitmapImage();
            WriteableBitmap bmp = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
            
            bmp.SetSource(ms);

            bmp = SetPinOnImage(ms, bmp);

            return bmp;
        }

        public WriteableBitmap SetPinOnImage(MemoryStream stream, WriteableBitmap bitmap)
        {

            int[] pixels = bitmap.Pixels;

            var res = System.Windows.Application.GetResourceStream(new Uri(@"wp8sfu;component/Assets/Maps/pin_map.png", UriKind.Relative));
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
                    Color pixelColor = pinBitmap.GetPixel(i, j);

                    int red = pixelColor.R;
                    int green = pixelColor.G;
                    int blue = pixelColor.B;
                    int alpha = pixelColor.A;
                    if(alpha == 0 && red == 0 && blue == 0 && alpha == 0)
                    {

                        bitmap.SetPixel(mSelectedRoom.X + i - 24, mSelectedRoom.Y + j - 24, bitmap.GetPixel(mSelectedRoom.X + i -24, mSelectedRoom.Y - 24));
                    }
                    else
                    {
                        bitmap.SetPixel(mSelectedRoom.X + i - 24, mSelectedRoom.Y + j - 24, Convert.ToByte(alpha), Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));    
                    }

                    
                }
            }

            CampusImage = bitmap;
            OnPropertyChanged("CampusImage");
            return bitmap;

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
