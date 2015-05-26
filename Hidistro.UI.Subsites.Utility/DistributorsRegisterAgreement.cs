using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorsRegisterAgreement : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litRequestInstruction;
		private System.Web.UI.WebControls.Literal litRequestProtocols;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorsRegisterAgreement.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litRequestInstruction = (System.Web.UI.WebControls.Literal)this.FindControl("litRequestInstruction");
			this.litRequestProtocols = (System.Web.UI.WebControls.Literal)this.FindControl("litRequestProtocols");
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
				this.litRequestInstruction.Text = siteSettings.DistributorRequestInstruction;
				this.litRequestProtocols.Text = siteSettings.DistributorRequestProtocols;
			}
		}
	}
}
