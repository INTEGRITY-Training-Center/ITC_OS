<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="OrderDetailInfo.aspx.cs" Inherits="OnlineShopping.Views.OrderDetailInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
        /* =============== Header Table =============== */
        .f-td{
            width:13%;
        }
        .s-td{
            width:2%;
        }
        .t-td{
            width:35%;
        }
        /* =============== Detail List section =============== */
        .item {
            width: 40%;
        }

        .qty {
            width: 15%;
            text-align: center;
        }

        .price {
            width: 15%;
            text-align: right;
        }
        .discount {
            width: 15%;
            text-align: right;
        }
        .total {
            width: 15%;
            text-align: right;
        }
        hr {
            margin-top: 10px;
            margin-bottom: 10px;
        }
        .qty a{
            text-decoration:none;
        }
        .qty label{
            margin-left:10px;
            margin-right:10px;
        }
        /* =============== Total section =============== */ 
        #t-div {
            margin-top: 20px;
            margin-bottom: 20px;
            width: 35%;
            float: right;
            font-size: 14px;
            color: dimgray;
            font-weight: bold;
        }
        .t-section {
            display: flex;
            justify-content: space-between;
        }
        .t-section div:last-child {
            text-align: right;
        }
        .alink{
            border:1px solid silver;
            text-decoration:none;
            padding:3px 6px 3px 6px;
            border-radius:50%;
            background-color:silver;
            color:dimgray;
        }
        .alink:hover{
            text-decoration:none;
        }        
        .ft{
            width:40%;
        }
        .st{
            width:3%;
        }
        .tt{
            width:57%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Order Detail Info</h5>
        <table style="width: 100%;margin-bottom:15px;font-size: 14px;">
            <tr>
                <td class="f-td"><asp:Label ID="Label10" runat="server" Text="Order No."></asp:Label></td>
                <td class="s-td">:</td>
                <td class="t-td"><asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label></td>
                <td class="f-td"><asp:Label ID="Label8" runat="server" Text="Customer Address"></asp:Label></td>
                <td class="s-td">:</td>
                <td class="t-td" rowspan="2" style="vertical-align:top;"><asp:Label ID="lblCustomerAddress" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="Label11" runat="server" Text="Order Date"></asp:Label></td>
                <td>:</td>
                <td><asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="Label4" runat="server" Text="Order Quantity"></asp:Label></td>
                <td>:</td>
                <td><asp:Label ID="lblOrderQuantity" runat="server" Text=""></asp:Label></td>
                <td><asp:Label ID="Label9" runat="server" Text="Addition Request"></asp:Label></td>
                <td>:</td>
                <td rowspan="3" style="vertical-align: top;"><asp:Label ID="lblAdditionalRequest" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="Label7" runat="server" Text="Customer Name"></asp:Label></td>
                <td>:</td>
                <td><asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label></td>                
            </tr>
             <tr>
                <td><asp:Label ID="Label3" runat="server" Text="Customer Mobile"></asp:Label></td>
                <td>:</td>
                <td><asp:Label ID="lblCustomerMobile" runat="server" Text=""></asp:Label></td>                
            </tr>
        </table>
        
        <div style="display:none;">
            <p id="order_id" runat="server"></p>
        </div>
        <div>
            <asp:ListView ID="productList" runat="server" DataKeyNames="ProductID">
                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <th class="item">Item</th>
                            <th class="qty">Quantity</th>
                            <th class="price">Price</th>
                            <th class="discount">Discount</th>
                            <th class="total">Total</th>
                        </tr>
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>                   
                    <tr style="vertical-align: middle;">
                        <td class="item">
                            <section style="display: flex;">
                                <div class="img-div" style="width: 20%;">
                                    <a href="#">
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# "data:image/jpg;base64," + Convert.ToBase64String((byte[])Eval("ProductImage")) %>' Width="80%" />
                                    </a>
                                </div>
                                <div style="width: 80%; margin: auto;">
                                    <span><%# Eval("ProductName") %></span>
                                </div>
                            </section>
                        </td>
                        <td class="qty">
                            <label><%# Eval("Quantity") %></label>
                        </td>
                        <td class="price">
                            <label><%# Eval("ProductPrice") %></label>     
                        </td>
                         <td class="discount">
                            <label><%# Eval("DiscountAmount") %></label>     
                        </td>
                        <td class="total">
                            <label style="margin-right:15px;"><%# Eval("TotalPrice") %></label>                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <hr />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div id="t-div">
            <section class="t-section">
                <div class="ft">
                     <asp:Label ID="Label1" runat="server" Text="Sub Total"></asp:Label>
                 </div>
                <div class="st">
                    :
                </div>
                <div class="tt">
                    <asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label>
                </div>
            </section>
            <hr />
            <section class="t-section">
                <div class="ft">
                     <asp:Label ID="LabelTax" runat="server" Text="Tax"></asp:Label>
                 </div>
                <div class="st">
                    :
                </div>
                <div class="tt">
                    <asp:Label ID="lblTax" runat="server" Text="0.00"></asp:Label>
                </div>
            </section>
            <hr />
             <section class="t-section">
                <div class="ft">
                    <asp:Label ID="Label2" runat="server" Text="Label">Delivery Charges</asp:Label>
                </div>
                <div class="st">
                    :
                </div>
                <div class="tt">
                    <asp:Label ID="lblDeliveryCharges" runat="server" Text="0.00"></asp:Label>
                </div>
            </section>
            <hr />      
            <section class="t-section">
                <div class="ft">
                    <asp:Label ID="Label5" runat="server" Text="Grand Total"></asp:Label>
                 </div>
                <div class="st">
                    :
                </div>
                <div class="tt">
                    <asp:Label ID="lblGrandTotal" runat="server" Text="0.00"></asp:Label>
                </div>
            </section>
            <hr />
            <div id="deliverydiv" runat="server">
                <section class="t-section">
                    <div class="ft">
                        <asp:Label ID="Label6" runat="server" Text="Label">Est. Delivery Date</asp:Label>
                    </div>
                    <div class="st">
                        :
                    </div>
                    <div class="tt">
                        <input type="date" id="deliveryDate" class="form-control" required runat="server">
                    </div>
                </section>
                <hr />
            </div>
            <div id="lbldeliverydiv" runat="server">
                <section class="t-section">
                    <div class="ft">
                        <asp:Label ID="Label12" runat="server" Text="Label">Est. Delivery Date</asp:Label>
                    </div>
                    <div class="st">
                        :
                    </div>
                    <div class="tt">
                        <asp:Label ID="lblDeliveryDate" runat="server" Text=""></asp:Label>
                    </div>
                </section>
                <hr />
            </div>
            <div style="text-align: right;">
                <asp:Button ID="btnCheckOrder" OnClientClick="javascript:return confirm('Do you really want to do Order Confirm?');"  runat="server" Text="Order" CssClass="btn-dark" Style="font-size: 13px; width: 40%; padding-top: 5px; padding-bottom: 5px;" OnClick="btnCheckOrder_Click"/>
            </div>
        </div>
    </div>
    
</asp:Content>
