using ASPNET.WebControls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_SupGrades : AdminPage
	{
		protected Grid grdMemberRankList;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdMemberRankList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberRankList_RowDeleting);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindMemberRanks();
			}
		}
		private void grdMemberRankList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int auto = (int)this.grdMemberRankList.DataKeys[e.RowIndex].Value;
			int num = Methods.Supplier_SupplierGradeHasSupNum(auto);
			if (num > 0)
			{
				this.BindMemberRanks();
				this.ShowMsg("错误：该等级有供应商，不能直接删除", false);
				return;
			}
			Methods.Supplier_SupplierGradeDelete(auto);
			this.BindMemberRanks();
			this.ShowMsg("删除成功", true);
		}
		private void BindMemberRanks()
		{
			this.grdMemberRankList.DataSource = Methods.Supplier_SupplierGradeSGet();
			this.grdMemberRankList.DataBind();
		}
	}
}
