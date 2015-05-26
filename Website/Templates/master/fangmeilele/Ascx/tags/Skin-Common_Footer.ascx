<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>


<div class="o-hidden" id="footer" style=" padding-top:2px;">
 <div class="fm_footer">
 <div class="footer_1">
    <div class="footer-help  o-hidden" style=" height:220px; overflow:hidden;">
      <span class="footer_ad1">  <Hi:Common_CustomAd runat="server" AdId="19" /> </span>
          <ul class="o-hidden">
              <Hi:Common_Help runat="server" TemplateFile="/ascx/tags/Common_Comment/Skin-Common_Help.ascx" />
          </ul> 
          <span class="footer_ad2">  <Hi:Common_ImageAd runat="server" AdId="20" /></span>      
    </div>
    
    <div class="footer_ad7"> <Hi:Common_ImageAd runat="server" AdId="8" /></div>
    <div class="footer-custom pt20">
            <Hi:PageFooter ID="PageFooter1" runat="server" />
             <Hi:CnzzShow ID="CnzzShow1" runat="server" />
    </div>
</div></div></div>

</body>
</html> 