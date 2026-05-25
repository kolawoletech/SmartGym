using System;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym.BLL
{
    /// <summary>
    /// Business-logic layer for member operations.
    /// Wraps DAL with validation, hashing and error handling.
    /// </summary>
    public class MemberService
    {
        private readonly MemberDAL _memberDal = new MemberDAL();
        private readonly AccountDAL _accountDal = new AccountDAL();

        public int Register(string fullName, string email, string password,
                            string phone, string question, string answer)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name required.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.");

            if (_memberDal.GetMemberByEmail(email) != null)
                throw new InvalidOperationException("An account with this email already exists.");

            Member m = new Member
            {
                FullName = fullName.Trim(),
                Email = email.Trim().ToLower(),
                PasswordHash = PasswordHasher.Hash(password),
                PhoneNumber = phone,
                SecurityQuestion = question,
                SecurityAnswer = PasswordHasher.Hash((answer ?? "").Trim().ToLower()),
                Role = "Member"
            };

            int memberId = _memberDal.RegisterMember(m);

            // Automatically create a default Standard account with 0 credits
            _accountDal.CreateAccount(new MembershipAccount
            {
                MemberId = memberId,
                AccountName = fullName.Trim() + " - Personal",
                MembershipType = "Standard",
                CreditBalance = 0m
            });

            return memberId;
        }

        public Member Authenticate(string email, string password)
        {
            Member m = _memberDal.GetMemberByEmail((email ?? "").Trim().ToLower());
            if (m == null) return null;
            return PasswordHasher.Verify(password, m.PasswordHash) ? m : null;
        }

        public string GetSecurityQuestion(string email)
        {
            Member m = _memberDal.GetMemberByEmail((email ?? "").Trim().ToLower());
            return m?.SecurityQuestion;
        }

        /// <summary>
        /// Verifies the security answer and resets the password.
        /// </summary>
        public bool RecoverPassword(string email, string answer, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                throw new ArgumentException("New password must be at least 6 characters.");

            Member m = _memberDal.GetMemberByEmail((email ?? "").Trim().ToLower());
            if (m == null) return false;

            string answerHash = PasswordHasher.Hash((answer ?? "").Trim().ToLower());
            if (!string.Equals(answerHash, m.SecurityAnswer, StringComparison.OrdinalIgnoreCase))
                return false;

            return _memberDal.UpdatePassword(m.MemberId, PasswordHasher.Hash(newPassword));
        }
    }
}
