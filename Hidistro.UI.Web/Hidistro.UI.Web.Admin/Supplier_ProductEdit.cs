using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_ProductEdit : ProductBasePage
	{
		protected Script Script2;
		protected Script Script1;
		protected System.Web.UI.WebControls.Literal litCategoryName;
		protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
		protected ProductTypeDownList dropProductTypes;
		protected BrandCategoriesDropDownList dropBrandCategories;
		protected ProductLineDropDownList dropProductLines;
		protected System.Web.UI.WebControls.HyperLink hlinkDistributor;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdlineId;
		protected TrimTextBox txtProductName;
		protected TrimTextBox txtDisplaySequence;
		protected TrimTextBox txtProductCode;
		protected TrimTextBox txtUnit;
		protected TrimTextBox txtMarketPrice;
		protected TrimTextBox txtCostPrice;
		protected TrimTextBox txtAttributes;
		protected TrimTextBox txtSku;
		protected TrimTextBox txtStock;
		protected TrimTextBox txtAlertStock;
		protected TrimTextBox txtWeight;
		protected TrimTextBox txtSkus;
		protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
		protected ImageUploader uploader1;
		protected ImageUploader uploader2;
		protected ImageUploader uploader3;
		protected ImageUploader uploader4;
		protected ImageUploader uploader5;
		protected TrimTextBox txtShortDescription;
		protected KindeditorControl fckDescription;
		protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.CheckBox chkPenetration;
		protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
		protected ProductTagsLiteral litralProductTag;
		protected TrimTextBox txtProductTag;
		protected TrimTextBox txtTitle;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected TrimTextBox txtLowestSalePrice;
		protected TrimTextBox txtPurchasePrice;
		protected TrimTextBox txtDistributorPrices;
		protected TrimTextBox txtSalePrice;
		protected TrimTextBox txtMemberPrices;
		protected System.Web.UI.WebControls.Button btnSave;
		private int productId;
		private int categoryId;
		private string toline = "";
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(base.Request.QueryString["productId"], out this.productId);
			int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
			if (!this.Page.IsPostBack)
			{
				System.Collections.Generic.IList<int> list = null;
				System.Collections.Generic.IList<int> list2 = null;
				System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs;
				ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out attrs, out list, out list2);
				if (productDetails == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
				{
					this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
					this.ViewState["ProductCategoryId"] = this.categoryId;
					this.lnkEditCategory.NavigateUrl = "Supplier_ProductSelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				else
				{
					this.litCategoryName.Text = CatalogHelper.GetFullCategory(productDetails.CategoryId);
					this.ViewState["ProductCategoryId"] = productDetails.CategoryId;
					this.lnkEditCategory.NavigateUrl = "Supplier_ProductSelectCategory.aspx?categoryId=" + productDetails.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				System.Web.UI.WebControls.HyperLink expr_146 = this.lnkEditCategory;
				expr_146.NavigateUrl = expr_146.NavigateUrl + "&productId=" + productDetails.ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.litralProductTag.SelectedValue = list2;
				if (list2.Count > 0)
				{
					foreach (int current in list2)
					{
						TrimTextBox expr_19E = this.txtProductTag;
						expr_19E.Text = expr_19E.Text + current.ToString() + ",";
					}
					this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
				}
				this.dropProductTypes.DataBind();
				this.dropProductLines.DataBind();
				this.dropBrandCategories.DataBind();
				this.LoadProduct(productDetails, attrs);
				System.Data.DataTable dataTable = Methods.Supplier_PtGet(this.productId);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					System.Data.DataRow dataRow = dataTable.Rows[0];
					int num = (int)dataRow["checkstatus"];
					if (num == 3)
					{
						this.btnSave.Visible = false;
					}
				}
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.categoryId == 0)
			{
				this.categoryId = (int)this.ViewState["ProductCategoryId"];
			}
			int displaySequence;
			decimal num;
			decimal num2;
			decimal num3;
			decimal? num4;
			decimal? marketPrice;
			int stock;
			int alertStock;
			decimal? num5;
			if (!this.ValidateConverts(this.chkSkuEnabled.Checked, out displaySequence, out num, out num2, out num3, out num4, out marketPrice, out stock, out alertStock, out num5))
			{
				return;
			}
			decimal d = 0m;
			decimal.TryParse(this.txtCostPrice.Text, out d);
			if (d == 0m)
			{
				this.ShowMsg("供货价必填", false);
				return;
			}
			if (!this.chkSkuEnabled.Checked)
			{
				if (num3 <= 0m)
				{
					this.ShowMsg("商品一口价必须大于0", false);
					return;
				}
				if (num4.HasValue && num4.Value >= num3)
				{
					this.ShowMsg("商品成本价必须小于商品一口价", false);
					return;
				}
				if (!(num <= num2))
				{
					this.ShowMsg("分销商采购价必须要小于等于其最低零售价", false);
					return;
				}
			}
			string text = this.fckDescription.Text;
			if (this.ckbIsDownPic.Checked)
			{
				text = base.DownRemotePic(text);
			}
			ProductInfo productInfo = new ProductInfo
			{
				ProductId = this.productId,
				CategoryId = this.categoryId,
				TypeId = this.dropProductTypes.SelectedValue,
				ProductName = this.txtProductName.Text,
				ProductCode = this.txtProductCode.Text,
				DisplaySequence = displaySequence,
				LineId = this.dropProductLines.SelectedValue.Value,
				LowestSalePrice = num2,
				MarketPrice = marketPrice,
				Unit = this.txtUnit.Text,
				ImageUrl1 = this.uploader1.UploadedImageUrl,
				ImageUrl2 = this.uploader2.UploadedImageUrl,
				ImageUrl3 = this.uploader3.UploadedImageUrl,
				ImageUrl4 = this.uploader4.UploadedImageUrl,
				ImageUrl5 = this.uploader5.UploadedImageUrl,
				ThumbnailUrl40 = this.uploader1.ThumbnailUrl40,
				ThumbnailUrl60 = this.uploader1.ThumbnailUrl60,
				ThumbnailUrl100 = this.uploader1.ThumbnailUrl100,
				ThumbnailUrl160 = this.uploader1.ThumbnailUrl160,
				ThumbnailUrl180 = this.uploader1.ThumbnailUrl180,
				ThumbnailUrl220 = this.uploader1.ThumbnailUrl220,
				ThumbnailUrl310 = this.uploader1.ThumbnailUrl310,
				ThumbnailUrl410 = this.uploader1.ThumbnailUrl410,
				ShortDescription = this.txtShortDescription.Text,
				Description = (string.IsNullOrEmpty(text) || text.Length <= 0) ? null : text,
				PenetrationStatus = this.chkPenetration.Checked ? PenetrationStatus.Already : PenetrationStatus.Notyet,
				Title = this.txtTitle.Text,
				MetaDescription = this.txtMetaDescription.Text,
				MetaKeywords = this.txtMetaKeywords.Text,
				AddedDate = System.DateTime.Now,
				BrandId = this.dropBrandCategories.SelectedValue
			};
			ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
			if (this.radInStock.Checked)
			{
				saleStatus = ProductSaleStatus.OnStock;
			}
			if (this.radUnSales.Checked)
			{
				saleStatus = ProductSaleStatus.UnSale;
			}
			if (this.radOnSales.Checked)
			{
				saleStatus = ProductSaleStatus.OnSale;
			}
			productInfo.SaleStatus = saleStatus;
			CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
			if (category != null)
			{
				productInfo.MainCategoryPath = category.Path + "|";
			}
			System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
			System.Collections.Generic.Dictionary<string, SKUItem> dictionary;
			if (this.chkSkuEnabled.Checked)
			{
				productInfo.HasSKU = true;
				dictionary = base.GetSkus(this.txtSkus.Text);
			}
			else
			{
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>
				{

					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = this.txtSku.Text,
							SalePrice = num3,
							CostPrice = num4.HasValue ? num4.Value : 0m,
							PurchasePrice = num,
							Stock = stock,
							AlertStock = alertStock,
							Weight = num5.HasValue ? num5.Value : 0m
						}
					}
				};
				if (this.txtMemberPrices.Text.Length > 0)
				{
					base.GetMemberPrices(dictionary["0"], this.txtMemberPrices.Text);
				}
				if (this.txtDistributorPrices.Text.Length > 0)
				{
					base.GetDistributorPrices(dictionary["0"], this.txtDistributorPrices.Text);
				}
			}
			if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
			{
				attrs = base.GetAttributes(this.txtAttributes.Text);
			}
			ValidationResults validationResults = Validation.Validate<ProductInfo>(productInfo);
			if (!validationResults.IsValid)
			{
				this.ShowMsg(validationResults);
				return;
			}
			if (this.ViewState["distributorUserIds"] == null)
			{
				this.ViewState["distributorUserIds"] = new System.Collections.Generic.List<int>();
			}
			int num6 = 0;
			if (productInfo.LineId > 0 && int.Parse(this.hdlineId.Value) > 0 && productInfo.LineId != int.Parse(this.hdlineId.Value))
			{
				num6 = 6;
			}
			if (!this.chkPenetration.Checked)
			{
				num6 = 5;
			}
			if (num6 == 5)
			{
				SendMessageHelper.SendMessageToDistributors(productInfo.ProductId.ToString(), num6);
			}
			else
			{
				if (num6 == 6)
				{
					this.toline = this.dropProductLines.SelectedItem.Text;
					SendMessageHelper.SendMessageToDistributors(this.hdlineId.Value + "|" + this.toline, num6);
				}
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
			{
				string text2 = this.txtProductTag.Text.Trim();
				string[] array;
				if (text2.Contains(","))
				{
					array = text2.Split(new char[]
					{
						','
					});
				}
				else
				{
					array = new string[]
					{
						text2
					};
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					list.Add(System.Convert.ToInt32(value));
				}
			}
			ProductActionStatus productActionStatus = ProductHelper.UpdateProduct(productInfo, dictionary, attrs, (System.Collections.Generic.IList<int>)this.ViewState["distributorUserIds"], list);
			if (productActionStatus == ProductActionStatus.Success)
			{
				this.litralProductTag.SelectedValue = list;
				this.ShowMsg("修改商品成功", true);
				return;
			}
			if (productActionStatus == ProductActionStatus.AttributeError)
			{
				this.ShowMsg("修改商品失败，保存商品属性时出错", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.DuplicateName)
			{
				this.ShowMsg("修改商品失败，商品名称不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.DuplicateSKU)
			{
				this.ShowMsg("修改商品失败，商家编码不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.SKUError)
			{
				this.ShowMsg("修改商品失败，商家编码不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.OffShelfError)
			{
				this.ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.ProductTagEroor)
			{
				this.ShowMsg("修改商品失败，保存商品标签时出错", false);
				return;
			}
			this.ShowMsg("修改商品失败，未知错误", false);
		}
		private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out decimal? weight)
		{
			string text = string.Empty;
			costPrice = null;
			marketPrice = null;
			weight = null;
			alertStock = 0;
			stock = 0;
			displaySequence = 0;
			purchasePrice = (lowestSalePrice = (salePrice = 0m));
			if (!this.dropProductLines.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择产品线");
			}
			if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
			{
				text += Formatter.FormatErrorMessage("请正确填写商品排序");
			}
			if (this.txtProductCode.Text.Length > 20)
			{
				text += Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
			}
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
			{
				decimal value;
				if (decimal.TryParse(this.txtMarketPrice.Text, out value))
				{
					marketPrice = new decimal?(value);
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的市场价");
				}
			}
			if (string.IsNullOrEmpty(this.txtLowestSalePrice.Text) || !decimal.TryParse(this.txtLowestSalePrice.Text, out lowestSalePrice))
			{
				text += Formatter.FormatErrorMessage("请正确填写分销商最低零售价");
			}
			if (!skuEnabled)
			{
				if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品一口价");
				}
				if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
				{
					decimal value2;
					if (decimal.TryParse(this.txtCostPrice.Text, out value2))
					{
						costPrice = new decimal?(value2);
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的成本价");
					}
				}
				if (string.IsNullOrEmpty(this.txtPurchasePrice.Text) || !decimal.TryParse(this.txtPurchasePrice.Text, out purchasePrice))
				{
					text += Formatter.FormatErrorMessage("请正确填写分销商采购价格");
				}
				if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的库存数量");
				}
				if (string.IsNullOrEmpty(this.txtAlertStock.Text) || !int.TryParse(this.txtAlertStock.Text, out alertStock))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的警戒库存");
				}
				if (!string.IsNullOrEmpty(this.txtWeight.Text))
				{
					decimal value3;
					if (decimal.TryParse(this.txtWeight.Text, out value3))
					{
						weight = new decimal?(value3);
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的重量");
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private void LoadProduct(ProductInfo product, System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs)
		{
			this.dropProductTypes.SelectedValue = product.TypeId;
			this.dropProductLines.SelectedValue = new int?(product.LineId);
			this.hdlineId.Value = product.LineId.ToString();
			this.dropBrandCategories.SelectedValue = product.BrandId;
			this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
			this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
			this.txtProductCode.Text = product.ProductCode;
			this.txtUnit.Text = product.Unit;
			if (product.MarketPrice.HasValue)
			{
				this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
			}
			this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
			this.txtTitle.Text = product.Title;
			this.txtMetaDescription.Text = product.MetaDescription;
			this.txtMetaKeywords.Text = product.MetaKeywords;
			this.txtLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
			this.chkPenetration.Checked = (product.PenetrationStatus == PenetrationStatus.Already);
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
			if (attrs != null && attrs.Count > 0)
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("<xml><attributes>");
				foreach (int current in attrs.Keys)
				{
					stringBuilder.Append("<item attributeId=\"").Append(current.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(current).UsageMode).ToString()).Append("\" >");
					foreach (int current2 in attrs[current])
					{
						stringBuilder.Append("<attValue valueId=\"").Append(current2.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
					}
					stringBuilder.Append("</item>");
				}
				stringBuilder.Append("</attributes></xml>");
				this.txtAttributes.Text = stringBuilder.ToString();
			}
			this.chkSkuEnabled.Checked = product.HasSKU;
			if (product.HasSKU)
			{
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.Append("<xml><productSkus>");
				foreach (string current3 in product.Skus.Keys)
				{
					SKUItem sKUItem = product.Skus[current3];
					string text = string.Concat(new string[]
					{
						"<item skuCode=\"",
						sKUItem.SKU,
						"\" salePrice=\"",
						sKUItem.SalePrice.ToString("F2"),
						"\" costPrice=\"",
						(sKUItem.CostPrice > 0m) ? sKUItem.CostPrice.ToString("F2") : "",
						"\" purchasePrice=\"",
						sKUItem.PurchasePrice.ToString("F2"),
						"\" qty=\"",
						sKUItem.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"\" alertQty=\"",
						sKUItem.AlertStock.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"\" weight=\"",
						(sKUItem.Weight > 0m) ? sKUItem.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".0000", ".00") : "",
						"\">"
					});
					text += "<skuFields>";
					foreach (int current4 in sKUItem.SkuItems.Keys)
					{
						string str = string.Concat(new string[]
						{
							"<sku attributeId=\"",
							current4.ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" valueId=\"",
							sKUItem.SkuItems[current4].ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" />"
						});
						text += str;
					}
					text += "</skuFields>";
					if (sKUItem.MemberPrices.Count > 0)
					{
						text += "<memberPrices>";
						foreach (int current5 in sKUItem.MemberPrices.Keys)
						{
							text += string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", current5.ToString(System.Globalization.CultureInfo.InvariantCulture), sKUItem.MemberPrices[current5].ToString("F2"));
						}
						text += "</memberPrices>";
					}
					if (sKUItem.DistributorPrices.Count > 0)
					{
						text += "<distributorPrices>";
						foreach (int current6 in sKUItem.DistributorPrices.Keys)
						{
							text += string.Format("<distributorGrande id=\"{0}\" price=\"{1}\" />", current6.ToString(System.Globalization.CultureInfo.InvariantCulture), sKUItem.DistributorPrices[current6].ToString("F2"));
						}
						text += "</distributorPrices>";
					}
					text += "</item>";
					stringBuilder2.Append(text);
				}
				stringBuilder2.Append("</productSkus></xml>");
				this.txtSkus.Text = stringBuilder2.ToString();
			}
			SKUItem defaultSku = product.DefaultSku;
			this.txtSku.Text = product.SKU;
			this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
			this.txtCostPrice.Text = ((defaultSku.CostPrice > 0m) ? defaultSku.CostPrice.ToString("F2") : "");
			this.txtPurchasePrice.Text = defaultSku.PurchasePrice.ToString("F2");
			this.txtStock.Text = defaultSku.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.txtAlertStock.Text = defaultSku.AlertStock.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.txtWeight.Text = ((defaultSku.Weight > 0m) ? defaultSku.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".0000", ".00") : "");
			if (defaultSku.MemberPrices.Count > 0)
			{
				this.txtMemberPrices.Text = "<xml><gradePrices>";
				foreach (int current7 in defaultSku.MemberPrices.Keys)
				{
					TrimTextBox expr_87B = this.txtMemberPrices;
					expr_87B.Text += string.Format("<grande id=\"{0}\" price=\"{1}\" />", current7.ToString(System.Globalization.CultureInfo.InvariantCulture), defaultSku.MemberPrices[current7].ToString("F2"));
				}
				TrimTextBox expr_8DC = this.txtMemberPrices;
				expr_8DC.Text += "</gradePrices></xml>";
			}
			if (defaultSku.DistributorPrices.Count > 0)
			{
				this.txtDistributorPrices.Text = "<xml><gradePrices>";
				foreach (int current8 in defaultSku.DistributorPrices.Keys)
				{
					TrimTextBox expr_938 = this.txtDistributorPrices;
					expr_938.Text += string.Format("<grande id=\"{0}\" price=\"{1}\" />", current8.ToString(System.Globalization.CultureInfo.InvariantCulture), defaultSku.DistributorPrices[current8].ToString("F2"));
				}
				TrimTextBox expr_999 = this.txtDistributorPrices;
				expr_999.Text += "</gradePrices></xml>";
			}
		}
	}
}
