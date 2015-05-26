<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipOrders.aspx.cs" Inherits="Hidistro.UI.Web.ShipOrders" %>
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
    <div style="font-size:12px; color:#404040;">
        <table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse:collapse;">
        <asp:Repeater ID="dlstOrderItems" runat="server"  >
        <ItemTemplate>
            <tr style="height:18px; line-height:18px;">
                <td style="padding:5px;"><strong><%# Eval("SupplierTypeName") %>：</strong><%# Eval("username")%></td>
                <td style="padding:5px;"><strong>发货单：</strong><%# Eval("OrderId")%></td>
                <td style="padding:5px;"><strong>发货状态：</strong><Hi:Supplier_DBLabel_ShipOrderStatus ID="lblOrderStatus" runat="server" />
                    <%# Eval("RealModeName")%> <%# Eval("ShipOrderNumber")%></td>
            </tr>
      </ItemTemplate>
      </asp:Repeater>
      </table>
      <div style="height:20px;">&nbsp;</div>
    </form>
</body>
</html>
