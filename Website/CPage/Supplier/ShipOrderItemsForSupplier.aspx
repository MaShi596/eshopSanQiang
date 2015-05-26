<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipOrderItemsForSupplier.aspx.cs" Inherits="Hidistro.UI.Web.ShipOrderItemsForSupplier" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发货信息查看</title>

    <script type="text/javascript">
        function QueryString() {
            var name, value, i;
            var str = location.href;
            var num = str.indexOf("?")
            str = str.substr(num + 1);
            var arrtmp = str.split("&");
            for (i = 0; i < arrtmp.length; i++) {
                num = arrtmp[i].indexOf("=");
                if (num > 0) {
                    name = arrtmp[i].substring(0, num);
                    value = arrtmp[i].substr(num + 1);
                    this[name] = value;
                }
            }
        }
        var Request = new QueryString();
        var orderid = Request["orderid"]

        // 自适应高度
        function AutoHeight() {
            var AutoHeight = parent.window.document.getElementById("htmlIframePts_" + orderid);
            AutoHeight.height = document.body.scrollHeight;
        }
    </script>

</head>
<body onload="AutoHeight()">
    <form id="form1" runat="server">
    <div style="font-size:12px;">
        <table border="1" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse:collapse;width:100%; text-align:center;">
	    <tr class="table_title" style="height:24px; line-height:24px; ">
	        <td colspan="2" >商品名称</td>
            <td width="12%" >供货价</td>
            <td width="10%" >重量</td>
	        <td width="10%" >发货数量</td>
        </tr>
        <asp:Repeater ID="dlstOrderItems" runat="server"  >
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
             <td><Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("CostPrice") %>'/></td>
             <td><%#Eval("weight") %></td>
	          <td>×<asp:Literal runat="server" ID="litShipmentQuantity" Text='<%#Eval("ShipmentQuantity") %>' /></td>
            </tr>
 
      </ItemTemplate>
        </asp:Repeater>
        </table>
    </div>
    <div style="height:20px;">&nbsp;</div>
    </form>
</body>
</html>
