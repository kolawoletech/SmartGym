using System;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym.BLL
{
    /// <summary>
    /// Business-logic for class bookings and credit top-ups.
    /// Co-ordinates DAL operations, file logging, and audit logging.
    /// </summary>
    public class BookingService
    {
        private readonly AccountDAL _accountDal = new AccountDAL();
        private readonly ClassDAL _classDal = new ClassDAL();
        private readonly MemberDAL _memberDal = new MemberDAL();
        private readonly TransactionDAL _txDal = new TransactionDAL();

        /// <summary>
        /// Books a fitness class for a given account.
        /// 1. Validates credits
        /// 2. Deducts balance
        /// 3. Records Transaction + BookingLog rows
        /// 4. Appends to text log file (StreamWriter)
        /// </summary>
        public string BookClass(int accountId, int classId)
        {
            MembershipAccount acc = _accountDal.GetAccountById(accountId);
            if (acc == null) throw new InvalidOperationException("Account not found.");

            FitnessClass cls = _classDal.GetClassById(classId);
            if (cls == null) throw new InvalidOperationException("Class not found.");

            if (acc.CreditBalance < cls.CreditCost)
                throw new InvalidOperationException(
                    $"Insufficient credits. Required: {cls.CreditCost:F2}, Available: {acc.CreditBalance:F2}.");

            // Deduct
            _accountDal.AdjustBalance(accountId, -cls.CreditCost);
            decimal newBalance = acc.CreditBalance - cls.CreditCost;

            // Record transaction
            _txDal.AddTransaction(new Transaction
            {
                AccountId = accountId,
                ClassId = classId,
                TransactionType = "Booking",
                Amount = cls.CreditCost,
                BalanceAfter = newBalance,
                Notes = "Booked " + cls.ClassName
            });

            // Audit log row
            _txDal.AddBookingLog(acc.MemberId, classId, cls.CreditCost, newBalance);

            // File I/O log
            Member member = _memberDal.GetMemberById(acc.MemberId);
            FileLogger.LogBooking(member.FullName, cls.ClassName, cls.CreditCost, newBalance);

            return $"Booking confirmed for {cls.ClassName}. New balance: {newBalance:F2} credits.";
        }

        /// <summary>
        /// Tops up credits and records a transaction.
        /// </summary>
        public string TopUp(int accountId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Top-up amount must be positive.");

            MembershipAccount acc = _accountDal.GetAccountById(accountId);
            if (acc == null) throw new InvalidOperationException("Account not found.");

            _accountDal.AdjustBalance(accountId, amount);
            decimal newBalance = acc.CreditBalance + amount;

            _txDal.AddTransaction(new Transaction
            {
                AccountId = accountId,
                ClassId = null,
                TransactionType = "TopUp",
                Amount = amount,
                BalanceAfter = newBalance,
                Notes = "Credit top-up"
            });

            return $"Top-up successful. New balance: {newBalance:F2} credits.";
        }
    }
}
