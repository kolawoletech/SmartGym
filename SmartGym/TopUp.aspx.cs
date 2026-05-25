using System;
using System.Linq;
using SmartGym.BLL;
using SmartGym.DAL;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class TopUp : System.Web.UI.Page
    {
        private readonly AccountDAL _accountDal = new AccountDAL();
        private readonly BookingService _bookingService = new BookingService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MemberId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack) BindAccounts();
        }

        private void BindAccounts()
        {
            int memberId = (int)Session["MemberId"];
            var accounts = _accountDal.GetAccountsForMember(memberId)
                .Select(a => new
                {
                    a.AccountId,
                    DisplayText = $"{a.AccountName} ({a.MembershipType}) - {a.CreditBalance:F2} credits"
                }).ToList();
            ddlAccount.DataSource = accounts;
            ddlAccount.DataBind();
        }

        protected void btnTopUp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int accountId = int.Parse(ddlAccount.SelectedValue);
                decimal amount = decimal.Parse(txtAmount.Text);

                string result = _bookingService.TopUp(accountId, amount);
                Show(result, false);
                txtAmount.Text = "";
                BindAccounts();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("TopUp: " + ex.Message);
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
