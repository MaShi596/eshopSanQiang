using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyProduct : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litCategoryName;
		protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
		protected ProductTypeDownList dropProductTypes;
		protected BrandCategoriesDropDownList dropBrandCategories;
		protected ProductAttributeDisplay ProductAttributeDisplay1;
		protected TrimTextBox txtProductName;
		protected TrimTextBox txtDisplaySequence;
		protected ProductLineDropDownList dropProductLines;
		protected System.Web.UI.WebControls.Literal litLowestSalePrice;
		protected TrimTextBox txtMarketPrice;
		protected TrimTextBox txtSkuPrice;
		protected System.Web.UI.WebControls.Literal litProductCode;
		protected System.Web.UI.WebControls.Literal litUnit;
		protected ImageUploader uploader1;
		protected ImageUploader uploader2;
		protected ImageUploader uploader3;
		protected ImageUploader uploader4;
		protected ImageUploader uploader5;
		protected TrimTextBox txtShortDescription;
		protected KindeditorControl fckDescription;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.HtmlControls.HtmlGenericControl li_tags;
		protected ProductTagsLiteral litralProductTag;
		protected TrimTextBox txtProductTag;
		protected TrimTextBox txtTitle;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.Button btnUpdate;
		private int productId;
		private int categoryId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ProductId"], out this.productId))
			{
				base.GotoResourceNotFound();
				return;
			}
			int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
			if (!this.Page.IsPostBack)
			{
				ProductInfo product = SubSiteProducthelper.GetProduct(this.productId);
				if (product == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
				{
					this.litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(this.categoryId);
					this.ViewState["ProductCategoryId"] = this.categoryId;
					this.lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				else
				{
					this.litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(product.CategoryId);
					this.ViewState["ProductCategoryId"] = product.CategoryId;
					this.lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + product.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				System.Web.UI.WebControls.HyperLink expr_148 = this.lnkEditCategory;
				expr_148.NavigateUrl = expr_148.NavigateUrl + "&productId=" + product.ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
				list = SubSiteProducthelper.GetProductTags(this.productId);
				this.litralProductTag.SelectedValue = list;
				if (list.Count > 0)
				{
					foreach (int current in list)
					{
						TrimTextBox expr_1B4 = this.txtProductTag;
						expr_1B4.Text = expr_1B4.Text + current.ToString() + ",";
					}
					this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
				}
				this.dropProductTypes.Enabled = false;
				this.dropProductTypes.DataBind();
				this.dropProductTypes.SelectedValue = product.TypeId;
				this.dropProductLines.Enabled = false;
				this.dropProductLines.DataBind();
				this.dropProductLines.SelectedValue = new int?(product.LineId);
				this.dropBrandCategories.Enabled = false;
				this.dropBrandCategories.DataBind();
				this.dropBrandCategories.SelectedValue = product.BrandId;
				this.LoadProudct(product);
			}
		}
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			if (this.categoryId == 0)
			{
				this.categoryId = (int)this.ViewState["ProductCategoryId"];
			}
			if (this.categoryId == 0)
			{
				this.ShowMsg("请选择商品分类", false);
				return;
			}
			ProductInfo productInfo = new ProductInfo();
			productInfo.ProductId = this.productId;
			productInfo.CategoryId = this.categoryId;
			CategoryInfo category = SubsiteCatalogHelper.GetCategory(productInfo.CategoryId);
			if (category != null)
			{
				productInfo.MainCategoryPath = category.Path + "|";
			}
			productInfo.ProductName = this.txtProductName.Text;
			productInfo.ShortDescription = this.txtShortDescription.Text;
			productInfo.Description = this.fckDescription.Text;
			productInfo.Title = this.txtTitle.Text;
			productInfo.MetaDescription = this.txtMetaDescription.Text;
			productInfo.MetaKeywords = this.txtMetaKeywords.Text;
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
			{
				productInfo.MarketPrice = new decimal?(decimal.Parse(this.txtMarketPrice.Text));
			}
			System.Collections.Generic.Dictionary<string, decimal> skuSalePrice = null;
			if (!string.IsNullOrEmpty(this.txtSkuPrice.Text))
			{
				skuSalePrice = this.GetSkuPrices();
			}
			ProductSaleStatus productSaleStatus = ProductSaleStatus.OnStock;
			if (this.radInStock.Checked)
			{
				productSaleStatus = ProductSaleStatus.OnStock;
			}
			if (this.radUnSales.Checked)
			{
				productSaleStatus = ProductSaleStatus.UnSale;
			}
			if (this.radOnSales.Checked)
			{
				productSaleStatus = ProductSaleStatus.OnSale;
			}
			if (productSaleStatus == ProductSaleStatus.OnSale)
			{
				bool flag = false;
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				try
				{
					xmlDocument.LoadXml(this.txtSkuPrice.Text);
					System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
					if (xmlNodeList != null && xmlNodeList.Count > 0)
					{
						foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
						{
							decimal d = decimal.Parse(xmlNode.Attributes["price"].Value);
							if (d < decimal.Parse(this.litLowestSalePrice.Text))
							{
								flag = true;
								break;
							}
						}
					}
				}
				catch
				{
				}
				if (flag)
				{
					this.ShowMsg("此商品的一口价已经低于了最低零售价,不允许上架", false);
					return;
				}
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
			{
				string text = this.txtProductTag.Text.Trim();
				string[] array;
				if (text.Contains(","))
				{
					array = text.Split(new char[]
					{
						','
					});
				}
				else
				{
					array = new string[]
					{
						text
					};
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					list.Add(System.Convert.ToInt32(value));
				}
			}
			productInfo.SaleStatus = productSaleStatus;
			int displaySequence = 0;
			int.TryParse(this.txtDisplaySequence.Text, out displaySequence);
			productInfo.DisplaySequence = displaySequence;
			if (SubSiteProducthelper.UpdateProduct(productInfo, skuSalePrice, list))
			{
				this.litralProductTag.SelectedValue = list;
				this.ShowMsg("修改商品成功", true);
			}
		}
		private void LoadProudct(ProductInfo product)
		{
			this.txtProductName.Text = product.ProductName;
			this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
			this.litLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
			if (product.MarketPrice.HasValue)
			{
				this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
			}
			this.litProductCode.Text = product.ProductCode;
			this.litUnit.Text = product.Unit;
			this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
			this.txtTitle.Text = product.Title;
			this.txtMetaDescription.Text = product.MetaDescription;
			this.txtMetaKeywords.Text = product.MetaKeywords;
			if (product.SaleStatus == ProductSaleStatus.OnSale)
			{
				this.radOnSales.Checked = true;
			}
			else
			{
				if (product.SaleStatus == ProductSaleStatus.UnSale)
				{
					this.radUnSales.Checked = true;
				}
				else
				{
					this.radInStock.Checked = true;
				}
			}
			this.uploader1.UploadedImageUrl = product.ImageUrl1;
			this.uploader2.UploadedImageUrl = product.ImageUrl2;
			this.uploader3.UploadedImageUrl = product.ImageUrl3;
			this.uploader4.UploadedImageUrl = product.ImageUrl4;
			this.uploader5.UploadedImageUrl = product.ImageUrl5;
		}
		private System.Collections.Generic.Dictionary<string, decimal> GetSkuPrices()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.Dictionary<string, decimal> dictionary = null;
			System.Collections.Generic.Dictionary<string, decimal> result;
			try
			{
				xmlDocument.LoadXml(this.txtSkuPrice.Text);
				System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList != null && xmlNodeList.Count != 0)
				{
					System.Collections.Generic.IList<SKUItem> skus = SubSiteProducthelper.GetSkus(this.productId.ToString());
					dictionary = new System.Collections.Generic.Dictionary<string, decimal>();
					foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
					{
						string value = xmlNode.Attributes["skuId"].Value;
						decimal num = decimal.Parse(xmlNode.Attributes["price"].Value);
						foreach (SKUItem current in skus)
						{
							if (current.SkuId == value && current.SalePrice != num)
							{
								dictionary.Add(value, num);
							}
						}
					}
					return dictionary;
				}
				result = null;
			}
			catch
			{
				return dictionary;
			}
			return result;
		}
	}
}
