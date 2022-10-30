using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private const int SaltSize = 16;

        private const int HashSize = 20;

        OnePasswordDbContext context;
        public Registration()
        {
            InitializeComponent();

            context = new OnePasswordDbContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (context.Users.FirstOrDefault(u => u.Username == txtUsername.Text) == null)
            {
                if (txtPassword.Password == txtComfirmPassword.Password)
                {

                    var hash = Hash(txtPassword.Password);

                    MainWindow main = new MainWindow(txtUsername.Text, txtPassword.Password, null);

                    main.Show();
                    this.Close();
                }
                else
                {
                    txtError.Text = "Try again!";
                    txtComfirmPassword.Password = "";
                    txtPassword.Password = "";
                }
            }
            else
            {
                txtUsernameTaken.Text = "Username is already taken";
            }          
        }

        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }
    }
}
