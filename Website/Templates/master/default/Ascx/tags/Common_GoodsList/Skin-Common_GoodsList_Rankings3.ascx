<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li id="divImage" runat="server">
	  <div class="rank2image">
	    <Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl40" /> 
        </Hi:ProductDetailsLink>
  </div> 
	  <div class="rank2right">
	    <div class="rank2name">
	      <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" StringLenth="10"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
	      </div>
	    <div class="pro3">
	<b>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("SalePrice") %>' runat="server" /></b>
	￥<p><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></p>
</div>
  </div>
</li>
<li id="divName" runat="server" class="index_paihangbang_name02" >
	<Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" StringLenth="10"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
</li>

 