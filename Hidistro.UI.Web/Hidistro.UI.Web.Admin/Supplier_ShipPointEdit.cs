using Hidistro.ControlPanel.Store;
using Hidistro.Entities;
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
	public class Supplier_ShipPointEdit : AdminPage
	{
		private int userId;
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected RoleDropDownList dropRole;
		protected System.Web.UI.WebControls.TextBox txtprivateEmail;
		protected KindeditorControl fkRemark;
		protected FormatedTimeLabel lblRegsTimeValue;
		protected FormatedTimeLabel lblLastLoginTimeValue;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZip;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.Button btnEditProfile;
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
					this.ShowMsg("匿名用户或非区域发货点用户不能编辑", false);
					return;
				}
				this.GetAccountInfo(manager);
				this.GetPersonaInfo(manager);
                this.fkRemark.Text = manager.Comment;
				System.Data.DataTable dataTable = Methods.Supplier_SupGet(this.userId);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					System.Data.DataRow dataRow = dataTable.Rows[0];
					if (dataRow["Supplier_RegionId"] != System.DBNull.Value)
					{
						this.rsddlRegion.SetSelectedRegionId(new int?((int)dataTable.Rows[0]["Supplier_RegionId"]));
					}
					if (dataRow["Supplier_RealName"] != System.DBNull.Value)
					{
						this.txtRealName.Text = (string)dataRow["Supplier_RealName"];
					}
					if (dataRow["Supplier_Address"] != System.DBNull.Value)
					{
						this.txtAddress.Text = (string)dataRow["Supplier_Address"];
					}
					if (dataRow["Supplier_Zipcode"] != System.DBNull.Value)
					{
						this.txtZip.Text = (string)dataRow["Supplier_Zipcode"];
					}
					if (dataRow["Supplier_TelPhone"] != System.DBNull.Value)
					{
						this.txtPhone.Text = (string)dataRow["Supplier_TelPhone"];
					}
					if (dataRow["Supplier_CellPhone"] != System.DBNull.Value)
					{
						this.txtCellPhone.Text = (string)dataRow["Supplier_CellPhone"];
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
			Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
			manager.Email = this.txtprivateEmail.Text;
			if (!this.ValidationManageEamilr(manager))
			{
				return;
			}
			string[] userRoleNames = Hidistro.Membership.Core.RoleHelper.GetUserRoleNames(manager.Username);
			string[] array2 = userRoleNames;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!Hidistro.Membership.Core.RoleHelper.IsBuiltInRole(text) || string.Compare(text, "SystemAdministrator") == 0)
				{
					Hidistro.Membership.Core.RoleHelper.RemoveUserFromRole(manager.Username, text);
				}
			}
			Hidistro.Membership.Core.RoleHelper.AddUserToRole(manager.Username, "区域发货点");
			if (ManagerHelper.Update(manager))
			{
				Methods.Supplier_ShipPointUpdate(this.userId, this.fkRemark.Text, new int?(value), selectedRegions, this.txtRealName.Text.Trim(), this.txtAddress.Text.Trim(), this.txtZip.Text.Trim(), this.txtCellPhone.Text.Trim(), this.txtPhone.Text.Trim());
				this.ShowMsg("成功修改了当前区域发货点的个人资料", true);
				return;
			}
			this.ShowMsg("当前区域发货点的个人信息修改失败", false);
		}
		private void GetAccountInfo(Hidistro.Membership.Context.SiteManager user)
		{
			this.lblLoginNameValue.Text = user.Username;
			this.lblRegsTimeValue.Time = user.CreateDate;
			this.lblLastLoginTimeValue.Time = user.LastLoginDate;
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
