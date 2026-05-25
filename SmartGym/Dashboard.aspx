<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SmartGym.Dashboard" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Dashboard</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Welcome, <asp:Literal ID="litName" runat="server" /></h1>

        <div class="tiles">
            <div class="tile">
                <h3>Total Accounts</h3>
                <div class="value"><asp:Literal ID="litAccountCount" runat="server" /></div>
            </div>
            <div class="tile" style="border-color:#43a047;">
                <h3>Total Credits</h3>
                <div class="value"><asp:Literal ID="litCredits" runat="server" /></div>
            </div>
            <div class="tile" style="border-color:#fb8c00;">
                <h3>Member Since</h3>
                <div class="value"><asp:Literal ID="litSince" runat="server" /></div>
            </div>
        </div>
    </div>

    <div class="card">
        <h2>My Membership Accounts</h2>
        <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="false" CssClass="grid">
            <Columns>
                <asp:BoundField DataField="AccountId"      HeaderText="ID" />
                <asp:BoundField DataField="AccountName"    HeaderText="Account Name" />
                <asp:BoundField DataField="MembershipType" HeaderText="Type" />
                <asp:BoundField DataField="CreditBalance"  HeaderText="Credits" DataFormatString="{0:F2}" />
                <asp:BoundField DataField="DateCreated"    HeaderText="Created" DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
            <EmptyDataTemplate>
                <p>You don't have any accounts yet.</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div class="card">
        <h2>Add an Additional Account</h2>
        <div class="form-row">
            <label>Account Name</label>
            <asp:TextBox ID="txtAccountName" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="txtAccountName"
                ErrorMessage="Required" CssClass="error" Display="Dynamic"
                ValidationGroup="addAcc" runat="server" />
        </div>
        <div class="form-row">
            <label>Membership Type</label>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem>Standard</asp:ListItem>
                <asp:ListItem>Premium</asp:ListItem>
                <asp:ListItem>Family</asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnAddAccount" runat="server" Text="Create Account"
            CssClass="btn accent" ValidationGroup="addAcc" OnClick="btnAddAccount_Click" />
        <asp:Label ID="lblMessage" runat="server" Visible="false" />
    </div>
</asp:Content>
