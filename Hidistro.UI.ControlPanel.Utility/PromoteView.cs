using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using kindeditor.Net;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	[ParseChildren(true)]
	public class PromoteView : TemplatedWebControl
	{
		private TextBox txtPromoteSalesName;
		private KindeditorControl fckDescription;
		private PromotionInfo promotion = null;
		private string errors;
		private bool isValid = true;
		public PromotionInfo Item
		{
			get
			{
				this.errors = string.Empty;
				PromotionInfo promotionInfo = new PromotionInfo();
				promotionInfo.Name = this.txtPromoteSalesName.Text;
				promotionInfo.Description = this.fckDescription.Text;
				if (string.IsNullOrEmpty(promotionInfo.Description))
				{
					this.isValid = false;
					this.errors += "促销活详细信息为必填项，请填写好";
				}
				return promotionInfo;
			}
			set
			{
				this.promotion = value;
			}
		}
		public string CurrentErrors
		{
			get
			{
				return this.errors;
			}
		}
		public bool IsValid
		{
			get
			{
				return this.isValid;
			}
		}
		protected override void AttachChildControls()
		{
			this.txtPromoteSalesName = (TextBox)this.FindControl("txtPromoteSalesName");
			this.fckDescription = (KindeditorControl)this.FindControl("fckDescription");
			if (!this.Page.IsPostBack && this.promotion != null)
			{
				this.txtPromoteSalesName.Text = this.promotion.Name;
                this.fckDescription.Text = this.promotion.Description;
			}
		}
		public void Reset()
		{
			this.txtPromoteSalesName.Text = string.Empty;
            this.fckDescription.Text = string.Empty;
		}
	}
}
