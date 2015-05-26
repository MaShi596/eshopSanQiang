using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class GroupBuyProductDetails : HtmlTemplatedWebControl
	{
		private int productId;
		private Common_Location common_Location;
		private System.Web.UI.WebControls.Literal litProductName;
		private SkuLabel lblSku;
		private StockLabel lblStock;
		private System.Web.UI.WebControls.Label litWeight;
		private System.Web.UI.WebControls.Literal litUnit;
		private System.Web.UI.WebControls.Label litCount;
		private System.Web.UI.WebControls.Label litMaxCount;
		private System.Web.UI.WebControls.Literal litContent;
		private System.Web.UI.WebControls.Literal litRemainTime;
		private System.Web.UI.WebControls.Literal litBrosedNum;
		private System.Web.UI.WebControls.Literal litBrand;
		private System.Web.UI.WebControls.Literal litNeedCount;
		private FormatedMoneyLabel lblCurrentSalePrice;
		private FormatedMoneyLabel lblNeedPrice;
		private FormatedTimeLabel lblEndTime;
		private FormatedTimeLabel lblStartTime;
		private FormatedMoneyLabel lblSalePrice;
		private TotalLabel lblTotalPrice;
		private System.Web.UI.WebControls.Literal litDescription;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private BuyButton btnOrder;
		private System.Web.UI.WebControls.HyperLink hpkProductConsultations;
		private System.Web.UI.WebControls.HyperLink hpkProductReviews;
		private Common_ProductImages images;
		private ThemedTemplatedRepeater rptExpandAttributes;
		private SKUSelector skuSelector;
		private Common_ProductReview reviews;
		private Common_ProductConsultations consultations;
		private Common_GoodsList_Correlative correlative;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-GroupBuyProductDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductName = (System.Web.UI.WebControls.Literal)this.FindControl("litProductName");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (System.Web.UI.WebControls.Literal)this.FindControl("litUnit");
			this.litWeight = (System.Web.UI.WebControls.Label)this.FindControl("litWeight");
			this.litBrosedNum = (System.Web.UI.WebControls.Literal)this.FindControl("litBrosedNum");
			this.litBrand = (System.Web.UI.WebControls.Literal)this.FindControl("litBrand");
			this.lblSalePrice = (FormatedMoneyLabel)this.FindControl("lblSalePrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.btnOrder = (BuyButton)this.FindControl("btnOrder");
			this.hpkProductConsultations = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductConsultations");
			this.hpkProductReviews = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductReviews");
			this.lblCurrentSalePrice = (FormatedMoneyLabel)this.FindControl("lblCurrentSalePrice");
			this.litCount = (System.Web.UI.WebControls.Label)this.FindControl("litCount");
			this.lblNeedPrice = (FormatedMoneyLabel)this.FindControl("lblNeedPrice");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.lblEndTime = (FormatedTimeLabel)this.FindControl("lblEndTime");
			this.lblStartTime = (FormatedTimeLabel)this.FindControl("lblStartTime");
			this.litRemainTime = (System.Web.UI.WebControls.Literal)this.FindControl("litRemainTime");
			this.litNeedCount = (System.Web.UI.WebControls.Literal)this.FindControl("litNeedCount");
			this.litMaxCount = (System.Web.UI.WebControls.Label)this.FindControl("litMaxCount");
			this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
			this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
			this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
			this.reviews = (Common_ProductReview)this.FindControl("list_Common_ProductReview");
			this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
			this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
			if (!this.Page.IsPostBack)
			{
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(this.reviews.MaxNum), new int?(this.consultations.MaxNum));
				GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(this.productId);
				if (productBrowseInfo.Product == null || productGroupBuyInfo == null)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品参与的团购活动已经结束；或已被管理员删除"));
				}
				else
				{
					this.LoadPageSearch(productBrowseInfo.Product);
					this.hpkProductReviews.Text = "查看全部" + ProductBrowser.GetProductReviewNumber(this.productId).ToString() + "条评论";
					this.hpkProductConsultations.Text = "查看全部" + ProductBrowser.GetProductConsultationNumber(this.productId).ToString() + "条咨询";
					this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
					this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
					this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);
					this.LoadProductGroupBuyInfo(productGroupBuyInfo);
					this.btnOrder.Stock = productBrowseInfo.Product.Stock;
					BrowsedProductQueue.EnQueue(this.productId);
					this.images.ImageInfo = productBrowseInfo.Product;
					if (productBrowseInfo.DbAttribute != null)
					{
						this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
						this.rptExpandAttributes.DataBind();
					}
					if (productBrowseInfo.DbSKUs != null)
					{
						this.skuSelector.ProductId = this.productId;
						this.skuSelector.DataSource = productBrowseInfo.DbSKUs;
					}
					if (productBrowseInfo.DBReviews != null)
					{
						this.reviews.DataSource = productBrowseInfo.DBReviews;
						this.reviews.DataBind();
					}
					if (productBrowseInfo.DBConsultations != null)
					{
						this.consultations.DataSource = productBrowseInfo.DBConsultations;
						this.consultations.DataBind();
					}
					if (productBrowseInfo.DbCorrelatives != null)
					{
						this.correlative.DataSource = productBrowseInfo.DbCorrelatives;
						this.correlative.DataBind();
					}
				}
			}
		}
		private void LoadPageSearch(ProductInfo productDetails)
		{
			if (!string.IsNullOrEmpty(productDetails.MetaKeywords))
			{
				MetaTags.AddMetaKeywords(productDetails.MetaKeywords, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.MetaDescription))
			{
				MetaTags.AddMetaDescription(productDetails.MetaDescription, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Title))
			{
				PageTitle.AddSiteNameTitle(productDetails.Title, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			else
			{
				PageTitle.AddSiteNameTitle(productDetails.ProductName, Hidistro.Membership.Context.HiContext.Current.Context);
			}
		}
		private void LoadProductGroupBuyInfo(GroupBuyInfo groupBuy)
		{
			int orderCount = ProductBrowser.GetOrderCount(groupBuy.GroupBuyId);
			decimal currentPrice = ProductBrowser.GetCurrentPrice(groupBuy.GroupBuyId, orderCount);
			this.litCount.Text = orderCount.ToString();
			this.lblCurrentSalePrice.Money = currentPrice;
			this.lblNeedPrice.Money = groupBuy.NeedPrice;
			this.litMaxCount.Text = groupBuy.MaxCount.ToString();
			this.litContent.Text = groupBuy.Content;
			this.lblTotalPrice.Value = new decimal?(currentPrice);
			this.lblEndTime.Time = groupBuy.EndDate;
			this.lblStartTime.Time = groupBuy.StartDate;
			this.litRemainTime.Text = "";
			this.litNeedCount.Text = groupBuy.GroupBuyConditions[0].Count.ToString();
		}
		private void LoadProductInfo(ProductInfo productDetails, string brandName)
		{
			if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
			{
				this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
				this.common_Location.ProductName = productDetails.ProductName;
			}
			this.litProductName.Text = productDetails.ProductName;
			this.lblSku.Value = productDetails.SkuId;
			this.lblSku.Text = productDetails.SKU;
			this.lblStock.Stock = productDetails.Stock;
			this.litUnit.Text = productDetails.Unit;
			if (productDetails.Weight > 0m)
			{
				this.litWeight.Text = string.Format("{0} g", productDetails.Weight);
			}
			else
			{
				this.litWeight.Text = "无";
			}
			this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
			this.litBrand.Text = brandName;
			this.lblSalePrice.Money = productDetails.MaxSalePrice;
			this.litDescription.Text = productDetails.Description;
			if (this.litShortDescription != null)
			{
				this.litShortDescription.Text = productDetails.ShortDescription;
			}
		}
	}
}
