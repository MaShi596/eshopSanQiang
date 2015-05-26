<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_ShipOrders.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_ShipOrders" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Import Namespace="Hidistro.Entities.Sales"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
			<li  id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server" ><span>所有订单</span></asp:HyperLink></li>
			<li  id="anchors1" style="display:none;"><asp:HyperLink ID="hlinkNotPay" runat="server" Text=""><span>等待买家付款</span></asp:HyperLink></li>
			<li  id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server" Text=""><span>等待发货</span></asp:HyperLink></li>
            <li  id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server" Text=""><span>已发货</span></asp:HyperLink></li>
            <li  id="anchors5" style="display:none;"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功订单</span></asp:HyperLink></li>
            <li  id="anchors4" style="display:none;"><asp:HyperLink ID="hlinkClose" runat="server" Text=""><span>已关闭</span></asp:HyperLink></li>
            <li  id="anchors99" style="display:none;"><asp:HyperLink ID="hlinkHistory" runat="server" Text=""><span>历史订单</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--选项卡-->

<div class="dataarea mainwidth">
		<!--搜索-->
		<div class="searcharea clearfix br_search">
		  <ul>
		    <li> <span>选择时间段：</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" Width="80" />
		      </span> <span class="Pg_1010">至</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" Width="80"/>
		        </span></li>
		    <li style="display:none;"><span>会员名：</span><span>
		      <asp:TextBox ID="txtUserName" runat="server" cssclass="forminput" />
		    </span></li>
		    <li><span>发货单号：</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" /><asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>		     
		      <li style="display:none;"><span>商品名称：</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>收货人：</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput" Width="80"></asp:TextBox>
		      </span></li>
		      <li><span>打印状态：</span><span>
		        <abbr class="formselect">  <asp:DropDownList runat="server" ID="ddlIsPrinted" /></abbr>
		      </span></li>
		      <li>&nbsp;</li>
		      	      <li><span>配送方式：</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li style="width:450px;">
		      <abbr>  <Hi:RegionSelector runat="server" ID="dropRegion" /></abbr>
		      </li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
	        </li>
	      </ul>
  </div>
		<!--结束-->
         <div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea" style="width:550px;">
				<ul>
					<li class="paginalNum">
                        <span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                     </li>
                     <li runat="server" id="htmlLiShipOrderPriceAll" style="margin-left:20px; display:none;">
                        当前搜索条件下，发货金额总计：<strong style="color:Red;">￥<asp:Literal runat="server" ID="litlShipOrderPriceAll"></asp:Literal></strong>
                     </li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
			</div>
			<!--结束-->

  <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true"/></span>
		  <span class="printorder"><a href="javascript:printPosts()">批量打印快递单</a></span>
          <span class="printorder"><a href="javascript:printGoods()">批量打印发货单</a></span>
          <span class="downproduct"><a href="javascript:downOrder()">下载配货单</a></span>
           <span class="sendproducts"><a href="javascript:batchSend()" onclick="">批量发货</a></span>
		</li>
        </ul>
      </div>  </div>
		<input type="hidden" id="hidOrderId" runat="server" />
		<!--数据列表区域-->
	  <div class="datalist">
	  <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" Width="100%">
	  
	  <HeaderTemplate>
      <table width="0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="200" class="td_left td_right_fff">发货</td>
		      <td width="180" class="td_right td_left">发货状态</td>
		      <td width="70" class="td_right td_left" style="display:none;">发货金额</td>
		      <td class="td_right td_left">收货信息</td>
	        </tr>
	    </HeaderTemplate>
         <ItemTemplate>
	   
	   <tr class="td_bg" >
		      <td style="background:#F3F3F3;color:#010101;"><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />发货单号：<%#Eval("OrderId") %></td>
		      <td style="background:#F3F3F3;color:#010101;">提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td style="background:#F3F3F3;color:#010101;"><%# (bool)Eval("IsPrinted")?"已打印":"未打印" %></td>
		      <td align="right" style="background:#F3F3F3;color:#010101;">		       
		        <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>','<%#Eval("ManagerRemark") %>');" ><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
		      </td>
	        </tr>
		    <tr>
		      <td>
		        <asp:Label CssClass="submit_faihuo" id="lkbtnSendGoods" Visible="false" runat="server" > 
                    <a style="color:Red" href="javascript:DialogFrame('<%# "../admin/Cpage/Supplier/Supplier_Admin_ShipOrderSendGoods.aspx?OrderId="+ Eval("OrderId") %>','发货单发货',null,null)">发货</a> </asp:Label>&nbsp;
		     </td>
		      <td>
                <Hi:Supplier_DBLabel_ShipOrderStatus ID="lblOrderStatus" runat="server" />
                <div>
                    <%#Eval("RealModeName")%> <%#Eval("ShipOrderNumber")%>
                </div>
              </td>
		      <td style="display:none;"><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td>
                <%#Eval("ShipTo") %> <%#Eval("CellPhone")%> <%#Eval("TelPhone")%>
                <%#Eval("ModeName")%> <%#Eval("ShippingRegion")%> <%#Eval("Address")%> <%#Eval("ZipCode")%>
                <div>送货上门时间：<%#Eval("shiptodate")%></div>
                <div>买家备注：<%#Eval("remark")%></div>
              </td>
	        </tr>
            <tr>
            <td colspan="4">
                <iframe id='<%# "htmlIframePts_" + Eval("orderid") %>' width="100%" scrolling="no" frameborder="0"  ></iframe>
                <script type="text/javascript">
                    function showOrderItem(auto) {
                        
                        if (auto != undefined && auto != "") {
                            var currentDate1 = new Date();
                            var ptUrl = applicationPath + "/CPage/Supplier/ShipOrderItems.aspx?orderid=" + auto + "&t=" + currentDate1.getTime();
                            $("#htmlIframePts_" + auto).attr({ "src": ptUrl });
                        }
                    }
                </script>
                <%# "<script>showOrderItem('" + Eval("orderid") + "')</script>"%>
            </td>
            </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>  
        </asp:DataList>

      <div class="blank5 clearfix"></div>
      
  </div>
          
    <!--数据列表底部功能区域-->
	  <div class="page">
	 <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
               </div>

			</div>
		</div>
      </div>
      </div>

