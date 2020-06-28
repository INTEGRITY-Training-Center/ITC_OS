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
        .addToCart {
            border: 1px solid #011f34;
            text-decoration: none;
            padding: 2px 6px 2px 6px;
            border-radius: 5px;
            background-color: #011f34;
            color: white;
        }
        .addToCart:hover{
            text-decoration:none;
            color:white;
        }
        /* ============== Search Textbox =============*/
        .search-div{
            width:100%;
            display:flex;
            justify-content:space-around;
        }
        .search-div span {
            position: absolute;
            z-index: 2;
            display: block;
            width: 35px;
            margin-top: 10px;
            text-align: center;
            pointer-events: none;
            color: #aaa;
        }
        .search-div .form-control {
            padding-left: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <div class="form-group search-div">
            <div class="col-md-6">
                <span class="fa fa-search"></span>
                <%--<input type="text" class="form-control" placeholder="Search with Item Name..." id="txt_Search">--%>
                <asp:TextBox ID="txtSearching" CssClass="form-control" placeholder="Search with Item Name..." runat="server" OnTextChanged="txtSearching_TextChanged"></asp:TextBox>
            </div>            
        </div>

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
                                <span id="pname<%# Eval("ProductID") %>"><%# Eval("ProductName") %></span>
                                <br />
                                <span style="display: block; margin-bottom: 5px;" id="pprice<%# Eval("ProductID") %>"><b>Price: <%# Eval("ProductPrice") %></b></span>

                                <%--/AddToCart.aspx?productID=<%# Eval("ProductID") %>--%>
                                <a href="#" style="text-decoration: none; padding: 5px; border-radius: 5px;" class="addToCart" id="<%# Eval("ProductID") %>">
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

            //var availableTags = ["States", "Australia", "Indies"]
            //$("#txtSearching").autocomplete({
            //    source: availableTags
            //});

            //$("#txtSearching").autocomplete({
            //    source: function (request, response) {
            //        var param = { ProductName: $('#txtSearching').val() };
            //        $.ajax({
            //            type: "POST",
            //            url: "Index.aspx/GetAllProductsByName",
            //            data: JSON.stringify(param),
            //            dataType: "json",
            //            contentType: "application/json; charset=utf-8",
            //            dataFilter: function (data) { return data; },
            //            success: function (data) {
            //                console.log(JSON.stringify(data));
            //                response($.map(data.d, function (item) {
            //                    return {
            //                        value: item.ProductName + " (" + item.ProductPrice + ")"
            //                    }
            //                }))
            //            },
            //            error: function (XMLHttpRequest, textStatus, errorThrown) {
            //                var err = eval("(" + XMLHttpRequest.responseText + ")");
            //                alert(err.Message)
            //                // console.log("Ajax Error!");    
            //            }
            //        });
            //    },
            //    minLength: 1 //This is the Char length of inputTextBox    
            //}); 

            //$('#txtSearching').on("keypress", function (e) {
            //    if (e.keyCode == 13) {
            //        alert($(this).val());
            //        return false; //Because of "System.Web.HttpException: Maximum request length exceeded." error
            //    }
            //});

            $('.addToCart').click(function () {
                var product_id = $(this).attr("id");
                var product_name = $("#pname" + product_id).text();
                var pprice = $("#pprice" + product_id).text().split(' ');
                var product_price = pprice[1];

                //alert(product_name + "::" + product_price);
                $.ajax({
                    type: "POST",
                    url: "Index.aspx/InsertCart",
                    data: JSON.stringify({ product_id: product_id, product_name: product_name, product_price: product_price }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "Fail") {
                            alert("WARNING :: Firstly, you need to login.")
                            window.location.href = "/Views/Login.aspx";
                            //console.dir(response.d);
                        }
                        else {
                            var lblCartQty = document.getElementById('<%=Master.FindControl("lblCartQty").ClientID%>');
                            if (lblCartQty.innerText.length > 0) {
                                var cartQty = parseInt(lblCartQty.innerText) + parseInt(1);
                                lblCartQty.innerText = cartQty;
                            }
                            else {
                                lblCartQty.innerText = "1";
                            }
                                                        
                            //alert("Successfully added to the card!.");
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
