using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu
{
    public class MapDetailsVM
    {
        private Room mSelectedRoom;
        private string mEntity;

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
    }
}
