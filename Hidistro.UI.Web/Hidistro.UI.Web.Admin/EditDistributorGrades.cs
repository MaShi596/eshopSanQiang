using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditDistributorGrade)]
	public class EditDistributorGrades : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtRankName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRankNameTip;
		protected System.Web.UI.WebControls.TextBox txtValue;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtValueTip;
		protected System.Web.UI.WebControls.TextBox txtRankDesc;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRankDescTip;
		protected System.Web.UI.WebControls.Button btnEditDistrbutor;
		private int gradeId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnEditDistrbutor.Click += new System.EventHandler(this.btnEditDistrbutor_Click);
			if (!int.TryParse(base.Request.QueryString["GradeId"], out this.gradeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				DistributorGradeInfo distributorGradeInfo = DistributorHelper.GetDistributorGradeInfo(this.gradeId);
				if (distributorGradeInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(distributorGradeInfo, false);
				this.txtRankName.Text = distributorGradeInfo.Name;
				this.txtValue.Text = distributorGradeInfo.Discount.ToString();
				this.txtRankDesc.Text = distributorGradeInfo.Description;
			}
		}
		private void btnEditDistrbutor_Click(object sender, System.EventArgs e)
		{
			DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
			distributorGradeInfo.Name = this.txtRankName.Text.Trim();
			distributorGradeInfo.Description = this.txtRankDesc.Text.Trim();
			distributorGradeInfo.GradeId = this.gradeId;
			int discount;
			if (!int.TryParse(this.txtValue.Text, out discount) || this.txtValue.Text.Contains("."))
			{
				this.ShowMsg("等级折扣必须为正整数", false);
				return;
			}
			distributorGradeInfo.Discount = discount;
			if (!this.ValidationMemberGrade(distributorGradeInfo))
			{
				return;
			}
			if (DistributorHelper.UpdateDistributorGrade(distributorGradeInfo))
			{
				this.ShowMsg("修改分销商等级成功", true);
				return;
			}
			this.ShowMsg("修改分销商等级失败", false);
		}
		private bool ValidationMemberGrade(DistributorGradeInfo distributorGrade)
		{
			ValidationResults validationResults = Validation.Validate<DistributorGradeInfo>(distributorGrade, new string[]
			{
				"ValDistributorGrade"
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
