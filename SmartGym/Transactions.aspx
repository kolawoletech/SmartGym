<%@ Page Title="Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="SmartGym.Transactions" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Transactions</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Booking & Transaction History</h1>

        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <asp:Button ID="btnExportXml" runat="server" Text="Export to XML"
            CssClass="btn" OnClick="btnExportXml_Click" />
        <asp:Button ID="btnShowLog" runat="server" Text="View File Log"
            CssClass="btn accent" OnClick="btnShowLog_Click" />

        <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="false" CssClass="grid">
            <Columns>
                <asp:BoundField DataField="TransactionId"   HeaderText="#" />
                <asp:BoundField DataField="AccountName"     HeaderText="Account" />
                <asp:BoundField DataField="ClassName"       HeaderText="Class" />
                <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                <asp:BoundField DataField="Amount"          HeaderText="Amount"  DataFormatString="{0:F2}" />
                <asp:BoundField DataField="BalanceAfter"    HeaderText="Balance" DataFormatString="{0:F2}" />
                <asp:BoundField DataField="TransactionDate" HeaderText="Date"    DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="Notes"           HeaderText="Notes" />
            </Columns>
            <EmptyDataTemplate>
                <p>No transactions yet.</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <asp:Panel ID="pnlLog" runat="server" CssClass="card" Visible="false">
        <h2>Raw File Log (booking_log.txt)</h2>
        <pre style="background:#1f2937;color:#e5e7eb;padding:1rem;border-radius:6px;overflow:auto;">
<asp:Literal ID="litLog" runat="server" /></pre>
    </asp:Panel>
</asp:Content>
