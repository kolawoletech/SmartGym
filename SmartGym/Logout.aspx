<%@ Page Title="Logout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="SmartGym.LogoutPage" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Logout</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width:480px;margin:auto;text-align:center;">
        <h1>Goodbye!</h1>
        <p>You have been successfully logged out of SmartGym.</p>
        <a class="btn" href="Login.aspx">Log in again</a>
    </div>
</asp:Content>
