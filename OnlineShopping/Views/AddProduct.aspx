<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="OnlineShopping.Views.AddProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #f-div {
            margin-top: 20px;
        }
        #m-div {
            display: flex;
            justify-content: center;
        }
        #s-div {
            width: 35%;
        }
        #t-div {
            width: 20%;
            margin-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <h5 style="font-size:25px; text-align: center;margin-bottom:20px;">Adding Product</h5>

        <div id="m-div">
            <div id="s-div">
                <asp:Label ID="Label1" runat="server" Text="Label">Product Name</asp:Label>
                <asp:TextBox ID="txtProductName" runat="server" AutoComplete="off" Required CssClass="form-control mb-2 " placeholder="@ Product Name" MaxLength="100"></asp:TextBox>

                <asp:Label ID="Label2" runat="server" Text="Label">Product Category</asp:Label>
                <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-control mb-2">
                    <asp:ListItem>DRESS</asp:ListItem>
                    <asp:ListItem>SKIRTS</asp:ListItem>
                    <asp:ListItem>SHORTS</asp:ListItem>
                    <asp:ListItem>PANTS</asp:ListItem>
                    <asp:ListItem>TOPS</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="Label4" runat="server" Text="Label">Product Price</asp:Label>
                <asp:TextBox ID="txtPrice" runat="server" AutoComplete="off" Required CssClass="form-control mb-2" placeholder="@ Product Price" TextMode="Number"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text="Label">Product Description</asp:Label>
                <asp:TextBox ID="txtDescription" runat="server" AutoComplete="off" CssClass="form-control mb-4 " TextMode="MultiLine" placeholder="@ Product Description" MaxLength="500"></asp:TextBox>
                
            </div>

            <div id="t-div">
                <asp:FileUpload ID="fluPicture" runat="server" onchange="this.form.submit();" CssClass="mb-2"/>
                <asp:Image ID="imgPicture" Width="200" Height="220" runat="server" />
                <asp:Label ID="lblImagePath" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>

        <section style="display:flex; justify-content:center;">
            <div style="text-align: center; width: 55%;display:flex; justify-content:center;">
                <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-dark w-25" OnClick="btnAddProduct_Click" />
            </div>
        </section>
    </div>
</asp:Content>
