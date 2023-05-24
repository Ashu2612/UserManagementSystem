using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UserManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for AddUserRole.xaml
    /// </summary>
    public partial class AddUserRole : UserControl
    {
        public AddUserRole()
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
    }
}
