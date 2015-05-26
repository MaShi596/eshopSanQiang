using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ForgotPasswordSuccess : HtmlTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivEmailMessage;
		private System.Web.UI.WebControls.Literal litUserNameEmail;
		private System.Web.UI.WebControls.Literal litEmail;
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivAnswerMessage;
		private System.Web.UI.WebControls.Literal litUserNameAnswer;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ForgotPasswordSuccess.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.htmDivEmailMessage = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivEmailMessage");
			this.litUserNameEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litUserNameEmail");
			this.litEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litEmail");
			this.htmDivAnswerMessage = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivAnswerMessage");
			this.litUserNameAnswer = (System.Web.UI.WebControls.Literal)this.FindControl("litUserNameAnswer");
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				text = this.Page.Request.QueryString["UserName"];
			}
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Email"]))
			{
				text2 = this.Page.Request.QueryString["Email"];
			}
			PageTitle.AddSiteNameTitle("找回密码", Hidistro.Membership.Context.HiContext.Current.Context);
			this.htmDivEmailMessage.Visible = false;
			this.htmDivAnswerMessage.Visible = false;
			if (!string.IsNullOrEmpty(text2))
			{
				this.htmDivEmailMessage.Visible = true;
				this.litUserNameEmail.Text = text;
				this.litEmail.Text = text2;
			}
			else
			{
				if (!string.IsNullOrEmpty(text))
				{
					this.htmDivAnswerMessage.Visible = true;
					this.litUserNameAnswer.Text = text;
				}
			}
		}
	}
}
