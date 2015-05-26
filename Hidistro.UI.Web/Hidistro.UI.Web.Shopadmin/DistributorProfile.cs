using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class DistributorProfile : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.TextBox txtCompanyName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCompanyNameTip;
		protected System.Web.UI.WebControls.TextBox txtprivateEmail;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtprivateEmailTip;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtAddressTip;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtZipcodeTip;
		protected System.Web.UI.WebControls.TextBox txtQQ;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtQQTip;
		protected System.Web.UI.WebControls.TextBox txtWangwang;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtWangwangTip;
		protected System.Web.UI.WebControls.TextBox txtMSN;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMSNTip;
		protected System.Web.UI.WebControls.TextBox txtTel;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTelTip;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCellPhoneTip;
		protected System.Web.UI.WebControls.Button btnSave;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
				if (distributor != null && distributor.UserRole == Hidistro.Membership.Core.Enums.UserRole.Distributor)
				{
					this.BindData(distributor);
				}
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidationInput())
			{
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			distributor.RealName = this.txtRealName.Text.Trim().Replace('"', '“');
			distributor.CompanyName = this.txtCompanyName.Text.Trim();
			if (this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				distributor.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
				distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
			}
			distributor.Email = this.txtprivateEmail.Text.Trim();
			distributor.Address = this.txtAddress.Text.Trim();
			distributor.Zipcode = this.txtZipcode.Text.Trim();
			distributor.QQ = this.txtQQ.Text.Trim();
			distributor.Wangwang = this.txtWangwang.Text.Trim();
			distributor.MSN = this.txtMSN.Text.Trim();
			distributor.TelPhone = this.txtTel.Text.Trim();
			distributor.CellPhone = this.txtCellPhone.Text.Trim();
			distributor.IsCreate = false;
			if (!this.ValidationDistributorRequest(distributor))
			{
				return;
			}
			if (SubsiteStoreHelper.UpdateDistributor(distributor))
			{
				this.ShowMsg("成功的修改了信息", true);
				return;
			}
			this.ShowMsg("修改失败", false);
		}
		private void BindData(Hidistro.Membership.Context.Distributor distributor)
		{
			this.txtRealName.Text = distributor.RealName;
			this.txtCompanyName.Text = distributor.CompanyName;
			this.txtprivateEmail.Text = distributor.Email;
			this.txtAddress.Text = distributor.Address;
			this.txtZipcode.Text = distributor.Zipcode;
			this.txtQQ.Text = distributor.QQ;
			this.txtWangwang.Text = distributor.Wangwang;
			this.txtMSN.Text = distributor.MSN;
			this.txtTel.Text = distributor.TelPhone;
			this.txtCellPhone.Text = distributor.CellPhone;
			this.rsddlRegion.SetSelectedRegionId(new int?(distributor.RegionId));
		}
		private bool ValidationDistributorRequest(Hidistro.Membership.Context.Distributor distributor)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.Distributor>(distributor, new string[]
			{
				"ValDistributor"
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
		public bool ValidationInput()
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.txtQQ.Text) && string.IsNullOrEmpty(this.txtWangwang.Text) && string.IsNullOrEmpty(this.txtMSN.Text))
			{
				text += "QQ,旺旺,MSN,三者必填其一";
			}
			if (string.IsNullOrEmpty(this.txtTel.Text) && string.IsNullOrEmpty(this.txtCellPhone.Text))
			{
				text += "<br/>固定电话和手机,二者必填其一";
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
