using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using wp8sfu.Services;
using wp8sfu.Utilities;
using wp8sfu.UI;

namespace wp8sfu.VMs
{
    public class MainPageVM
    {
        private NavigationService mNavigationService;

        public MainPageVM()
        {
        }

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

        public ICommand CoursesCommand
        {
            get { return new DelegateCommand(ExecuteCourses, CanExecuteCourses); }
        }

        public ICommand BooksCommand
        {
            get { return new DelegateCommand(ExecuteBooks, CanExecuteBooks); }
        }

        public ICommand MapsCommand
        {
            get { return new DelegateCommand(ExecuteMaps, CanExecuteMaps); }
        }

        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }

        private void ExecuteLogin(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/LoginDetailsPage.xaml", UriKind.Relative));
        }

        private bool CanExecuteProtectedServices(object parameter)
        {
            return true;
        }

        private void ExecuteProtectedServices(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/ProtectedServicesPage.xaml", UriKind.Relative));
        }

        private bool CanExecuteCourses(object parameter)
        {
            return true;
        }

        private void ExecuteCourses(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/SchedulePage.xaml", UriKind.Relative));
        }

        private bool CanExecuteBooks(object parameter)
        {
            return true;
        }

        private void ExecuteBooks(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/BooksPage.xaml", UriKind.Relative));
        }

        private bool CanExecuteMaps(object parameter)
        {
            return true;

        }

        private void ExecuteMaps(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/MapsPage.xaml", UriKind.Relative));
        }
    }
}
