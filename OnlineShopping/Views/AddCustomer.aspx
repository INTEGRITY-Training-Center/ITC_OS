<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="AddCustomer.aspx.cs" Inherits="OnlineShopping.Views.AddCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #f-div {
            display: flex;
            justify-content: space-around;
            margin-top: 20px;
        }
        #s-div {
            width: 35%;
        }
        .t-section {
            display: flex;
            justify-content: space-between;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <div id="s-div">
            <h5 style="font-size:25px; text-align: center;margin-bottom:10px;">Sign Up</h5>

            <asp:Label ID="Label1" runat="server" Text="Label">Customer Name</asp:Label>
            <asp:TextBox ID="txtCustomerName" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Customer Name" MaxLength="100"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Label">Email</asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Email" MaxLength="50" TextMode="Email"></asp:TextBox>

            <asp:Label ID="Label3" runat="server" Text="Label">Mobile Phone</asp:Label>
            <asp:TextBox ID="txtMobilePhone" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Mobile Phone" MaxLength="20"></asp:TextBox>

            <section class="t-section" >
                <section style="width:50%;margin-right:7px;">
                    <asp:Label ID="Label5" runat="server" Text="Label">Password</asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Passwrod" TextMode="Password"></asp:TextBox>
                </section>
                <section style="width:50%;margin-left:7px;">
                    <asp:Label ID="Label6" runat="server" Text="Label">Confirm Password</asp:Label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Confirm Passwrod" TextMode="Password"></asp:TextBox>
                </section>
            </section>

            <asp:Label ID="Label4" runat="server" Text="Label">Customer Address</asp:Label>
            <asp:TextBox ID="txtAddress" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Customer Address" MaxLength="500" TextMode="MultiLine"></asp:TextBox>

            <asp:Label ID="Label7" runat="server" Text="Label">Township</asp:Label>
            <asp:DropDownList ID="ddlTownship" runat="server" Required  CssClass="form-control mb-4">
                <asp:ListItem Text="-- Select Township --" Value="0" />
            </asp:DropDownList>

            <div style="text-align:center;">
                <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" CssClass="btn btn-dark w-50 mb-4" OnClick="btnSignUp_Click"/>
            </div>
            <div style="text-align:center">
                If account already exist, go to <a href="Login.aspx" style="text-decoration: none">Login</a> page.
            </div>
        </div>
    </div>
</asp:Content>
