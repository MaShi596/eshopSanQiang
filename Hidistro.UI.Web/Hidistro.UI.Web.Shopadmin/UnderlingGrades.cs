using ASPNET.WebControls;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class UnderlingGrades : DistributorPage
	{
		protected Grid grdUnderlingGrades;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdUnderlingGrades.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdUnderlingGrades_RowDeleting);
			this.grdUnderlingGrades.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdUnderlingGrades_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindUnderlingGrades();
			}
		}
		private void grdUnderlingGrades_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int gradeId = (int)this.grdUnderlingGrades.DataKeys[e.RowIndex].Value;
			if (UnderlingHelper.DeleteUnderlingGrade(gradeId))
			{
				this.BindUnderlingGrades();
				this.ShowMsg("已经成功删除选择的会员等级", true);
				return;
			}
			this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
		}
		private void grdUnderlingGrades_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "SetYesOrNo")
			{
				System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
				int defalutUnderlingGrade = (int)this.grdUnderlingGrades.DataKeys[gridViewRow.RowIndex].Value;
				UnderlingHelper.SetDefalutUnderlingGrade(defalutUnderlingGrade);
				this.BindUnderlingGrades();
			}
		}
		private void BindUnderlingGrades()
		{
			this.grdUnderlingGrades.DataSource = UnderlingHelper.GetUnderlingGrades();
			this.grdUnderlingGrades.DataBind();
		}
	}
}
