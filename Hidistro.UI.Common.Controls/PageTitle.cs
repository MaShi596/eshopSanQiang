using Hidistro.Membership.Context;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(false), PersistChildren(true)]
	public class PageTitle : Control
	{
		private const string titleKey = "Hishop.Title.Value";
		public static void AddTitle(string title, HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Items["Hishop.Title.Value"] = title;
		}
		public static void AddSiteNameTitle(string title, HttpContext context)
		{
			PageTitle.AddTitle(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", new object[]
			{
				title,
				HiContext.Current.SiteSettings.SiteName
			}), context);
		}
		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.Context.Items["Hishop.Title.Value"] as string;
			if (string.IsNullOrEmpty(text))
			{
				text = HiContext.Current.SiteSettings.SiteName;
			}
			writer.WriteLine("<title>{0}</title>", text);
		}
	}
}
