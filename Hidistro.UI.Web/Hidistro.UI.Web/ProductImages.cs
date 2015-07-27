using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

namespace Hidistro.UI.Web
{
	public class ProductImages : System.Web.UI.Page
	{
		protected PageTitle PageTitle1;
		protected Script Script1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.HyperLink productName;
        protected System.Web.UI.WebControls.HyperLink productIn3D;
		protected SiteUrl SiteUrl1;
		protected System.Web.UI.HtmlControls.HtmlInputText image1url;
		protected System.Web.UI.HtmlControls.HtmlInputText image2url;
		protected System.Web.UI.HtmlControls.HtmlInputText image3url;
		protected System.Web.UI.HtmlControls.HtmlInputText image4url;
		protected System.Web.UI.HtmlControls.HtmlInputText image5url;
		protected HiImage image1;
		protected HiImage image2;
		protected HiImage image3;
		protected HiImage image4;
		protected HiImage image5;
		protected System.Web.UI.HtmlControls.HtmlImage imgBig;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int productId = 0;
			int.TryParse(this.Page.Request.QueryString["ProductId"], out productId);
			if (!this.Page.IsPostBack)
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
				if (productSimpleInfo != null)
				{
					this.BindImages(productSimpleInfo);
					if (!string.IsNullOrEmpty(productSimpleInfo.Title))
					{
						PageTitle.AddSiteNameTitle(productSimpleInfo.Title + " 相册", Hidistro.Membership.Context.HiContext.Current.Context);
						return;
					}
					PageTitle.AddSiteNameTitle(productSimpleInfo.ProductName + " 相册", Hidistro.Membership.Context.HiContext.Current.Context);
				}
			}
		}
		private void BindImages(ProductInfo prductImageInfo)
		{
            string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/image3D/" + prductImageInfo.ProductId.ToString() + "//" + "index.html");

            if (File.Exists(text2))
            {
                this.productIn3D.Text = "3D图样";
                this.productIn3D.NavigateUrl = Globals.ApplicationPath + "/image3D/" + prductImageInfo.ProductId.ToString() + "//" + "index.html";//Utils.ApplicationPath + "/Product3D.aspx?ProductId=" + prductImageInfo.ProductId;
            }
            else
            {
                this.productIn3D.Text = "";
            }
            
            this.productName.Text = prductImageInfo.ProductName;
			this.productName.NavigateUrl = Utils.ApplicationPath + "/ProductDetails.aspx?ProductId=" + prductImageInfo.ProductId;

			this.imgBig.Src = (this.image1url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl1);
			this.image2url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl2;
			this.image3url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl3;
			this.image4url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl4;
			this.image5url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl5;
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl1))
			{
				this.image1.ImageUrl = prductImageInfo.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl2))
			{
				this.image2.ImageUrl = prductImageInfo.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl3))
			{
				this.image3.ImageUrl = prductImageInfo.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl4))
			{
				this.image4.ImageUrl = prductImageInfo.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl5))
			{
				this.image5.ImageUrl = prductImageInfo.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (prductImageInfo.ImageUrl1 == null && prductImageInfo.ImageUrl2 == null && prductImageInfo.ImageUrl3 == null && prductImageInfo.ImageUrl4 == null && prductImageInfo.ImageUrl5 == null)
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(true);
				this.imgBig.Src = Globals.ApplicationPath + masterSettings.DefaultProductImage;
				this.imgBig.Align = "center";
			}
		}
	}
}
