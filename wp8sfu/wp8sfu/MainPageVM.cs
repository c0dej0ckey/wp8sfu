using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace wp8sfu
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

        private bool CanExecuteCourses(object parameter)
        {
            return true;
        }

        private void ExecuteCourses(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/SchedulePage.xaml", UriKind.Relative));
        }

        private bool CanExecuteBooks(object parameter)
        {
            return true;
        }

        private void ExecuteBooks(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/BooksPage.xaml", UriKind.Relative));
        }
    }
}
