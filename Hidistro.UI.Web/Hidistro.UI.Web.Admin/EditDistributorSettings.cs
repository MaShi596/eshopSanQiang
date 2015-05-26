using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditDistributor)]
	public class EditDistributorSettings : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litUserName;
		protected WangWangConversations WangWangConversations;
		protected DistributorGradeDropDownList drpDistributorGrade;
		protected ProductLineCheckBoxList chkListProductLine;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
		protected System.Web.UI.WebControls.Button btnEditDistributorSettings;
		private int userId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnEditDistributorSettings.Click += new System.EventHandler(this.btnEditDistributorSettings_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				this.drpDistributorGrade.AllowNull = false;
				this.drpDistributorGrade.DataBind();
				this.chkListProductLine.DataBind();
				this.LoadControl();
			}
		}
		private void LoadControl()
		{
			Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
			if (distributor == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.litUserName.Text = distributor.Username;
			this.WangWangConversations.WangWangAccounts = distributor.Wangwang;
			this.drpDistributorGrade.SelectedValue = new int?(distributor.GradeId);
			this.txtRemark.Text = distributor.Remark;
			System.Collections.Generic.IList<int> distributorProductLines = DistributorHelper.GetDistributorProductLines(this.userId);
			this.chkListProductLine.SelectedValue = distributorProductLines;
		}
		private void btnEditDistributorSettings_Click(object sender, System.EventArgs e)
		{
			if (this.txtRemark.Text.Trim().Length > 300)
			{
				this.ShowMsg("合作备忘录的长度限制在300个字符以内", false);
				this.chkListProductLine.DataBind();
				this.LoadControl();
				return;
			}
			if (this.chkListProductLine.SelectedValue.Count == 0)
			{
				this.ShowMsg("请选择至少一个授权产品线", false);
				this.chkListProductLine.DataBind();
				this.LoadControl();
				return;
			}
			if (DistributorHelper.UpdateDistributorSettings(this.userId, this.drpDistributorGrade.SelectedValue.Value, this.txtRemark.Text.Trim()))
			{
				if (DistributorHelper.AddDistributorProductLines(this.userId, this.chkListProductLine.SelectedValue))
				{
					ProductHelper.DeleteNotinProductLines(this.userId);
					this.ShowMsg("成功的修改了分销商基本设置", true);
					return;
				}
			}
			else
			{
				this.ShowMsg("修改失败", false);
			}
		}
	}
}
