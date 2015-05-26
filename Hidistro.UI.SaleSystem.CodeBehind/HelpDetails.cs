using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class HelpDetails : HtmlTemplatedWebControl
	{
		private int helpId = 0;
		private System.Web.UI.WebControls.Literal litHelpTitle;
		private System.Web.UI.WebControls.Literal litHelpDescription;
		private System.Web.UI.WebControls.Literal litHelpContent;
		private FormatedTimeLabel litHelpAddedDate;
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
				this.SkinName = "Skin-HelpDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["helpId"], out this.helpId))
			{
				base.GotoResourceNotFound();
			}
			this.litHelpAddedDate = (FormatedTimeLabel)this.FindControl("litHelpAddedDate");
			this.litHelpDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litHelpDescription");
			this.litHelpContent = (System.Web.UI.WebControls.Literal)this.FindControl("litHelpContent");
			this.litHelpTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litHelpTitle");
			this.lblFront = (System.Web.UI.WebControls.Label)this.FindControl("lblFront");
			this.lblNext = (System.Web.UI.WebControls.Label)this.FindControl("lblNext");
			this.lblFrontTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblNextTitle");
			this.aFront = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("front");
			this.aNext = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("next");
			if (!this.Page.IsPostBack)
			{
				HelpInfo help = CommentBrowser.GetHelp(this.helpId);
				if (help != null)
				{
					PageTitle.AddSiteNameTitle(help.Title, Hidistro.Membership.Context.HiContext.Current.Context);
					if (!string.IsNullOrEmpty(help.MetaKeywords))
					{
						MetaTags.AddMetaKeywords(help.MetaKeywords, Hidistro.Membership.Context.HiContext.Current.Context);
					}
					if (!string.IsNullOrEmpty(help.MetaDescription))
					{
						MetaTags.AddMetaDescription(help.MetaDescription, Hidistro.Membership.Context.HiContext.Current.Context);
					}
					this.litHelpTitle.Text = help.Title;
					this.litHelpDescription.Text = help.Description;
					string str = Hidistro.Membership.Context.HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails", new object[]
					{
						this.helpId
					});
					this.litHelpContent.Text = help.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litHelpAddedDate.Time = help.AddedDate;
					HelpInfo frontOrNextHelp = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Front");
					if (frontOrNextHelp != null && frontOrNextHelp.HelpId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "HelpDetails.aspx?helpId=" + frontOrNextHelp.HelpId;
							this.lblFrontTitle.Text = frontOrNextHelp.Title;
						}
					}
					else
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = false;
						}
					}
					HelpInfo frontOrNextHelp2 = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Next");
					if (frontOrNextHelp2 != null && frontOrNextHelp2.HelpId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "HelpDetails.aspx?helpId=" + frontOrNextHelp2.HelpId;
							this.lblNextTitle.Text = frontOrNextHelp2.Title;
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
