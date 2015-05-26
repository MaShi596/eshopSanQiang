using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RegisterAgreement : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litlAgreemen;
		protected override void AttachChildControls()
		{
			this.litlAgreemen = (System.Web.UI.WebControls.Literal)this.FindControl("litlAgreemen");
			if (!this.Page.IsPostBack && this.litlAgreemen != null)
			{
				this.litlAgreemen.Text = Hidistro.Membership.Context.HiContext.Current.SiteSettings.RegisterAgreement;
			}
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RegisterAgreement.html";
			}
			base.OnInit(eventArgs_0);
		}
	}
}
