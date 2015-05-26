using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyGifts : DistributorPage
	{
		private int Id;
		protected System.Web.UI.WebControls.TextBox txtGiftName;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox ckpromotion;
		protected ImageUploader uploader1;
		protected System.Web.UI.WebControls.Label lblUnit;
		protected System.Web.UI.WebControls.Label lblPurchasePrice;
		protected System.Web.UI.WebControls.Label lblMarketPrice;
		protected System.Web.UI.WebControls.TextBox txtNeedPoint;
		protected System.Web.UI.WebControls.Label lblShortDescription;
		protected System.Web.UI.WebControls.Label fcDescription;
		protected System.Web.UI.WebControls.TextBox txtGiftTitle;
		protected System.Web.UI.WebControls.TextBox txtTitleKeywords;
		protected System.Web.UI.WebControls.TextBox txtTitleDescription;
		protected System.Web.UI.WebControls.Button btnUpdate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["GiftId"], out this.Id))
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
			GiftInfo myGiftsDetails = SubsiteGiftHelper.GetMyGiftsDetails(this.Id);
			this.txtGiftName.Text = Globals.HtmlDecode(myGiftsDetails.Name);
			this.lblPurchasePrice.Text = string.Format("{0:F2}", myGiftsDetails.PurchasePrice);
			this.txtNeedPoint.Text = myGiftsDetails.NeedPoint.ToString();
			if (!string.IsNullOrEmpty(myGiftsDetails.Unit))
			{
				this.lblUnit.Text = myGiftsDetails.Unit;
			}
			if (myGiftsDetails.MarketPrice.HasValue)
			{
				this.lblMarketPrice.Text = string.Format("{0:F2}", myGiftsDetails.MarketPrice);
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.ShortDescription))
			{
				this.lblShortDescription.Text = Globals.HtmlDecode(myGiftsDetails.ShortDescription);
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.LongDescription))
			{
				this.fcDescription.Text = myGiftsDetails.LongDescription;
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.Title))
			{
				this.txtGiftTitle.Text = Globals.HtmlDecode(myGiftsDetails.Title);
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Description))
			{
				this.txtTitleDescription.Text = Globals.HtmlDecode(myGiftsDetails.Meta_Description);
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Keywords))
			{
				this.txtTitleKeywords.Text = Globals.HtmlDecode(myGiftsDetails.Meta_Keywords);
			}
			if (!string.IsNullOrEmpty(myGiftsDetails.ImageUrl))
			{
				this.uploader1.UploadedImageUrl = myGiftsDetails.ImageUrl;
			}
			if (myGiftsDetails.IsPromotion)
			{
				this.ckpromotion.Checked = true;
			}
		}
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			GiftInfo giftInfo = new GiftInfo();
			int needPoint;
			if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
			{
				this.ShowMsg("兑换所需积分不能为空，大小0-10000之间", false);
				return;
			}
			giftInfo.GiftId = this.Id;
			giftInfo.NeedPoint = needPoint;
			giftInfo.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
			giftInfo.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
			giftInfo.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
			giftInfo.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
			giftInfo.IsPromotion = this.ckpromotion.Checked;
			Globals.EntityCoding(giftInfo, false);
			if (SubsiteGiftHelper.UpdateMyGifts(giftInfo))
			{
				this.ShowMsg("成功修改了一件礼品的基本信息", true);
				return;
			}
			this.ShowMsg("修改件礼品的基本信息失败", true);
		}
	}
}
