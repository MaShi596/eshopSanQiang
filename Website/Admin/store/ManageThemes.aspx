﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageThemes.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageThemes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  <div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>您正在使用“<a href="ManageThemes.aspx"><asp:Literal ID="litThemeName" runat="server"></asp:Literal></a>”模板</h1>
		<span>店铺的页面风格，好比实体店面的装修，您可以从以下列表中选择您喜欢的风格</span>
    </div>
    <div class="Tempimg">
      <table width="98%" border="0" cellspacing="0">
        <tr>
          <td width="25%" rowspan="4"><asp:Image runat="server" ID="imgThemeImgUrl" width="235" height="145" /></td>
          <td width="2%" rowspan="4">&nbsp;</td>
          <td width="54%" rowspan="4" align="right"><asp:Image runat="server" ID="Image1" width="510" height="145" /></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <div class="blank12 clearfix"></div>
	<div class="datafrom">
      <div class="Template">
        <h1>可供您选择的模板（总数为：<asp:Literal ID="lblThemeCount" runat="server"  />）</h1>
          <asp:DataList runat="server" ID="dtManageThemes" RepeatColumns="3" DataKeyField="ThemeName"  RepeatDirection="Horizontal">                                   
                <ItemTemplate>
                  <ul>
                      <li>
                        <span><Hi:DisplayThemesImages ID="themeImg" runat="server" Src='<%#  Eval("ThemeImgUrl") %>' ThemeName='<%# Eval("ThemeName") %>' /></span>
                        <em>
                          <p><%# Eval("Name") %></p>
                          <asp:LinkButton ID="btnManageThemesOK" runat="server" CommandName="btnUse"  Text="应用"/>
                          <!--<asp:LinkButton ID="btnDownload" runat="server" CommandName="download"  Text="下载"/>-->
                          <a href="javascript:ShowUpload('<%# Eval("ThemeName") %>');">上传</a>
                          <asp:LinkButton ID="btnback" runat="server" style="display:inline;" CommandName="back"  Text="恢复" OnClientClick="javascript:return confirm('恢复后，模板被还原到初始模板，您确定仍需要恢复吗？');"/>
                        </em>
                      </li>                                                                                                           
                 </ul>
                </ItemTemplate>
            </asp:DataList>   
	   </div>

	</div>
</div>
  <%--<div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>--%>
  
  <div class="Pop_up" id="div_templeteupload" style="display: none;">
    <h1>
        上传模板
    </h1>
    <div class="img_datala">
        <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
    <div class="mianform" style="text-align:center;">
    <p>上传模板文件：<asp:FileUpload ID="fileTemplate" runat="server" />
           <br /><em style="font-style:normal;">注意:上传文件必须为zip格式，并将会覆盖原来模板</em></p>
                <p> <asp:Button ID="btnUpload2" runat="server" Text="上传" CssClass="submit_DAqueding" OnClientClick="javascript:return CheckUpload('fileTemplate');"/></p>
                <input type="hidden" id="hdtempname" runat="server" />
    </div>
</div> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
function ShowUpload(themname){
    if(themname.replace(/\s/g,"").length<=0){
        alert("请选择要上传的模板");
        return false;
    }
    $("#ctl00_contentHolder_hdtempname").val(themname);
    DivWindowOpen(400,180,'div_templeteupload');
}

function CheckUpload(filecontrol){
    if($("#ctl00_contentHolder_"+filecontrol).val().replace(/\s/g,"").length<=0){
        alert("请选择要上传的文件");
        return false;
    }
}
</script>
</asp:Content>

