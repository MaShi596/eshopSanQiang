<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 
 <li>
    <div class="pic"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="true">
                    <Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl60" /></Hi:ProductDetailsLink>
                </div>
    <div class="info">
    <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="false"/></div>
    <span class="colorA" style=" color:#ff6600;"><%# Eval("UserName") %> </span> </td><td><span class="colorC" style=" display:none;">评论时间：<%# Eval("ReviewDate")%></span>
    <div class="reviews"><asp:Label ID="Label2" runat="server" Text='<%# Eval("ReviewText") %>'></asp:Label></div>
    
    </div>
</li>