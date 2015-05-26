using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class GiftDetails : HtmlTemplatedWebControl
	{
		private int giftId;
		private System.Web.UI.WebControls.Literal litGiftTite;
		private System.Web.UI.WebControls.Literal litGiftName;
		private FormatedMoneyLabel lblMarkerPrice;
		private System.Web.UI.WebControls.Label litNeedPoint;
		private System.Web.UI.WebControls.Label litCurrentPoint;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.Literal litDescription;
		private HiImage imgGiftImage;
		private System.Web.UI.WebControls.Button btnChage;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GiftDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
			{
				base.GotoResourceNotFound();
			}
			this.litGiftTite = (System.Web.UI.WebControls.Literal)this.FindControl("litGiftTite");
			this.litGiftName = (System.Web.UI.WebControls.Literal)this.FindControl("litGiftName");
			this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
			this.litNeedPoint = (System.Web.UI.WebControls.Label)this.FindControl("litNeedPoint");
			this.litCurrentPoint = (System.Web.UI.WebControls.Label)this.FindControl("litCurrentPoint");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.imgGiftImage = (HiImage)this.FindControl("imgGiftImage");
			this.btnChage = (System.Web.UI.WebControls.Button)this.FindControl("btnChage");
			this.btnChage.Click += new System.EventHandler(this.btnChage_Click);
			GiftInfo gift = ProductBrowser.GetGift(this.giftId);
			if (gift == null)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件礼品已经不再参与积分兑换；或被管理员删除"));
			}
			else
			{
				if (!this.Page.IsPostBack)
				{
					this.litGiftName.Text = gift.Name;
					this.lblMarkerPrice.Money = gift.MarketPrice;
					this.litNeedPoint.Text = gift.NeedPoint.ToString();
					this.litShortDescription.Text = gift.ShortDescription;
					this.litDescription.Text = gift.LongDescription;
					this.imgGiftImage.ImageUrl = gift.ThumbnailUrl310;
					this.LoadPageSearch(gift);
				}
				bool arg_209_0;
				if (Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member)
				{
					if (Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Underling)
					{
						arg_209_0 = true;
						goto IL_209;
					}
				}
				arg_209_0 = (gift.NeedPoint <= 0);
				IL_209:
				if (!arg_209_0)
				{
					this.btnChage.Enabled = true;
					this.btnChage.Text = "立即兑换";
					this.litCurrentPoint.Text = ((Hidistro.Membership.Context.Member)Hidistro.Membership.Context.HiContext.Current.User).Points.ToString();
				}
				else
				{
					if (gift.NeedPoint <= 0)
					{
						this.btnChage.Enabled = false;
						this.btnChage.Text = "礼品不允许兑换";
					}
					else
					{
						this.btnChage.Enabled = false;
						this.btnChage.Text = "请登录方能兑换";
						this.litCurrentPoint.Text = string.Format("<a href=\"{0}\">请登录</a>", Globals.ApplicationPath + "/Login.aspx");
					}
				}
			}
		}
		private void btnChage_Click(object sender, System.EventArgs e)
		{
			if (Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member && Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Underling)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("请登录后才能购买"));
			}
			else
			{
				if (int.Parse(this.litNeedPoint.Text) <= int.Parse(this.litCurrentPoint.Text) && ShoppingCartProcessor.AddGiftItem(this.giftId, 1, PromoteType.NotSet))
				{
					this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
				}
			}
		}
		private void LoadPageSearch(GiftInfo gift)
		{
			if (!string.IsNullOrEmpty(gift.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(gift.Meta_Keywords, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Meta_Description))
			{
				MetaTags.AddMetaDescription(gift.Meta_Description, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(gift.Title))
			{
				PageTitle.AddSiteNameTitle(gift.Title, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			else
			{
				PageTitle.AddSiteNameTitle(gift.Name, Hidistro.Membership.Context.HiContext.Current.Context);
			}
		}
	}
}
