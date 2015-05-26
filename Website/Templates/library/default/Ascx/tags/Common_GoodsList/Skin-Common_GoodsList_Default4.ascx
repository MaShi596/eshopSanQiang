<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
<div class="pro4">
	<Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true"><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl100" /> 
        </Hi:ProductDetailsLink>
</div>
<div class="pro5">
  <div class="pro6">
    <Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" StringLenth="16" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:ProductDetailsLink>
  </div>
  <div class="pro3">
    <Hi:RankPriceName ID="RankPriceName1" runat="server" PriceName="零售价" />：<b>￥<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("RankPrice") %>' runat="server" /></b><br/>
    市场价：￥<p><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></p>
	<div class="buyit">
	  <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="">马上购买 </Hi:ProductDetailsLink>
	  </div>
  </div>
</div>
</li>