<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server"><em>&nbsp;</em>
购物车<b><asp:Literal ID="cartNum" runat="server" Text="0" /></b> 件<b style="display:none">,共计
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" />元 
去结算</Hi:SiteUrl></b>
 