<%@ Page Title="" Language="C#" MasterPageFile="~/Views/OnlieShopping.Master" AutoEventWireup="true" CodeBehind="DeliItemList.aspx.cs" Inherits="OnlineShopping.Views.DeliItemInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #main-div {
            margin: 20px 120px 0px 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-div">
        <h5 style="font-size:25px; text-align: center; margin-bottom: 20px;">Delivery Item List</h5>
        <div>
            <asp:GridView ID="gvDeliItem" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="Silver" Font-Size="11px">
                <HeaderStyle HorizontalAlign="center" BackColor="#011f34" ForeColor="white"/>
                <Columns>
                    <asp:BoundField DataField="OrderNo" HeaderText="Order No." ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="OrderQuantity" HeaderText="Quantity" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DeliveryCharges" HeaderText="Delivery Charges" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" />  
                    
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="130" />
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Customer Mobile" ItemStyle-Width="130" />
                    <asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" ItemStyle-Width="230" />
                    <asp:BoundField DataField="OrderDescription" HeaderText="Additional Order Request" ItemStyle-Width="220" />

                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EstDeliveryDate" HeaderText="Est. Delivery Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="UpdatedDate" HeaderText="Delivery Date" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                    
                    <asp:BoundField DataField="DeliMan_Name" HeaderText="Deli Man" ItemStyle-Width="130" />
                    <asp:BoundField DataField="DeliMan_Mobile" HeaderText="Deli Man Mobile" ItemStyle-Width="130" />
                    <asp:BoundField DataField="Status" HeaderText="Is Delivered" ItemStyle-Width="90" />
                    <asp:BoundField DataField="OrderID" HeaderText="Link" HtmlEncode="False" DataFormatString="<a href='DeliItemDetailInfo.aspx?OrderID={0}'>Go to Detail</a>" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
