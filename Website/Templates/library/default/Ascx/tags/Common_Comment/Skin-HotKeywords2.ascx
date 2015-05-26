<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<span><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("keywordsSearch",Globals.UrlEncode(Eval("Keywords").ToString()), Eval("CategoryId"))%>'>
<Hi:SubStringLabel ID="SubStringLabel" Field="Keywords" runat="server"  /></a></span>
