<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_Admin_PricetemplateEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_Admin_PricetemplateEdit" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../../images/04.gif" width="32" height="32" /></em>
            <h1>编辑供应商价格模板</h1>
            <span>设定不同级别供应商商品的供货参数</span>
          </div>
      <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_120">供应商等级名称：<em >*</em></span>
            <asp:TextBox ID="txtRankName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtRankNameTip">供应商等级名称不能为空，长度限制在20字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_120">商品一口价：<em >*</em></span>
             <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="70">供货价  ×</td>
                            <td width="39"><asp:TextBox ID="txtSalePrice" CssClass="forminput" Width="90px" runat="server"  /></td>
                            <td width="25" align="center"> %</td>
                            <td width="269" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
		    <p>根据供货价设定合适的平台一口价</p>
          </li>
          <li> <span class="formitemtitle Pw_120">商品分销商最低零售价：<em >*</em></span>
             <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="70">供货价  ×</td>
                            <td width="39"><asp:TextBox ID="txtLowestSalePrice" CssClass="forminput" Width="90px" runat="server"  /></td>
                            <td width="25" align="center"> %</td>
                            <td width="269" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
		    <p>根据供货价设定合适的平台分销商最低零售价</p>
          </li>
          <li> <span class="formitemtitle Pw_120">商品分销采购价：<em >*</em></span>
             <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="70">供货价  ×</td>
                            <td width="39"><asp:TextBox ID="txtPurchasePrice" CssClass="forminput" Width="90px" runat="server"  /></td>
                            <td width="25" align="center"> %</td>
                            <td width="269" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
		    <p>根据供货价设定合适的平台分销采购价</p>
          </li>
          <li><span class="formitemtitle Pw_120">备注：</span>
            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine"  CssClass="forminput" Width="450" Height="120"></asp:TextBox>
            <p id="ctl00_contentHolder_txtRemark"></p>
          </li>
      </ul>
      <ul class="btn Pa_120">
        <asp:Button ID="btnSubmitMemberRanks" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtRankName', 1, 60, false, null, '供应商等级名称不能为空，长度限制在60个字符以内'));
                //initValid(new InputValidator('ctl00_contentHolder_txtPoint', 1, 10, false, '-?[0-9]\\d*', '设置供应商的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                //appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPoint', 0, 2147483647, '设置供应商的积分达到多少分以后自动升级到此等级，为大于等于0的整数'));
                //initValid(new InputValidator('ctl00_contentHolder_txtValue', 1, 10, false, '-?[0-9]\\d*', '等级折扣为不能为空，且是整数'));
                //appendValid(new NumberRangeValidator('ctl00_contentHolder_txtValue', 1, 100, '等级折扣必须在1-100之间'));
                initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 100, true, null, '备注的长度限制在100个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>