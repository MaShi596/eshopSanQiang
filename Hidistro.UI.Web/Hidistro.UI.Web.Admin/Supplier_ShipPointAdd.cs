using Hidistro.ControlPanel.Store;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_ShipPointAdd : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtPasswordagain;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected RoleDropDownList dropRole;
		protected KindeditorControl fkRemark;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZip;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtPhone;
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
			siteManager.Email = "supplier_" + System.DateTime.Now.Ticks.ToString() + "@tom.com";
			siteManager.Password = this.txtPassword.Text.Trim();
			siteManager.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			siteManager.Comment = this.fkRemark.Text;
			if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
				return;
			}
			if (!this.ValidationAddManager(siteManager))
			{
				return;
			}
			if (!this.rsddlRegion.GetSelectedRegionId().HasValue || string.IsNullOrEmpty(this.txtRealName.Text.Trim()) || string.IsNullOrEmpty(this.txtAddress.Text.Trim()) || string.IsNullOrEmpty(this.txtZip.Text.Trim()) || string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
			{
				this.ShowMsg("错误:收货人、所在区域、具体地址、邮编、手机必填", false);
				return;
			}
			int value = this.rsddlRegion.GetSelectedRegionId().Value;
			string selectedRegions = this.rsddlRegion.SelectedRegions;
			string[] array = selectedRegions.Split("，".ToCharArray());
			if (array.Length == 1 && RegionHelper.GetCitys(value).Count > 0)
			{
				this.ShowMsg("所在区域必填填写完整", false);
				return;
			}
			if (array.Length == 2 && RegionHelper.GetCountys(value).Count > 0)
			{
				this.ShowMsg("所在区域必填填写完整", false);
				return;
			}
			try
			{
				createUserStatus = ManagerHelper.Create(siteManager, "区域发货点");
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
				Methods.Supplier_ShipPointUpdate(siteManager.UserId, this.fkRemark.Text, new int?(value), selectedRegions, this.txtRealName.Text.Trim(), this.txtAddress.Text.Trim(), this.txtZip.Text.Trim(), this.txtCellPhone.Text.Trim(), this.txtPhone.Text.Trim());
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('添加成功');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
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
