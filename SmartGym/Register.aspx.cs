using System;
using SmartGym.BLL;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly MemberService _memberService = new MemberService();

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int id = _memberService.Register(
                    txtFullName.Text,
                    txtEmail.Text,
                    txtPassword.Text,
                    txtPhone.Text,
                    ddlQuestion.SelectedValue,
                    txtAnswer.Text);

                Session["FlashMessage"] = "Registration successful! Please log in.";
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Register failed: " + ex.Message);
                ShowError(ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;
        }
    }
}
