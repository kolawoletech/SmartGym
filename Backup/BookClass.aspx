<%@ Page Title="Book Class" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookClass.aspx.cs" Inherits="SmartGym.BookClass" %>

<asp:Content ID="cTitle" ContentPlaceHolderID="TitlePlaceHolder" runat="server">Book Class</asp:Content>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Book a Fitness Class</h1>

        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <div class="form-row">
            <label>Select an Account (used to deduct credits)</label>
            <asp:DropDownList ID="ddlAccount" runat="server" DataTextField="DisplayText" DataValueField="AccountId" />
        </div>

        <div class="form-row">
            <label>Select a Class</label>
            <asp:DropDownList ID="ddlClass" runat="server" DataTextField="DisplayText" DataValueField="ClassId" />
        </div>

        <asp:Button ID="btnBook" runat="server" Text="Confirm Booking" CssClass="btn accent" OnClick="btnBook_Click" />
    </div>

    <div class="card">
        <h2>Available Classes</h2>
        <asp:GridView ID="gvClasses" runat="server" AutoGenerateColumns="false" CssClass="grid">
            <Columns>
                <asp:BoundField DataField="ClassId"    HeaderText="ID" />
                <asp:BoundField DataField="ClassName"  HeaderText="Class" />
                <asp:BoundField DataField="Instructor" HeaderText="Instructor" />
                <asp:BoundField DataField="Schedule"   HeaderText="Schedule" DataFormatString="{0:ddd dd MMM HH:mm}" />
                <asp:BoundField DataField="Capacity"   HeaderText="Capacity" />
                <asp:BoundField DataField="CreditCost" HeaderText="Cost" DataFormatString="{0:F2}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
