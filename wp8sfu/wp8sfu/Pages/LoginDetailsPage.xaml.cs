using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using wp8sfu.VMs;

namespace wp8sfu.Pages
{
    public partial class LoginDetailsPage : PhoneApplicationPage
    {
        public LoginDetailsPage()
        {
            InitializeComponent();
            if (this.DataContext == null)
            {
                this.DataContext = new LoginDetailsVM();
            }
        }

    }
}