<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendGoodMsg.aspx.cs" Inherits="Hidistro.UI.Web.SendGoodMsg" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发货信息查看</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:12px;">
        <asp:DataList ID="dlstOrderItems" runat="server"  Width="100%" >
        <ItemTemplate>

        <div style="padding-left:5px; color:#404040; border-top:1px solid #E6E6E6;border-left:1px solid #E6E6E6;border-right:1px solid #E6E6E6; background:#F3F3F3; height:24px; line-height:24px;">
            <strong><%# Eval("SupplierTypeName")%></strong> <strong>发货单：</strong><%# Eval("OrderId") %> 
            <strong style="margin-left:20px;">发货状态：</strong><Hi:Supplier_DBLabel_ShipOrderStatus runat="server" ></Hi:Supplier_DBLabel_ShipOrderStatus> <%# Eval("RealModeName") %> <%# Eval("ShipOrderNumber") %>
        </div>

        <div>
        <table border="1" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse:collapse;width:100%; text-align:center;">
	    <tr class="table_title" style="height:18px; line-height:18px;">
	        <td colspan="2" >商品名称</td>
	        <td width="10%" >发货数量</td>
        </tr>

        <asp:Repeater ID="dlstOrderItems2" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Two") %>' >
            <ItemTemplate>      
              <tr>
	          <td width="7%"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank">
                                    <Hi:ListImage ID="HiImage2"  runat="server" DataField="ThumbnailsUrl" /></a> </td>
	          <td width="32%" align="left" style="padding-left:5px;">
                <a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId") %>' target="_blank" style="color:#3366CC; text-decoration:none;">
	                <%# Eval("ItemDescription")%>
                </a>
                <div class="colorC">
                    货号：<asp:Literal runat="server" ID="litCode" Text='<%#Eval("sku") %>' />
	                <asp:Literal ID="litSKUContent" runat="server" Text='<%# Eval("SKUContent") %>'></asp:Literal>
                </div>
	         </td>
	          <td>×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
            </tr>        
            </ItemTemplate>
          </asp:Repeater>
          </table>



	          <asp:Repeater ID="dlstOrderItems3" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Two2") %>' Visible='<%# Eval("giftnum").ToString()=="0"?Convert.ToBoolean("False"):Convert.ToBoolean("True") %>'>
                <HeaderTemplate>
                <table border="1" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse:collapse;width:100%; text-align:center;">
	            <tr class="table_title" style="height:18px; line-height:18px;">
	                <td colspan="2" >礼品名称</td>
	                <td width="10%" >发货数量</td>
                </tr>
                </HeaderTemplate>
                <ItemTemplate>
                <tr>
                    <td width="7%"><Hi:HiImage ID="HiImage1" AutoResize="true" runat="server" DataField="ThumbnailsUrl" /></td>
                    <td width="32%" align="left" style="padding-left:5px;"><span><asp:Literal ID="giftName" runat="server" Text='<%# Eval("GiftName") %>'></asp:Literal></span> </td>
            
                    <td>×<asp:Literal ID="litQuantity" runat="server" Text='<%# Eval("Quantity") %>'  ></asp:Literal></td>
            
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </table>
                </FooterTemplate>
              </asp:Repeater>
        </div>
        <div style="border-bottom:1px solid #A3A3A3; padding:5px; margin-bottom:20px;">
            <span style="color:Red;"><Hi:Supplier_DBLabel_SendGoodStatus runat="server"></Hi:Supplier_DBLabel_SendGoodStatus></span>
        </div>
 
      </ItemTemplate>
      </asp:DataList>
    </div>
    </form>
</body>
</html>
