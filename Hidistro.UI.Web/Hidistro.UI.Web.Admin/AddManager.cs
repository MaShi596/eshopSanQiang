using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class AddManager : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtPasswordagain;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected RoleDropDownList dropRole;
		protected System.Web.UI.WebControls.Button btnCreate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Core.Enums.CreateUserStatus createUserStatus = Hidistro.Membership.Core.Enums.CreateUserStatus.UnknownFailure;
			Hidistro.Membership.Context.SiteManager siteManager = new Hidistro.Membership.Context.SiteManager();
			siteManager.IsApproved = true;
			siteManager.Username = this.txtUserName.Text.Trim();
			siteManager.Email = this.txtEmail.Text.Trim();
			siteManager.Password = this.txtPassword.Text.Trim();
			siteManager.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
				return;
			}
			if (!this.ValidationAddManager(siteManager))
			{
				return;
			}
			try
			{
				string text = this.dropRole.SelectedItem.Text;
				if (string.Compare(text, "超级管理员") == 0)
				{
					text = "SystemAdministrator";
				}
				createUserStatus = ManagerHelper.Create(siteManager, text);
			}
			catch (Hidistro.Membership.Core.CreateUserException ex)
			{
				createUserStatus = ex.CreateUserStatus;
			}
			switch (createUserStatus)
			{
			case Hidistro.Membership.Core.Enums.CreateUserStatus.UnknownFailure:
				this.ShowMsg("未知错误", false);
				return;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.Created:
				this.txtEmail.Text = string.Empty;
				this.txtUserName.Text = string.Empty;
				this.ShowMsg("成功添加了一个管理员", true);
				break;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateUsername:
				this.ShowMsg("您输入的用户名已经被注册使用", false);
				return;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateEmailAddress:
				this.ShowMsg("您输入的电子邮件地址已经被注册使用", false);
				return;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidFirstCharacter:
			case Hidistro.Membership.Core.Enums.CreateUserStatus.Updated:
			case Hidistro.Membership.Core.Enums.CreateUserStatus.Deleted:
			case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidQuestionAnswer:
				break;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.DisallowedUsername:
				this.ShowMsg("用户名被禁止注册", false);
				return;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidPassword:
				this.ShowMsg("无效的密码", false);
				return;
			case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidEmail:
				this.ShowMsg("无效的电子邮件地址", false);
				return;
			default:
				return;
			}
		}
		private bool ValidationAddManager(Hidistro.Membership.Context.SiteManager siteManager)
		{
			bool flag = true;
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteManager>(siteManager, new string[]
			{
				"ValManagerName"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				flag = false;
			}
			validationResults = Validation.Validate<Hidistro.Membership.Context.SiteManager>(siteManager, new string[]
			{
				"ValManagerPassword"
			});
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current2 in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current2.Message);
				}
				flag = false;
			}
			validationResults = Validation.Validate<Hidistro.Membership.Context.SiteManager>(siteManager, new string[]
			{
				"ValManagerEmail"
			});
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current3 in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current3.Message);
				}
				flag = false;
			}
			if (!flag)
			{
				this.ShowMsg(text, false);
			}
			return flag;
		}
	}
}
