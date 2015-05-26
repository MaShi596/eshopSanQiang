using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class BlogIt : HtmlTemplatedWebControl
	{
		private int productId = 0;
		private System.Web.UI.WebControls.Label lblProductNameLinkText;
		private System.Web.UI.WebControls.HyperLink hlinkProductTitle;
		private System.Web.UI.WebControls.HyperLink hlinkProductContent;
		private System.Web.UI.WebControls.Label lblUrl;
		private System.Web.UI.WebControls.Label lblUrl2;
		private System.Web.UI.WebControls.Label lblImgUrl;
		private HiImage imgUrl;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BlogIt.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productID"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.lblProductNameLinkText = (System.Web.UI.WebControls.Label)this.FindControl("lblProductNameLinkText");
			this.hlinkProductTitle = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkProductTitle");
			this.hlinkProductContent = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkProductContent");
			this.lblImgUrl = (System.Web.UI.WebControls.Label)this.FindControl("lblImgUrl");
			this.lblUrl = (System.Web.UI.WebControls.Label)this.FindControl("lblUrl");
			this.lblUrl2 = (System.Web.UI.WebControls.Label)this.FindControl("lblUrl2");
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			if (!this.Page.IsPostBack)
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
				if (productSimpleInfo == null)
				{
					base.GotoResourceNotFound();
				}
				PageTitle.AddSiteNameTitle(productSimpleInfo.ProductName + " 推荐到博客", Hidistro.Membership.Context.HiContext.Current.Context);
				string text = "productDetails";
				if (productSimpleInfo.SaleStatus == ProductSaleStatus.UnSale)
				{
					text = "unproductdetails";
				}
				string text2 = Globals.GetSiteUrls().UrlData.FormatUrl(text, new object[]
				{
					this.productId
				});
				this.hlinkProductTitle.Text = (this.hlinkProductContent.Text = productSimpleInfo.ProductName);
				this.hlinkProductTitle.NavigateUrl = (this.hlinkProductContent.NavigateUrl = text2);
				this.lblProductNameLinkText.Text = string.Format("插入这段代码，可以在你的博客中显示“{0}”的文字链接", string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", text2, "Text"));
				this.imgUrl.ImageUrl = productSimpleInfo.ImageUrl1;
				if (!string.IsNullOrEmpty(productSimpleInfo.ImageUrl1))
				{
					this.lblImgUrl.Text = Globals.FullPath(Globals.ApplicationPath + this.imgUrl.ImageUrl);
				}
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.HiContext.Current.User;
				if (user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling)
				{
					this.lblUrl.Text = (this.lblUrl2.Text = Globals.FullPath(System.Web.HttpContext.Current.Request.Url.PathAndQuery).Replace("BlogIt", text) + "&ReferralUserId=" + user.UserId);
				}
				else
				{
					this.lblUrl.Text = (this.lblUrl2.Text = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl(text, new object[]
					{
						this.productId
					})));
				}
			}
		}
	}
}
