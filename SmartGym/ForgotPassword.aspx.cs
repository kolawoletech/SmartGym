using System;
using SmartGym.BLL;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private readonly MemberService _memberService = new MemberService();

        protected void btnFindQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                string question = _memberService.GetSecurityQuestion(txtEmail.Text);
                if (string.IsNullOrEmpty(question))
                {
                    Show("No account found with that email.", true);
                    return;
                }
                Session["RecoveryEmail"] = txtEmail.Text;
                lblQuestion.Text = question;
                mvSteps.ActiveViewIndex = 1;
            }
            catch (Exception ex)
            {
                FileLogger.LogError("ForgotPassword find: " + ex.Message);
                Show("An error occurred.", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["RecoveryEmail"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    mvSteps.ActiveViewIndex = 0;
                    return;
                }

                bool ok = _memberService.RecoverPassword(email, txtAnswer.Text, txtNewPassword.Text);
                if (!ok)
                {
                    Show("Incorrect security answer.", true);
                    return;
                }

                Session.Remove("RecoveryEmail");
                Session["FlashMessage"] = "Password reset successful. Please log in.";
                Response.Redirect("Login.aspx");
            }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (Exception ex)
            {
                FileLogger.LogError("ForgotPassword reset: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        private void Show(string msg, bool isError)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = isError ? "alert-error" : "alert-success";
            lblMessage.Visible = true;
        }
    }
}
