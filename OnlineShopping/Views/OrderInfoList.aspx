<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="OrderInfoList.aspx.cs" Inherits="OnlineShopping.Views.OrderInfoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Order List</h5>
        <div>
            <asp:GridView ID="gvOrderInfo" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="Silver" Font-Size="12px">
                <HeaderStyle HorizontalAlign="center" BackColor="#011f34" ForeColor="white"/>
                <Columns>
                    <asp:BoundField DataField="OrderNo" HeaderText="Order No." ItemStyle-Width="90" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="110" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="140" />
                    <asp:BoundField DataField="OrderQuantity" HeaderText="Quantity" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount" ItemStyle-Width="110" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" ItemStyle-Width="240" />
                    <asp:BoundField DataField="OrderDescription" HeaderText="Additional Order Request" ItemStyle-Width="240" />
                    <asp:BoundField DataField="OrderID" HeaderText="Link" HtmlEncode="False" DataFormatString="<a target='_blank' href='OrderDetailInfo.aspx?OrderID={0}'>Go to Detail</a>" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
