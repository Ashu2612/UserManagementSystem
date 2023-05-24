using System;

namespace UserManagementSystem.Models
{
    internal class UsersData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
    }
}
