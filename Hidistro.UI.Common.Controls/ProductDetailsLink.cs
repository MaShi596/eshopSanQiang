using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ProductDetailsLink : HyperLink
	{
		public const string TagID = "ProductDetailsLink";
		public bool IsUnSale
		{
			get;
			set;
		}
		public bool ImageLink
		{
			get;
			set;
		}
		public bool IsGroupBuyProduct
		{
			get;
			set;
		}
		public bool IsCountDownProduct
		{
			get;
			set;
		}
		public object ProductName
		{
			get;
			set;
		}
		public object ProductId
		{
			get;
			set;
		}
		public int? StringLenth
		{
			get;
			set;
		}
		public ProductDetailsLink()
		{
			base.ID = "ProductDetailsLink";
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.ProductId != null && this.ProductId != DBNull.Value)
			{
				if (this.IsGroupBuyProduct)
				{
					base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[]
					{
						this.ProductId
					});
				}
				else
				{
					if (this.IsCountDownProduct)
					{
						base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[]
						{
							this.ProductId
						});
					}
					else
					{
						if (this.IsUnSale)
						{
							base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[]
							{
								this.ProductId
							});
						}
						else
						{
							base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
							{
								this.ProductId
							});
						}
					}
				}
			}
			if (!this.ImageLink && this.ProductId != null && this.ProductId != DBNull.Value)
			{
				if (this.StringLenth.HasValue && this.ProductName.ToString().Length > this.StringLenth.Value)
				{
					base.Text = this.ProductName.ToString().Substring(0, this.StringLenth.Value) + "...";
				}
				else
				{
					base.Text = this.ProductName.ToString();
				}
			}
			if (!HiContext.Current.User.IsInRole("Manager") && base.Text.IndexOf("】-") != -1)
			{
				base.Text = base.Text.Substring(base.Text.IndexOf("】-") + 3);
			}
			base.Target = "_blank";
			base.Render(writer);
		}
	}
}
