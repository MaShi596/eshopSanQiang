<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipOrdersMatch.aspx.cs"
    Inherits="Hidistro.UI.Web.Admin.ShipOrdersMatch" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>
<%@ Register TagPrefix="cc1" TagName="OrderMatch_ItemsList" Src="~/Admin/Cpage/Supplier/Supplier_Admin_OrderMatchItemsList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>发货信息查看</title>
    <link href="../../css/css.css" rel="stylesheet" type="text/css" />
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
        var orderid1 = Request["orderid"]

        // 自适应高度
        function AutoHeightMatch() {
            var AutoHeight = parent.window.document.getElementById("htmlIframeMatchPts_" + orderid1);
            var height1 = document.documentElement.scrollHeight; // 谷歌
            var height2 = document.body.scrollHeight; // 其他
            if (height1 < height2)
                AutoHeight.height = height1;
            else
                AutoHeight.height = height2;
        }

    </script>
</head>
<body onload="AutoHeightMatch()">
    <form id="form1" runat="server">
    <div style="font-size: 12px; color: #404040;" class="">
        <table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse: collapse;
            width: 100%;">
            <tr>
                <td colspan="2" style="padding: 5px; color: #FFB366; text-align: center; border-bottom: 1px solid #E0DCCE;">
                    <b>发货点自动匹配结果</b>
                </td>
            </tr>
            <tr style="height: 18px; line-height: 18px;">
                <td style="padding: 5px; width: 10%; border-bottom: 1px solid #E0DCCE;">
                    <b>订单发货点：</b>
                </td>
                <td style="float: left; padding: 5px; border-bottom: 1px solid #E0DCCE; width: 100%;">&nbsp;
                    <asp:Literal runat="server" ID="txtShipPointNameAuto"></asp:Literal><asp:Literal
                        runat="server" ID="txtShipPointNameAuto2"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding: 5px;">
                    <b>商品供应商：</b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="dataarea mainwidth databody" style="padding: 0 0 0px;">
                        <div class="list" style="margin: 0; border: 0px; padding: 0 0px 0px;">
                            <div class="Settlement">
                                <cc1:OrderMatch_ItemsList runat="server" ID="itemsList" />
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 20px;">
        &nbsp;</div>
    </form>
</body>
</html>
