<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<li> <div class="brandimg">
    <a href='<%# Globals.GetSiteUrls().SubBrandDetails(Convert.ToInt32(Eval("BrandId")), Eval("RewriteName"))%>' runat="server">
    <Hi:HiImage runat="server" DataField="Logo" /></a>
</div>
<div><a href='<%# Globals.GetSiteUrls().SubBrandDetails(Convert.ToInt32(Eval("BrandId")), Eval("RewriteName"))%>' runat="server">
     <asp:Label ID="lblBrandName" runat="server" Text='<%# Bind("BrandName") %>' ></asp:Label></a>
</div>
</li>