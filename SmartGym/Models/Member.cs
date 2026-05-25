using System;

namespace SmartGym.Models
{
    /// <summary>
    /// Represents a registered SmartGym member.
    /// </summary>
    public class Member
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string Role { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
