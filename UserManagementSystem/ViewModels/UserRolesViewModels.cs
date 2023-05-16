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
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using System.IO;
using DocumentFormat.OpenXml;
using System.Data;
using System.Windows.Controls;
using CheckBox = System.Windows.Controls.CheckBox;

namespace UserManagementSystem.ViewModels
{
    internal class UserRolesViewModels : ViewModelBase
    {
        private UserRoles userRoles;
        private UsersData userData;

        private SolidColorBrush borderBrush;
        private SolidColorBrush userIdBorder;
        private SolidColorBrush firstNameBorder;
        private SolidColorBrush lastNameBorder;
        private SolidColorBrush locationBorder;
        private SolidColorBrush dobBorder;
        private SolidColorBrush emailBorder;
        private SolidColorBrush userRoleBorder;


        public event Action NavigateToMainPage;

        public ICommand NewRolwSubmitCommand { get; }
        public ICommand NewUserSubmit { get; }
        public ICommand ExportToExcelSubmit { get; }
        public ICommand EditUserSubmit { get; }

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
            userData = new UsersData();
            BorderBrush = Brushes.Black;
            UserIdBorder = Brushes.Black;
            FirstNameBorder = Brushes.Black;
            LastNameBorder = Brushes.Black;
            LocationBorder = Brushes.Black;
            DOBBorder = Brushes.Black;
            EmailBorder = Brushes.Black;
            UserRoleBorder = Brushes.Black;
            NewRolwSubmitCommand = new RelayCommand((param) => Submit(param));
            NewUserSubmit = new RelayCommand((param) => NewUser(param));
            ExportToExcelSubmit = new RelayCommand((param) => ExportToExcel(param));
            EditUserSubmit = new RelayCommand((param) => EditUser(param));

        }

