<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Supplier_ShipPointEditRegion.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_ShipPointEditRegion" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnleft clearfix" style="display: none;">
            <ul>
                <li><a href="Supplier_SupManage.aspx"><span>区域发货点管理</span></a></li>
            </ul>
        </div>
        <div class="columnright">
            <div class="title title_height">
                <em>
                    <img src="../../images/04.gif" width="32" height="32" /></em>
                <h1 class="title_line">
                    编辑区域发货点信息</h1>
            </div>
            <div class="formtab Pg_45">
                <ul>
                    <li><a href='<%="Supplier_ShipPointEdit.aspx?userId="+Page.Request.QueryString["userId"] %>'>
                        基本信息</a></li>
                    <li class="visited">发货点区域</li>
                    <li><a href='<%="Supplier_ShipPointEditPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>
                        修改密码</a></li>
                </ul>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_100">用户名：</span> <strong class="colorG">
                        <asp:Literal ID="lblLoginNameValue" runat="server" /></strong> </li>
                    <li><span class="formitemtitle Pw_110">区域选择：</span>
                        <Hi:RegionSelector runat="server" ID="rsddlRegion" />
                        &nbsp;
                        <asp:Button CssClass="submit_btnbianji" runat="server" Text="添加区域" Height="20px"
                            ID="btn_addRegion"></asp:Button>
                    </li>
                    <li><span class="formitemtitle Pw_110">所在区域：</span>
                        <table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="border-collapse: collapse;">
                            <asp:Repeater ID="dlstRegion" runat="server">
                                <ItemTemplate>
                                    <tr style="height: 18px; line-height: 18px;">
                                        <td style="padding: 5px;">
                                            <strong>省份：</strong><%# Eval("Province") %>
                                        </td>
                                        <td style="padding: 5px;">
                                            <strong>市：</strong><%# Eval("City")%>
                                        </td>
                                        <td style="padding: 5px;">
                                            <strong>区：</strong><%# Eval("Area")%>
                                        </td>
                                        <td style="padding: 5px;">
                                            <asp:LinkButton ID="BtnDel" runat="server" OnCommand="BtnDel_Click" CommandName='<%#DataBinder.Eval(Container, "DataItem.Id") %>'>删除</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="databottom">
        <div class="databottom_bg">
        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
