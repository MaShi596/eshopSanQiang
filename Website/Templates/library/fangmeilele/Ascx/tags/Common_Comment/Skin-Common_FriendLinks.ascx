<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<a href='<%#Eval("LinkUrl") %>' target="_blank"><%#(string.IsNullOrEmpty(DataBinder.Eval(Container,"DataItem.ImageUrl").ToString())) ? (Eval("Title")):""%></a>
<span><a href='<%#Eval("LinkUrl") %>' target="_blank"><Hi:HiImage ID="Common_Image1" runat="server" DataField="ImageUrl"  /></a></span>
