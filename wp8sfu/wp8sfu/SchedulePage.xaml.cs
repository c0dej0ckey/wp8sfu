using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace wp8sfu
{
    public partial class SchedulePage : PhoneApplicationPage
    {
        public SchedulePage()
        {
            InitializeComponent();
            if(this.DataContext == null)
            {
                this.DataContext = new ScheduleVM();
            }
        }
    }
}