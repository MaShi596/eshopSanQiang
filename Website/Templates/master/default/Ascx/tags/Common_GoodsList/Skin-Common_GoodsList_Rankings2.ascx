<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li id="divImage" runat="server" style="height:60px; overflow:hidden; padding:5px 0 5px 20px ;">
<table border="0" cellspacing="0" cellpadding="0" align="center" >
  <tr>
    <td width="60" height="60"  style="background:#fff;" align="center" valign="middle">	<Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
<Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl60" /> </Hi:ProductDetailsLink>
	</td>
	<td valign="top" style="padding-left:6px;">
	<div class="index_paihangbang_name02"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" StringLenth="14"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink></div>
	 
	<div class="index_paihangbang_price" ><span class="yj">￥</span><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("SalePrice") %>' /></div>
	</td>
  </tr>
</table>
</li>
<li id="divName" runat="server" class="index_paihangbang_name02" >
<Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" StringLenth="12"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
</li>

 