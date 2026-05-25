<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SmartGym.ForgotPassword" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Forgot Password</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width:520px;margin:auto;">
        <h1>Password Recovery</h1>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <asp:MultiView ID="mvSteps" runat="server" ActiveViewIndex="0">

            <asp:View ID="vEmail" runat="server">
                <p>Enter your email to retrieve your security question.</p>
                <div class="form-row">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
                </div>
                <asp:Button ID="btnFindQuestion" runat="server" Text="Continue" CssClass="btn" OnClick="btnFindQuestion_Click" />
            </asp:View>

            <asp:View ID="vReset" runat="server">
                <div class="form-row">
                    <label>Security Question</label>
                    <asp:Label ID="lblQuestion" runat="server" CssClass="alert-info" />
                </div>
                <div class="form-row">
                    <label>Your Answer</label>
                    <asp:TextBox ID="txtAnswer" runat="server" />
                </div>
                <div class="form-row">
                    <label>New Password</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />
                </div>
                <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn accent" OnClick="btnReset_Click" />
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
