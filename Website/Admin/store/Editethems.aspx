
<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Editethems.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Editethems" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  <div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>您正在使用“<a href="ManageThemes.aspx"><asp:Literal ID="litThemeName" runat="server" /></a>”模板</h1>
		<span>店铺的页面风格，好比实体店面的装修，您可以从以下列表中选择您喜欢的风格</span>
    </div>

	<div class="datafrom">
      <div class="Template">
        <h1>可编辑的模板页面</h1>
            <ul class="templateedit">
                      <li class="index">
                        <a title="编辑首页" href="../../Desig_Templete.aspx?skintemp=default" target="_blank"><smp></smp></a>
                      </li>  
                      <li class="login">
                        <a title="编辑登陆" href="../../Desig_Templete.aspx?skintemp=login" target="_blank"><smp></smp></a>
                      </li> 
                      <li class="brand">
                        <a title="编辑品牌" href="../../Desig_Templete.aspx?skintemp=brand" target="_blank"><smp></smp></a>
                      </li> 
                      <li class="branddetail">
                        <a title="编辑品牌详细" href="../../Desig_Templete.aspx?skintemp=branddetail" target="_blank"><smp></smp></a>
                      </li>  
                      <li class="product">
                        <a title="编辑商品列表" href="../../Desig_Templete.aspx?skintemp=product" target="_blank"><smp></smp></a>
                      </li>  
                      <li class="productdetail">
                        <a title="编辑商品详细" href="../../Desig_Templete.aspx?skintemp=productdetail" target="_blank"><smp></smp></a>
                      </li>  
                     <li class="article">
                        <a title="编辑文章列表" href="../../Desig_Templete.aspx?skintemp=article" target="_blank"><smp></smp></a>
                      </li> 
                     <li class="articledetail">
                        <a title="编辑文章详细列表" href="../../Desig_Templete.aspx?skintemp=articledetail" target="_blank"><smp></smp></a>
                      </li>  
                     <li class="countdown">
                        <a title="编辑限时抢购列表" href="../../Desig_Templete.aspx?skintemp=cuountdown" target="_blank"><smp></smp></a>
                      </li>   
                     <li class="countdowndetail">
                        <a title="编辑限时抢购详细" href="../../Desig_Templete.aspx?skintemp=cuountdowndetail" target="_blank"><smp></smp></a>
                      </li>  
                     <li class="group">
                        <a title="编辑团购列表" href="../../Desig_Templete.aspx?skintemp=groupbuy" target="_blank"><smp></smp></a>
                      </li>   
                     <li class="groupdetail">
                        <a title="编辑团购详细" href="../../Desig_Templete.aspx?skintemp=groupbuydetail" target="_blank"><smp></smp></a>
                      </li>    
                     <li class="helplist">
                        <a title="编辑帮助中心列表" href="../../Desig_Templete.aspx?skintemp=help" target="_blank"><smp></smp></a>
                      </li>  
                      <li class="helpdetail">
                        <a title="编辑帮助中心详细" href="../../Desig_Templete.aspx?skintemp=helpdetail" target="_blank"><smp></smp></a>
                      </li>      
                      <li class="gift">
                        <a title="编辑礼品列表" href="../../Desig_Templete.aspx?skintemp=gift" target="_blank"><smp></smp></a>
                      </li>       
                      <li class="giftdetail">
                        <a title="编辑礼品详细" href="../../Desig_Templete.aspx?skintemp=giftdetail" target="_blank"><smp></smp></a>
                      </li> 
                      <li class="shopcart">
                        <a title="编辑购物车" href="../../Desig_Templete.aspx?skintemp=shopcart" target="_blank"><smp></smp></a>
                      </li>                                                                                                                                                                                                   
                 </ul> 
	   
       </div>

	</div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>