        public string UserId
        {
            get => userData.UserId;
            set
            {
                userData.UserId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        public SolidColorBrush UserIdBorder
        {
            get => userIdBorder;
            set
            {
                userIdBorder = value;
                OnPropertyChanged(nameof(UserIdBorder));
            }
        }
        public string FirstName
        {
            get => userData.FirstName;
            set
            {
                userData.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public SolidColorBrush FirstNameBorder
        {
            get => firstNameBorder;
            set
            {
                firstNameBorder = value;
                OnPropertyChanged(nameof(FirstNameBorder));
            }
        }
        public string LastName
        {
            get => userData.LastName;
            set
            {
                userData.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public SolidColorBrush LastNameBorder
        {
            get => lastNameBorder;
            set
            {
                lastNameBorder = value;
                OnPropertyChanged(nameof(LastNameBorder));
            }
        }
        public string Location
        {
            get => userData.Location;
            set
            {
                userData.Location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        public SolidColorBrush LocationBorder
        {
            get => locationBorder;
            set
            {
                locationBorder = value;
                OnPropertyChanged(nameof(LocationBorder));
            }
        }
        public DateTime DOB
        {
            get => userData.DOB = userData.DOB.ToString() == "01-01-0001 00:00:00" ? DateTime.Now : userData.DOB;
            set
            {
                userData.DOB = value;
                OnPropertyChanged(nameof(DOB));
            }
        }
        public SolidColorBrush DOBBorder
        {
            get => dobBorder;
            set
            {
                dobBorder = value;
                OnPropertyChanged(nameof(DOBBorder));
            }
        }
        public string Email
        {
            get => userData.Email;
            set
            {
                userData.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public SolidColorBrush EmailBorder
        {
            get => emailBorder;
            set
            {
                emailBorder = value;
                OnPropertyChanged(nameof(EmailBorder));
            }
        }
        public string UsersRole
        {
            get => userData.UserRole;
            set
            {
                userData.UserRole = value;
                OnPropertyChanged(nameof(UsersRole));
            }
        }
        public SolidColorBrush UserRoleBorder
        {
            get => userRoleBorder;
            set
            {
                userRoleBorder = value;
                OnPropertyChanged(nameof(UserRoleBorder));
            }
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
        private void NewUser(object parameter)
        {
            bool isValid = false;
            bool iserror = false;
            StringBuilder errormsg = new StringBuilder();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                iserror = true;
                errormsg.AppendLine("User Id");
                UserIdBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.FirstName))
            {
                iserror = true;
                errormsg.AppendLine("First Name");
                FirstNameBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.LastName))
            {
                iserror = true;
                errormsg.AppendLine("Last Name");
                LastNameBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.Location))
            {
                iserror = true;
                errormsg.AppendLine("Location");
                LocationBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.Email))
            {
                iserror = true;
                errormsg.AppendLine("Email Id");
                EmailBorder = Brushes.Red;
            }
            try
            {
                if (string.IsNullOrEmpty(userData.UserRole))
                {
                    iserror = true;
                    errormsg.AppendLine("User Role");
                    UserRoleBorder = Brushes.Red;
                }
            }
            catch
            {
                iserror = true;
                errormsg.AppendLine("User Role");
                UserRoleBorder = Brushes.Red;
            }

            if (iserror == true)
            {
                string message = errormsg.ToString();
                MessageBox.Show(string.Concat("Fill the below fields !\n", message));
                return;
            }
            else
            {

                // Get the values from the text boxes and date picker
                int userId = int.Parse(userData.UserId);
                string firstName = userData.FirstName;
                string lastName = userData.LastName;
                DateTime? dateOfBirth = userData.DOB; // Nullable DateTime
                string location = userData.Location;
                string email = userData.Email;
                string userRole = userData.UserRole;

                // Check if UserId is already present in the database
                bool isUserIdExists = CommonClass.CheckUserIdExists(userId, CommonClass.connectionString);

                if (isUserIdExists)
                {
                    CommonClass.ErrorLogging($"User already exists - User Id: {userData.UserId}");
                    MessageBox.Show($"User with User Id {userData.UserId} already exists.");
                }
                else
                {
                    try
                    {
                        // Insert the data into the UsersTable
                        using (SqlConnection connection = new SqlConnection(CommonClass.connectionString))
                        {
                            connection.Open();

                            // Prepare the SQL query
                            string query = "INSERT INTO UsersTable (UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole) " +
                                           "VALUES (@UserId, @FirstName, @LastName, @DateOfBirth, @Location, @Email, @UserRole)";

                            // Create the command object
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Set the parameter values
                                command.Parameters.AddWithValue("@UserId", userId);
                                command.Parameters.AddWithValue("@FirstName", firstName);
                                command.Parameters.AddWithValue("@LastName", lastName);
                                command.Parameters.AddWithValue("@DateOfBirth", (object)dateOfBirth ?? DBNull.Value);
                                command.Parameters.AddWithValue("@Location", location);
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@UserRole", userRole);

                                // Execute the query
                                command.ExecuteNonQuery();
                            }

                            connection.Close();
                        }
                        UserIdBorder = Brushes.Black;
                        FirstNameBorder = Brushes.Black;
                        LastNameBorder = Brushes.Black;
                        LocationBorder = Brushes.Black;
                        EmailBorder = Brushes.Black;
                        UserRoleBorder = Brushes.Black;
                        CommonClass.ErrorLogging($"New user added - User Id: {userData.UserId}");
                        MessageBox.Show("User saved successfully.", $"User Id :- {userId}", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch { MessageBox.Show("Failed to register the user."); }
                }
            }
        }
        private void ExportToExcel(object parameter)
        {
            try
            {
                string DownloadPath = CommonClass.GetDownloadFolderPath() + "\\UserDetails.csv";
                int count = 1;

                string fileNameOnly = Path.GetFileNameWithoutExtension(Path.ChangeExtension(DownloadPath, ".xlsx"));
                string extension = Path.GetExtension(DownloadPath);
                string path = Path.GetDirectoryName(Path.ChangeExtension(DownloadPath, ".xlsx"));
                string newFullPath = Path.ChangeExtension(DownloadPath, ".xlsx");

                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(path, tempFileName + ".xlsx");
                }
                CommonClass.ErrorLogging($"User details exported to Excel sheet - File path: {newFullPath}");
                CommonClass.SQLToCSV("SELECT UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole FROM UsersTable", Path.ChangeExtension(newFullPath, ".csv"));
                MessageBox.Show($"User details downloaded successfully.", "", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch { MessageBox.Show("Failed to export the user list to excel sheet."); }
        }
        private void EditUser(object parameter)
        {
            bool isValid = false;
            bool iserror = false;
            StringBuilder errormsg = new StringBuilder();
            if (string.IsNullOrEmpty(userData.UserId))
            {
                iserror = true;
                errormsg.AppendLine("User Id");
                UserIdBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.FirstName))
            {
                iserror = true;
                errormsg.AppendLine("First Name");
                FirstNameBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.LastName))
            {
                iserror = true;
                errormsg.AppendLine("Last Name");
                LastNameBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.Location))
            {
                iserror = true;
                errormsg.AppendLine("Location");
                LocationBorder = Brushes.Red;
            }
            if (string.IsNullOrEmpty(userData.Email))
            {
                iserror = true;
                errormsg.AppendLine("Email Id");
                EmailBorder = Brushes.Red;
            }
            try
            {
                if (string.IsNullOrEmpty(userData.UserRole))
                {
                    iserror = true;
                    errormsg.AppendLine("User Role");
                    UserRoleBorder = Brushes.Red;
                }
            }
            catch
            {
                iserror = true;
                errormsg.AppendLine("User Role");
                UserRoleBorder = Brushes.Red;
            }

            if (iserror == true)
            {
                string message = errormsg.ToString();
                MessageBox.Show(string.Concat("Fill the below fields !\n", message));
                return;
            }
            else
            {
                int userId = int.Parse(userData.UserId);
                string firstName = userData.FirstName;
                string lastName = userData.LastName;
                DateTime? dateOfBirth = userData.DOB; // Nullable DateTime
                string location = userData.Location;
                string email = userData.Email;
                string userRole = userData.UserRole;

                // Check if UserId is already present in the database
                bool isUserIdExists = CommonClass.CheckUserIdExists(userId, CommonClass.connectionString);

                if (!isUserIdExists)
                {
                    CommonClass.ErrorLogging($"User data doesn't exists to edit - User Id: {userData.UserId}");
                    MessageBox.Show("User data doesn't exists to edit.");
                }
                else
                {
                    // Insert the data into the UsersTable
                    using (SqlConnection connection = new SqlConnection(CommonClass.connectionString))
                    {
                        connection.Open();

                        // Prepare the SQL query
                        string query = "UPDATE UsersTable SET FirstName = @FirstName, LastName = @LastName, " +
                                       "DateOfBirth = @DateOfBirth, Location = @Location, Email = @Email, " +
                                       "UserRole = @UserRole WHERE UserId = @UserId";

                        // Create the command object
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Set the parameter values
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@DateOfBirth", (object)dateOfBirth ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Location", location);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@UserRole", userRole);

                            // Execute the query
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                UserIdBorder = Brushes.Black;
                FirstNameBorder = Brushes.Black;
                LastNameBorder = Brushes.Black;
                LocationBorder = Brushes.Black;
                EmailBorder = Brushes.Black;
                UserRoleBorder = Brushes.Black;
                CommonClass.ErrorLogging($"New user added - User Id: {userData.UserId}");
                MessageBox.Show("User saved successfully.", $"User Id :- {userId}", MessageBoxButton.OK, MessageBoxImage.Information);
                MainPage mainPage = new MainPage();
                mainPage.NewRoleGrid.Visibility = Visibility.Hidden;
                mainPage.NewUserGrid.Visibility = Visibility.Hidden;
                mainPage.NewRole.Background = Brushes.Transparent;
                mainPage.NewUser.Background = Brushes.Transparent;
                mainPage.ViewUser.Background = Brushes.LightSteelBlue;
                mainPage.ViewUserList.Visibility = Visibility.Visible;
                mainPage.ErrorMsgLbl.Visibility = Visibility.Hidden;
                mainPage.EditUserData.Visibility = Visibility.Hidden;
                mainPage.UserSave.Visibility = Visibility.Visible;
            }
        }
       
    }
}
