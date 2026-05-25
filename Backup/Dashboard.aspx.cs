using System;
using System.Collections.Generic;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly MemberDAL _memberDal = new MemberDAL();
        private readonly AccountDAL _accountDal = new AccountDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MemberId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            int memberId = (int)Session["MemberId"];

            try
            {
                Member m = _memberDal.GetMemberById(memberId);
                if (m == null) return;

                litName.Text = m.FullName;
                litSince.Text = m.DateRegistered.ToString("yyyy-MM-dd");

                List<MembershipAccount> accounts = _accountDal.GetAccountsForMember(memberId);
                gvAccounts.DataSource = accounts;
                gvAccounts.DataBind();

                litAccountCount.Text = accounts.Count.ToString();
                decimal total = 0m;
                foreach (var a in accounts) total += a.CreditBalance;
                litCredits.Text = total.ToString("F2");
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Dashboard load: " + ex.Message);
            }
        }

        protected void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                _accountDal.CreateAccount(new MembershipAccount
                {
                    MemberId = (int)Session["MemberId"],
                    AccountName = txtAccountName.Text.Trim(),
                    MembershipType = ddlType.SelectedValue,
                    CreditBalance = 0m
                });

                ShowMessage("Account created successfully.", false);
                txtAccountName.Text = "";
                LoadDashboard();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AddAccount: " + ex.Message);
                ShowMessage(ex.Message, true);
            }
        }

        private void ShowMessage(string msg, bool isError)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = isError ? "alert-error" : "alert-success";
            lblMessage.Visible = true;
        }
    }
}
