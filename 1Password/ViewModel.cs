using data_base.Entities;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1Password
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        private ObservableCollection<AccountInfo> accounts;

        public ViewModel()
        {
            accounts = new ObservableCollection<AccountInfo>();
        }
        
        public IEnumerable<AccountInfo> AccountsInfo => accounts;
        public void AddMessage(AccountInfo account)
        {
            accounts.Add(account);
        }
        public void ClearMessages()
        {
            accounts.Clear();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class AccountInfo
    {
        
    }
}
