using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class AddGift : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtGiftName;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chkDownLoad;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chkPromotion;
		protected ImageUploader uploader1;
		protected System.Web.UI.WebControls.TextBox txtUnit;
		protected System.Web.UI.WebControls.TextBox txtCostPrice;
		protected System.Web.UI.WebControls.TextBox txtPurchasePrice;
		protected System.Web.UI.WebControls.TextBox txtMarketPrice;
		protected System.Web.UI.WebControls.TextBox txtNeedPoint;
		protected System.Web.UI.WebControls.TextBox txtShortDescription;
		protected KindeditorControl fcDescription;
		protected System.Web.UI.WebControls.TextBox txtGiftTitle;
		protected System.Web.UI.WebControls.TextBox txtTitleKeywords;
		protected System.Web.UI.WebControls.TextBox txtTitleDescription;
		protected System.Web.UI.WebControls.Button btnCreate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			decimal? costPrice;
			decimal purchasePrice;
			decimal? marketPrice;
			int needPoint;
			if (!this.ValidateValues(out costPrice, out purchasePrice, out marketPrice, out needPoint))
			{
				return;
			}
			GiftInfo giftInfo = new GiftInfo
			{
				PurchasePrice = purchasePrice,
				CostPrice = costPrice,
				MarketPrice = marketPrice,
				NeedPoint = needPoint,
				Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim()),
				Unit = this.txtUnit.Text.Trim(),
				ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim()),
				LongDescription = string.IsNullOrEmpty(this.fcDescription.Text) ? null : this.fcDescription.Text.Trim(),
				Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim()),
				Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim()),
				Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim()),
				IsDownLoad = this.chkDownLoad.Checked,
				IsPromotion = this.chkPromotion.Checked
			};
			giftInfo.ImageUrl = this.uploader1.UploadedImageUrl;
			giftInfo.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
			giftInfo.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
			giftInfo.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
			giftInfo.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
			giftInfo.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
			giftInfo.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
			giftInfo.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
			giftInfo.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
			ValidationResults validationResults = Validation.Validate<GiftInfo>(giftInfo, new string[]
			{
				"ValGift"
			});
			string text = string.Empty;
			if (giftInfo.PurchasePrice < giftInfo.CostPrice)
			{
				text += Formatter.FormatErrorMessage("礼品采购价不能小于成本价");
			}
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return;
			}
			GiftActionStatus giftActionStatus = GiftHelper.AddGift(giftInfo);
			if (giftActionStatus == GiftActionStatus.Success)
			{
				this.ShowMsg("成功的添加了一件礼品", true);
				return;
			}
			if (giftActionStatus == GiftActionStatus.UnknowError)
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			if (giftActionStatus == GiftActionStatus.DuplicateSKU)
			{
				this.ShowMsg("已经存在相同的商家编码", false);
				return;
			}
			if (giftActionStatus == GiftActionStatus.DuplicateName)
			{
				this.ShowMsg("已经存在相同的礼品名称", false);
			}
		}
		private bool ValidateValues(out decimal? costPrice, out decimal purchasePrice, out decimal? marketPrice, out int needPoint)
		{
			string text = string.Empty;
			costPrice = null;
			marketPrice = null;
			if (!string.IsNullOrEmpty(this.txtCostPrice.Text.Trim()))
			{
				decimal value;
				if (decimal.TryParse(this.txtCostPrice.Text.Trim(), out value))
				{
					costPrice = new decimal?(value);
				}
				else
				{
					text += Formatter.FormatErrorMessage("成本价金额无效，大小在10000000以内");
				}
			}
			if (!decimal.TryParse(this.txtPurchasePrice.Text.Trim(), out purchasePrice))
			{
				text += Formatter.FormatErrorMessage("采购价金额无效，大小在10000000以内");
			}
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text.Trim()))
			{
				decimal value2;
				if (decimal.TryParse(this.txtMarketPrice.Text.Trim(), out value2))
				{
					marketPrice = new decimal?(value2);
				}
				else
				{
					text += Formatter.FormatErrorMessage("市场参考价金额无效，大小在10000000以内");
				}
			}
			if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
			{
				text += Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
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
