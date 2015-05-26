<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_ShipPointAdd.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_ShipPointAdd" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../../images/04.gif" width="32" height="32" /></em>
            <h1>添加新的区域发货点</h1>
            <span>&nbsp;</span>
          </div>
        <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110">用户名：<em >*</em></span>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />            
            <p id="ctl00_contentHolder_txtUserNameTip">用户名不能为空，必须以汉字或是字母开头,且在3-20个字符之间</p>
          </li>
           <li> <span class="formitemtitle Pw_110">密码：<em >*</em></span>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="forminput" />
            <p id="ctl00_contentHolder_txtPasswordTip">密码长度在6-20个字符之间</p>
          </li>
           <li> <span class="formitemtitle Pw_110">确认密码：<em >*</em></span>
            <asp:TextBox ID="txtPasswordagain" runat="server" TextMode="Password" CssClass="forminput" />
            <p id="ctl00_contentHolder_txtPasswordagainTip">请重复一次上面输入的登录密码</p>
          </li>
          <li style="display:none;"> <span class="formitemtitle Pw_110">电子邮件地址：<em >*</em></span>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="forminput" Text="123@tom.com" />
            <p id="ctl00_contentHolder_txtEmailTip">请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内</p>
          </li>
          <li style="display:none;"> <span class="formitemtitle Pw_110">所属部门：<em >*</em></span><abbr class="formselect">
            <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" />
          </abbr></li>
            <li><span class="formitemtitle Pw_110">联系信息：</span>
              <span style="display:block; float:left;height:300px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fkRemark" runat="server" Width="500px" Height="300px"/></span>
            </li>
      </ul>
          <div style=" font-size:14px; font-weight:bold; clear:both;">收货人信息</div>
          <ul>
          <li> 
            <span class="formitemtitle Pw_110">收货人：<em >*</em></span>
            <asp:TextBox ID="txtRealName" CssClass="forminput" runat="server"></asp:TextBox>
          </li>
          <li> 
            <span class="formitemtitle Pw_110">所在区域：<em >*</em></span>
            <Hi:RegionSelector runat="server" ID="rsddlRegion" />
            <p>所在区域必填填写完整</p>
          </li>
          <li> 
            <span class="formitemtitle Pw_110">具体地址：<em >*</em></span>
            <asp:TextBox ID="txtAddress" CssClass="forminput" runat="server"></asp:TextBox>
          </li>
          <li> 
            <span class="formitemtitle Pw_110">邮编：<em >*</em></span>
            <asp:TextBox ID="txtZip" CssClass="forminput" runat="server"></asp:TextBox>
          </li>
          <li> 
            <span class="formitemtitle Pw_110">手机：<em >*</em></span>
            <asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server"></asp:TextBox>
          </li>
          <li> 
            <span class="formitemtitle Pw_110">电话：</span>
            <asp:TextBox ID="txtPhone" CssClass="forminput" runat="server"></asp:TextBox>
          </li>
        </ul>
      <ul class="btn Pa_110" style="clear:both;">
        <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text="添 加"  CssClass="submit_DAqueding" style="float:left;"/>
        </ul>
      </div>

      </div>
  </div>


<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtUserName', 3, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户名不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtPassword', 6, 20, false, null, '密码长度在6-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtPasswordagain', 6, 20, false, null, '请重复一次上面输入的登录密码'))
        appendValid(new CompareValidator('ctl00_contentHolder_txtPasswordagain', 'ctl00_contentHolder_txtPassword', '重复密码错误'));
        initValid(new InputValidator('ctl00_contentHolder_txtEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内'))
        initValid(new SelectValidator('ctl00_contentHolder_dropRole', false, '选择区域发货点要加入的部门'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

