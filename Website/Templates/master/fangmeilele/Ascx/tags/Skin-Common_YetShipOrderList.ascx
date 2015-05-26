<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<span>订单号：<%# Eval("OrderId")%></span> 
 
<span>发货单号：<asp:Literal ID="Literal1" runat="server" Text='<%# Eval("ShipOrderNumber") %>'>'></asp:Literal></span>
 
<span>配送方式：<asp:Literal ID="Literal2" runat="server" Text='<%# Eval("RealModeName") %>'></asp:Literal></span>&nbsp;&nbsp; 