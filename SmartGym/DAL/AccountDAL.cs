using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SmartGym.Models;

namespace SmartGym.DAL
{
    /// <summary>
    /// Data-access for MembershipAccounts.
    /// </summary>
    public class AccountDAL
    {
        public int CreateAccount(MembershipAccount account)
        {
            const string sql = @"
                INSERT INTO MembershipAccounts
                    (MemberId, AccountName, MembershipType, CreditBalance)
                VALUES
                    (@MemberId, @AccountName, @MembershipType, @CreditBalance);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object newId = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@MemberId", account.MemberId),
                new SqlParameter("@AccountName", account.AccountName),
                new SqlParameter("@MembershipType", account.MembershipType),
                new SqlParameter("@CreditBalance", account.CreditBalance));

            return Convert.ToInt32(newId);
        }

        public List<MembershipAccount> GetAccountsForMember(int memberId)
        {
            List<MembershipAccount> accounts = new List<MembershipAccount>();
            const string sql = "SELECT * FROM MembershipAccounts WHERE MemberId = @Id ORDER BY AccountId";

            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", memberId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accounts.Add(new MembershipAccount
                        {
                            AccountId = Convert.ToInt32(reader["AccountId"]),
                            MemberId = Convert.ToInt32(reader["MemberId"]),
                            AccountName = reader["AccountName"].ToString(),
                            MembershipType = reader["MembershipType"].ToString(),
                            CreditBalance = Convert.ToDecimal(reader["CreditBalance"]),
                            DateCreated = Convert.ToDateTime(reader["DateCreated"])
                        });
                    }
                }
            }
            return accounts;
        }

        public MembershipAccount GetAccountById(int accountId)
        {
            const string sql = "SELECT * FROM MembershipAccounts WHERE AccountId = @Id";
            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", accountId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MembershipAccount
                        {
                            AccountId = Convert.ToInt32(reader["AccountId"]),
                            MemberId = Convert.ToInt32(reader["MemberId"]),
                            AccountName = reader["AccountName"].ToString(),
                            MembershipType = reader["MembershipType"].ToString(),
                            CreditBalance = Convert.ToDecimal(reader["CreditBalance"]),
                            DateCreated = Convert.ToDateTime(reader["DateCreated"])
                        };
                    }
                }
            }
            return null;
        }

        public bool AdjustBalance(int accountId, decimal delta)
        {
            const string sql = @"UPDATE MembershipAccounts
                                 SET CreditBalance = CreditBalance + @Delta
                                 WHERE AccountId = @Id";

            int rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@Delta", delta),
                new SqlParameter("@Id", accountId));
            return rows > 0;
        }
    }
}
