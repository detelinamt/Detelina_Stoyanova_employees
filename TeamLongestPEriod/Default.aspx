<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeamLongestPEriod._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="title">
        Pair of employees who have worked together
    </div>
    <div>
        Choose .csv file:
    </div>
    <div class="row">
        <div style="display:inline-block;">
            <asp:FileUpload ID="fuFilePicker" ClientIDMode="Static"  runat="server" />
        </div>
        <div style="display:inline-block;margin-left: 10px;">
            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Load" />
        </div>
    </div>
    <div class="errorMessage">
        <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
    </div>
    <div>
        <asp:DataGrid ID="dgTeamWorkingDays" runat="server" CssClass="dataGrid">
        </asp:DataGrid>
    </div>
</asp:Content>
