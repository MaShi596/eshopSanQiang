using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorRequests)]
	public class AcceptDistributorRequest : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litName;
		protected DistributorGradeDropDownList dropDistributorGrade;
		protected ProductLineCheckBoxList chklProductLine;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnAddDistrbutor;
		private int userId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAddDistrbutor.Click += new System.EventHandler(this.btnAddDistrbutor_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
				if (distributor == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litName.Text = distributor.Username;
				this.dropDistributorGrade.AllowNull = false;
				this.dropDistributorGrade.DataBind();
				this.chklProductLine.DataBind();
			}
		}
		private void btnAddDistrbutor_Click(object sender, System.EventArgs e)
		{
			if (this.chklProductLine.SelectedValue.Count == 0)
			{
				this.ShowMsg("至少选择一个产品线", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
			distributor.GradeId = this.dropDistributorGrade.SelectedValue.Value;
			distributor.Remark = this.txtRemark.Text;
			distributor.IsApproved = true;
			if (!this.ValidationDistributor(distributor))
			{
				return;
			}
			if (DistributorHelper.AcceptDistributorRequest(distributor, this.chklProductLine.SelectedValue))
			{
				Messenger.AcceptRequest(distributor);
				this.CloseWindow();
			}
		}
		private bool ValidationDistributor(Hidistro.Membership.Context.Distributor distributor)
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
	}
}
