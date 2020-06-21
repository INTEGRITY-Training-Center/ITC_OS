<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OnlineShopping.Views.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #f-div {
            display: flex;
            justify-content: space-around;
            margin-top: 150px;
        }
        #s-div {
            width: 35%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <div id="s-div">
            <h5 style="font-size:40px; text-align: center;margin-bottom: 20px;">Login </h5>

            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <asp:TextBox ID="txtCusName" runat="server" AutoComplete="off" Required CssClass="br form-control mb-2" placeholder="@ Name" MaxLength="50"></asp:TextBox>

            <asp:TextBox ID="txtPassword" runat="server" Required CssClass="form-control mb-2" placeholder="@ Password" TextMode="Password" ></asp:TextBox>

            <div style="text-align:center;">
                 <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-25 mb-4" OnClick="btnLogin_Click" />
            </div>
            <div style="text-align:center">
                If you have no account, click <a href="AddCustomer.aspx" style="text-decoration: none">Sign Up</a>.
            </div>
            
        </div>
    </div>

</asp:Content>
