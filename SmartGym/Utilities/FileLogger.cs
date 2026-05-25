using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace SmartGym.Utilities
{
    /// <summary>
    /// File I/O helper. Uses StreamWriter to append booking log entries and
    /// StreamReader to read them back for display.
    /// Log format (pipe-delimited, one line per booking):
    /// yyyy-MM-dd HH:mm:ss | MemberName | ClassName | CreditsDeducted | RemainingBalance
    /// </summary>
    public static class FileLogger
    {
        private static string LogFolder
        {
            get
            {
                string configured = ConfigurationManager.AppSettings["LogFolder"] ?? "~/App_Data/Logs/";
                return HttpContext.Current != null
                    ? HttpContext.Current.Server.MapPath(configured)
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Logs\\");
            }
        }

        public static string BookingLogPath => Path.Combine(LogFolder, "booking_log.txt");
        public static string ErrorLogPath => Path.Combine(LogFolder, "error_log.txt");

        /// <summary>
        /// Appends a booking entry using StreamWriter.
        /// </summary>
        public static void LogBooking(string memberName, string className,
                                      decimal creditsDeducted, decimal remainingBalance)
        {
            try
            {
                EnsureFolderExists();
                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss} | {1} | {2} | {3:F2} | {4:F2}",
                    DateTime.Now, memberName, className, creditsDeducted, remainingBalance);

                using (StreamWriter writer = new StreamWriter(BookingLogPath, append: true))
                {
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                LogError("LogBooking failed: " + ex.Message);
            }
        }

        public static void LogError(string message)
        {
            try
            {
                EnsureFolderExists();
                using (StreamWriter writer = new StreamWriter(ErrorLogPath, append: true))
                {
                    writer.WriteLine("{0:yyyy-MM-dd HH:mm:ss} | {1}", DateTime.Now, message);
                }
            }
            catch
            {
                // Swallow - logging must never throw to caller.
            }
        }

        /// <summary>
        /// Reads back all booking log entries using StreamReader.
        /// </summary>
        public static string ReadBookingLog()
        {
            if (!File.Exists(BookingLogPath))
                return "(no bookings have been logged yet)";

            using (StreamReader reader = new StreamReader(BookingLogPath))
            {
                return reader.ReadToEnd();
            }
        }

        private static void EnsureFolderExists()
        {
            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);
        }
    }
}
