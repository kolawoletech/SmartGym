<%@ Page Title="Admin Classes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminClasses.aspx.cs" Inherits="SmartGym.AdminClasses" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Admin Classes</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Manage Fitness Classes</h1>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <asp:Button ID="btnExportXml" runat="server" Text="Export Schedule to XML"
            CssClass="btn" OnClick="btnExportXml_Click" />

        <asp:GridView ID="gvClasses" runat="server" AutoGenerateColumns="false" CssClass="grid"
            DataKeyNames="ClassId"
            OnRowEditing="gvClasses_RowEditing"
            OnRowUpdating="gvClasses_RowUpdating"
            OnRowCancelingEdit="gvClasses_RowCancelingEdit"
            OnRowDeleting="gvClasses_RowDeleting">
            <Columns>
                <asp:BoundField DataField="ClassId"    HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="ClassName"  HeaderText="Class Name" />
                <asp:BoundField DataField="Instructor" HeaderText="Instructor" />
                <asp:BoundField DataField="Schedule"   HeaderText="Schedule" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="Capacity"   HeaderText="Capacity" />
                <asp:BoundField DataField="CreditCost" HeaderText="Cost" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </div>

    <div class="card">
        <h2>Add New Class</h2>
        <div class="form-row"><label>Class Name</label>
            <asp:TextBox ID="txtName" runat="server" /></div>
        <div class="form-row"><label>Instructor</label>
            <asp:TextBox ID="txtInstructor" runat="server" /></div>
        <div class="form-row"><label>Schedule (yyyy-MM-dd HH:mm)</label>
            <asp:TextBox ID="txtSchedule" runat="server" /></div>
        <div class="form-row"><label>Capacity</label>
            <asp:TextBox ID="txtCapacity" runat="server" TextMode="Number" /></div>
        <div class="form-row"><label>Credit Cost</label>
            <asp:TextBox ID="txtCost" runat="server" TextMode="Number" /></div>
        <div class="form-row"><label>Description</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" /></div>
        <asp:Button ID="btnAdd" runat="server" Text="Add Class" CssClass="btn accent" OnClick="btnAdd_Click" />
    </div>
</asp:Content>
