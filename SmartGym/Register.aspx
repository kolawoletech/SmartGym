<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SmartGym.Register" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Register</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Create Your SmartGym Account</h1>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error" Visible="false" />

        <div class="form-row">
            <label>Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="txtFullName" ErrorMessage="Full name is required."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <div class="form-row">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
            <asp:RequiredFieldValidator ControlToValidate="txtEmail" ErrorMessage="Email is required."
                CssClass="error" Display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator ControlToValidate="txtEmail"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                ErrorMessage="Invalid email format." CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <div class="form-row">
            <label>Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" />
        </div>

        <div class="form-row">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password is required."
                CssClass="error" Display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator ControlToValidate="txtPassword"
                ValidationExpression="^.{6,}$" ErrorMessage="Password must be 6+ characters."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <div class="form-row">
            <label>Confirm Password</label>
            <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" />
            <asp:CompareValidator ControlToValidate="txtConfirm" ControlToCompare="txtPassword"
                ErrorMessage="Passwords do not match." CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <div class="form-row">
            <label>Security Question</label>
            <asp:DropDownList ID="ddlQuestion" runat="server">
                <asp:ListItem Text="What city were you born in?" />
                <asp:ListItem Text="Name of your first pet?" />
                <asp:ListItem Text="Mother's maiden name?" />
                <asp:ListItem Text="Favourite teacher's name?" />
            </asp:DropDownList>
        </div>

        <div class="form-row">
            <label>Security Answer</label>
            <asp:TextBox ID="txtAnswer" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="txtAnswer" ErrorMessage="Security answer is required."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn accent" OnClick="btnRegister_Click" />
        <a class="btn" href="Login.aspx" style="background:#6b7280;">Back to Login</a>
    </div>
</asp:Content>
