<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_Pricetemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_Pricetemplate" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../../images/04.gif" width="32" height="32" /></em>
	    <h1>供应商价格模板管理</h1>
<span>&nbsp;</span></div>
	  <!-- 添加按钮-->
	
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
    <UI:Grid ID="grdMemberRankList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="Auto" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="模板名称" ItemStyle-Width="200px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%#Eval("Name") %>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "Supplier_Admin_PricetemplateEdit.aspx?GradeId="+Eval("auto")%>' ></asp:HyperLink></span>
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品一口价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("SalePrice")).ToString("f2") %>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品分销商采购价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("PurchasePrice")).ToString("f2")%>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="商品分销商最低零售价" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           供货价*<%# ((decimal)Eval("LowestSalePrice")).ToString("f2")%>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <%#Eval("remark") %>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>	   
    </div>
  </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
</asp:Content>