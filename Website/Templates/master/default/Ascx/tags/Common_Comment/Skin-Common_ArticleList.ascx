<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>    
          	<table border="0" cellspacing="0" cellpadding="0" width="100%">
              <tr>
                <td width="16" height="25" align="center" valign="middle"><Hi:HiImage ID="ThemesImage1" src="images/icon5.jpg" runat="server" /></td>
                <td nowrap="nowrap"><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails",Eval("ArticleId"))%>' target="_blank"  Title='<%#Eval("Title") %>'>
                <Hi:SubStringLabel ID="SubStringLabel" Field="Title" StrLength="10" runat="server"  />
                 </a></td>
              </tr>
            </table>