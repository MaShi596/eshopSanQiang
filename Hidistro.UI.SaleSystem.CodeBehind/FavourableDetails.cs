using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FavourableDetails : HtmlTemplatedWebControl
	{
		private int activityId = 0;
		private System.Web.UI.WebControls.HyperLink hlkLink;
		private System.Web.UI.WebControls.Literal litHelpTitle;
		private System.Web.UI.WebControls.Literal litHelpDescription;
		private System.Web.UI.WebControls.Label lblFront;
		private System.Web.UI.WebControls.Label lblNext;
		private System.Web.UI.WebControls.Label lblFrontTitle;
		private System.Web.UI.WebControls.Label lblNextTitle;
		private System.Web.UI.HtmlControls.HtmlAnchor aFront;
		private System.Web.UI.HtmlControls.HtmlAnchor aNext;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-FavourableDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
			}
			this.hlkLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkLink");
			this.litHelpDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litHelpDescription");
			this.litHelpTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litHelpTitle");
			this.lblFront = (System.Web.UI.WebControls.Label)this.FindControl("lblFront");
			this.lblNext = (System.Web.UI.WebControls.Label)this.FindControl("lblNext");
			this.lblFrontTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblNextTitle");
			this.aFront = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("front");
			this.aNext = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("next");
			if (!this.Page.IsPostBack && this.activityId > 0)
			{
				PromotionInfo promote = CommentBrowser.GetPromote(this.activityId);
				if (promote == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					PageTitle.AddSiteNameTitle(promote.Name, Hidistro.Membership.Context.HiContext.Current.Context);
					this.litHelpTitle.Text = promote.Name;
					this.litHelpDescription.Text = promote.Description;
					PromotionInfo frontOrNextPromoteInfo = CommentBrowser.GetFrontOrNextPromoteInfo(promote, "Front");
					if (frontOrNextPromoteInfo != null)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "FavourableDetails.aspx?activityId=" + frontOrNextPromoteInfo.ActivityId;
							this.lblFrontTitle.Text = frontOrNextPromoteInfo.Name;
						}
					}
					else
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = false;
						}
					}
					PromotionInfo frontOrNextPromoteInfo2 = CommentBrowser.GetFrontOrNextPromoteInfo(promote, "Next");
					if (frontOrNextPromoteInfo2 != null)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "FavourableDetails.aspx?activityId=" + frontOrNextPromoteInfo2.ActivityId;
							this.lblNextTitle.Text = frontOrNextPromoteInfo2.Name;
						}
					}
					else
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = false;
						}
					}
				}
			}
		}
	}
}
