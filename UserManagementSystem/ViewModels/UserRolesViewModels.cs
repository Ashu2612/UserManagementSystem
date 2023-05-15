using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using UserManagementSystem.Commands;
using UserManagementSystem.Views;
using UserManagementSystem.Models;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;

namespace UserManagementSystem.ViewModels
{
    internal class UserRolesViewModels : ViewModelBase
    {
        private UserRoles userRoles;
        private SolidColorBrush borderBrush;
        public event Action NavigateToMainPage;

        public ICommand SubmitCommand { get; }
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
            SubmitCommand = new RelayCommand((param) => Submit(param));
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
