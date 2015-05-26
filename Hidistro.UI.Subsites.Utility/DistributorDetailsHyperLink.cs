using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorDetailsHyperLink : System.Web.UI.WebControls.HyperLink
	{
		public object Title
		{
			get
			{
				object result;
				if (this.ViewState["Title"] != null)
				{
					result = this.ViewState["Title"];
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["Title"] = value;
				}
			}
		}
		public object DetailsId
		{
			get
			{
				object result;
				if (this.ViewState["DetailsId"] != null)
				{
					result = this.ViewState["DetailsId"];
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["DetailsId"] = value;
				}
			}
		}
		public string DetailsPageUrl
		{
			get
			{
				string result;
				if (this.ViewState["DetailsPageUrl"] != null)
				{
					result = this.ViewState["DetailsPageUrl"].ToString();
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["DetailsPageUrl"] = value;
				}
			}
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (this.DetailsId != null && this.DetailsId != System.DBNull.Value)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				base.NavigateUrl = string.Concat(new object[]
				{
					"http://",
					siteSettings.SiteUrl,
					Globals.ApplicationPath,
					this.DetailsPageUrl,
					this.DetailsId
				});
			}
			if (this.DetailsId != null && this.DetailsId != System.DBNull.Value)
			{
				base.Text = this.Title.ToString();
			}
			base.Target = "_blank";
			base.Render(writer);
		}
	}
}
