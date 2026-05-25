using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SmartGym.Models;

namespace SmartGym.DAL
{
    /// <summary>
    /// Data-access for FitnessClasses.
    /// Demonstrates DataSet usage via DatabaseHelper.ExecuteDataSet.
    /// </summary>
    public class ClassDAL
    {
        public int AddClass(FitnessClass fc)
        {
            const string sql = @"
                INSERT INTO FitnessClasses
                    (ClassName, Instructor, Schedule, Capacity, CreditCost, Description)
                VALUES
                    (@Name, @Instr, @Sched, @Cap, @Cost, @Desc);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object id = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@Name", fc.ClassName),
                new SqlParameter("@Instr", fc.Instructor),
                new SqlParameter("@Sched", fc.Schedule),
                new SqlParameter("@Cap", fc.Capacity),
                new SqlParameter("@Cost", fc.CreditCost),
                new SqlParameter("@Desc", (object)fc.Description ?? DBNull.Value));
            return Convert.ToInt32(id);
        }

        public bool UpdateClass(FitnessClass fc)
        {
            const string sql = @"UPDATE FitnessClasses
                                 SET ClassName=@Name, Instructor=@Instr, Schedule=@Sched,
                                     Capacity=@Cap, CreditCost=@Cost, Description=@Desc
                                 WHERE ClassId=@Id";
            int rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@Id", fc.ClassId),
                new SqlParameter("@Name", fc.ClassName),
                new SqlParameter("@Instr", fc.Instructor),
                new SqlParameter("@Sched", fc.Schedule),
                new SqlParameter("@Cap", fc.Capacity),
                new SqlParameter("@Cost", fc.CreditCost),
                new SqlParameter("@Desc", (object)fc.Description ?? DBNull.Value));
            return rows > 0;
        }

        public bool DeleteClass(int classId)
        {
            const string sql = "DELETE FROM FitnessClasses WHERE ClassId=@Id";
            int rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@Id", classId));
            return rows > 0;
        }

        public List<FitnessClass> GetAllClasses()
        {
            List<FitnessClass> list = new List<FitnessClass>();
            DataTable table = DatabaseHelper.ExecuteDataTable(
                "SELECT * FROM FitnessClasses ORDER BY Schedule");

            foreach (DataRow row in table.Rows)
            {
                list.Add(new FitnessClass
                {
                    ClassId = Convert.ToInt32(row["ClassId"]),
                    ClassName = row["ClassName"].ToString(),
                    Instructor = row["Instructor"].ToString(),
                    Schedule = Convert.ToDateTime(row["Schedule"]),
                    Capacity = Convert.ToInt32(row["Capacity"]),
                    CreditCost = Convert.ToDecimal(row["CreditCost"]),
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                });
            }
            return list;
        }

        public FitnessClass GetClassById(int classId)
        {
            const string sql = "SELECT * FROM FitnessClasses WHERE ClassId=@Id";
            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", classId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new FitnessClass
                        {
                            ClassId = Convert.ToInt32(reader["ClassId"]),
                            ClassName = reader["ClassName"].ToString(),
                            Instructor = reader["Instructor"].ToString(),
                            Schedule = Convert.ToDateTime(reader["Schedule"]),
                            Capacity = Convert.ToInt32(reader["Capacity"]),
                            CreditCost = Convert.ToDecimal(reader["CreditCost"]),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a DataSet for XML export demonstration.
        /// </summary>
        public DataSet GetClassesDataSet()
        {
            return DatabaseHelper.ExecuteDataSet(
                "SELECT ClassId, ClassName, Instructor, Schedule, Capacity, CreditCost, Description FROM FitnessClasses ORDER BY Schedule",
                "FitnessClasses");
        }
    }
}
