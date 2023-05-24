using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UserManagementSystem.Commands;
using UserManagementSystem.Models;
using UserManagementSystem.Views;
using Windows.System;

namespace UserManagementSystem.ViewModels
{
    internal class UserRolesViewModels : ViewModelBase
    {
        private UserRoles userRoles;


        private SolidColorBrush borderBrush;


        public event Action NavigateToMainPage;

        public ICommand NewRolwSubmitCommand { get; }
        public ICommand NewUserAdd { get; }
        public ICommand UserData { get; }


        public SolidColorBrush BorderBrush
        {
            get => borderBrush;
            set
            {
                borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
        public UserRolesViewModels()
        {
            userRoles = new UserRoles();
            BorderBrush = Brushes.Black;
            NewRolwSubmitCommand = new RelayCommand((param) => Submit(param));
            NewUserAdd = new RelayCommand((param) => NewUser(param));
            UserData = new RelayCommand((param) => ViewSearchUser(param));

        }


        public string UserRole
        {
            get => userRoles.UserRole;
            set
            {
                userRoles.UserRole = value;
                OnPropertyChanged(nameof(UserRole));
            }
        }
        public string Description
        {
            get => userRoles.Description;
            set
            {
                userRoles.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private void NewUser(object parameter)
        {
            NewUser newUser = new NewUser();
            Window currentWindow = Application.Current.MainWindow;
            currentWindow.Content = newUser;
        }
        private void ViewSearchUser(object parameter)
        {
            ViewUser view = new ViewUser();
            Window currentWindow = Application.Current.MainWindow;
            currentWindow.Content = view;
        }
        private void Submit(object parameter)
        {
            if (string.IsNullOrEmpty(userRoles.UserRole))
            {
                BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(CommonClass.connectionString))
                    {
                        connection.Open();
                        string InsertRole = $"INSERT INTO [UserManagementSystem].[dbo].[RolesTable] ([UserRole], [RoleDescription])\r\nSELECT '{userRoles.UserRole}', '{userRoles.Description}'\r\nWHERE NOT EXISTS (\r\n    SELECT 1\r\n    FROM [UserManagementSystem].[dbo].[RolesTable]\r\n    WHERE [UserRole] = '{userRoles.UserRole}'\r\n);";
                        using (SqlCommand command = new SqlCommand(InsertRole, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected != 0)
                            {
                                CommonClass.ErrorLogging($"New User role added - {userRoles.UserRole}");
                                MessageBox.Show("New user role registration successful.");
                            }
                            else
                            {
                                CommonClass.ErrorLogging($"User already exists - {userRoles.UserRole}");
                                MessageBox.Show("User role already exists.");
                            }
                            connection.Close();

                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to update the user role.");
                    CommonClass.ErrorLogging($"New User addition failed - {userRoles.UserRole}");
                }
            }
        }


    }
}
