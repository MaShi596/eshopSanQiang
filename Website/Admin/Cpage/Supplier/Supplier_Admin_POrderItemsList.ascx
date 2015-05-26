<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Supplier_Admin_POrderItemsList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_POrderItemsList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>

    <table style="margin-bottom:5px;">
    <tr>
    <td>
        批量手工分配供应商：<Hi:Supplier_Drop_Suppliers runat="server" ID="drpSupplier"></Hi:Supplier_Drop_Suppliers> <asp:Button ID="btnFenPei" runat="server" Text="分配" />
        <div style="color:#5A5E65;"><asp:Literal runat="server" ID="litlSupplierComment"></asp:Literal> </div>
    </td>
    </tr>
    </table>

    <asp:DataList ID="dlstOrderItems" runat="server"  Width="100%" DataKeyField="SkuId" >
         <HeaderTemplate>
      <table width="100%" border="0" cellspacing="0">
	    <tr class="table_title">
          <td class="td_right td_left" >&nbsp;</td>
          <td width="15%" class="td_right td_left">分配供应商</td>
          <td width="15%" class="td_right td_left">原供应商</td>
	      <td colspan="2" class="td_right td_left">商品名称</td>
	      <td width="8%" class="td_right td_left">采购价</td>
	      <td width="5%" class="td_right td_left">数量 </td>
	      <td width="12%" class="td_left td_right_fff">总采购价</td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>        
          <tr>
          <td><asp:CheckBox runat="server" ID="cbDBId" /></td>
          <td><%#Eval("SupplierNameAuto")%></td>
          <td><%#Eval("SupplierNamePt")%></td>
	      <td width="7%"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
	      <td width="32%"><span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank"><%# Eval("ItemHomeSiteDescription")%></a></span> 
	        <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' /> <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span></td>
	      <td><Hi:FormatedMoneyLabel ID="lblItemPurchasePrice" runat="server" Money='<%# Eval("ItemPurchasePrice") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
	      <td><strong class="colorG"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2"  runat="server" Money='<%# (decimal)Eval("ItemPurchasePrice")*(int)Eval("Quantity") %>'/></strong></td>
        </tr>        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
      
	  <div class="Price">
	    <table width="200" border="0" cellspacing="0">
	       <tr class="bg">
	        <td align="right"  class="Pg_top td_none">
	            <Hi:PurchaseOrderItemUpdateHyperLink runat="server" ID="purchaseOrderItemUpdateHyperLink" Text="修改采购单项" /></td>
	        <td width="8%" ></td>
	       </tr>
	       <tr class="bg">
	      <td class="Pg_top td_none" width="88%" align="right" >商品金额（元）：</td>
	      <td class="Pg_top td_none" width="12%" ><strong class="fonts colorG"><Hi:FormatedMoneyLabel ID="lblGoodsAmount" runat="server" /></strong></td>
        </tr>
	    <tr class="bg">
	      <td class="Pg_bot" align="right">商品总重量（克）：</td>
	      <td class="Pg_bot" ><strong class="fonts "><asp:Literal ID="lblWeight" runat="server" /></strong></td>
        </tr>
        </table>
	  </div>
	  <div runat="server" id="giftsList">
	  <h1>礼品列表</h1>
	  <asp:DataList ID="grdOrderGift" runat="server" DataKeyField="GiftId" Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
        <tr class="table_title">
            <td colspan="2" class="td_right td_left">商品名称</td>
            <td width="15%" class="td_right td_left">采购价(元) </td>
            <td width="15%" class="td_right td_left">数量 </td>
            <td width="15%" class="td_left td_right_fff">总采购价(元) </td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td width="7%"><Hi:HiImage ID="HiImage1" AutoResize="true" Width="60" Height="60" runat="server" DataField="ThumbnailsUrl" /></td>
            <td width="45%"><span class="Name"><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span></td>
            <td><Hi:FormatedMoneyLabel ID="giftPrice" runat="server" Text='<%# Eval("PurchasePrice") %>'></Hi:FormatedMoneyLabel></td>
            <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal></td>
            <td><strong class="colorG"><Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" Money='<%# (decimal)Eval("PurchasePrice")*(int)Eval("Quantity") %>' /></strong></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
      </div>


   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
 