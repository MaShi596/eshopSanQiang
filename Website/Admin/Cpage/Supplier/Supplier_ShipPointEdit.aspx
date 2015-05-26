<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Supplier_ShipPointEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Supplier_ShipPointEdit" %>
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
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑区域发货点信息</h1>
          </div>
          <div class="formtab Pg_45">
            <ul>
                <li class="visited">基本信息</li>  
                <li><a href='<%="Supplier_ShipPointEditRegion.aspx?userId="+Page.Request.QueryString["userId"] %>'>发货点区域</a></li>                                    
               <li><a href='<%="Supplier_ShipPointEditPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>修改密码</a></li>             
            </ul>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100">用户名：</span>
          <strong class="colorG"><asp:Literal ID="lblLoginNameValue" runat="server" /></strong></li>
          <li style="display:none;"> <span class="formitemtitle Pw_100">所属部门：<em >*</em></span><abbr class="formselect">
           <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" />
          </abbr></li>
          <li style="display:none;"> <span class="formitemtitle Pw_100">邮件地址：<em >*</em></span>
           <asp:TextBox ID="txtprivateEmail" CssClass="forminput" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtprivateEmailTip">请输入正确电子邮件，长度在1-256个字符以内</p>
          </li>
            <li><span class="formitemtitle Pw_110">联系信息：</span>
              <span style="display:block; float:left;height:150px;overflow:hidden;"><Kindeditor:KindeditorControl ID="fkRemark" runat="server" Width="500px" Height="150px"/></span>
            </li>
          <li style="display:none;"> <span class="formitemtitle Pw_100">注册日期：</span><Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" /> </li>
          <li style="display:none;"> <span class="formitemtitle Pw_100">最后登录日期：</span>
            <Hi:FormatedTimeLabel ID="lblLastLoginTimeValue" runat="server" />
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
      <ul class="btn Pa_100">
        <asp:Button ID="btnEditProfile" runat="server" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding" Text="保 存" />
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

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入正确电子邮件，长度在1-256个字符以内'));
        }
        
        function link()
        {
            window.location.href='Managers.aspx';
        }
			        
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

