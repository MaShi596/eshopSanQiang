using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_SupEdit : AdminPage
	{
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected Supplier_Drop_SupplierGrades drpSupplierGrades;
		protected RoleDropDownList dropRole;
		protected System.Web.UI.WebControls.TextBox txtprivateEmail;
		protected KindeditorControl fkRemark;
		protected FormatedTimeLabel lblRegsTimeValue;
		protected FormatedTimeLabel lblLastLoginTimeValue;
		protected System.Web.UI.WebControls.Button btnEditProfile;
		private int userId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
				Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
				if (manager == null)
				{
					this.ShowMsg("匿名用户或非供应商用户不能编辑", false);
					return;
				}
				this.GetAccountInfo(manager);
				this.GetPersonaInfo(manager);
                this.fkRemark.Text = manager.Comment;
				System.Data.DataTable dataTable = Methods.Supplier_SupGet(this.userId);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					System.Data.DataRow dataRow = dataTable.Rows[0];
					if (dataRow["Supplier_GradeId"] != System.DBNull.Value)
					{
						this.drpSupplierGrades.SelectedValue = dataRow["Supplier_GradeId"].ToString();
					}
				}
			}
		}
		private void btnEditProfile_Click(object sender, System.EventArgs e)
		{
			if (!this.Page.IsValid)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.drpSupplierGrades.SelectedValue))
			{
				this.ShowMsg("错误：供应商等级必选", false);
				return;
			}
			Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
			manager.Email = this.txtprivateEmail.Text;
			if (!this.ValidationManageEamilr(manager))
			{
				return;
			}
			string[] userRoleNames = Hidistro.Membership.Core.RoleHelper.GetUserRoleNames(manager.Username);
			string[] array = userRoleNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (!Hidistro.Membership.Core.RoleHelper.IsBuiltInRole(text) || string.Compare(text, "SystemAdministrator") == 0)
				{
					Hidistro.Membership.Core.RoleHelper.RemoveUserFromRole(manager.Username, text);
				}
			}
			Hidistro.Membership.Core.RoleHelper.AddUserToRole(manager.Username, "供应商");
			if (ManagerHelper.Update(manager))
			{
				Methods.Supplier_SupUpdate(this.userId, this.fkRemark.Text, int.Parse(this.drpSupplierGrades.SelectedValue));
				this.ShowMsg("成功修改了当前供应商的个人资料", true);
				return;
			}
			this.ShowMsg("当前供应商的个人信息修改失败", false);
		}
		private void GetAccountInfo(Hidistro.Membership.Context.SiteManager user)
		{
			this.lblLoginNameValue.Text = user.Username;
			this.lblRegsTimeValue.Time = user.CreateDate;
			this.lblLastLoginTimeValue.Time = user.LastLoginDate;
			string[] userRoleNames = Hidistro.Membership.Core.RoleHelper.GetUserRoleNames(user.Username);
			string[] array = userRoleNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (string.Compare(text, "SystemAdministrator") == 0)
				{
					this.dropRole.SelectedIndex = this.dropRole.Items.IndexOf(this.dropRole.Items.FindByText("超级供应商"));
				}
				if (Hidistro.Membership.Context.HiContext.Current.User.UserId == this.userId)
				{
					this.dropRole.Enabled = false;
				}
				if (!Hidistro.Membership.Core.RoleHelper.IsBuiltInRole(text))
				{
					this.dropRole.SelectedIndex = this.dropRole.Items.IndexOf(this.dropRole.Items.FindByText(text));
					return;
				}
			}
		}
		private void GetPersonaInfo(Hidistro.Membership.Context.SiteManager user)
		{
			this.txtprivateEmail.Text = user.Email;
		}
		private bool ValidationManageEamilr(Hidistro.Membership.Context.SiteManager siteManager)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteManager>(siteManager, new string[]
			{
				"ValManagerEmail"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
