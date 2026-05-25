using System;
using SmartGym.BLL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly MemberService _memberService = new MemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["FlashMessage"] != null)
            {
                lblFlash.Text = Session["FlashMessage"].ToString();
                lblFlash.Visible = true;
                Session.Remove("FlashMessage");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                Member m = _memberService.Authenticate(txtEmail.Text, txtPassword.Text);
                if (m == null)
                {
                    ShowError("Invalid email or password.");
                    return;
                }

                // Session-based authentication
                Session["MemberId"] = m.MemberId;
                Session["MemberName"] = m.FullName;
                Session["Role"] = m.Role;

                Response.Redirect("Dashboard.aspx");
            }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (Exception ex)
            {
                FileLogger.LogError("Login error: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        private void ShowError(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;
        }
    }
}
