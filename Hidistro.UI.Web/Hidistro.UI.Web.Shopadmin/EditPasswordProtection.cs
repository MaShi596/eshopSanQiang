using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditPasswordProtection : DistributorPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl ulOld;
		protected System.Web.UI.WebControls.Literal litOldQuestion;
		protected System.Web.UI.WebControls.TextBox txtOldAnswer;
		protected System.Web.UI.WebControls.TextBox txtNewQuestion;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewQuestionTip;
		protected System.Web.UI.WebControls.TextBox txtNewAnswer;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewAnswerTip;
		protected System.Web.UI.WebControls.Button btnEditProtection;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnEditProtection.Click += new System.EventHandler(this.btnEditProtection_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.LoadOldControl();
			}
		}
		private void LoadOldControl()
		{
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(Hidistro.Membership.Context.HiContext.Current.User.UserId, false);
			if (user != null)
			{
				this.ulOld.Visible = !string.IsNullOrEmpty(user.PasswordQuestion);
				this.litOldQuestion.Text = user.PasswordQuestion;
				this.txtOldAnswer.Text = string.Empty;
				this.txtNewQuestion.Text = string.Empty;
				this.txtNewAnswer.Text = string.Empty;
			}
		}
		private void btnEditProtection_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtNewQuestion.Text))
			{
				this.ShowMsg("请输入新密保问题", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtNewAnswer.Text))
			{
				this.ShowMsg("请输入新密保答案", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			if (string.IsNullOrEmpty(distributor.PasswordQuestion))
			{
				if (distributor.ChangePasswordQuestionAndAnswer(this.txtNewQuestion.Text.Trim(), this.txtNewAnswer.Text.Trim()))
				{
					Hidistro.Membership.Context.Users.ClearUserCache(distributor);
					this.LoadOldControl();
					this.ShowMsg("成功修改了密码答案", true);
					return;
				}
				this.ShowMsg("修改密码答案失败", false);
				return;
			}
			else
			{
				if (distributor.ChangePasswordQuestionAndAnswer(this.txtOldAnswer.Text.Trim(), this.txtNewQuestion.Text.Trim(), this.txtNewAnswer.Text.Trim()))
				{
					Hidistro.Membership.Context.Users.ClearUserCache(distributor);
					this.LoadOldControl();
					this.ShowMsg("成功修改了密码答案", true);
					return;
				}
				this.ShowMsg("修改密码答案失败，可能是您原来的问题答案输入错误", false);
				return;
			}
		}
	}
}
