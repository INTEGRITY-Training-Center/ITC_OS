<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="TEST_API.aspx.cs" Inherits="OnlineShopping.Views.TEST_API" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        #f-div {
            display: flex;
            justify-content: space-around;
            margin-top: 20px;
            font-size:12px;
        }
        #s-div {
            width: 30%;
            padding:0px 20px 0px 20px;
        }
        #g-div{
            width: 70%;
        }
        .form-control {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="f-div">
        <div id="s-div">
            <h5 style="font-style:italic;text-align:center;margin-bottom: 10px;">Adding Expense </h5>
                        
            <asp:Label ID="Label5" runat="server" Text="Label">Expense Date</asp:Label>
            <input type="date" id="expenseDate" class="form-control mb-2" runat="server">

            <asp:Label ID="Label1" runat="server" Text="Label">Item Name</asp:Label>
            <asp:TextBox ID="txtItemName" runat="server" AutoComplete="off" CssClass="form-control mb-2 " placeholder="@ Item Name" MaxLength="100"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Text="Label">Expense Category</asp:Label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control mb-2">
                <asp:ListItem>Beauty</asp:ListItem>
                <asp:ListItem>Breakfast</asp:ListItem>
                <asp:ListItem>Cooking</asp:ListItem>
                <asp:ListItem>Dinner</asp:ListItem>
                <asp:ListItem>Donation</asp:ListItem>
                <asp:ListItem>Education</asp:ListItem>
                <asp:ListItem>Lunch</asp:ListItem>
                <asp:ListItem>Transporation</asp:ListItem>
                <asp:ListItem>Snack</asp:ListItem>
            </asp:DropDownList>

            <section style="display: flex;">
                <div style="width: 50%; margin-right: 5px;">
                    <asp:Label ID="Label3" runat="server" Text="Label">Quantity</asp:Label>
                    <asp:TextBox ID="txtQuantity" runat="server" AutoComplete="off" CssClass="form-control mb-2" placeholder="@ Quantity" TextMode="Number"></asp:TextBox>
                </div>

                <div style="width: 50%; margin-left: 5px;">
                    <asp:Label ID="Label4" runat="server" Text="Label">Price</asp:Label>
                    <asp:TextBox ID="txtPrice" runat="server" AutoComplete="off" CssClass="form-control mb-2" placeholder="@ Price" TextMode="Number"></asp:TextBox>
                </div>
            </section>

            <section style="display: flex;">
                <div style="width: 50%; margin-right: 5px;">
                    <asp:Label ID="Label7" runat="server" Text="Label">Payment Type</asp:Label>
                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control mb-2">
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Credit</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 50%; margin-left: 5px;">
                    <asp:Label ID="Label8" runat="server" Text="Label">Currency</asp:Label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control mb-2">
                        <asp:ListItem>MMK KS</asp:ListItem>
                        <asp:ListItem>USD $</asp:ListItem>
                        <asp:ListItem>SG $</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </section>

            <asp:Label ID="Label6" runat="server" Text="Label">Description</asp:Label>
            <asp:TextBox ID="txtDescription" runat="server" AutoComplete="off" CssClass="form-control mb-4 " TextMode="MultiLine" placeholder="@ Description" MaxLength="200"></asp:TextBox>

            <div style="text-align:center;">
                <asp:Button ID="btnAddExpense" runat="server" Text="Add Expense" Font-Size="12px" CssClass="btn btn-primary w-50" OnClick="btnAddExpense_Click"/>
            </div>
            <hr />
            <asp:Label ID="Label9" runat="server" Text="Label">Expense ID</asp:Label>
            <asp:TextBox ID="txtExpenseID" runat="server" AutoComplete="off" CssClass="form-control mb-2 " placeholder="@ Expense ID to DELETE"></asp:TextBox>

            <div style="text-align:center;">
                <asp:Button ID="btnDeleteExpense" runat="server" Text="Delete Expense" Font-Size="12px" CssClass="btn btn-primary w-50" OnClick="btnDeleteExpense_Click"/>
            </div>

             <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
        <div id="g-div">
            <asp:GridView ID="gvExpenses" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#5bcb68">
                <HeaderStyle HorizontalAlign="center" BackColor="#2e972e" ForeColor="white"/>
                <Columns>
                    <asp:BoundField DataField="ExpenseID" HeaderText="Expense ID"/>
                    <asp:BoundField DataField="Date" HeaderText="Expense Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="110" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="130" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="130" />

<%--                <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                    <asp:BoundField DataField="Currency" HeaderText="Currency" ItemStyle-Width="100" />--%>

                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    
</asp:Content>
