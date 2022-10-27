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

namespace _1Password
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        //OnePasswordDbContext onePasswordDbContext = new OnePasswordDbContext(); // to connect AccountsInfo with Accounts in DB
        private ObservableCollection<AccountInfo> accounts;

        public ViewModel()
        {
            accounts = new ObservableCollection<AccountInfo>();
        }
        public User CurrentUser { get; set; }
        public IEnumerable<AccountInfo> AccountsInfo => accounts;
        public void AddAccount(AccountInfo account)
        {
            accounts.Add(account);
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
