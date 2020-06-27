<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="AddToCart.aspx.cs" Inherits="OnlineShopping.Views.AddToCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
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
        .total {
            width: 20%;
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
        .alink img{
            width:20px;
        }
        /*.alink{
            border:1px solid silver;
            text-decoration:none;
            padding:3px 6px 3px 6px;
            border-radius:50%;
            background-color:silver;
            color:dimgray;
        }
        .alink:hover{
            text-decoration:none;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Your Cart</h5>
        
        <div style="display:none;">
            <p id="customer_id" runat="server"></p>
        </div>
        <div>
            <asp:ListView ID="productList" runat="server" DataKeyNames="ProductID" OnItemCommand="productList_ItemCommand" OnItemDeleting="productList_ItemDeleting">
                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <th class="item">Item</th>
                            <th class="qty">Quantity</th>
                            <th class="price">Price</th>
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
                            <a href="#" class="<%# Eval("CartID") %>" id="add">
                                <img src="../Images/add.png" style="width: 20px; border: 0.5px solid dimgray;" />
                            </a>
                            <label id="qty<%# Eval("CartID") %>"><%# Eval("Quantity") %></label>
                            <a href="#" class="<%# Eval("CartID") %>" id="sub">
                                <img src="../Images/sub.png" style="width: 20px; border: 0.5px solid dimgray;" />
                            </a>
                        </td>
                        <td class="price">
                            <label id="price<%# Eval("CartID") %>"><%# Eval("ProductPrice") %></label>     
                            <%--<asp:Label ID="lbl_ProductID" runat="server" Text='<%# Eval("ProductID") %>' Visible="false"></asp:Label>--%>
                        </td>
                        <td class="total">
                            <label id="total<%# Eval("CartID") %>" style="margin-right:15px;"><%# Eval("TotalPrice") %></label>
                            <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("CartID") %>'
                                OnClientClick="javascript:return confirm('Are you sure you want to remove?');" CssClass="alink" ToolTip="Remove Item">
                                <img src="../Images/delete.png" />
                            </asp:LinkButton>                           
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <hr />
                    <p style="text-align: center; font-size: 15px;font-weight:bold;color:dimgray;">There is no Cart Info.</p>
                    <hr />
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <div id="t-div">
            <section class="t-section">
                <asp:Label ID="Label1" runat="server" Text="Sub Total :"></asp:Label>
                <asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label>
            </section>
            <hr />
            <section class="t-section">
                <asp:Label ID="Label3" runat="server" Text="Tax :"></asp:Label>
                <asp:Label ID="lblTax" runat="server" Text="0.00"></asp:Label>
            </section>
            <hr />
            <section class="t-section">
                <asp:Label ID="Label5" runat="server" Text="Grand Total :"></asp:Label>
                <asp:Label ID="lblGrandTotal" runat="server" Text="0.00"></asp:Label>
            </section>
            <hr />
            <div>
               <asp:TextBox ID="txtOrderDescription" runat="server" CssClass="form-control" TextMode="MultiLine" AutoComplete="off" placeholder="@ Additional Order Request" MaxLength="500"></asp:TextBox>
            </div>
            <hr />
            <div style="text-align: right;">
                <asp:Button ID="btnCreateOrder" OnClientClick="javascript:return confirm('Do you really want to create order?');"  runat="server" Text="Create Order" CssClass="btn-dark" Style="font-size: 13px; width: 40%; padding-top: 5px; padding-bottom: 5px;" OnClick="btnCreateOrder_Click"/>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".qty").on("click", "a", function () {
                var cart_id = $(this).attr("class");
                var customer_id = $('p').html();//can get with only tag name
                
                if (customer_id.length <= 0) {
                    window.location.href = "/Views/Login.aspx";
                }
                else {
                    var lblqty = "#qty" + cart_id;
                    var lbltotal = "#total" + cart_id;
                    var lblprice = "#price" + cart_id;

                    var currentQty = $(lblqty).text();
                    var price = parseFloat($(lblprice).text());
                    var subTotal = parseFloat($('#<%=lblSubTotal.ClientID%>').html());
                    var cartQty = parseInt(0);
                    var lblCartQty = document.getElementById('<%=Master.FindControl("lblCartQty").ClientID%>');
                    if (lblCartQty.innerText.length > 0) {
                        cartQty = parseInt(lblCartQty.innerText);
                    }

                    if (($(this).attr("id")) == "add") {
                        cartQty = cartQty + parseInt(1);
                        currentQty = parseInt(currentQty) + parseInt(1);
                        subTotal = subTotal + price;
                    }
                    else {
                        cartQty = cartQty - parseInt(1);
                        currentQty = parseInt(currentQty) - parseInt(1);
                        subTotal = subTotal - price;
                    }
                    
                    var total = parseInt(currentQty) * price;

                    $.ajax({
                        type: "POST",
                        url: "AddToCart.aspx/UpdateCart",
                        data: JSON.stringify({ cart_id: cart_id, currentQty: currentQty, customer_id: customer_id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == "Success") {
                                $(lblqty).text(currentQty);
                                $(lbltotal).text(total.toFixed(2));
                                $('#<%=lblSubTotal.ClientID%>').html(subTotal.toFixed(2));
                                $('#<%=lblGrandTotal.ClientID%>').html(subTotal.toFixed(2));
                                if (cartQty > 0) {
                                    lblCartQty.innerText = cartQty;                                    
                                }
                                else {
                                    lblCartQty.innerText = "";
                                }
                            }
                            else {
                                alert("WARNING :: Can't update data. Something wrong!")                               
                            }
                        },
                        failure: function (response) {
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
