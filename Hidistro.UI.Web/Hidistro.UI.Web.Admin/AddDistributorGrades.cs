using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
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
	[PrivilegeCheck(Privilege.AddDistributorGrade)]
	public class AddDistributorGrades : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtRankName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRankNameTip;
		protected System.Web.UI.WebControls.TextBox txtValue;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtValueTip;
		protected System.Web.UI.WebControls.TextBox txtRankDesc;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRankDescTip;
		protected System.Web.UI.WebControls.Button btnAddDistrbutor;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAddDistrbutor.Click += new System.EventHandler(this.btnAddDistrbutor_Click);
		}
		private void btnAddDistrbutor_Click(object sender, System.EventArgs e)
		{
			if (DistributorHelper.ExistGradeName(this.txtRankName.Text.Trim()))
			{
				this.ShowMsg("已经存在相同名称的分销商等级", false);
				return;
			}
			DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
			distributorGradeInfo.Name = this.txtRankName.Text.Trim();
			distributorGradeInfo.Description = this.txtRankDesc.Text.Trim();
			int discount;
			if (!int.TryParse(this.txtValue.Text, out discount) || this.txtValue.Text.Contains("."))
			{
				this.ShowMsg("等级折扣必须只能为正整数", false);
				return;
			}
			distributorGradeInfo.Discount = discount;
			if (!this.ValidationMemberGrade(distributorGradeInfo))
			{
				return;
			}
			if (DistributorHelper.AddDistributorGrade(distributorGradeInfo))
			{
				this.ResetText();
				this.ShowMsg("成功添加了一个分销商等级", true);
				return;
			}
			this.ShowMsg("添加分销商等级失败", false);
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
		private void ResetText()
		{
			this.txtRankName.Text = string.Empty;
			this.txtRankDesc.Text = string.Empty;
			this.txtValue.Text = string.Empty;
		}
	}
}
