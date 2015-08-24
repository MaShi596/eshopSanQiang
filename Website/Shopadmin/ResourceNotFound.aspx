<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ResourceNotFound.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ResourceNotFound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
   <div class="datalist">
		<div class="allerror mainwidth">
			<h2>系统处理错误，可能是：</h2>
			<div>
				1、您登录已经超时。<br />
				2、URL非法。<br />
				3、重复的操作，或者当前操作不可用。<br />
				
			</div>
		</div>
	  </div>

	  <div class="blank12 clearfix"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
