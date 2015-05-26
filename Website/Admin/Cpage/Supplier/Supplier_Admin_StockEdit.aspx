<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="Supplier_Admin_StockEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_StockEdit" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../../images/04.gif" width="32" height="32" /></em>
                <h1 id="h_h1" runat="server">
                    修改入库单</h1>
                <span>&nbsp;</span>
            </div>
            <div class="formitem validator4">
                <div class="title">
                    <h1 class="colorE" id="h_h2" runat="server">
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
                            <li><span class="formitemtitle Pw_140" id="h_h4" runat="server">入库单号：<em>*</em></span>
                                <asp:TextBox ID="txtStockCode" CssClass="forminput" runat="server"></asp:TextBox>
                                <p id="ctl00_contentHolder_txtStockCodeTip">
                                    默认：日期+用户ID+随机数生成唯一单号</p>
                            </li>
                            <li><span class="formitemtitle Pw_140">备注：</span>
                                <asp:TextBox ID="txt_Options" Width="300px" TextMode="MultiLine" runat="server" CssClass="forminput"></asp:TextBox>
                                <p id="ctl00_contentHolder_txt_OptionsTip">
                                    备注200个字符以内</p>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="title">
                    <h1 class="colorE" id="h_h3" runat="server">
                        入库商品信息</h1>
                    <span>&nbsp;</span>
                </div>
                <div style="width: 890px;">
                    <input id="selectProductsinfo" name="selectProductsinfo" type="hidden" />
                    <input id="selectAllNums" name="selectAllNums" type="hidden" />
                    <%--<table border="0" bordercolor="#E6E6E6" cellspacing="0px" style="margin-top: 20px;
                        border-collapse: collapse; width: 880px;">
                        <tr style="height: 18px; line-height: 18px;">
                            <td width="25%">
                                货号扫描：<asp:TextBox ID="txtStockCode_Scan" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <div style="height: 25px; line-height: 25px;">
                                    <input type="button" id="btn_SelProjects" value="选择商品" onclick="ShowAddDiv();" />
                                    总入库：<asp:Label ID="lab_Allcount" runat="server" Text=""></asp:Label>件商品</div>
                            </td>
                            <td style="float:right;display:none;">
                                <asp:Button ID="btn_Submit_Add" runat="server" OnClientClick="return PageIsValid()&&CollectInfos();" Text="确认提交" CssClass="submit_DAqueding"/>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3">
                                
                            </td>
                        </tr>
                    </table>--%>
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
                                原库存
                            </th>
                            <th class="td_right td_left" scope="col">
                                修正后库存
                            </th>
                        </tr>
                        <asp:Repeater ID="Rpbinditems" runat="server">
                            <ItemTemplate>
                                <tr name='appendlist'>
                                    <td>
                                        <div style="float:left; margin-right:10px;">
                                <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <br />
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                                    </td>
                                    <td>
                                          <Hi:SkuContentLabel runat="server" ID="litSkuContent" Text='<%#Eval("SkuId") %>' />
                                       
                                 <%--       <%# Eval("SkusName").ToString()!=""?Eval("SkusName").ToString():"无规格" %>--%>
                                    </td>
                                    <td>
                                        <%#Eval("SalePrice")%>
                                    </td>
                                    <td>
                                        <%#Eval("Stock") %>
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtUpdateStock" Text='<%#Eval("UpdateStock") %>' CssClass="forminput" Enabled='<%#(Eval("UpdateStock").ToString().Trim()==""&&(Eval("Status").ToString().Trim()=="1"))?true:false %>' runat="server"></asp:TextBox>
                                    </td>
                                    <td style='display: none'>
                                      <%#(Eval("UpdateStock").ToString().Trim()=="")?"":"no" %>
                                    </td>
                                     <td style='display: none'>
                                        <%#Eval("Id")%>|<%#Eval("skuid") %>|<%#Eval("Stock_Id") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <br />
                    <div style="clear: both; float: left;">
                        <asp:Button ID="btn_Submits_Add" OnClientClick="return PageIsValid()&&CollectInfos_Update();"
                            runat="server" Text="确认提交" CssClass="submit_DAqueding" Style="float: right;" />
                    </div>
                </div>
            </div>
            <div style="display: none;">
                <asp:Button ID="btn_Submits" runat="server" Text="确认提交" CssClass="submit_DAqueding" />
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
    <script type="text/javascript" src="SupplierAdmin/BundlingProduct.js"></script>
    <script type="text/javascript">


        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtStockCode', 1, 60, false, null, '入库单号不能为空'));
            initValid(new InputValidator('ctl00_contentHolder_txt_Options', 0, 200, true, null, '入库备注必须限制在200个字符以内'));
        }

        function onEnterDown() {
            if (window.event.keyCode == 13) {
                onButOk();
            }
        }
        $(document).ready(function () { InitValidators(); GetTotalPrice(); });
    </script>
</asp:Content>
