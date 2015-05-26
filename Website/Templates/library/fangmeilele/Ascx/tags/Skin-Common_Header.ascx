<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle runat="server" />
    <Hi:MetaTags runat="server" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/globals.js" />
    <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
</head>
<script type="text/javascript">
$(function(){
		   $('.category_list ul li:last').css(' border-bottom','none');
		   })
</script>
<body ><Hi:Common_OnlineServer ID="Common_OnlineServer1" runat="server"></Hi:Common_OnlineServer>
    <div id="header">
        <div class="top-nav">
            <div class="w1200">
                    <div class="top-phone float-r"><Hi:Common_ImageAd runat="server" AdId="2" /></div>
                    <div class="jiathis_style float-r">
                           <Hi:Common_CustomAd runat="server" AdId="3" />
                     </div>
                    <div class="top-right float-r">
                        <span>
                            <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />
                        </span> 
                        <span>
                            [<Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" />]
                        </span> 
                        <span>
                            [<Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />]
                        </span>
                        <span>|</span>
                        <span id="xinren_Frame">
                            <em onmouseover="Showxinren_tab();" onmouseout="Hiddenxinren_tab();">信任登录</em>  
                            <div id="xinren_tab" onmouseover="Showxinren_tab();" onmouseout="Hiddenxinren_tab();">
                            <div class="xinren_tab_tishi"> 您还可以使用以下账号</div>
                                <Hi:Common_Link_OpenId ID="Common_Link_OpenId1" runat="server"  /> 
                            </div>
                        </span>
                        <span>|</span>
                        <span><Hi:Common_CustomAd runat="server" AdId="1" /></span>
                        <span>|</span>
                        <span>
                            <Hi:SiteUrl ID="SiteUrl4" UrlName="user_UserOrders" runat="server">订单查询</Hi:SiteUrl>
                        </span>
                        <span>|</span>
                        <span>
                            <Hi:SiteUrl   UrlName="AllHelps" runat="server">帮助中心</Hi:SiteUrl>
                        </span>
                        <span>|</span> 
                        <span>关注我们:</span>
                    </div>
            </div>
        </div>
        <div class="w1200 pt20 pb20 o-hidden c-both">
            <div class="logo float-l">
                <Hi:Common_Logo ID="Common_Logo1" runat="server" />
            </div>
            <div class="float-l">
                <div class="search">
                    <Hi:Common_Search SkinName="/ascx/tags/Common_Search/Skin-Common_Search.ascx" ID="Common_Search" runat="server" />
                </div>
                <div class="hot-key">
                    <em>热门关键字：</em>
                    <Hi:Common_SubjectKeyword ID="Common_SubjectKeyword1" runat="server" CommentId="1" />
                </div>
            </div>
            <div class="buycart float-r">
                <Hi:Common_ShoppingCart_Info ID="Common_ShoppingCart_Info1" runat="server" />
            </div>
            <div class="buycart myuser float-r">
               <em></em> <a href="#">我的帐户</a>
            </div>
        </div>
        <div class="nav1">
            <div class="nav w1200 c-both">
                <div class="nav-l" >
                      <div class="all float-l">
                          <h2> <Hi:SiteUrl ID="SiteUrl5" UrlName="Category" runat="server">全部商品分类</Hi:SiteUrl></h2>
                          <div class="my_left_category float-l">
                              <Hi:Common_CategoriesWithWindow ID="Common_CategoriesWithWindow1" MaxCNum="7" runat="server" />
                          </div>
                      </div>
                      <div class="nav2 float-l" id="ty_menu_title">
                           <ul>
                              <li class="home"><a href="/"><span>首页</span></a></li>
                              <Hi:Common_PrimaryClass ID="Common_PrimaryClass1" runat="server" />
                              <Hi:Common_HeaderMune ID="Common_HeaderMune1" runat="server" />
                          </ul>
                      </div> 
                      <div class="custom1 float-r"><Hi:Common_CustomAd runat="server" AdId="4" /></div>  
                </div>
        </div>
        
        </div>
        
        
    </div>
    <!--my_left_category-->
    <script>
      var currenturl = location.href.substr(location.href.lastIndexOf('/') + 1, location.href.length - 20);
        if (currenturl != "" && currenturl != null && currenturl != "Default.aspx" && currenturl !=         "Desig_Templete.aspx?skintemp=default") {
                        $(".all").hover(function () {
                            $(".my_left_category").css({ 'display': 'block' });
                        }, function () {
                            $(".my_left_category").css({ 'display': 'none' });
                        });
                    } else {
                        $(".my_left_category").css({ 'display': 'block' });
                    }
    </script> 
    
    <!--xinren-->
    <script type="text/javascript">
			 function Showxinren_tab() {
				 document.getElementById("xinren_tab").style.display = "block";
			 }
	  
			 function Hiddenxinren_tab() {
				 document.getElementById("xinren_tab").style.display = "none";
			 }
	</script>
    <!--fenxiang-->
    <script type="text/javascript" src="http://v3.jiathis.com/code/jia.js?uid=1350294149446525" charset="utf-8"></script>