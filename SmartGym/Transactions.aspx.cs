using System;
using System.Data;
using SmartGym.DAL;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class Transactions : System.Web.UI.Page
    {
        private readonly TransactionDAL _txDal = new TransactionDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MemberId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack) LoadTransactions();
        }

        private void LoadTransactions()
        {
            try
            {
                int memberId = (int)Session["MemberId"];
                DataTable dt = _txDal.GetTransactionsForMember(memberId);
                gvTransactions.DataSource = dt;
                gvTransactions.DataBind();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Transactions load: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        protected void btnExportXml_Click(object sender, EventArgs e)
        {
            try
            {
                int memberId = (int)Session["MemberId"];
                string path = XmlReporter.ExportTransactionsToXml(memberId);
                Show("XML report saved: " + path, false);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Export XML: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        protected void btnShowLog_Click(object sender, EventArgs e)
        {
            try
            {
                litLog.Text = Server.HtmlEncode(FileLogger.ReadBookingLog());
                pnlLog.Visible = true;
            }
            catch (Exception ex)
            {
                FileLogger.LogError("Show log: " + ex.Message);
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