</div>


<!--关闭订单--->
<div id="closeOrder" style="display:none;">
    <div class="frame-content">
            <p><em>关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em></p>
             <p><span class="frame-span frame-input110">关闭该订单的理由:</span> <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" /></p>
    </div>
</div>

<!--编辑备注--->
<div id="RemarkOrder"  style="display:none;">
    <div class="frame-content">
          <p><span class="frame-span frame-input100">订单号：</span><span id="spanOrderId"></span></p>
          <p><span class="frame-span frame-input100">提交时间：</span><span id="lblOrderDateForRemark"></span></p>
          <p><span class="frame-span frame-input100">订单实收款(元)：</span><strong class="colorA"><Hi:FormatedMoneyLabel ID="lblOrderTotalForRemark" runat="server" /></strong></p>
          <span class="frame-span frame-input100">标志：<em>*</em></span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" />
          <p><span>备忘录：</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></p>
    </div>
</div>
<div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align:center;">
            <input type="button" id="btnorderph" onclick="javascript:Setordergoods();" class="submit_DAqueding" value="订单配货表"/>
        &nbsp;
        <input type="button" id="Button1" onclick="javascript:Setproductgoods();" class="submit_DAqueding" value="商品配货表"/>
            <p>导出内容只包括等待发货状态的订单</p>
            <p>订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。</p>
        </div>
    </div>
