﻿using data_base;
using data_base.Entities;
using data_base.Repositories;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace _1Password
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        public OnePasswordDbContext context = new OnePasswordDbContext();
        private ObservableCollection<AccountInfo> accounts;
        public IEnumerable<AccountInfo> AccountsInfo => accounts;

        XORCipher XORcipher = new XORCipher();
        string key = @"><w\\Dr{GlZIp.x8CFp&i:^HB4B<x#Fpmn0kw,sC>vY&evTwGtqV6r1sDR8@cP#-4nsgXlmqkYH0Iz$.D5fzeE+cl%:I8XN+P4o0s";

        public ViewModel()
        {
            accounts = new ObservableCollection<AccountInfo>();
        }

        public User CurrentUser { get; set; }

        public void AddAccount(string name, string username, string password, string linkToSite)
        {
            Account account = new Account()
            {
                Name = name,
                Password = XORcipher.Encrypt(password, key),
                UserId = CurrentUser.Id,
                UserName = username,
                LinkToSite = linkToSite
            };
            context.Accounts.Add(account);
            context.SaveChanges();

            AddAccountToList(name, username, password, linkToSite, account);
        }

        public void AddAccountToList(string name, string username, string password, string linkToSite, Account account)
        {
            var info = new AccountInfo(name, username, password, linkToSite);

            info.SetCommandDelete((o) =>
            {
                context.Accounts.Remove(account);
                accounts.Remove(info);
                context.SaveChanges();
            });
            info.SetCommandChange((o) =>
            {
                account.Password = XORcipher.Encrypt(info.Password, key);
                account.UserName = info.UserName;
                account.LinkToSite = info.LinkToSite;
                context.Accounts.Update(account);
                context.SaveChanges();
            });

            info.Difficulty = CheckDifficulty(password);

            accounts.Add(info);
        }

        public void AddAccountAfterLogIn()
        {
            IQueryable<Account> collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id);
            foreach (var item in collection)
            {
                AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item);           
            }
        }

        public void AddUser(string username, string password)
        {
            User user = new User()
            {
                Username = username,
                Password = password
            };

            context.Users.Add(user);
            CurrentUser = user;
            context.SaveChanges();
        }

        public void ChangeUser(string username, string password)
        {
            CurrentUser.Username = username;
            CurrentUser.Password = password;

            context.Users.Update(CurrentUser);
            context.SaveChanges();
        }

        public string CheckDifficulty(string password)
        {
            int difficulty = 0;
            if (password.Any(c => char.IsDigit(c)))
            {
                difficulty++;
            }
            if (password.Any(c => char.IsLetter(c)))
            {
                difficulty++;
            }
            if (password.Any(c => char.IsUpper(c)) && password.Any(c => char.IsLower(c)))
            {
                difficulty++;
            }
            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                difficulty++;
            }
            if (password.Length > 12)
            {
                difficulty++;
            }
            switch (difficulty)
            {
                case 1: return "Very easy";
                case 2: return "Easy";
                case 3: return "Normal";
                case 4: return "Hard";
                case 5: return "Very hard";
                default: return "null";
            }
        }

        private bool IsSortDesc = false;
        public void SortByName()
        {
            IQueryable<Account> collection;
            ClearAccounts();

            if (IsSortDesc)
            {
                collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id).OrderBy(a => a.Name);
                IsSortDesc = false;
            }
            else
            {
                collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id).OrderByDescending(a => a.Name);
                IsSortDesc = true;
            }

            foreach (var item in collection)
            {
                AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item);
            }
        }

        public void SearchByName(string name)
        {
            IQueryable<Account> collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id);
            ClearAccounts();

            foreach (var item in collection)
            {
                if (item.Name.Contains(name))
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item);
                }
            }
        }

        public void ClearAccounts()
        {
            accounts.Clear();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class ProfileInfo
    {
        public ProfileInfo(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class AccountInfo
    {
        public AccountInfo(string name, string username, string password, string linkToSite)
        {
            this.Name = name;
            this.UserName = username;
            this.Password = password;
            this.LinkToSite = linkToSite;  
        }

        public void SetCommandDelete(Action<object> action)
        {
            deleteCommand = new RelayCommand((o) => action(o));
        }
        public void SetCommandChange(Action<object> action)
        {
            changeCommand = new RelayCommand((o) => action(o));
        }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LinkToSite { get; set; }
        private RelayCommand deleteCommand;
        private RelayCommand changeCommand;
        public ICommand DeleteCmd => deleteCommand;
        public ICommand ChangeCmd => changeCommand;
        public string Difficulty { get; set; }
    }
}
