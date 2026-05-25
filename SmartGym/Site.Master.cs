using System;
using System.Web.UI;

namespace SmartGym
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = Session["MemberId"] != null;
            bool isAdmin = string.Equals(Session["Role"] as string, "Admin", StringComparison.OrdinalIgnoreCase);

            lnkDashboard.Visible = loggedIn;
            lnkBookClass.Visible = loggedIn;
            lnkTopUp.Visible = loggedIn;
            lnkTransactions.Visible = loggedIn;
            lnkAdmin.Visible = loggedIn && isAdmin;
            lnkLogout.Visible = loggedIn;
            lnkLogin.Visible = !loggedIn;
            lnkRegister.Visible = !loggedIn;
        }
    }
}
