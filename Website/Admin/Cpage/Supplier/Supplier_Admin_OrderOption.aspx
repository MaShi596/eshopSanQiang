<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_OrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_OrderOption" MaintainScrollPositionOnPostback="true"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Cpage/Supplier/Supplier_Admin_OrderItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<%@ Register TagPrefix="Hi" Namespace="Hishop.Web.CustomMade" Assembly="Hishop.Web.CustomMade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<style type="text/css">
#abc span{ display:inline;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title title_height m_none td_bottom"> <em><img src="../../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line">生成发货单</h1>
  </div>
  <div class="list" style="display:none;">
    <div class="State" >
     <h1>订单详情</h1>
      <table width="200" border="0" cellspacing="0">
        <tr>
          <td width="10%" align="right">订单编号：</td>
          <td width="20%"><asp:Label ID="lblOrderId" runat="server"></asp:Label></td>
          <td width="10%" align="right">创建时间：</td>
          <td width="28%"><Hi:FormatedTimeLabel runat="server" ID="lblOrderTime" ></Hi:FormatedTimeLabel></td>
          <td width="10%" align="right">&nbsp;</td>
          <td width="20%">&nbsp;</td>
        </tr>
      </table>
	  </div>
    </div>

  <div class="blank12 clearfix"></div>
	<div class="list">
     <h1>收货信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td align="right"><asp:Literal runat="server" ID="litShippingModeName" /></td>
            <td width="85%" class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="10%" class="a_none"><span class="Name"><a href="javascript:UpdateShippAddress('<%=Page.Request.QueryString["OrderId"] %>')">修改收货地址</a></span></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">送货上门时间：</td>
            <td colspan="2" class="a_none"><asp:Label ID="litShipToDate"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2" class="a_none">&nbsp; <asp:Label ID="litRemark"  runat="server" style="word-wrap: break-word; word-break: break-all;"/></td>
          </tr>
        </table>
        </div>
  </div>


   <div class="list">
          <h1>订单区域发货点匹配</h1>
      <div class="Settlement">
        
         <table width="100%" border="0" cellspacing="0">
          <tr>
            <td valign="top" width="90">自动匹配结果：</td>
            <td valign="top">
                <asp:Literal runat="server" ID="txtShipPointNameAuto"></asp:Literal>
                <div style="color:#5A5E65;"><asp:Literal runat="server" ID="txtShipPointNameAuto2"></asp:Literal></div>
            </td>
           </tr>
           <tr>
            <td valign="top" width="90">
                <asp:CheckBox runat="server" ID="btnShipPointSelf" />手工匹配：
                <div style="color:Red; padding:5px;">注：如开启手工匹配，则按手工选择结果处理</div>
            </td>
            <td id="abc" valign="top">
                <div runat="server" id="htmlDivShipPoint" style="display:none;" >

                <table>
                <tr>
                    <td width="100">手工选择发货点：</td>
                    <td>
                        <asp:Literal runat="server" ID="txtShipPointName"></asp:Literal>
                        <div style="color:#5A5E65;"><asp:Literal runat="server" ID="txtShipPointName2"></asp:Literal></div>
                    </td>
                </tr>
                </table>

                <table style="margin-top:5px;">
                <tr>
                    <td><Hi:RegionSelector runat="server" ID="rsddlRegion" /></td>
                    <td><asp:Button ID="btnSearchShipPoint" runat="server" class="searchbutton" Text="查询" /></td>
                </tr>
                </table>
                 
                <div style="margin-top:5px;">
                <UI:Grid ID="grdShipPoints" runat="server" AutoGenerateColumns="False"  ShowHeader="false" DataKeyNames="UserId" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="CreateDate" SortOrder="ASC" Width="100%" >
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="50px" HeaderStyle-CssClass="td_right td_left" >
                            <ItemTemplate>
	                            <span class="submit_shanchu"><Hi:ImageLinkButton CommandName="Select" runat="server" Text="选择" IsShow="false" /></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left" >
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>   
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="联系信息" HeaderStyle-CssClass="td_right td_left" ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <%# Eval("Supplier_RegionName") %>
                                <div style="color:#5A5E65;"><%# Eval("comment") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
  
                    </Columns>
                </UI:Grid> 
                </div>
                </div>
                &nbsp;
            </td>
           </tr>
       </table>
         </div>  
   </div>

   <div class="list">
          <h1>商品供应商匹配</h1>
      <div class="Settlement">

            <cc1:Order_ItemsList  runat="server" ID="itemsList" />

         </div>  
   </div>

    <div class="bnt Pa_140 Pg_15 Pg_18">
    <asp:Button ID="btnSendGoods" runat="server" Text="生成发货单" class="submit_DAqueding" />
    </div>


  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtShipOrderNumber', 1, 20, false, null, '运单号码不能为空，在1至20个字符之间'));
    }
    $(document).ready(function () { InitValidators(); });


    function UpdateShippAddress(ordernumber){
        var pathurl = "sales/ShippAddress.aspx?action=update&orderId=" + ordernumber;
        DialogFrame(pathurl, "修改收货地址", 640, 300);
    }
</script>
</asp:Content>
