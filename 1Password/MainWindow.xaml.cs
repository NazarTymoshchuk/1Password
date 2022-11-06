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
            AddNewItem item = new AddNewItem(viewModel);
            item.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.SortByName();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            viewModel.SearchByName(txtSearchByName.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                viewModel.SearchByName(txtSearchByName.Text);
            }
        }

        private void txtSearchByName_TouchEnter(object sender, TouchEventArgs e)
        {
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.FilterByCategory();
        }
    }
}
