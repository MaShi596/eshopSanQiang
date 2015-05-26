using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ReplyMyProductConsultations : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litUserName;
		protected FormatedTimeLabel lblTime;
		protected System.Web.UI.WebControls.Literal litConsultationText;
		protected KindeditorControl fckReplyText;
		protected System.Web.UI.WebControls.Button btnReplyProductConsultation;
		private int consultationId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyProductConsultation.Click += new System.EventHandler(this.btnReplyProductConsultation_Click);
			if (!this.Page.IsPostBack)
			{
				ProductConsultationInfo productConsultation = SubsiteCommentsHelper.GetProductConsultation(this.consultationId);
				if (productConsultation == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litUserName.Text = productConsultation.UserName;
				this.litConsultationText.Text = productConsultation.ConsultationText;
				this.lblTime.Time = productConsultation.ConsultationDate;
			}
		}
		protected void btnReplyProductConsultation_Click(object sender, System.EventArgs e)
		{
			ProductConsultationInfo productConsultation = SubsiteCommentsHelper.GetProductConsultation(this.consultationId);
			if (string.IsNullOrEmpty(this.fckReplyText.Text))
			{
				productConsultation.ReplyText = null;
			}
			else
			{
				productConsultation.ReplyText = this.fckReplyText.Text;
			}
			productConsultation.ReplyUserId = new int?(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			productConsultation.ReplyDate = new System.DateTime?(System.DateTime.Now);
			ValidationResults validationResults = Validation.Validate<ProductConsultationInfo>(productConsultation, new string[]
			{
				"Reply"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (SubsiteCommentsHelper.ReplyProductConsultation(productConsultation))
			{
                this.fckReplyText.Text = string.Empty;
				this.ShowMsg("成功回复了选择的商品咨询", true);
				return;
			}
			this.ShowMsg("回复商品咨询失败", false);
		}
	}
}
