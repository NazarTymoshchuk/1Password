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
using data_base.Entities;

namespace _1Password
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private const int SaltSize = 16;

        private const int HashSize = 20;

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
                if (Verify(txtPassword.Password, user.Password))
                {
                    MainWindow mainWindow = new MainWindow(txtUsername.Text, txtPassword.Password, user);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    txtIncorrect.Text = "Incorrect password";
                }
            }
            else
            {
                txtIncorrect.Text = "User not found";
            }
        }

        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
