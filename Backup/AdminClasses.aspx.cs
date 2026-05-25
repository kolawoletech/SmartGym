using System;
using System.Web.UI.WebControls;
using SmartGym.DAL;
using SmartGym.Models;
using SmartGym.Utilities;

namespace SmartGym
{
    public partial class AdminClasses : System.Web.UI.Page
    {
        private readonly ClassDAL _classDal = new ClassDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Authorisation: must be logged in AND have Admin role
            if (Session["MemberId"] == null ||
                !string.Equals(Session["Role"] as string, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack) BindGrid();
        }

        private void BindGrid()
        {
            gvClasses.DataSource = _classDal.GetAllClasses();
            gvClasses.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var fc = new FitnessClass
                {
                    ClassName = txtName.Text.Trim(),
                    Instructor = txtInstructor.Text.Trim(),
                    Schedule = DateTime.Parse(txtSchedule.Text.Trim()),
                    Capacity = int.Parse(txtCapacity.Text),
                    CreditCost = decimal.Parse(txtCost.Text),
                    Description = txtDescription.Text.Trim()
                };
                _classDal.AddClass(fc);
                Show("Class added.", false);
                ClearForm();
                BindGrid();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AdminAdd: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        protected void gvClasses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvClasses.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvClasses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvClasses.EditIndex = -1;
            BindGrid();
        }

        protected void gvClasses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int classId = (int)gvClasses.DataKeys[e.RowIndex].Value;
                GridViewRow row = gvClasses.Rows[e.RowIndex];

                var fc = new FitnessClass
                {
                    ClassId = classId,
                    ClassName = ((TextBox)row.Cells[1].Controls[0]).Text,
                    Instructor = ((TextBox)row.Cells[2].Controls[0]).Text,
                    Schedule = DateTime.Parse(((TextBox)row.Cells[3].Controls[0]).Text),
                    Capacity = int.Parse(((TextBox)row.Cells[4].Controls[0]).Text),
                    CreditCost = decimal.Parse(((TextBox)row.Cells[5].Controls[0]).Text),
                    Description = _classDal.GetClassById(classId)?.Description
                };

                _classDal.UpdateClass(fc);
                gvClasses.EditIndex = -1;
                BindGrid();
                Show("Class updated.", false);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AdminUpdate: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        protected void gvClasses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int classId = (int)gvClasses.DataKeys[e.RowIndex].Value;
                _classDal.DeleteClass(classId);
                BindGrid();
                Show("Class deleted.", false);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AdminDelete: " + ex.Message);
                Show("Cannot delete: class may have existing bookings.", true);
            }
        }

        protected void btnExportXml_Click(object sender, EventArgs e)
        {
            try
            {
                string path = XmlReporter.ExportClassScheduleToXml();
                Show("Schedule exported to: " + path, false);
            }
            catch (Exception ex)
            {
                FileLogger.LogError("AdminExport: " + ex.Message);
                Show(ex.Message, true);
            }
        }

        private void ClearForm()
        {
            txtName.Text = txtInstructor.Text = txtSchedule.Text =
                txtCapacity.Text = txtCost.Text = txtDescription.Text = "";
        }

        private void Show(string msg, bool isError)
        {
            lblMessage.Text = msg;
            lblMessage.CssClass = isError ? "alert-error" : "alert-success";
            lblMessage.Visible = true;
        }
    }
}
