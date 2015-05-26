<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
<div style="text-align:left;"><a href='<%# Globals.GetSiteUrls().SubBrandDetails(Convert.ToInt32(Eval("BrandId")), Eval("RewriteName"))%>' runat="server">
     <asp:Label ID="lblBrandName" runat="server" Text='<%# Bind("BrandName") %>' ></asp:Label></a>
</div>
</li>
