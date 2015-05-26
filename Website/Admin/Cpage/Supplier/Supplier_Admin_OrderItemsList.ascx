<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Supplier_Admin_OrderItemsList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_OrderItemsList"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
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
      <table width="200" border="0" cellspacing="0">
	    <tr class="table_title">
         <td class="td_right td_left">&nbsp;</td>
          <td width="15%" class="td_right td_left">分配供应商</td>
          <td width="15%" class="td_right td_left">原供应商</td>
	      <td colspan="2" class="td_right td_left">商品名称</td>
	      <td width="10%" class="td_right td_left">商品单价</td>
	      <td width="10%" class="td_right td_left">购买数量</td>
	      <td width="10%" class="td_right td_left">发货数量</td>
	      <td width="10%" class="td_left td_right_fff">小计</td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>      
          <tr>
          <td><asp:CheckBox runat="server" ID="cbDBId" /></td>
          <td><%#Eval("SupplierNameAuto")%></td>
          <td><%#Eval("SupplierNamePt")%></td>
	      <td width="7%"><a href='<%#"../../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
	      <td width="32%"><span class="Name"><a href='<%#"../../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
	        <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
	        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
	     </td>
	      <td><Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' /></td>
	      <td>×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
	      <td>
	        <div class="color_36c">
            <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
            </div>
	        <strong class="colorG"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2"  runat="server"  Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>'/></strong>
	      </td>
        </tr>        
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>
      
	  <div class="Price">
	    <span>商品重量：<asp:Literal runat="server" ID="litWeight" />g</span>
	    <span style="margin-left:400px;"><asp:Literal runat="server" ID="lblAmoutPrice" /></span>
        <span><asp:HyperLink ID="hlkReducedPromotion" runat="server" Target="_blank" /></span>
        <strong>购物车小计：<Hi:FormatedMoneyLabel ID="lblTotalPrice" runat="server" /></strong>
        <asp:Literal runat="server" ID="lblBundlingPrice" />
	  </div>
	  
	  <h1><asp:Label ID="lblOrderGifts" runat="server" Text="礼品列表(主站供应)"></asp:Label> </h1>
	  <asp:DataList ID="grdOrderGift" runat="server" DataKeyField="GiftId" Width="100%" >
         <HeaderTemplate>
      <table width="200" border="0" cellspacing="0">
        <tr class="table_title">
            <td width="170" class="td_right td_left">礼品名称</td>
            <td width="30" class="td_right td_left">数量 </td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td ><Hi:HiImage ID="HiImage1" AutoResize="true" runat="server" DataField="ThumbnailsUrl" /> <span><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span> </td>
            
            <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal></td>
            
        </tr>
        </ItemTemplate>
        <FooterTemplate>
      </table>
      </FooterTemplate>
      </asp:DataList>




