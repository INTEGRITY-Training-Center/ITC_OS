<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="DeliItemDetailInfo.aspx.cs" Inherits="OnlineShopping.Views.DeliItemDetailInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #f-div {
            display: flex;
            justify-content: space-around;
            margin-top: 20px;
        }
        #s-div {
            width: 60%;
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
            <h5 style="font-size:25px; text-align: center;margin-bottom:10px;">Delivery Item Detail</h5>
            <div style="display: none;">
                <p id="order_id" runat="server"></p>
            </div>
            <section class="t-section">
                <section style="width: 50%; margin-right: 7px;">
                    <asp:Label ID="Label1" runat="server" Text="Label">Order No.</asp:Label>
                    <asp:TextBox ID="txtOrderNo" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
                <section style="width: 50%; margin-left: 7px;">
                    <asp:Label ID="Label8" runat="server" Text="Label">Order Quantity</asp:Label>
                    <asp:TextBox ID="txtOrderQuantity" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
            </section>
            <section class="t-section">
                <section style="width: 50%; margin-right: 7px;">
                    <asp:Label ID="Label9" runat="server" Text="Label">Order Amount</asp:Label>
                    <asp:TextBox ID="txtOrderAmount" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
                <section style="width: 50%; margin-left: 7px;">
                    <asp:Label ID="Label10" runat="server" Text="Label">Delivery Charges</asp:Label>
                    <asp:TextBox ID="txtDeliveryCharges" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
            </section>
            <section class="t-section">
                <section style="width: 50%; margin-right: 7px;">
                    <asp:Label ID="Label11" runat="server" Text="Label">Customer Name</asp:Label>
                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
                <section style="width: 50%; margin-left: 7px;">
                    <asp:Label ID="Label3" runat="server" Text="Label">Customer Mobile</asp:Label>
                    <asp:TextBox ID="txtCustomerMobile" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
                </section>
            </section>
            <section class="t-section">
                <section style="width: 50%; margin-right: 7px;">
                    <asp:Label ID="Label4" runat="server" Text="Label">Customer Address</asp:Label>
                    <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control mb-2 " TextMode="MultiLine"></asp:TextBox>
                </section>
                <section style="width: 50%; margin-left: 7px;">
                    <asp:Label ID="Label2" runat="server" Text="Label">Additional Order Request</asp:Label>
                    <asp:TextBox ID="txtOrderDescription" runat="server" CssClass="form-control mb-2 " TextMode="MultiLine"></asp:TextBox>
                </section>
            </section>
            <asp:Label ID="Label7" runat="server" Text="Label">Delivery Man</asp:Label>
            <asp:DropDownList ID="ddlDeliMan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDeliMan_SelectedIndexChanged" CssClass="form-control mb-2"></asp:DropDownList>

            <asp:Label ID="Label12" runat="server" Text="Label">Delivery Man Mobile</asp:Label>
            <asp:TextBox ID="txtDeliManMobile" runat="server" CssClass="form-control mb-2 " ReadOnly="true"></asp:TextBox>
             
            <div style="text-align:center;">
                <asp:Button ID="btnAddDeliMan" runat="server" Text="Add Deli Man" CssClass="btn btn-dark w-50 mb-4" OnClick="btnAddDeliMan_Click"/>
            </div>
        </div>
    </div>
</asp:Content>
