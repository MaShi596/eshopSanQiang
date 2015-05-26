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
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class EditGift : AdminPage
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
		protected System.Web.UI.WebControls.Button btnUpdate;
		private int giftId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadGift();
			}
		}
		private void LoadGift()
		{
			GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
			if (giftDetails == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			Globals.EntityCoding(giftDetails, false);
			this.txtGiftName.Text = Globals.HtmlDecode(giftDetails.Name);
			this.txtPurchasePrice.Text = string.Format("{0:F2}", giftDetails.PurchasePrice);
			this.txtNeedPoint.Text = giftDetails.NeedPoint.ToString();
			if (!string.IsNullOrEmpty(giftDetails.Unit))
			{
				this.txtUnit.Text = giftDetails.Unit;
			}
			if (giftDetails.CostPrice.HasValue)
			{
				this.txtCostPrice.Text = string.Format("{0:F2}", giftDetails.CostPrice);
			}
			if (giftDetails.MarketPrice.HasValue)
			{
				this.txtMarketPrice.Text = string.Format("{0:F2}", giftDetails.MarketPrice);
			}
			if (!string.IsNullOrEmpty(giftDetails.ShortDescription))
			{
				this.txtShortDescription.Text = Globals.HtmlDecode(giftDetails.ShortDescription);
			}
			if (!string.IsNullOrEmpty(giftDetails.LongDescription))
			{
                this.fcDescription.Text = giftDetails.LongDescription;
			}
			if (!string.IsNullOrEmpty(giftDetails.Title))
			{
				this.txtGiftTitle.Text = Globals.HtmlDecode(giftDetails.Title);
			}
			if (!string.IsNullOrEmpty(giftDetails.Meta_Description))
			{
				this.txtTitleDescription.Text = Globals.HtmlDecode(giftDetails.Meta_Description);
			}
			if (!string.IsNullOrEmpty(giftDetails.Meta_Keywords))
			{
				this.txtTitleKeywords.Text = Globals.HtmlDecode(giftDetails.Meta_Keywords);
			}
			if (!string.IsNullOrEmpty(giftDetails.ImageUrl))
			{
				this.uploader1.UploadedImageUrl = giftDetails.ImageUrl;
			}
			this.chkDownLoad.Checked = giftDetails.IsDownLoad;
			this.chkPromotion.Checked = giftDetails.IsPromotion;
		}
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
			new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			decimal? costPrice;
			decimal purchasePrice;
			decimal? marketPrice;
			int needPoint;
			if (!this.ValidateValues(out costPrice, out purchasePrice, out marketPrice, out needPoint))
			{
				return;
			}
			giftDetails.PurchasePrice = purchasePrice;
			giftDetails.CostPrice = costPrice;
			giftDetails.MarketPrice = marketPrice;
			giftDetails.NeedPoint = needPoint;
			giftDetails.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
			giftDetails.Unit = this.txtUnit.Text.Trim();
			giftDetails.ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim());
			giftDetails.LongDescription = this.fcDescription.Text.Trim();
			giftDetails.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
			giftDetails.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
			giftDetails.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
			giftDetails.IsDownLoad = this.chkDownLoad.Checked;
			giftDetails.IsPromotion = this.chkPromotion.Checked;
			giftDetails.ImageUrl = this.uploader1.UploadedImageUrl;
			giftDetails.ThumbnailUrl40 = this.uploader1.ThumbnailUrl40;
			giftDetails.ThumbnailUrl60 = this.uploader1.ThumbnailUrl60;
			giftDetails.ThumbnailUrl100 = this.uploader1.ThumbnailUrl100;
			giftDetails.ThumbnailUrl160 = this.uploader1.ThumbnailUrl160;
			giftDetails.ThumbnailUrl180 = this.uploader1.ThumbnailUrl180;
			giftDetails.ThumbnailUrl220 = this.uploader1.ThumbnailUrl220;
			giftDetails.ThumbnailUrl310 = this.uploader1.ThumbnailUrl310;
			giftDetails.ThumbnailUrl410 = this.uploader1.ThumbnailUrl410;
			ValidationResults validationResults = Validation.Validate<GiftInfo>(giftDetails, new string[]
			{
				"ValGift"
			});
			string text = string.Empty;
			if (giftDetails.PurchasePrice < giftDetails.CostPrice)
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
			GiftActionStatus giftActionStatus = GiftHelper.UpdateGift(giftDetails);
			GiftActionStatus giftActionStatus2 = giftActionStatus;
			switch (giftActionStatus2)
			{
			case GiftActionStatus.Success:
				this.ShowMsg("成功修改了一件礼品的基本信息", true);
				return;
			case GiftActionStatus.DuplicateName:
				this.ShowMsg("已经存在相同的礼品名称", false);
				return;
			case GiftActionStatus.DuplicateSKU:
				this.ShowMsg("已经存在相同的商家编码", false);
				return;
			default:
				if (giftActionStatus2 != GiftActionStatus.UnknowError)
				{
					return;
				}
				this.ShowMsg("未知错误", false);
				return;
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
