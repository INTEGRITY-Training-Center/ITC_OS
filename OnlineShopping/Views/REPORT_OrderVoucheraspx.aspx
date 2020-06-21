<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="REPORT_OrderVoucheraspx.aspx.cs" Inherits="OnlineShopping.Views.REPORT_OrderVoucheraspx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <rsweb:ReportViewer ID="rvOrderVoucher" Width="100%" runat="server" BackColor="Red"></rsweb:ReportViewer>
    </div>
    
</asp:Content>
