using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace PhoneApp1
{
    public class ProtectedServicesVM
    {
        private List<string> mServices = new List<string>();

        public ProtectedServicesVM()
        {
            mServices.Add("webct");
            mServices.Add("sims");
            mServices.Add("go sfu");
            mServices.Add("sfu connect");


        }

        public List<string> Services
        {
            get { return mServices; }
        }

        public void OpenService(string service)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();

            if(service.Equals("webct"))
            {
                
            }
            else if(service.Equals("sims"))
            {
                
            }
            else if(service.Equals("go sfu"))
            {
                
            }
            else if(service.Equals("sfu connect"))
            {
                
            }
        }

    }
}
