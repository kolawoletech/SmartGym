using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;
using SmartGym.DAL;

namespace SmartGym.Utilities
{
    /// <summary>
    /// Generates XML reports for transactions and class schedules and
    /// demonstrates reading XML back into a DataSet.
    /// </summary>
    public static class XmlReporter
    {
        private static string ReportFolder
        {
            get
            {
                string configured = ConfigurationManager.AppSettings["ReportFolder"] ?? "~/App_Data/Reports/";
                return HttpContext.Current != null
                    ? HttpContext.Current.Server.MapPath(configured)
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Reports\\");
            }
        }

        /// <summary>
        /// Writes a member's transaction history to XML using DataSet.WriteXml.
        /// </summary>
        public static string ExportTransactionsToXml(int memberId)
        {
            EnsureFolder();
            string path = Path.Combine(ReportFolder,
                $"transactions_member{memberId}_{DateTime.Now:yyyyMMdd_HHmmss}.xml");

            DataSet ds = new TransactionDAL().GetTransactionsDataSet(memberId);
            ds.WriteXml(path, XmlWriteMode.WriteSchema);
            return path;
        }

        /// <summary>
        /// Writes the fitness class schedule to XML using a manual XmlWriter,
        /// to demonstrate low-level XML writing.
        /// </summary>
        public static string ExportClassScheduleToXml()
        {
            EnsureFolder();
            string path = Path.Combine(ReportFolder,
                $"class_schedule_{DateTime.Now:yyyyMMdd_HHmmss}.xml");

            DataSet ds = new ClassDAL().GetClassesDataSet();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                Encoding = System.Text.Encoding.UTF8
            };

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ClassSchedule");
                writer.WriteAttributeString("generated", DateTime.Now.ToString("u"));

                foreach (DataRow row in ds.Tables["FitnessClasses"].Rows)
                {
                    writer.WriteStartElement("Class");
                    writer.WriteAttributeString("id", row["ClassId"].ToString());
                    writer.WriteElementString("Name", row["ClassName"].ToString());
                    writer.WriteElementString("Instructor", row["Instructor"].ToString());
                    writer.WriteElementString("Schedule", Convert.ToDateTime(row["Schedule"]).ToString("u"));
                    writer.WriteElementString("Capacity", row["Capacity"].ToString());
                    writer.WriteElementString("CreditCost", row["CreditCost"].ToString());
                    writer.WriteElementString("Description",
                        row["Description"] == DBNull.Value ? "" : row["Description"].ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return path;
        }

        /// <summary>
        /// Demonstrates reading an XML file back into a DataSet.
        /// </summary>
        public static DataSet ReadXmlIntoDataSet(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
                throw new FileNotFoundException("XML report not found.", xmlFilePath);

            DataSet ds = new DataSet();
            ds.ReadXml(xmlFilePath);
            return ds;
        }

        private static void EnsureFolder()
        {
            if (!Directory.Exists(ReportFolder))
                Directory.CreateDirectory(ReportFolder);
        }
    }
}
