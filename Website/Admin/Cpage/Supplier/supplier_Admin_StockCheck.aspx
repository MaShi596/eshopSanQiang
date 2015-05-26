<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="supplier_Admin_StockCheck.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Cpage.Supplier.supplier_Admin_StockCheck" %>
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
          <h1><strong>进销记录</strong></h1>
<%--          <span>供应商可以对商城进行供货以及对发货单进行发货</span>--%>
		</div>
    
    <!--数据列表区域-->
    <div class="datalist">
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li><span>日期：</span> <span style="height:25px; margin-bottom:5px;"><asp:RadioButtonList style="line-height:25px;border-style:none;" Width="280px" Height="25px" ID="rdo_ListDate" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem Value="1">今天</asp:ListItem>
        <asp:ListItem Value="7">近七天</asp:ListItem>
        <asp:ListItem Value="30">近30天</asp:ListItem>
        <asp:ListItem Value="3">三个月</asp:ListItem>
        </asp:RadioButtonList> </span>
        
        <span>&nbsp;入库单号：</span> <span>
          <asp:TextBox ID="txtStockCode" runat="server" CssClass="forminput"  />
        </span> </li>
        <li style="display:none;"> <abbr class="formselect">
          <Hi:RoleDropDownList ID="dropRolesList" runat="server" SystemAdmin="true" NullToDisplay="全部" CssClass="forminput"  />
        </abbr> </li>
      </ul>
      <ul>
        <li>
       <span>日期：</span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" /> &nbsp;至&nbsp; </span><span><UI:WebCalendar CalendarType="StartDate" ID="calendarEndDate" runat="server" CssClass="forminput" /></span>
       <span>&nbsp;进销类型：</span>
       <asp:DropDownList ID="ddl_status" runat="server">
       <asp:ListItem Value="">全部</asp:ListItem>
          <asp:ListItem Value="1">入库</asp:ListItem>
          <asp:ListItem Value="2">出库</asp:ListItem>
          </asp:DropDownList>
       </li>
        <li>
          <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton"/>
        </li>
      </ul>
    </div>
    <div class="functionHandleArea clearfix">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li><a href="Supplier_Admin_StockAdd.aspx" class="submit_jia">添加入库单</a></li>
        </ul>
      </div>
      <div class="pageNumber">
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
        </div></div>
      <!--结束-->
    </div>
    <UI:Grid ID="grdManager" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="Stock_Id" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="AddDate" SortOrder="ASC" Width="100%">
                    <Columns>
                                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="180" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# "Supplier_Admin_StockEdit.aspx?StockId=" + Eval("Stock_Id")+"&Status="+Eval("Status") %> '>详细</a></span>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="业务日期" ItemStyle-Width="180px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                               <%# DateTime.Parse(Eval("AddDate").ToString()).ToString("yyyy-MM-dd")%>                          
                                    </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="进销存类型" ItemStyle-Width="180px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <%# Eval("StatusName")%>  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="入库单号" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Stock_Code") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="总量" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("AllCount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Options")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>                
     
      <div class="blank5 clearfix"></div>
      <!--转移供应商--->
    </div>
    <!--数据列表底部功能区域-->
    <div class="bottomPageNumber clearfix">
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
      </div>
    </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
