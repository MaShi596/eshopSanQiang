<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_ShipOrdersPriceTjForSupplier.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_ShipOrdersPriceTjForSupplier" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
     <div class="title">
		  <em><img src="../../images/02.gif" width="32" height="32" /></em>
          <h1><strong>供应商发货统计</strong></h1>
          <span>了解在某个时间范围内供应商发了多少货,供应商商品已供货价为准,主站商品已成本价为准</span>
		</div>
    
    <!--数据列表区域-->
    <div class="datalist">
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li> <span>用户名：</span> <span>
          <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
        </span> </li>
		    <li> <span>发货时间：</span><span>
		      <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" Width="80" />
		      </span> <span class="Pg_1010">至</span> <span>
		        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" Width="80"/>
		        </span></li>
        <li style="display:none;"> <abbr class="formselect">
          <Hi:RoleDropDownList ID="dropRolesList" runat="server" SystemAdmin="true" NullToDisplay="全部" CssClass="forminput"  />
        </abbr> </li>
       
        <li>
          <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton"/>
        </li>
      </ul>
    </div>

    <UI:Grid ID="grdManager" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="UserId" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="CreateDate" SortOrder="ASC" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" ItemStyle-Width="180px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <%# Eval("UserName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="发货金额(按供货价即成本价)" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# ((decimal)Eval("price")).ToString("f2") %>
                                <a style="cursor:pointer;" onclick=<%# "showWindow_Page(" + Eval("userid") + ")" %> >明细</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>                
     
      <div class="blank5 clearfix"></div>
    </div>

</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function showWindow_Page(auto) {
        var currentDate = new Date();
        DialogFrame("../admin/Cpage/Supplier/Supplier_Admin_ShipOrdersPriceTjForSupplierMingXi.aspx?userid=" + auto + "&t=" + currentDate.getTime(), "发货明细", null, null);
    }
</script>
</asp:Content>
