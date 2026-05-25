<%@ Page Title="Top Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TopUp.aspx.cs" Inherits="SmartGym.TopUp" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Top Up</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width:600px;margin:auto;">
        <h1>Top Up Training Credits</h1>

        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <div class="form-row">
            <label>Account</label>
            <asp:DropDownList ID="ddlAccount" runat="server" DataTextField="DisplayText" DataValueField="AccountId" />
        </div>

        <div class="form-row">
            <label>Amount (credits)</label>
            <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" />
            <asp:RequiredFieldValidator ControlToValidate="txtAmount" ErrorMessage="Amount is required."
                CssClass="error" Display="Dynamic" runat="server" />
            <asp:RangeValidator ControlToValidate="txtAmount" Type="Double"
                MinimumValue="1" MaximumValue="100000"
                ErrorMessage="Amount must be between 1 and 100000."
                CssClass="error" Display="Dynamic" runat="server" />
        </div>

        <asp:Button ID="btnTopUp" runat="server" Text="Top Up" CssClass="btn accent" OnClick="btnTopUp_Click" />
    </div>
</asp:Content>
