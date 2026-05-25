using System;
using System.Collections.Generic;
using System.Linq;
using SmartGym.BLL;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class BookClass : System.Web.UI.Page
    {
        private readonly AccountDAL _accountDal = new AccountDAL();
        private readonly ClassDAL _classDal = new ClassDAL();
        private readonly BookingService _bookingService = new BookingService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MemberId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindDropDowns();
                BindClassesGrid();
            }
        }

        private void BindDropDowns()
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

            var classes = _classDal.GetAllClasses()
                .Select(c => new
                {
                    c.ClassId,
                    DisplayText = $"{c.ClassName} - {c.Schedule:ddd dd MMM HH:mm} - {c.CreditCost:F2} cr"
                }).ToList();
            ddlClass.DataSource = classes;
            ddlClass.DataBind();
        }

        private void BindClassesGrid()
        {
            gvClasses.DataSource = _classDal.GetAllClasses();
            gvClasses.DataBind();
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAccount.SelectedValue == "" || ddlClass.SelectedValue == "")
                {
                    Show("Please select an account and a class.", true);
                    return;
                }

                int accountId = int.Parse(ddlAccount.SelectedValue);
                int classId = int.Parse(ddlClass.SelectedValue);

                string result = _bookingService.BookClass(accountId, classId);
                Show(result, false);

                BindDropDowns();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("BookClass: " + ex.Message);
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
