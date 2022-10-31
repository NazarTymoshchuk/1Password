using data_base;
using data_base.Entities;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1Password
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        public OnePasswordDbContext context = new OnePasswordDbContext(); // to connect AccountsInfo with Accounts in DB
        private ObservableCollection<AccountInfo> accounts;
        XORCipher XORcipher = new XORCipher();
        string key = @"><w\\Dr{GlZIp.x8CFp&i:^HB4B<x#Fpmn0kw,sC>vY&evTwGtqV6r1sDR8@cP#-4nsgXlmqkYH0Iz$.D5fzeE+cl%:I8XN+P4o0s";
        public ViewModel()
        {
            accounts = new ObservableCollection<AccountInfo>();
        }
        public User CurrentUser { get; set; }
        public IEnumerable<AccountInfo> AccountsInfo => accounts;
        public void AddAccount(AccountInfo account)
        {
            accounts.Add(account);
            context.Accounts.Add(new Account()
            {
                Name = account.Name,
                Password = XORcipher.Encrypt(account.Password, key),
                UserId = CurrentUser.Id,
                UserName = account.UserName,
                LinkToSite = account.LinkToSite,
            });
            context.SaveChanges();
        }

        public void AddAccountToList()
        {
            IQueryable<Account> collection = context.Accounts.Where(a => a.UserId == CurrentUser.Id);
            foreach (var item in collection)
            {
                accounts.Add(new AccountInfo()
                {
                    Name = item.Name,
                    UserName = item.UserName,
                    Password = XORcipher.Decrypt(item.Password, key),
                    LinkToSite = item.LinkToSite
                });
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
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LinkToSite { get; set; }
    }
}
