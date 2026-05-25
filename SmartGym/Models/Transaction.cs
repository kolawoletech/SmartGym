using System;

namespace SmartGym.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public int? ClassId { get; set; }
        public string TransactionType { get; set; } // Booking | TopUp | Refund
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Notes { get; set; }
    }
}
