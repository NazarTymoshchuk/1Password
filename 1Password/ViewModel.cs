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

        private ObservableCollection<Category> categories;
        public IEnumerable<Category> Categories => categories;
        XORCipher XORcipher = new XORCipher();
        string key = @"><w\\Dr{GlZIp.x8CFp&i:^HB4B<x#Fpmn0kw,sC>vY&evTwGtqV6r1sDR8@cP#-4nsgXlmqkYH0Iz$.D5fzeE+cl%:I8XN+P4o0s";
        public Category SelectedCategory { get; set; }
        private ObservableCollection<AccountInfo> favouriteAccounts;
        public IEnumerable<AccountInfo> FavouriteAccountsInfo => favouriteAccounts;
        public ViewModel()
        {
            accounts = new ObservableCollection<AccountInfo>();
            favouriteAccounts = new ObservableCollection<AccountInfo>();
            categories = new ObservableCollection<Category>(context.Categories);
        }

        public User CurrentUser { get; set; }

        public void AddAccount(string name, string username, string password, string linkToSite, string categoryName)
        {
            Account account;
            if (categoryName != "")
            {
                account = new Account()
                {
                    Name = name,
                    Password = XORcipher.Encrypt(password, key),
                    UserId = CurrentUser.Id,
                    UserName = username,
                    LinkToSite = linkToSite,
                    CategoryId = context.Categories.Where(c => c.Name == categoryName).First().Id,
                };
            }
            else
            {
                account = new Account()
                {
                    Name = name,
                    Password = XORcipher.Encrypt(password, key),
                    UserId = CurrentUser.Id,
                    UserName = username,
                    LinkToSite = linkToSite,
                };
            }
            context.Accounts.Add(account);
            context.SaveChanges();

            AddAccountToList(name, username, password, linkToSite, account, categoryName);
        }

        public void AddAccountToList(string name, string username, string password, string linkToSite, Account account, string categoryName)
        {
            var info = new AccountInfo(name, username, password, linkToSite, categoryName);

            info.SetCommandDelete((o) =>
            {
                favouriteAccounts.Remove(info);
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
            info.SetCommandMoveToFavorite((o) =>
            {
                if (info.isInFavorite == false)
                {
                    favouriteAccounts.Remove(info);
                }
                else
                {
                    favouriteAccounts.Add(info);
                }
            });

            info.Difficulty = CheckDifficulty(password);

            accounts.Add(info);
        }
        public void FilterByCategory()
        {
            IQueryable<Account> collection;
            ClearAccounts();
            collection = context.Accounts.Where(a => a.Category.Name == SelectedCategory.Name);
            foreach (var item in collection)
            {
                if (item.Category != null)
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, item.Category.Name);
                }
                else
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, "");
                }
            }
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
        
        public void AddAccountAfterLogIn()
        {
            IQueryable<Account> collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id);
            foreach (var item in collection)
            {
                if (item.Category != null)
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, item.Category.Name);
                }
                else
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, "");
                }
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
                if (item.Category != null)
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, item.Category.Name);
                }
                else
                {
                    AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, "");
                }
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
                    if (item.Category != null)
                    {
                        AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, item.Category.Name);
                    }
                    else
                    {
                        AddAccountToList(item.Name, item.UserName, XORcipher.Decrypt(item.Password, key), item.LinkToSite, item, "");
                    }
                }
            }
        }

        public void ClearAccounts()
        {
            accounts.Clear();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class AccountInfo
    {
        public AccountInfo(string name, string username, string password, string linkToSite, string categoryName)
        {
            this.Name = name;
            this.UserName = username;
            this.Password = password;
            this.LinkToSite = linkToSite;
            this.CategoryName = categoryName;
        }

        public void SetCommandDelete(Action<object> action)
        {
            deleteCommand = new RelayCommand((o) => action(o));
        }
        public void SetCommandChange(Action<object> action)
        {
            changeCommand = new RelayCommand((o) => action(o));
        }
        public void SetCommandMoveToFavorite(Action<object> action)
        {
            moveToFavoriteCommand = new RelayCommand((o) => action(o));
        }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LinkToSite { get; set; }
        private RelayCommand deleteCommand;
        private RelayCommand changeCommand;
        private RelayCommand moveToFavoriteCommand;
        public ICommand DeleteCmd => deleteCommand;
        public ICommand ChangeCmd => changeCommand;
        public ICommand MoveToFavoriteCommand => moveToFavoriteCommand;
        public string Difficulty { get; set; }
        public string CategoryName { get; set; }
        public bool isInFavorite { get; set; }
    }
}
