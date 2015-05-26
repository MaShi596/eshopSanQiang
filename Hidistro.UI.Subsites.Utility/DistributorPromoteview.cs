using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorPromoteview : TemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtPromoteSalesName;
		private KindeditorControl fckDescription;
		private PromotionInfo promotion = null;
		private string errors;
		private bool isValid = true;
		public PromotionInfo Item
		{
			get
			{
				this.errors = string.Empty;
				return new PromotionInfo
				{
					Name = this.txtPromoteSalesName.Text,
					Description = this.fckDescription.Text
				};
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
			this.txtPromoteSalesName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPromoteSalesName");
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
