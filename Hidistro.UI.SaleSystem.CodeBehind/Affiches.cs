using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Affiches : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptAffiches;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Affiches.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptAffiches = (ThemedTemplatedRepeater)this.FindControl("rptAffiches");
			if (!this.Page.IsPostBack)
			{
				this.rptAffiches.DataSource = CommentBrowser.GetAfficheList();
				this.rptAffiches.DataBind();
			}
		}
	}
}
