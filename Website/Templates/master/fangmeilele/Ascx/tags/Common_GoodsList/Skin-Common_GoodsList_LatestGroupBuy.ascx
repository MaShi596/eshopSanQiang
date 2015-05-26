<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Repeater ID="regroupbuy" runat="server">
<ItemTemplate>
<li>

              <p class="tuangou_time" style="display:none"><span id='<%# "htmlspan"+Eval("ProductId") %>'></span><Hi:LeaveListTime runat="server" ID="LeaveListTime" /> </p>  
            <div class="pic"><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl160" /></div>
            <div class="info">
            <div class="name"><Hi:ProductDetailsLink ID="ProductDetailsLink1" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true"><%# Eval("ProductName") %></Hi:ProductDetailsLink></div>
            <div class="tuangou_yj"><span><em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="lblPrice" Money='<%# Eval("OldPrice") %>'/></span></div>
            
            <div class="tuangou_bg">
            <b>团购价：<em>￥</em><Hi:FormatedMoneyLabel runat="server" ID="FormatedMoneyLabel1" Money='<%# Eval("Price") %>' /></b>
            <Hi:ProductDetailsLink ID="ProductDetailsLink3" runat="server" IsGroupBuyProduct="true" ProductId='<%# Eval("ProductId") %>' ImageLink="true"></Hi:ProductDetailsLink></div>
            </div>

            </li>
            </ItemTemplate>
</asp:Repeater>