<div style="display:none">
   <asp:Button ID="btnCloseOrder"  runat="server" CssClass="submit_DAqueding" Text="关闭订单"   />
    <asp:Button runat="server" ID="btnRemark" Text="编辑备注信息" CssClass="submit_DAqueding"/>
    <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="订单配货表" />&nbsp;
            <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="submit_DAqueding" /> 
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript">
     var formtype = "";

     function ConfirmPayOrder() {
         return confirm("如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态\n\n此操作成功完成以后，订单的当前状态将变为已付款状态，确认客户已付款？");
     }

     function ShowOrderState() {
         var status;
         if (navigator.appName.indexOf("Explorer") > -1) {

             status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

         } else {

             status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

         }
         if (status != "0") {
             document.getElementById("anchors0").className = 'optionstar';
         }
         if (status != "99") {
             document.getElementById("anchors99").className = 'optionend';
         }
         document.getElementById("anchors" + status).className = 'menucurrent';
     }

     $(document).ready(function () { ShowOrderState(); });


     //备注信息
     function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
         arrytext = null;
         formtype = "remark";
         $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
         $("#ctl00_contentHolder_hidOrderId").val(OrderId);
         $("#spanOrderId").html(OrderId);
         $("#lblOrderDateForRemark").html(OrderDate);

         


         for (var i = 0; i <=5; i++) {
             if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark) {
                 setArryText("ctl00_contentHolder_orderRemarkImageForRemark_" + i, "true");
                 $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", true);
             }
             else {
                 $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", false);
             }

         }
         setArryText("ctl00_contentHolder_txtRemark", managerRemark);
         DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
     }

     function CloseOrder(orderId) {
         arrytext = null;
         formtype = "close";
         $("#ctl00_contentHolder_hidOrderId").val(orderId);
         DialogShow("关闭订单", 'closeframe', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
     }

     function ValidationCloseReason() {
         var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
         if (reason == "请选择关闭的理由") {
             alert("请选择关闭的理由");
             return false;
         }
         setArryText("ctl00_contentHolder_ddlCloseReason", reason);
         return true;
     }

     // 批量打印发货单
     function printGoods() {
         var orderIds = "";
         $("input:checked[name='CheckBoxGroup']").each(function () {
             orderIds += $(this).val() + ",";
         }
             );
         if (orderIds == "") {
             alert("请选要打印的订单");
         }
         else {
             var url = "../admin/Cpage/Supplier/Supplier_BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
             DialogFrame(url, "批量打印发货单", null, null);
         }
     }

     //批量发货
     function batchSend() {
         var orderIds = "";
         $("input:checked[name='CheckBoxGroup']").each(function () {
             orderIds += $(this).val() + ",";
         }
             );
         if (orderIds == "") {
             alert("请选要发货的订单");
         }
         else if (confirm('将当前选中结果中筛选出已付款未发货的订单进行批量发货，是否继续？')) {
             var url = "../admin/sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds;
             DialogFrame(url, "批量发货", null, null);
             window.parent.FramLinkToSet("");
         }
     }
     function Setordergoods() {
         $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
     }
     function Setproductgoods() {
         $("#ctl00_contentHolder_btnProductGoods").trigger("click");
     }
     //批量打印快递单
     function printPosts() {
         var orderIds = "";
         $("input:checked[name='CheckBoxGroup']").each(function () {
             orderIds += $(this).val() + ",";
         }
             );
         if (orderIds == "") {
             alert("请选要打印的订单");
         }
         else {
             var url = "../admin/sales/BatchPrintData.aspx?OrderIds=" + orderIds;
             DialogFrame(url, "批量打印快递单", null, null);
             window.parent.FramLinkToSet("");
         }
     }

     //验证
     function validatorForm() {
         switch (formtype) {
             case "remark":
                 arrytext = null;
                 $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                 if ($radioId == null || $radioId == "undefined") {
                     alert('请先标记备注');
                     return false;
                 }
                 setArryText($radioId.id, "true");
                 setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                 break;
             case "shipptype":
                 return ValidationShippingMode();
                 break;
             case "close":
                 return ValidationCloseReason();
                 break;
         };
         return true;
     }
     // 下载配货单
     function downOrder() {
         var orderIds = "";
         $("input:checked[name='CheckBoxGroup']").each(function () {
             orderIds += $(this).val() + ",";
         }
             );
         if (orderIds == "") {
             alert("请选要下载配货单的订单");
         }
         else {
             ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
         }
     }
     $(function () {
         $(".datalist img[src$='tui.gif']").each(function (item, i) {
             $parent_link = $(this).parent();
             $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null);");
         });
     });

     // 供应商查看自己的
     function showWindow_ShipInfoPageForSupplier(auto,userid) {
         var currentDate = new Date();
         DialogFrame(applicationPath + "/Cpage/Supplier/SendGoodMsg.aspx?orderid=" + auto + "&Userid=" + userid + "&t=" + currentDate.getTime(), "查看发货信息", null, null);
         window.parent.FramLinkToSet("");
     }

     // 区域发货点查看全部
     function showWindow_ShipInfoPage(auto) {
         var currentDate = new Date();
         DialogFrame(applicationPath + "/Cpage/Supplier/SendGoodMsg.aspx?orderid=" + auto + "&t=" + currentDate.getTime(), "查看发货信息", null, null);
         window.parent.FramLinkToSet("");
     }
     </script>
</asp:Content>
