using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UserManagementSystem.Commands;
using UserManagementSystem.Models;
using UserManagementSystem.Views;

namespace UserManagementSystem.ViewModels
{
    internal class NewUserViewModel : ViewModelBase
    {
        private UsersData userData;

        private SolidColorBrush userIdBorder;
        private SolidColorBrush firstNameBorder;
        private SolidColorBrush lastNameBorder;
        private SolidColorBrush locationBorder;
        private SolidColorBrush dobBorder;
        private SolidColorBrush emailBorder;
        private SolidColorBrush userRoleBorder;


        public ICommand NewUserSubmit { get; }
        public ICommand UserData { get; }
        public ICommand AddNewRole { get; }



        public NewUserViewModel()
        {
            userData = new UsersData();
            UserIdBorder = Brushes.Black;
            FirstNameBorder = Brushes.Black;
            LastNameBorder = Brushes.Black;
            LocationBorder = Brushes.Black;
            DOBBorder = Brushes.Black;
            EmailBorder = Brushes.Black;
            UserRoleBorder = Brushes.Black;
            NewUserSubmit = new RelayCommand((param) => NewUser(param));
            UserData = new RelayCommand((param) => ViewSearchUser(param));
            AddNewRole = new RelayCommand((param) => AddUserRole(param));


        }
        private void ViewSearchUser(object parameter)
        {
            ViewUser view = new ViewUser();
            Window currentWindow = Application.Current.MainWindow;
            currentWindow.Content = view;
        }
        private void AddUserRole(object parameter)
        {
            AddUserRole addUserRole = new AddUserRole();
            Window currentWindow = Application.Current.MainWindow;
            currentWindow.Content = addUserRole;
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
       
    }
}
