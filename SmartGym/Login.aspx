<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SmartGym.Login" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Login</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width:500px;margin:auto;">
        <h1>Member Login</h1>

        <asp:Label ID="lblFlash"   runat="server" CssClass="alert-success" Visible="false" />
        <asp:Label ID="lblMessage" runat="server" CssClass="alert-error"   Visible="false" />

        <div class="form-row">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
            <asp:RequiredFieldValidator ControlToValidate="txtEmail" ErrorMessage="Email is required."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <div class="form-row">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password is required."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
        <a class="btn accent" href="Register.aspx">Register</a>
        <a class="btn" style="background:#6b7280;" href="ForgotPassword.aspx">Forgot Password?</a>
    </div>
</asp:Content>
