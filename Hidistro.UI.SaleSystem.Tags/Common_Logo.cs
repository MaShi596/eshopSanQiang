using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Logo : PlaceHolder
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.LogoUrl))
			{
				writer.Write(this.RendHtml());
			}
		}
		public string RendHtml()
		{
			string logoUrl = HiContext.Current.SiteSettings.LogoUrl;
			if (!HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				logoUrl = SettingsManager.GetMasterSettings(false).LogoUrl;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<div class=\"logo cssEdite\" type=\"logo\" id=\"logo_1\" >").AppendLine();
			stringBuilder.AppendFormat("<a href=\"{0}\">", Globals.GetSiteUrls().UrlData.FormatUrl("home")).AppendLine();
			stringBuilder.AppendFormat("<img src=\"{0}\" />", logoUrl).AppendLine();
			stringBuilder.Append("</a>").AppendLine();
			stringBuilder.Append("</div>").AppendLine();
			return stringBuilder.ToString();
		}
	}
}
