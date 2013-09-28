using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using wp8sfu.Resources;
using wp8sfu.Services;
using wp8sfu.VMs;

namespace wp8sfu.Pages
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (this.DataContext == null)
            {
                this.DataContext = new MainPageVM();
            }
            
            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPageVM mainPageVM = (MainPageVM)this.DataContext;
            mainPageVM.NavigationService = this.NavigationService;
            ServiceLocator.AddService<NavigationService>(this.NavigationService);
            mainPageVM.OnPropertyChanged("LoginStatus");
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if(e.Uri.OriginalString.Equals("/Pages/LoginDetailsPage.xaml"))
            {
                MainPageVM mainPageVM = this.DataContext as MainPageVM;
                mainPageVM.OnPropertyChanged("LoginStatus");
            }
            base.OnNavigatedFrom(e);
        }

        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}