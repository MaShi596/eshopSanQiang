<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<Hi:SiteUrl ID="SiteUrl1" UrlName="shoppingCart" Target="_blank" runat="server"><em>&nbsp;</em>
���ﳵ<b><asp:Literal ID="cartNum" runat="server" Text="0" /></b> ��<b style="display:none">,����
<Hi:FormatedMoneyLabel ID="cartMoney" NullToDisplay="0.00" runat="server" />Ԫ 
ȥ����</Hi:SiteUrl></b>
 