<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OnlineShopping.Views.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #f-div {
            margin: 20px 120px 0px 120px;
            font-size: 12px;
        }
        td {
            width: 25%;
            text-align: center;
        }
        .img-div {
            width: 100%;
            position: relative;
            overflow: hidden;
        }
        a {
            text-decoration: none;
        }
        .lblWelcome {
        }
        /* ============== Wishlist =============*/
        .wishlist-div {
            z-index: 2;
            position: absolute;
            top: 10px;
            right:35px;
        }
        #addWish i {
            color: red;
            font-size: 20px;
            font-weight:bold;
        }
        #addWish i:hover {
            color: springgreen;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <asp:ListView ID="productList" runat="server" DataKeyNames="ProductID" GroupItemCount="4">
            <LayoutTemplate>
                <table runat="server">
                    <tbody>
                        <tr runat="server">
                            <td runat="server">
                                <table id="groupPlaceholderContainer" runat="server">
                                    <tr id="groupPlaceholder" runat="server"></tr>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </LayoutTemplate>
            <GroupTemplate>
                <tr id="itemPlaceholderContainer" runat="server">
                    <td id="itemPlaceholder" runat="server"></td>
                </tr>
            </GroupTemplate>
            <ItemTemplate>
                <td runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="img-div">
                                    <a href="#"><%--ProductDetails.aspx?productID=<%# Eval("ProductID") %>--%>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# "data:image/jpg;base64," + Convert.ToBase64String((byte[])Eval("ProductImage")) %>' Width="80%" />
                                        <div class="wishlist-div">
                                            <a href="#" class="<%# Eval("ProductID") %>" id="addWish" data-toggle="tooltip" data-placement="top" title="Add to Wishlist">
                                                <i class="fa fa-heart-o"></i>
                                            </a>
                                        </div>
                                    </a>
                                </div>
                                <span><%# Eval("ProductName") %></span>
                                <br />
                                <span style="display: block; margin-bottom: 5px;"><b>Price: <%# Eval("ProductPrice") %></b></span>

                                <%--/AddToCart.aspx?productID=<%# Eval("ProductID") %>--%>
                                <a href="#" style="text-decoration: none; padding: 5px; border-radius: 5px;" class="btn-dark addToCart" id="<%# Eval("ProductID") %>">
                                    <b>Add To Cart<b>
                                </a>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.addToCart').click(function () {
                var product_id = $(this).attr("id");
                //var c = $.cookie("CustomerID");
                //alert(c);
                $.ajax({
                    type: "POST",
                    url: "Index.aspx/InsertCart",
                    data: JSON.stringify({ product_id: product_id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "Fail") {
                            alert("WARNING :: Firstly, you need to login.")
                            window.location.href = "/Views/Login.aspx";
                            //console.dir(response.d);
                        }
                        else {
                            alert("Successfully added to the card!.");
                        }
                    },
                    failure: function (response) {
                    }
                });
            });
            $('.wishlist-div').on('click', 'a', function () {
                var product_id = $(this).attr("class");
                $.ajax({
                    type: "POST",
                    url: "Index.aspx/InsertWishlist",
                    data: JSON.stringify({ product_id: product_id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "Fail") {
                            alert("WARNING :: Firstly, you need to login.")
                            window.location.href = "/Views/Login.aspx";
                            //console.dir(response.d);
                        }
                        else {
                            alert(response.d);
                        }
                    },
                    failure: function (response) {
                    }
                });
            });
        });
    </script>
</asp:Content>
