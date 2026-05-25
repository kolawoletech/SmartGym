using System;
using System.IO;
using System.Web;
using SmartGym.Utilities;

namespace SmartGym
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Ensure log + report folders exist on startup
            EnsureFolder("~/App_Data/Logs");
            EnsureFolder("~/App_Data/Reports");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception last = Server.GetLastError();
            if (last != null)
                FileLogger.LogError("UNHANDLED: " + last.ToString());
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Regenerate session ID on each new session (mitigates fixation)
        }

        private void EnsureFolder(string virtualPath)
        {
            try
            {
                string physical = Server.MapPath(virtualPath);
                if (!Directory.Exists(physical))
                    Directory.CreateDirectory(physical);
            }
            catch { /* ignore - logger will pick up later issues */ }
        }
    }
}
