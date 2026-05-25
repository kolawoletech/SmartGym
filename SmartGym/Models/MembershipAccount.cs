using System;

namespace SmartGym.Models
{
    /// <summary>
    /// A membership account belonging to a Member.
    /// A Member may have multiple accounts (e.g. Personal, Family).
    /// </summary>
    public class MembershipAccount
    {
        public int AccountId { get; set; }
        public int MemberId { get; set; }
        public string AccountName { get; set; }
        public string MembershipType { get; set; }
        public decimal CreditBalance { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
