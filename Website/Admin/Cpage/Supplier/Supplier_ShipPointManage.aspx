<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_ShipPointManage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_ShipPointManage" %>
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
          <h1><strong>区域发货点管理</strong></h1>
          <span>区域发货点可以对商城的发货单进行发货</span>
		</div>
    
    <!--数据列表区域-->
    <div class="datalist">
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li> <span>用户名：</span> <span>
          <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
        </span> </li>
        <li style="display:none;"> <abbr class="formselect">
          <Hi:RoleDropDownList ID="dropRolesList" runat="server" SystemAdmin="true" NullToDisplay="全部" CssClass="forminput"  />
        </abbr> </li>
       
        <li>
          <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="searchbutton"/>
        </li>
      </ul>
    </div>
    <div class="functionHandleArea clearfix">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li><a href="Supplier_ShipPointAdd.aspx" class="submit_jia">添加区域发货点</a></li>
        </ul>
      </div>
      <div class="pageNumber">
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
        </div></div>
      <!--结束-->
    </div>
    <UI:Grid ID="grdManager" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="UserId" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="CreateDate" SortOrder="ASC" Width="100%">
                    <Columns>
                       
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" ItemStyle-Width="180px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label> 
                                    <div style="color:#5A5E65;"><Hi:FormatedTimeLabel ID="lblCreateDate" Time='<%# Bind("CreateDate") %>' runat="server"></Hi:FormatedTimeLabel></div>     
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="100" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# "Supplier_ShipPointEdit.aspx?UserId=" + Eval("UserId") %> '>编辑</a></span>
		                         <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                             </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="联系信息" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <%# Eval("Supplier_RegionName") %>
                                <div style="color:#5A5E65;"><%# Eval("comment") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
  
                    </Columns>
                </UI:Grid>                
     
      <div class="blank5 clearfix"></div>
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
