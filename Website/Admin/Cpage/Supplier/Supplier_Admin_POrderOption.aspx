<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_POrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_POrderOption" MaintainScrollPositionOnPostback="true"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Items" Src="~/Admin/Cpage/Supplier/Supplier_Admin_POrderItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="PurchaseOrder_Charges" Src="~/Admin/Ascx/PurchaseOrder_Charges.ascx" %>


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
    <div class="Purchase" style="display:none;">
      <div class="StepsC">
        <ul>
        	<li><strong class="fonts">第<span class="colorG">1</span>步</strong> 分销商已下单</li>
            <li><strong class="fonts">第<span class="colorG">2</span>步</strong> 分销商付款</li>
            <li><strong class="fonts colorP">第3步</strong> <span class="colorO">发货</span></li>
            <li><strong class="fonts">第<span class="colorG">4</span>步</strong> 交易完成</li>                                          
        </ul>
      </div>    
    </div>
    
    <div class="list" style="display:none;">
     <h1>发货</h1>
      <div class="Settlement">
        <table width="200" border="0" cellspacing="0"  class="br_none">
          <tr>
            <td width="15%" align="right">配送方式：</td>
            <td >
              <Hi:ShippingModeRadioButtonList ID="radioShippingMode" AutoPostBack="true" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" class="br_none" /></td>
          </tr>
          <tr>
            <td width="15%" align="right"  nowrap="nowrap">物流公司：</td>
            <td class="a_none"><Hi:ExpressRadioButtonList runat="server" RepeatColumns="6" RepeatDirection="Horizontal" ID="expressRadioButtonList" /></td>
          </tr>
          <tr>
            <td align="right">运单号码：</td>
            <td><asp:TextBox runat="server" ID="txtShipOrderNumber" /></td>
          </tr>
        </table>
     </div>  
    </div>

    <div class="list">
        <h1>收货信息</h1>
        <div class="Settlement">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="90" align="right"><asp:Literal runat="server" ID="litShippingModeName" /></td>
            <td class="a_none"><asp:Literal runat="server" ID="litReceivingInfo" /></td>
            <td width="100" class="a_none"><span class="Name"><a href="javascript:DialogFrame('purchaseOrder/MondifyAddressFrame.aspx?action=update&PurchaseOrderId=<%=Page.Request.QueryString["PurchaseOrderId"] %>','修改收货地址',645,315);" >修改收货地址</a></span></td>
          </tr>
          <tr>
            <td align="right" nowrap="nowrap">送货上门时间：</td>
            <td colspan="2" class="a_none"><asp:Label ID="litShipToDate"  runat="server" style="word-wrap: break-word; word-break: break-all;"/>&nbsp;</td>
          </tr>
          <tr>
            <td align="right">买家备注：</td>
            <td colspan="2" class="a_none">  <asp:Literal runat="server"  ID="litRemark" />&nbsp;</td>
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
	                            <span class="submit_shanchu"><Hi:ImageLinkButton ID="ImageLinkButton1" CommandName="Select" runat="server" Text="选择" IsShow="false" /></span>
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

  <div class="blank12 clearfix"></div>
	<div class="list">
<cc1:PurchaseOrder_Items runat="server" ID="itemsList" />

  <h1 style="display:none;">采购单实收款结算</h1>
        <div class="Settlement" style="display:none;">
        <table width="200" border="0" cellspacing="0">
          <tr>
            <td width="15%" align="right">运费(元)：</td>
            <td width="12%"><asp:Literal ID="litFreight" runat="server" /> (<asp:Literal ID="lblModeName" runat="server" />)</td>
            <td width="73%">&nbsp;</td>
          </tr>
          <tr>
            <td align="right">涨价或折扣(元)： </td>
            <td colspan="2" class="colorB"><asp:Literal ID="litDiscount" runat="server" /></td>
          </tr>
          <tr class="bg">
            <td align="right" class="colorG">采购单实收款(元)：</td>
            <td colspan="2"> <strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
          </tr>
        </table>
    </div>
    </div>

      <div class="bnt Pa_140 Pg_15 Pg_18">
       <asp:Button ID="btnSendGoods" runat="server" Text="生成发货单" CssClass="submit_DAqueding" />
      </div>  
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>

