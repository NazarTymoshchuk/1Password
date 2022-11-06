using PropertyChanged;
using System;
using System.Windows.Input;

namespace _1Password
{
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
