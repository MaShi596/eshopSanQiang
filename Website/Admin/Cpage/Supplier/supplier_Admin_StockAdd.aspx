<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="supplier_Admin_StockAdd.aspx.cs" Inherits="Hidistro.UI.Web.Admin.supplier_Admin_StockAdd" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<script type="text/jscript">
    $(document).ready(function () {
        $('input[id*="txtStockCode_Scan"]').keydown(function (e) {
            if (e.keyCode == 13) {
                ShowAddDiv();
                return false;
            }
        });
    })

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../../images/04.gif" width="32" height="32" /></em>
                <h1>
                    入库操作</h1>
                <span>&nbsp;</span>
            </div>
            <div class="formitem validator4">
                <div class="title">
                    <h1 class="colorE">
                        入库基本信息</h1>
                    <span>&nbsp;</span>
                </div>
                <div>
                    <div class="formitem validator5">
                        <ul>
                            <li><span class="formitemtitle Pw_140">单据日期：<em>*</em></span>
                                <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                                <p id="ctl00_contentHolder_StartDateTip">
                                    默认当前日期</p>
                            </li>
                            <li><span class="formitemtitle Pw_140">入库单号：<em>*</em></span>
                                <asp:TextBox ID="txtStockCode" CssClass="forminput" runat="server"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtStockCodeTip">
                                    默认：日期+用户ID+随机数生成唯一单号</p>
                            </li>
                            <%--          <li>
          <asp:DropDownList ID="ddl_status" runat="server">
          <asp:ListItem Value="1">入库</asp:ListItem>
          </asp:DropDownList>
          </li>--%>
                            <li><span class="formitemtitle Pw_140">备注：</span>
                                <asp:TextBox ID="txt_Options" Width="300px" TextMode="MultiLine" runat="server" CssClass="forminput"></asp:TextBox>
                                <p id="ctl00_contentHolder_txt_OptionsTip">
                                    备注200个字符以内</p>
                            </li>
                        </ul>
                    </div>
                    <%--<table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="margin-top: 20px;
                        border-collapse: collapse; width: 100%;">
                        <tr style="height: 30px; line-height: 30px;">
                            <td width="10%">
                                单据日期：
                            </td>
                            <td width="20%">
                                <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="" /><em>*默认当前日期</em>
                            </td>
                            <td>
                                入库单号：<asp:TextBox ID="txtStockCode" CssClass="" runat="server"></asp:TextBox><em>默认：日期+用户ID+随机数生成唯一单号</em>
                            </td>
                        </tr>
                        <tr style="height: 30px; line-height: 30px;">
                            <td>
                                备注：
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_Options" Width="300px" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>--%>
                </div>
                <div style="display: none;">
                    <asp:Button ID="btn_Submits" runat="server" Text="确认提交" CssClass="submit_DAqueding" />
                </div>
                <div class="title">
                    <h1 class="colorE">
                        入库商品信息</h1>
                    <span>&nbsp;</span>
                </div>
                <div style="width: 890px;">
                    <input id="selectProductsinfo" name="selectProductsinfo" type="hidden" />
                    <input id="selectAllNums" name="selectAllNums" type="hidden" value="0"/>
                    <table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="margin-top: 20px;
                        border-collapse: collapse; width: 880px;">
                        <tr style="height: 18px; line-height: 18px;">
                            <td width="25%">
                                货号扫描：<input id="txtStockCode_Scan" type="text"/>
                            </td>
                            <td width="10%">
                                <div style="height: 25px; line-height: 25px;">
                                    <input type="button" id="btn_SelProjects" value="选择商品" onclick="ShowAddDiv();" />
                                    
                                    </div>
                            </td>
                            <td width="6%">
                            总入库：
                            </td>
                            <td width="30" id="d_allnums">
                            <table id="lab_Allcount" style="float:left;color:Red;"></table>
                            </td>
                            <td>件商品</td>
                            <td style="float: right; display: none;">
                                <asp:Button ID="btn_Submit_Add" runat="server" OnClientClick="return PageIsValid()&&CollectInfos();"
                                    Text="确认提交" CssClass="submit_DAqueding" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3">
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="880px" id="addlist">
                        <tr class="table_title">
                            <th class="td_right td_left" scope="col">
                                商品
                            </th>
                            <th class="td_right td_left" scope="col">
                                规格
                            </th>
                            <th class="td_right td_left" scope="col">
                                一口价
                            </th>
                            <th class="td_right td_left" scope="col">
                                库存
                            </th>
                            <th class="td_left td_right_fff" scope="col">
                                操作
                            </th>
                        </tr>
                    </table>
                    <br />
                    <div style="clear: both; float: left;">
                        <asp:Button ID="btn_Submits_Add" OnClientClick="return PageIsValid()&&CollectInfos();"
                            runat="server" Text="确认提交" CssClass="submit_DAqueding" Style="float: right;" />
                    </div>
                </div>
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtStockCode', 1, 60, false, null, '入库单号不能为空'));
        initValid(new InputValidator('ctl00_contentHolder_txt_Options', 0, 200, true, null, '入库备注必须限制在200个字符以内'));
    }
    $(document).ready(function () { InitValidators(); });

    function EnterPress(e) {
        var e = e || window.event;
        if (e.keyCode == 13) {

        }
    }

    function document.onkeydown() {
        if (event.keyCode == 13) {
    
        }
    }


    </script>
    <script type="text/javascript" src="SupplierAdmin/BundlingProduct.js"></script>
</asp:Content>
