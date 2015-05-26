<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<Hi:HeadContainer ID="HeadContainer1" runat="server" />
<Hi:PageTitle runat="server" />
<Hi:MetaTags runat="server" />
<Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
  <Hi:TemplateStyle ID="Stylee1" runat="server" Href="/style/style.css"></Hi:TemplateStyle>
    <Hi:TemplateStyle ID="Style1" runat="server" Href="/style/menu.css"></Hi:TemplateStyle>
</head>
<body>

<div class="top-nav">
            <div class="w1200" style=" width:1000px;">
                    <div class="top-left float-l"><Hi:Common_CustomAd runat="server" AdId="1" /></div>
                    <div class="top-phone float-r"><Hi:Common_ImageAd runat="server" AdId="2" /></div>
                    <div class="jiathis_style float-r">
                            <a class="jiathis_button_qzone"></a>
                            <a class="jiathis_button_tsina"></a>
                            <a class="jiathis_button_tqq"></a>
                     </div>
                    <div class="top-right float-r">
                        <span>
                            <Hi:Common_CurrentUser runat="server" ID="lblCurrentUser" />
                        </span> 
                        <span>
                            <Hi:Common_MyAccountLink ID="linkMyAccount" runat="server" />
                        </span> 
                        <span>
                            <Hi:Common_LoginLink ID="Common_Link_Login1" runat="server" />
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
                        <span>
                            <Hi:SiteUrl  UrlName="user_OrderDetails" runat="server">我的帐户</Hi:SiteUrl>
                        </span>
                        <span>|</span>
                        <span>
                            <Hi:SiteUrl ID="SiteUrl4" UrlName="user_UserOrders" runat="server">订单查询</Hi:SiteUrl>
                        </span>
                        <span>|</span>
                        <span>
                            <Hi:SiteUrl ID="SiteUrl2" UrlName="distributorLogin" runat="server">分销商登录</Hi:SiteUrl>
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
