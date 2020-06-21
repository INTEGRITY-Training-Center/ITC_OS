<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="OnlineShopping.Views.CustomerList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Customer List</h5>
        <div align="center">
            <asp:GridView ID="gvCustomerList" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="Silver" Font-Size="12px">
                <HeaderStyle HorizontalAlign="center" BackColor="#011f34" ForeColor="white"/>
                <Columns>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="180"/>
                    <asp:BoundField DataField="CustomerEmail" HeaderText="Customer Email" ItemStyle-Width="200" />
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Customer Mobile" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" ItemStyle-Width="400" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
