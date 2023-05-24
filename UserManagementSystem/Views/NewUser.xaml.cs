using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UserManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : UserControl
    {
        public NewUser()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(CommonClass.connectionString))
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
    }
}
