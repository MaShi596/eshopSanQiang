using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SaleSyetemResourceNotFound : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litMsg;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SaleSyetemResourceNotFound.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litMsg = (System.Web.UI.WebControls.Literal)this.FindControl("litMsg");
			if (this.litMsg != null)
			{
				this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
			}
		}
	}
}
