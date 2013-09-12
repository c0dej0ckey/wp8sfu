using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu
{
    public class MapsVM
    {
        private Dictionary<string, List<string>> mCampuses;

        public MapsVM()
        {
            mCampuses =  new Dictionary<string,List<string>>();
            
            List<string> surreyCampusFloors = new List<string>();
            surreyCampusFloors.Add("3rd Floor");
            surreyCampusFloors.Add("4th Floor");
            surreyCampusFloors.Add("5th Floor");
            mCampuses.Add("Surrey Campus", surreyCampusFloors);


        }


        public Dictionary<string, List<string>> Campuses
        {
            get { return this.mCampuses; }
            set { this.mCampuses = value; }
        }
    }

    
    
}
