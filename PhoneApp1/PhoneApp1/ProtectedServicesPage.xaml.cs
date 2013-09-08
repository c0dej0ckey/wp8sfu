using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp1
{
    public partial class ProtectedServicesPage : PhoneApplicationPage
    {
        public ProtectedServicesPage()
        {
            InitializeComponent();
        }

        private void ServiceChanged(object sender, SelectionChangedEventArgs e)
        {
            string service = (string)sender;
            ProtectedServicesVM vm = (ProtectedServicesVM)this.DataContext;
            vm.OpenService(service);
        }
    }
}