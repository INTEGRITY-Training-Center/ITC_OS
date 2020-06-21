<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="WishlistInfo.aspx.cs" Inherits="OnlineShopping.Views.WishlistInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
        .item {
            width: 45%;
        }
        .date {
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
        .wlink {
            text-decoration: none;
            padding: 5px 23px 5px 23px;
            border-radius: 5px;
            margin-right:15px;
        }
        .wlink:hover{
            text-decoration:none;
            color:white;
        }
        .alink img{
            width:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Your Wishlist</h5>
        
        <div style="display:none;">
            <p id="customer_id" runat="server"></p>
        </div>
        <div>
            <asp:ListView ID="productList" runat="server" DataKeyNames="ProductID" OnItemCommand="productList_ItemCommand" OnItemDeleting="productList_ItemDeleting" OnItemInserted="productList_ItemInserted">
                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <th class="item">Item</th>
                            <th class="date">Added Date</th>
                            <th class="price">Price</th>
                            <th class="total"></th>
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
                        <td class="date">
                            <label id="qty<%# Eval("WishlistID") %>"><%# Eval("AddedDate") %></label>
                        </td>
                        <td class="price">
                            <label id="price<%# Eval("WishlistID") %>"><%# Eval("ProductPrice") %></label>     
                        </td>
                        <td class="total">
                            <asp:LinkButton runat="server" CommandName="InsertData" CommandArgument='<%# Eval("WishlistID") %>'
                                OnClientClick="javascript:return confirm('Are you sure you want to add to cart?');" CssClass="btn-dark wlink" ToolTip="Add to Cart">Add to Cart</asp:LinkButton>        
                            
                            <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("WishlistID") %>'
                                OnClientClick="javascript:return confirm('Are you sure you want to remove?');" CssClass="alink" ToolTip="Remove Item"><img src="../Images/delete.png" /></asp:LinkButton>   
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
                    <p style="text-align: center; font-size: 15px;font-weight:bold;color:dimgray;">There is no Wishlist Info.</p>
                    <hr />
                </EmptyDataTemplate>
            </asp:ListView>
        </div>        
    </div>
    
</asp:Content>
