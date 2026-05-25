using System;
using System.Data;
using System.Data.SqlClient;
using SmartGym.Models;

namespace SmartGym.DAL
{
    /// <summary>
    /// Data-access for Transactions and BookingLogs.
    /// </summary>
    public class TransactionDAL
    {
        public int AddTransaction(Transaction t)
        {
            const string sql = @"
                INSERT INTO Transactions
                    (AccountId, ClassId, TransactionType, Amount, BalanceAfter, Notes)
                VALUES
                    (@AccountId, @ClassId, @Type, @Amount, @Balance, @Notes);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object id = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@AccountId", t.AccountId),
                new SqlParameter("@ClassId", (object)t.ClassId ?? DBNull.Value),
                new SqlParameter("@Type", t.TransactionType),
                new SqlParameter("@Amount", t.Amount),
                new SqlParameter("@Balance", t.BalanceAfter),
                new SqlParameter("@Notes", (object)t.Notes ?? DBNull.Value));

            return Convert.ToInt32(id);
        }

        public void AddBookingLog(int memberId, int classId, decimal creditsDeducted, decimal remainingBalance)
        {
            const string sql = @"INSERT INTO BookingLogs
                                    (MemberId, ClassId, CreditsDeducted, RemainingBalance)
                                 VALUES
                                    (@MemberId, @ClassId, @Deducted, @Balance)";

            DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MemberId", memberId),
                new SqlParameter("@ClassId", classId),
                new SqlParameter("@Deducted", creditsDeducted),
                new SqlParameter("@Balance", remainingBalance));
        }

        /// <summary>
        /// Returns the full transaction history for a member as a DataTable (binds easily to GridView).
        /// </summary>
        public DataTable GetTransactionsForMember(int memberId)
        {
            const string sql = @"
                SELECT  t.TransactionId,
                        a.AccountName,
                        ISNULL(f.ClassName, '-') AS ClassName,
                        t.TransactionType,
                        t.Amount,
                        t.BalanceAfter,
                        t.TransactionDate,
                        ISNULL(t.Notes,'') AS Notes
                FROM    Transactions t
                JOIN    MembershipAccounts a ON t.AccountId = a.AccountId
                LEFT JOIN FitnessClasses   f ON t.ClassId   = f.ClassId
                WHERE   a.MemberId = @MemberId
                ORDER BY t.TransactionDate DESC";

            return DatabaseHelper.ExecuteDataTable(sql,
                new SqlParameter("@MemberId", memberId));
        }

        /// <summary>
        /// Returns a DataSet to be used for XML reporting.
        /// </summary>
        public DataSet GetTransactionsDataSet(int memberId)
        {
            const string sql = @"
                SELECT  t.TransactionId, t.AccountId, t.ClassId, t.TransactionType,
                        t.Amount, t.BalanceAfter, t.TransactionDate, t.Notes
                FROM    Transactions t
                JOIN    MembershipAccounts a ON t.AccountId = a.AccountId
                WHERE   a.MemberId = @MemberId
                ORDER BY t.TransactionDate DESC";

            return DatabaseHelper.ExecuteDataSet(sql, "Transactions",
                new SqlParameter("@MemberId", memberId));
        }
    }
}
