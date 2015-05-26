using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorProductDetailsLink : System.Web.UI.WebControls.HyperLink
	{
		public const string TagID = "DistributorProductDetailsLink";
		private bool unSale = false;
		private bool imageLink = false;
		public bool UnSale
		{
			get
			{
				return this.unSale;
			}
			set
			{
				this.unSale = value;
			}
		}
		public bool ImageLink
		{
			get
			{
				return this.imageLink;
			}
			set
			{
				this.imageLink = value;
			}
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
			get
			{
				object result;
				if (this.ViewState["ProductName"] != null)
				{
					result = this.ViewState["ProductName"];
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["ProductName"] = value;
				}
			}
		}
		public object ProductId
		{
			get
			{
				object result;
				if (this.ViewState["ProductId"] != null)
				{
					result = this.ViewState["ProductId"];
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["ProductId"] = value;
				}
			}
		}
		public DistributorProductDetailsLink()
		{
			base.ID = "DistributorProductDetailsLink";
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (this.ProductId != null && this.ProductId != System.DBNull.Value)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (this.IsGroupBuyProduct)
				{
					base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[]
					{
						this.ProductId
					});
				}
				else
				{
					if (this.IsCountDownProduct)
					{
						base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[]
						{
							this.ProductId
						});
					}
					else
					{
						if (this.unSale)
						{
							base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[]
							{
								this.ProductId
							});
						}
						else
						{
							base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
							{
								this.ProductId
							});
						}
					}
				}
			}
			if (!this.imageLink && this.ProductId != null && this.ProductId != System.DBNull.Value)
			{
				base.Text = this.ProductName.ToString();
			}
			base.Target = "_blank";
			base.Render(writer);
		}
	}
}
