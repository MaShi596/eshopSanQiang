<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Supplier_Admin_OrderMatchItemsList.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_OrderMatchItemsList" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>
<asp:DataList ID="dlstOrderItems" runat="server" Width="100%" DataKeyField="SkuId">
    <HeaderTemplate>
        <table width="200" border="0" cellspacing="0">
            <tr class="table_title">
                <td width="15%" class="td_right td_left" style="background: #F3F3F3;">
                    供应商
                </td>
                <td colspan="2" class="td_right td_left" style="background: #F3F3F3;">
                    商品名称
                </td>
                <td width="10%" class="td_right td_left" style="background: #F3F3F3;">
                    商品单价
                </td>
                <td width="10%" class="td_right td_left" style="background: #F3F3F3;">
                    购买数量
                </td>
                <td width="10%" class="td_right td_left" style="background: #F3F3F3;">
                    发货数量
                </td>
                <td width="10%" class="td_left td_right_fff" style="background: #F3F3F3;">
                    小计
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <%#Eval("SupplierNameAuto")%>
            </td>
            <td width="7%">
                <a href='<%#"../../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                    <Hi:ListImage ID="HiImage2" runat="server" DataField="ThumbnailsUrl" /></a>
            </td>
            <td width="32%">
                <span class="Name"><a href='<%#"../../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>'
                    target="_blank">
                    <%# Eval("ItemDescription")%></a></span> <span class="colorC">货号：<asp:Literal runat="server"
                        ID="litCode" Text='<%#Eval("sku") %>' />
                        <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal></span>
            </td>
            <td style="color: #FF3A3B;">
                <b>
                    <Hi:FormatedMoneyLabel ID="lblItemListPrice" runat="server" Money='<%# Eval("ItemListPrice") %>' /></b>
            </td>
            <td>
                ×<asp:Literal runat="server" ID="litQuantity" Text='<%#Eval("Quantity") %>' />
            </td>
            <td>
                ×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' />
            </td>
            <td>
                <div class="color_36c">
                    <asp:HyperLink ID="hlinkPurchase" runat="server" NavigateUrl='<%# string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"),  Eval("PromotionId"))%>'
                        Text='<%# Eval("PromotionName")%>' Target="_blank"></asp:HyperLink>
                </div>
                <strong class="colorG">
                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# (decimal)Eval("ItemAdjustedPrice")*(int)Eval("Quantity") %>' /></strong>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:DataList>
