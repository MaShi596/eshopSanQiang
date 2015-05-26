using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_ProductAdd : ProductBasePage
	{
		protected Script Script2;
		protected Script Script1;
		protected System.Web.UI.WebControls.Literal litCategoryName;
		protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
		protected ProductTypeDownList dropProductTypes;
		protected BrandCategoriesDropDownList dropBrandCategories;
		protected ProductLineDropDownList dropProductLines;
		protected TrimTextBox txtProductName;
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
		protected KindeditorControl editDescription;
		protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.CheckBox chkPenetration;
		protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
		protected ProductTagsLiteral litralProductTag;
		protected TrimTextBox txtProductTag;
		protected YesNoRadioButtonList radlEnableMemberDiscount;
		protected TrimTextBox txtTitle;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected TrimTextBox txtLowestSalePrice;
		protected TrimTextBox txtSalePrice;
		protected TrimTextBox txtMemberPrices;
		protected TrimTextBox txtPurchasePrice;
		protected TrimTextBox txtDistributorPrices;
		private int categoryId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				base.DoCallback();
				return;
			}
			if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
				CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
				if (category == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (!string.IsNullOrEmpty(this.litralProductTag.Text))
				{
					this.l_tags.Visible = true;
				}
				this.lnkEditCategory.NavigateUrl = "Supplier_ProductSelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.dropProductTypes.DataBind();
				this.dropProductTypes.SelectedValue = category.AssociatedProductType;
				this.dropProductLines.DataBind();
				this.dropBrandCategories.DataBind();
				this.txtProductCode.Text = (this.txtSku.Text = category.SKUPrefix + new System.Random(System.DateTime.Now.Millisecond).Next(1, 99999).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(5, '0'));
			}
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			decimal num;
			decimal num2;
			decimal num3;
			decimal? num4;
			decimal? marketPrice;
			int stock;
			int alertStock;
			decimal? num5;
			int lineId;
			if (!this.ValidateConverts(this.chkSkuEnabled.Checked, out num, out num2, out num3, out num4, out marketPrice, out stock, out alertStock, out num5, out lineId))
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
					this.ShowMsg("分销商采购价必须要小于其最低零售价", false);
					return;
				}
			}
			string text = this.editDescription.Text;
			if (this.ckbIsDownPic.Checked)
			{
				text = base.DownRemotePic(text);
			}
			ProductInfo productInfo = new ProductInfo
			{
				CategoryId = this.categoryId,
				TypeId = this.dropProductTypes.SelectedValue,
				ProductName = this.txtProductName.Text,
				ProductCode = this.txtProductCode.Text,
				LineId = lineId,
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
				BrandId = this.dropBrandCategories.SelectedValue,
				MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
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
			ValidationResults validationResults = Validation.Validate<ProductInfo>(productInfo, new string[]
			{
				"AddProduct"
			});
			if (!validationResults.IsValid)
			{
				this.ShowMsg(validationResults);
				return;
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
			productInfo.PenetrationStatus = PenetrationStatus.Notyet;
			productInfo.SaleStatus = ProductSaleStatus.OnStock;
			ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, dictionary, attrs, list);
			if (productActionStatus == ProductActionStatus.Success)
			{
				Methods.Supplier_PtUpdate(productInfo.ProductId, 0, "仓库中", Hidistro.Membership.Context.HiContext.Current.User.UserId, Hidistro.Membership.Context.HiContext.Current.User.Username);
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('添加成功，您可以在商品列表操作提交审核操作');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
				return;
			}
			if (productActionStatus == ProductActionStatus.AttributeError)
			{
				this.ShowMsg("添加商品失败，保存商品属性时出错", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.DuplicateName)
			{
				this.ShowMsg("添加商品失败，商品名称不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.DuplicateSKU)
			{
				this.ShowMsg("添加商品失败，商家编码不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.SKUError)
			{
				this.ShowMsg("添加商品失败，商家编码不能重复", false);
				return;
			}
			if (productActionStatus == ProductActionStatus.ProductTagEroor)
			{
				this.ShowMsg("添加商品失败，保存商品标签时出错", false);
				return;
			}
			this.ShowMsg("添加商品失败，未知错误", false);
		}
		private bool ValidateConverts(bool skuEnabled, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out decimal? weight, out int lineId)
		{
			string text = string.Empty;
			costPrice = null;
			marketPrice = null;
			weight = null;
			lineId = 0;
			alertStock = 0;
			stock = 0;
			purchasePrice = (lowestSalePrice = (salePrice = 0m));
			if (!this.dropProductLines.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择商品所属的产品线");
			}
			else
			{
				lineId = this.dropProductLines.SelectedValue.Value;
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
	}
}
