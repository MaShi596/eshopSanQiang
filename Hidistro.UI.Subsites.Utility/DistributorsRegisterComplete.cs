using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorsRegisterComplete : HtmlTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorsRegisterComplete.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
		}
	}
}
