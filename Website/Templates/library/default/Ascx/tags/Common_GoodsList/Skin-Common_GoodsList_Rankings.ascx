<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li id="divImage" runat="server" style="height:72px; overflow:hidden;">
<table border="0" cellspacing="0" cellpadding="0" width="207"  align="center" >
  <tr>
    <td width="60" height="60"  style="background:#fff;" align="center" valign="middle">	<Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
<Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl60" /> </Hi:ProductDetailsLink>
	</td>
	<td valign="top" style="padding-left:8px;">
	<div  class="index_paihangbang_name02"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" StringLenth="18"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
	 
	<div  class="index_paihangbang_price" ><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("MarketPrice") %>' /></div>
	</td>
  </tr>
</table>
</li>
<li id="divName" runat="server" class="index_paihangbang_name02" style="margin-left:10px;">
<Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" StringLenth="17"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
</li>

 