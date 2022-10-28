using data_base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1Password
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        /*public MainWindow(string name, string password, string username, string? website)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            // password encryption
            viewModel.AddAccount(new AccountInfo() { Name = name, UserName = username, Password = password, LinkToSite = website });
        }*/
        public MainWindow(string username, string password)
        {
            InitializeComponent();
            viewModel = new ViewModel();
            this.DataContext = viewModel;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AddNewItem item = new AddNewItem();
            //item.Show();
            //this.Close();
     
            AddNewItem item = new AddNewItem(viewModel); // new AddNewItem window invoke
            item.Show();
        }
    }
}
