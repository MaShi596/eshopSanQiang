<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#btnOrderSearch').bind('click', function () {
            var $divCatlist = $("#divOrderInfo");
            $divCatlist.css("display", "block");
            var strShipOrderNumber = "";
            $divCatlist.empty();
            var orderId = $("#txtOrderId").attr("value");
            $.ajax({
                url: "Default.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { OrderId: orderId },
                async: false,
                success: function (data) {
                    $.each(data, function (i, item) {
                        if (item.OrderId == undefined) {
                            $divCatlist.append("您输入的订单号不存在！");
                        }
                        else {
                            if (item.ShipOrderNumber == undefined || item.ShipOrderNumber == "") {
                                strShipOrderNumber = "无";
                            }
                            else {
                                strShipOrderNumber = item.ShipOrderNumber;

                            }
                            $divCatlist.append("<div class=\"divapp\" onclick=\"javascript:$('#divOrderInfo').hide()\">&times;关闭</div>")
                            .append("<div>订单号：" + item.OrderId + "&nbsp;" + "</div>")
							.append("<div>订单状态：" + item.ShippingStatus + "</div>")
                            .append("<div>发货单号：" + strShipOrderNumber + "</div>")
                            .append("<div>配送方式：" + item.ShipModeName + "</div>");
                        }
                    });
                }
            });
            $("#txtOrderId").attr("value", "");
            return false;
        });

        $("#txtOrderId").keydown(function (e) {
            if (e.keyCode == 13) {
                $('#btnOrderSearch').focus();
                $('#btnOrderSearch').click(function () { });
            }
        });
    });
        </script>
        
        
        <input type="text" id="txtOrderId"  class="Default_Input"/>
        <input id="btnOrderSearch" type="button" value="" class="Default_OrderSearchBtn"/><br />
        <div id="divOrderInfo"></div>

        </style>
