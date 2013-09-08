using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PhoneApp1
{
    public class MainPageVM
    {
        private NavigationService mNavigationService;


        public string LogInOrOutText
        {
            get
            {
                if(Settings.GetSetting("LoggedIn") != null)
                {
                    return "LOGOUT";
                }
                else
                {
                    return "LOGIN";
                }
            }
        }

        public NavigationService NavigationService
        {
            get { return this.mNavigationService; }
            set { this.mNavigationService = value; }
        }

        public ICommand LoginCommand
        {
            get { return new DelegateCommand(ExecuteLogin, CanExecuteLogin); }
        }

        public ICommand ProtectedServicesCommand
        {
            get { return new DelegateCommand(ExecuteProtectedServices, CanExecuteProtectedServices); }
        }


        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }

        private void ExecuteLogin(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/LoginDetailsPage.xaml", UriKind.Relative));
        }

        private bool CanExecuteProtectedServices(object parameter)
        {
            return true;
        }

        private void ExecuteProtectedServices(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/ProtectedServicesPage.xaml", UriKind.Relative));
        }

    }
}
