using System;
using System.Data;
using System.Data.SqlClient;
using SmartGym.Models;

namespace SmartGym.DAL
{
    /// <summary>
    /// Data-access for the Members table. Demonstrates SqlDataReader (SELECT)
    /// and parameterised INSERT / UPDATE / DELETE.
    /// </summary>
    public class MemberDAL
    {
        // --- INSERT ----------------------------------------------------------
        public int RegisterMember(Member member)
        {
            const string sql = @"
                INSERT INTO Members
                    (FullName, Email, PasswordHash, PhoneNumber,
                     SecurityQuestion, SecurityAnswer, Role)
                VALUES
                    (@FullName, @Email, @PasswordHash, @PhoneNumber,
                     @SecurityQuestion, @SecurityAnswer, @Role);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object newId = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@FullName", member.FullName),
                new SqlParameter("@Email", member.Email),
                new SqlParameter("@PasswordHash", member.PasswordHash),
                new SqlParameter("@PhoneNumber", (object)member.PhoneNumber ?? DBNull.Value),
                new SqlParameter("@SecurityQuestion", member.SecurityQuestion),
                new SqlParameter("@SecurityAnswer", member.SecurityAnswer),
                new SqlParameter("@Role", member.Role ?? "Member"));

            return Convert.ToInt32(newId);
        }

        // --- SELECT (SqlDataReader) ------------------------------------------
        public Member GetMemberByEmail(string email)
        {
            const string sql = "SELECT * FROM Members WHERE Email = @Email";

            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapMember(reader);
                }
            }
            return null;
        }

        public Member GetMemberById(int memberId)
        {
            const string sql = "SELECT * FROM Members WHERE MemberId = @Id";

            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", memberId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapMember(reader);
                }
            }
            return null;
        }

        // --- UPDATE ----------------------------------------------------------
        public bool UpdatePassword(int memberId, string newPasswordHash)
        {
            const string sql = "UPDATE Members SET PasswordHash = @Hash WHERE MemberId = @Id";
            int rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@Hash", newPasswordHash),
                new SqlParameter("@Id", memberId));
            return rows > 0;
        }

        // --- DELETE ----------------------------------------------------------
        public bool DeleteMember(int memberId)
        {
            const string sql = "DELETE FROM Members WHERE MemberId = @Id";
            int rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@Id", memberId));
            return rows > 0;
        }

        // --- Helpers ---------------------------------------------------------
        private Member MapMember(SqlDataReader reader)
        {
            return new Member
            {
                MemberId = Convert.ToInt32(reader["MemberId"]),
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString(),
                SecurityQuestion = reader["SecurityQuestion"].ToString(),
                SecurityAnswer = reader["SecurityAnswer"].ToString(),
                Role = reader["Role"].ToString(),
                DateRegistered = Convert.ToDateTime(reader["DateRegistered"])
            };
        }
    }
}
