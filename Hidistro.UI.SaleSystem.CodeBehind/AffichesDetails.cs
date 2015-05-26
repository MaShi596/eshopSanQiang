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
	public class AffichesDetails : HtmlTemplatedWebControl
	{
		private int affichesId = 0;
		private System.Web.UI.WebControls.Literal litTilte;
		private System.Web.UI.WebControls.Literal litContent;
		private FormatedTimeLabel litAffichesAddedDate;
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
				this.SkinName = "Skin-AffichesDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["AfficheId"], out this.affichesId))
			{
				base.GotoResourceNotFound();
			}
			this.litAffichesAddedDate = (FormatedTimeLabel)this.FindControl("litAffichesAddedDate");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.litTilte = (System.Web.UI.WebControls.Literal)this.FindControl("litTilte");
			this.lblFront = (System.Web.UI.WebControls.Label)this.FindControl("lblFront");
			this.lblNext = (System.Web.UI.WebControls.Label)this.FindControl("lblNext");
			this.aFront = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("front");
			this.aNext = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("next");
			this.lblFrontTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblNextTitle");
			if (!this.Page.IsPostBack)
			{
				AfficheInfo affiche = CommentBrowser.GetAffiche(this.affichesId);
				if (affiche != null)
				{
					PageTitle.AddSiteNameTitle(affiche.Title, Hidistro.Membership.Context.HiContext.Current.Context);
					this.litTilte.Text = affiche.Title;
					string str = Hidistro.Membership.Context.HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("AffichesDetails", new object[]
					{
						this.affichesId
					});
					this.litContent.Text = affiche.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litAffichesAddedDate.Time = affiche.AddedDate;
					AfficheInfo frontOrNextAffiche = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Front");
					AfficheInfo frontOrNextAffiche2 = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Next");
					if (frontOrNextAffiche != null && frontOrNextAffiche.AfficheId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "AffichesDetails.aspx?afficheId=" + frontOrNextAffiche.AfficheId;
							this.lblFrontTitle.Text = frontOrNextAffiche.Title;
						}
					}
					else
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = false;
						}
					}
					if (frontOrNextAffiche2 != null && frontOrNextAffiche2.AfficheId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "AffichesDetails.aspx?afficheId=" + frontOrNextAffiche2.AfficheId;
							this.lblNextTitle.Text = frontOrNextAffiche2.Title;
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
