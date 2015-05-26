using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OnlineServer : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Comment/Skin-Common_OnlineServer.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			Literal literal = (Literal)this.FindControl("litOnlineServer");
			literal.Text = HiContext.Current.SiteSettings.HtmlOnlineServiceCode;
		}
	}
}
