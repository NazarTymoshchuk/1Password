﻿using data_base;
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

namespace _1Password
{
    /// <summary>
    /// Interaction logic for AddNewItem.xaml
    /// </summary>
    public partial class AddNewItem : Window
    {
        ViewModel viewModel;
        public AddNewItem(ViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow main = new MainWindow(txtName.Text, txtPassword.Password, txtUsername.Text, txtWebsite.Text);
            //main.Show();
            //this.Close();

            if (txtName.Text != "" && txtUsername.Text != "" && txtPassword.Password != "" && txtWebsite.Text != "")
            {
                viewModel.AddAccount(new AccountInfo() // new item inserting without MainWindow closing
                {
                    Name = txtName.Text,
                    UserName = txtUsername.Text,
                    Password = txtPassword.Password,
                    LinkToSite = txtWebsite.Text
                });
                this.Close();
            }
            else
            {
                // values cannot be null message
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //MainWindow main = new MainWindow();
            //main.Show();
            this.Close();
        }
    }
}
