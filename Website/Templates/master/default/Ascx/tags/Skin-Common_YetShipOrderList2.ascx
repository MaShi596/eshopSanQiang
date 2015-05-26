<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="wuliu">
<li>
    <span class="cRed">订单号：</span><%# Eval("OrderId")%>
</li>
<li>
    <span class="cRed">发货单号：</span><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("ShipOrderNumber") %>'>'></asp:Literal>
</li>
<li>
    <span class="cRed">配送方式：</span><asp:Literal ID="Literal2" runat="server" Text='<%# Eval("ModeName") %>'></asp:Literal>
</li>
</div>