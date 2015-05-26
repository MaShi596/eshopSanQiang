<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<li>
<a href='<%# Globals.GetSiteUrls().SubBrandDetails(Convert.ToInt32(Eval("BrandId")), Eval("RewriteName"))%>' runat="server"><Hi:HiImage runat="server"  width="108"   DataField="Logo" /></a>
</li>