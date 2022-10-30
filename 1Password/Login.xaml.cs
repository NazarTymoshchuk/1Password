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
using System.Windows.Shapes;
using data_base;
using data_base.Entities;

namespace _1Password
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        OnePasswordDbContext context;
        public Login()
        {
            InitializeComponent();
            context = new OnePasswordDbContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (context.Users.FirstOrDefault(u => u.Username == txtUsername.Text) != null)
            {
                User user = context.Users.Where(u => u.Username == txtUsername.Text).FirstOrDefault();
                if (user.Password == txtPassword.Password)
                {
                    MainWindow mainWindow = new MainWindow(txtUsername.Text, txtPassword.Password, user);
                    mainWindow.Show();
                    this.Close();
                }              
            }
            else
            {
                txtIncorrect.Text = "Incorrect login or password";
            }
        }
    }
}
