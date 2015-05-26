<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<span style="margin-right:10px;"><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%> </a></span>