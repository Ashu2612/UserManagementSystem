using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;
using CheckBox = System.Windows.Controls.CheckBox;
using Button = System.Windows.Controls.Button;
using DataTable = System.Data.DataTable;
using Page = System.Windows.Controls.Page;
using Path = System.IO.Path;
using System.IO;

namespace UserManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public static string connectionString = "Server=CQMPRDTNE01\\MSSQLSERVER01;Database=UserManagementSystem;Integrated Security=true;";

        public MainPage()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Get the entered text
            string enteredText = e.Text;

            // Validate the entered text (allow only alphanumeric characters)
            if (!IsAlphaNumeric(enteredText))
            {
                // Cancel the input event to prevent the special character from being entered
                e.Handled = true;
            }
        }
        private bool IsAlphaNumeric(string text)
        {
            // Regular expression pattern to match alphanumeric characters
            string pattern = "^[a-zA-Z0-9]+$";

            // Check if the text matches the pattern
            return System.Text.RegularExpressions.Regex.IsMatch(text, pattern);
        }

        private void NewRole_Click(object sender, RoutedEventArgs e)
        {
            UserRoleLbl.Text = string.Empty;
            DescriptionLbl.Text = string.Empty ;
            NewUserGrid.Visibility = Visibility.Hidden;
            NewRoleGrid.Visibility = Visibility.Visible;
            NewUser.Background = Brushes.Transparent;
            NewRole.Background = Brushes.LightSteelBlue;
            ViewUser.Background = Brushes.Transparent;
            ViewUserList.Visibility = Visibility.Hidden;
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            NewRoleGrid.Visibility = Visibility.Hidden;
            NewUserGrid.Visibility = Visibility.Visible;
            NewRole.Background = Brushes.Transparent;
            NewUser.Background = Brushes.LightSteelBlue;
            ViewUser.Background = Brushes.Transparent;
            ViewUserList.Visibility = Visibility.Hidden;
            UserIDLbl.Text = string.Empty;
            FirstNameLbl.Text = string.Empty;
            LastNameLbl.Text = string.Empty;
            DOBLbl.Text = string.Empty;
            LocationLbl.Text = string.Empty;
            EmailLbl.Text = string.Empty;
            UserRoleDrpdw.Text = string.Empty;
            EmailLbl.BorderBrush = Brushes.Black;
            ErrorMsgLbl.Visibility = Visibility.Hidden;
            EditCancel.Visibility = Visibility.Hidden;
            Cancel.Visibility = Visibility.Visible;
            EditUserData.Visibility = Visibility.Hidden;
            UserSave.Visibility = Visibility.Visible;
            // Connection string for the database

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL query to retrieve the UserRole values from the RolesTable
                string query = "SELECT UserRole FROM RolesTable";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Clear existing items in the UserRoleDrpdw ComboBox
                    UserRoleDrpdw.Items.Clear();

                    // Populate UserRoleDrpdw ComboBox with the UserRole values
                    while (reader.Read())
                    {
                        string userRole = reader["UserRole"].ToString();
                        UserRoleDrpdw.Items.Add(userRole);
                    }
                }
            }
        }

        private void EmailLbl_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidEmail = CommonClass.ValidateEmailFormat(EmailLbl.Text);
            if (isValidEmail)
            {
                EmailLbl.BorderBrush = Brushes.Black;
                EmailLbl.ToolTip = null; // Clear the tooltip if the email is valid
                UserSave.IsEnabled = true;
            }
            else
            {
                UserSave.IsEnabled = false;
                EmailLbl.BorderBrush = Brushes.Red;
                EmailLbl.ToolTip = "Invalid Email Format";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CommonClass.ErrorLogging($"New User addition canceled - User Id:{UserIDLbl.Text}, First Name:{FirstNameLbl.Text}, Last Name:{LastNameLbl.Text}");
            UserIDLbl.Text = string.Empty;
            FirstNameLbl.Text = string.Empty;
            LastNameLbl.Text = string.Empty;
            DOBLbl.Text = string.Empty;
            LocationLbl.Text = string.Empty;
            EmailLbl.Text = string.Empty;
            UserRoleDrpdw.Text = string.Empty;
            EmailLbl.BorderBrush = Brushes.Black;
        }
        public void ViewUser_Click(object sender, RoutedEventArgs e)
        {
            NewRoleGrid.Visibility = Visibility.Hidden;
            NewUserGrid.Visibility = Visibility.Hidden;
            NewRole.Background = Brushes.Transparent;
            NewUser.Background = Brushes.Transparent;
            ViewUser.Background = Brushes.LightSteelBlue;
            ViewUserList.Visibility = Visibility.Visible;
            ErrorMsgLbl.Visibility = Visibility.Hidden;
            EditCancel.Visibility = Visibility.Hidden;
            Cancel.Visibility = Visibility.Visible;
            EditUserData.Visibility = Visibility.Hidden;
            UserSave.Visibility = Visibility.Visible;
            SearchByUserIdLbl.Text = string.Empty;
            SearchByUserIdLbl.BorderBrush = Brushes.Black;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole FROM UsersTable", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    UserDataGrid.Children.Clear();

                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Vertical;
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("There is no user data available.");
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            StackPanel innerSp = new StackPanel();
                            innerSp.Orientation = Orientation.Horizontal;

                            TextBlock userId = new TextBlock();
                            userId.Width = 40;
                            userId.Text = row["UserId"].ToString();
                            innerSp.Children.Add(userId);

                            TextBox firstName = new TextBox();
                            firstName.Width = 50;
                            firstName.BorderBrush = Brushes.Transparent;
                            firstName.Text = row["FirstName"].ToString();
                            innerSp.Children.Add(firstName);

                            TextBox lastName = new TextBox();
                            lastName.BorderBrush = Brushes.Transparent;
                            lastName.Width = 50;
                            lastName.Text = row["LastName"].ToString();
                            innerSp.Children.Add(lastName);

                            TextBlock dateOfBirth = new TextBlock();
                            dateOfBirth.Width = 65;
                            dateOfBirth.Text = row["DateOfBirth"].ToString().Replace("00:00:00", "");
                            innerSp.Children.Add(dateOfBirth);

                            TextBlock location = new TextBlock();
                            location.Width = 65;
                            location.Text = row["Location"].ToString();
                            innerSp.Children.Add(location);

                            TextBlock email = new TextBlock();
                            email.Width = 135;
                            email.Text = row["Email"].ToString();
                            innerSp.Children.Add(email);

                            TextBlock userRole = new TextBlock();
                            userRole.Width = 75;
                            userRole.Text = row["UserRole"].ToString();
                            innerSp.Children.Add(userRole);

                            if (userRole.Text == "Administrator" || userRole.Text == "Admin")
                            {
                                // Set the background of the StackPanel to green
                                innerSp.Background = Brushes.LightBlue;
                                firstName.Background = Brushes.LightBlue;
                                lastName.Background = Brushes.LightBlue;
                            }

                            CheckBox cb = new CheckBox();
                            cb.Width = 20;
                            cb.HorizontalAlignment = HorizontalAlignment.Center;
                            cb.VerticalAlignment = VerticalAlignment.Center;
                            innerSp.Children.Add(cb);

                            DateTime dob = DateTime.Parse(row["DateOfBirth"].ToString());
                            if (dob.Month == DateTime.Today.Month)
                            {
                                // Set the background of the StackPanel to green
                                innerSp.Background = Brushes.LightGreen;
                                firstName.Background = Brushes.LightGreen;
                                lastName.Background = Brushes.LightGreen;
                            }
                            Button updt = new Button();
                            updt.Width = 45;
                            updt.Content = "Update";
                            updt.Background = Brushes.Green;
                            updt.Click += (sender, e) =>
                            {
                                // get the user ID from the TextBox
                                string userID = userId.Text;

                                // get the updated values from the TextBoxes
                                string updatedFirstName = firstName.Text;
                                string updatedLastName = lastName.Text;

                                try
                                {
                                    // update the database record
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();
                                        SqlCommand updateCmd = new SqlCommand("UPDATE UsersTable SET FirstName = @FirstName, LastName = @LastName WHERE UserId = @UserId", connection);
                                        updateCmd.Parameters.AddWithValue("@FirstName", updatedFirstName);
                                        updateCmd.Parameters.AddWithValue("@LastName", updatedLastName);
                                        updateCmd.Parameters.AddWithValue("@UserId", userID);
                                        updateCmd.ExecuteNonQuery();
                                        CommonClass.ErrorLogging($"User details updated successfully - User Id: {userId.Text}, First Name: {updatedFirstName}, Last Name: {updatedLastName}");
                                        MessageBox.Show($"User details updated successfully.", $"New First Name - {updatedFirstName} and New Last Name - {updatedLastName}", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error occurred while updating the user data " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            };
                            innerSp.Children.Add(updt);

                            Button Edit = new Button();
                            Edit.Width = 25;
                            Edit.Content = "Edit";
                            Edit.Background = Brushes.LightBlue;
                            Edit.Click += (sender, e) =>
                            {
                                string userIdValue = userId.Text;

                                NewRoleGrid.Visibility = Visibility.Hidden;
                                NewUserGrid.Visibility = Visibility.Visible;
                                NewRole.Background = Brushes.Transparent;
                                NewUser.Background = Brushes.Transparent;
                                ViewUser.Background = Brushes.LightSteelBlue;
                                ViewUserList.Visibility = Visibility.Hidden;
                                EditUserData.Visibility = Visibility.Visible;
                                UserSave.Visibility = Visibility.Hidden;
                                EditCancel.Visibility = Visibility.Visible;
                                Cancel.Visibility = Visibility.Hidden;
                                ErrorMsgLbl.Visibility = Visibility.Hidden;
                                try
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        // SQL query to retrieve the UserRole values from the RolesTable
                                        string query = "SELECT UserRole FROM RolesTable";

                                        SqlCommand command = new SqlCommand(query, connection);

                                        connection.Open();

                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            // Clear existing items in the UserRoleDrpdw ComboBox
                                            UserRoleDrpdw.Items.Clear();

                                            // Populate UserRoleDrpdw ComboBox with the UserRole values
                                            while (reader.Read())
                                            {
                                                string userRole = reader["UserRole"].ToString();
                                                UserRoleDrpdw.Items.Add(userRole);
                                            }
                                        }
                                    }
                                }
                                catch { }
                                try
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();
                                        SqlCommand cmd = new SqlCommand("SELECT UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole FROM UsersTable WHERE UserId = @UserId", connection);
                                        cmd.Parameters.AddWithValue("@UserId", userIdValue);
                                        SqlDataReader reader = cmd.ExecuteReader();

                                        if (reader.Read())
                                        {
                                            UserIDLbl.Text = reader["UserId"].ToString();
                                            FirstNameLbl.Text = reader["FirstName"].ToString();
                                            LastNameLbl.Text = reader["LastName"].ToString();
                                            DOBLbl.Text = reader["DateOfBirth"].ToString();
                                            LocationLbl.Text = reader["Location"].ToString();
                                            EmailLbl.Text = reader["Email"].ToString();
                                            UserRoleDrpdw.SelectedValue = reader["UserRole"].ToString();
                                            EmailLbl.BorderBrush = Brushes.Black;
                                        }
                                        reader.Close();
                                    }
                                }
                                catch (SqlException ex)
                                {

                                }
                            };
                            innerSp.Children.Add(Edit);

                            sp.Children.Add(innerSp);
                        }
                        Button delete = new Button();
                        delete.HorizontalAlignment = HorizontalAlignment.Left;
                        delete.Width = 60;
                        delete.Content = "Delete";
                        delete.Height = 20;
                        delete.Background = Brushes.Red;

                        delete.Click += (sender, e) =>
                        {
                            // loop through all the inner stack panels
                            if (sp.Children.Count >= 1)
                            {
                                for (int i = 0; i < sp.Children.Count; i++)
                                {
                                    if (sp.Children[i] is StackPanel innerSp)
                                    {
                                        // get the checkbox and the user id textbox from the inner stack panel
                                        CheckBox cb = innerSp.Children.OfType<CheckBox>().FirstOrDefault();
                                        TextBlock userId = innerSp.Children.OfType<TextBlock>().FirstOrDefault();

                                        // if the checkbox is checked, delete the corresponding row from the database
                                        if (cb.IsChecked == true)
                                        {
                                            using (SqlConnection connection = new SqlConnection(connectionString))
                                            {
                                                connection.Open();
                                                SqlCommand deleteCmd = new SqlCommand("DELETE FROM UsersTable WHERE UserId = @UserId", connection);
                                                deleteCmd.Parameters.AddWithValue("@UserId", userId.Text);
                                                try
                                                {
                                                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                                                    if (rowsAffected == 0)
                                                    {
                                                        MessageBox.Show($"User with the specified ID {userId.Text} does not exist.", "User Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                                                    }
                                                    else
                                                    {
                                                        CommonClass.ErrorLogging($"User deleted from the database - User Id: {userId.Text}");
                                                        // remove the inner stack panel from the UI
                                                        sp.Children.RemoveAt(i);
                                                        i--;
                                                    }
                                                }
                                                catch (SqlException ex)
                                                {
                                                    MessageBox.Show("An error occurred while deleting the user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                                }
                                            }
                                        }
                                    }
                                }
                                MessageBox.Show("User deleted successfully.");
                            }
                            else { MessageBox.Show("Please select a user to delete."); }

                        };

                        UserTableTopGrid.Children.Add(delete);

                        ScrollViewer sv = new ScrollViewer();
                        sv.Content = sp;

                        UserDataGrid.Children.Clear();
                        UserDataGrid.Children.Add(sv);
                    }

                }
            }
            catch { MessageBox.Show("Unable to fetch user data."); }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole FROM UsersTable Where UserId = @UserId", con);
                    cmd.Parameters.AddWithValue("@UserId", SearchByUserIdLbl.Text);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    UserDataGrid.Children.Clear();

                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Vertical;
                    if (dt.Rows.Count == 0)
                    {
                        SearchByUserIdLbl.BorderBrush = Brushes.Red;
                        SearchByUserIdLbl.ToolTip = "Invalid User Id";
                        MessageBox.Show($"Invalid User Id {SearchByUserIdLbl.Text}.");
                        SearchByUserIdLbl.Text = string.Empty;
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            StackPanel innerSp = new StackPanel();
                            innerSp.Orientation = Orientation.Horizontal;

                            TextBlock userId = new TextBlock();
                            userId.Width = 40;
                            userId.Text = row["UserId"].ToString();
                            innerSp.Children.Add(userId);

                            TextBox firstName = new TextBox();
                            firstName.Width = 50;
                            firstName.BorderBrush = Brushes.Transparent;
                            firstName.Text = row["FirstName"].ToString();
                            innerSp.Children.Add(firstName);

                            TextBox lastName = new TextBox();
                            lastName.BorderBrush = Brushes.Transparent;
                            lastName.Width = 50;
                            lastName.Text = row["LastName"].ToString();
                            innerSp.Children.Add(lastName);

                            TextBlock dateOfBirth = new TextBlock();
                            dateOfBirth.Width = 65;
                            dateOfBirth.Text = row["DateOfBirth"].ToString().Replace("00:00:00", "");
                            innerSp.Children.Add(dateOfBirth);

                            TextBlock location = new TextBlock();
                            location.Width = 65;
                            location.Text = row["Location"].ToString();
                            innerSp.Children.Add(location);

                            TextBlock email = new TextBlock();
                            email.Width = 135;
                            email.Text = row["Email"].ToString();
                            innerSp.Children.Add(email);

                            TextBlock userRole = new TextBlock();
                            userRole.Width = 75;
                            userRole.Text = row["UserRole"].ToString();
                            innerSp.Children.Add(userRole);

                            if (userRole.Text == "Administrator" || userRole.Text == "Admin")
                            {
                                // Set the background of the StackPanel to green
                                innerSp.Background = Brushes.LightBlue;
                                firstName.Background = Brushes.LightBlue;
                                lastName.Background = Brushes.LightBlue;
                            }

                            CheckBox cb = new CheckBox();
                            cb.Width = 20;
                            cb.HorizontalAlignment = HorizontalAlignment.Center;
                            cb.VerticalAlignment = VerticalAlignment.Center;
                            innerSp.Children.Add(cb);

                            DateTime dob = DateTime.Parse(row["DateOfBirth"].ToString());
                            if (dob.Month == DateTime.Today.Month)
                            {
                                // Set the background of the StackPanel to green
                                innerSp.Background = Brushes.LightGreen;
                                firstName.Background = Brushes.LightGreen;
                                lastName.Background = Brushes.LightGreen;
                            }
                            Button updt = new Button();
                            updt.Width = 45;
                            updt.Content = "Update";
                            updt.Background = Brushes.Green;
                            updt.Click += (sender, e) =>
                            {
                                // get the user ID from the TextBox
                                string userID = userId.Text;

                                // get the updated values from the TextBoxes
                                string updatedFirstName = firstName.Text;
                                string updatedLastName = lastName.Text;

                                try
                                {
                                    // update the database record
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();
                                        SqlCommand updateCmd = new SqlCommand("UPDATE UsersTable SET FirstName = @FirstName, LastName = @LastName WHERE UserId = @UserId", connection);
                                        updateCmd.Parameters.AddWithValue("@FirstName", updatedFirstName);
                                        updateCmd.Parameters.AddWithValue("@LastName", updatedLastName);
                                        updateCmd.Parameters.AddWithValue("@UserId", userID);
                                        updateCmd.ExecuteNonQuery();
                                        CommonClass.ErrorLogging($"User details updated successfully - User Id: {userId.Text}, First Name: {updatedFirstName}, Last Name: {updatedLastName}");
                                        MessageBox.Show($"User details updated successfully.", $"New First Name - {updatedFirstName} and New Last Name - {updatedLastName}", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("An error occurred while updating the user data " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            };
                            innerSp.Children.Add(updt);

                            Button Edit = new Button();
                            Edit.Width = 25;
                            Edit.Content = "Edit";
                            Edit.Background = Brushes.LightBlue;
                            Edit.Click += (sender, e) =>
                            {
                                string userIdValue = userId.Text;

                                NewRoleGrid.Visibility = Visibility.Hidden;
                                NewUserGrid.Visibility = Visibility.Visible;
                                NewRole.Background = Brushes.Transparent;
                                NewUser.Background = Brushes.Transparent;
                                ViewUser.Background = Brushes.LightSteelBlue;
                                ViewUserList.Visibility = Visibility.Hidden;
                                EditUserData.Visibility = Visibility.Visible;
                                UserSave.Visibility = Visibility.Hidden;
                                EditCancel.Visibility = Visibility.Visible;
                                Cancel.Visibility = Visibility.Hidden;
                                ErrorMsgLbl.Visibility = Visibility.Hidden;
                                try
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        // SQL query to retrieve the UserRole values from the RolesTable
                                        string query = "SELECT UserRole FROM RolesTable";

                                        SqlCommand command = new SqlCommand(query, connection);

                                        connection.Open();

                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            // Clear existing items in the UserRoleDrpdw ComboBox
                                            UserRoleDrpdw.Items.Clear();

                                            // Populate UserRoleDrpdw ComboBox with the UserRole values
                                            while (reader.Read())
                                            {
                                                string userRole = reader["UserRole"].ToString();
                                                UserRoleDrpdw.Items.Add(userRole);
                                            }
                                        }
                                    }
                                }
                                catch { }
                                try
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();
                                        SqlCommand cmd = new SqlCommand("SELECT UserId, FirstName, LastName, DateOfBirth, Location, Email, UserRole FROM UsersTable WHERE UserId = @UserId", connection);
                                        cmd.Parameters.AddWithValue("@UserId", userIdValue);
                                        SqlDataReader reader = cmd.ExecuteReader();

                                        if (reader.Read())
                                        {
                                            UserIDLbl.Text = reader["UserId"].ToString();
                                            FirstNameLbl.Text = reader["FirstName"].ToString();
                                            LastNameLbl.Text = reader["LastName"].ToString();
                                            DOBLbl.Text = reader["DateOfBirth"].ToString();
                                            LocationLbl.Text = reader["Location"].ToString();
                                            EmailLbl.Text = reader["Email"].ToString();
                                            UserRoleDrpdw.SelectedValue = reader["UserRole"].ToString();
                                            EmailLbl.BorderBrush = Brushes.Black;
                                        }
                                        reader.Close();
                                    }
                                }
                                catch (SqlException ex)
                                {

                                }
                            };
                            innerSp.Children.Add(Edit);

                            sp.Children.Add(innerSp);
                        }
                        Button delete = new Button();
                        delete.HorizontalAlignment = HorizontalAlignment.Left;
                        delete.Width = 60;
                        delete.Content = "Delete";
                        delete.Height = 20;
                        delete.Background = Brushes.Red;
                        delete.Click += (sender, e) =>
                        {
                            // loop through all the inner stack panels
                            for (int i = 0; i < sp.Children.Count; i++)
                            {
                                if (sp.Children[i] is StackPanel innerSp)
                                {
                                    // get the checkbox and the user id textbox from the inner stack panel
                                    CheckBox cb = innerSp.Children.OfType<CheckBox>().FirstOrDefault();
                                    TextBlock userId = innerSp.Children.OfType<TextBlock>().FirstOrDefault();

                                    // if the checkbox is checked, delete the corresponding row from the database
                                    if (cb.IsChecked == true)
                                    {
                                        using (SqlConnection connection = new SqlConnection(connectionString))
                                        {
                                            connection.Open();
                                            SqlCommand deleteCmd = new SqlCommand("DELETE FROM UsersTable WHERE UserId = @UserId", connection);
                                            deleteCmd.Parameters.AddWithValue("@UserId", userId.Text);
                                            try
                                            {
                                                int rowsAffected = deleteCmd.ExecuteNonQuery();

                                                if (rowsAffected == 0)
                                                {
                                                    MessageBox.Show($"User with the specified ID {userId.Text} does not exist.", "User Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                                                }
                                                else
                                                {
                                                    CommonClass.ErrorLogging($"User deleted from the database - User Id: {userId.Text}");
                                                    sp.Children.RemoveAt(i);
                                                    i--;
                                                }
                                            }
                                            catch (SqlException ex)
                                            {
                                                MessageBox.Show("An error occurred while deleting the user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                        }
                                    }
                                }
                            }
                            MessageBox.Show("User deleted successfully.");
                        };

                        UserTableTopGrid.Children.Add(delete);

                        ScrollViewer sv = new ScrollViewer();
                        sv.Content = sp;

                        UserDataGrid.Children.Clear();
                        UserDataGrid.Children.Add(sv);
                    }

                }
            }
            catch { MessageBox.Show("Unable to fetch user data."); }
        }

        private void UserIDLbl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            CommonClass.ErrorLogging($"User data editing canceled - User Id:{UserIDLbl.Text}, First Name:{FirstNameLbl.Text}, Last Name:{LastNameLbl.Text}");
            UserIDLbl.Text = string.Empty;
            FirstNameLbl.Text = string.Empty;
            LastNameLbl.Text = string.Empty;
            DOBLbl.Text = string.Empty;
            LocationLbl.Text = string.Empty;
            EmailLbl.Text = string.Empty;
            UserRoleDrpdw.Text = string.Empty;
            EmailLbl.BorderBrush = Brushes.Black;
            NewRoleGrid.Visibility = Visibility.Hidden;
            NewUserGrid.Visibility = Visibility.Hidden;
            NewRole.Background = Brushes.Transparent;
            NewUser.Background = Brushes.Transparent;
            ViewUser.Background = Brushes.LightSteelBlue;
            ViewUserList.Visibility = Visibility.Visible;
            EditUserData.Visibility = Visibility.Hidden;
            UserSave.Visibility = Visibility.Visible;
            ErrorMsgLbl.Visibility = Visibility.Hidden;
        }
    }
}
