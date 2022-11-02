using data_base;
using data_base.Entities;
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
        public MainWindow(string username, string password, User user)
        {
            InitializeComponent();
            viewModel = new ViewModel();
            if (user != null)
            {
                viewModel.CurrentUser = user;
                viewModel.AddAccountAfterLogIn();
            }
            else
            {
                viewModel.context.Users.Add(new User()
                {
                    Username = username,
                    Password = password,
                });
                viewModel.context.SaveChanges();
                viewModel.CurrentUser = viewModel.context.Users.Where(u => u.Username == username).FirstOrDefault();
            }
            this.DataContext = viewModel;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem item = new AddNewItem(viewModel); // new AddNewItem window invoke
            item.Show();
        }
    }
}
