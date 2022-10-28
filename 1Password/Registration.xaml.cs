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

namespace _1Password
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == txtComfirmPassword.Password)
            {
                MainWindow main = new MainWindow(txtUsername.Text, txtPassword.Password);
                main.Show();
                this.Close();
            }
            txtError.Text = "Try again!";
            txtComfirmPassword.Password = "";
            txtPassword.Password = "";
        }
    }
}
