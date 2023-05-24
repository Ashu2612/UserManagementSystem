using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UserManagementSystem.Commands;
using UserManagementSystem.Models;
using UserManagementSystem.Views;

namespace UserManagementSystem.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        //using MVVMSampleApp.Commands;
        //using MVVMSampleApp.Models;
        //using System.Windows;
        //using System.Windows.Input;
        private Users user;
        private SolidColorBrush borderBrush;
        public event Action NavigateToMainPage;

        public ICommand LoginCommand { get; }
        public SolidColorBrush BorderBrush
        {
            get => borderBrush;
            set
            {
                borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
        public LoginViewModel()
        {
            user = new Users();
            BorderBrush = Brushes.Black;
            LoginCommand = new RelayCommand((param) => LoggedIn(param));
        }

        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private void LoggedIn(object parameter)
        {

            if (user.UserName == "UserAdmin" && user.Password == "UserAdmin")
            {
                BorderBrush = Brushes.Black;
                AddUserRole addUserRole = new AddUserRole();
                Window currentWindow = Application.Current.MainWindow;
                currentWindow.Content = addUserRole;
            }
            else
            {
                BorderBrush = Brushes.Red;

                MessageBox.Show("Wrong User Name or Password !");
            }
        }
    }
}
