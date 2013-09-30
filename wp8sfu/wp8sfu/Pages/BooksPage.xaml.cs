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
    public partial class BooksPage : PhoneApplicationPage
    {
        public BooksPage()
        {
            InitializeComponent();
            if(this.DataContext == null)
            {
                this.DataContext = new BooksVM();
            }
        }

        private void BooksListBox_LayoutUpdated(object sender, EventArgs e)
        {
            BooksVM vm = this.DataContext as BooksVM;
            ListBox listBox = this.FindName("BooksListBox") as ListBox;
            listBox.ItemsSource = vm.Books;
            //force data bind refresh
        }

        




    }
}