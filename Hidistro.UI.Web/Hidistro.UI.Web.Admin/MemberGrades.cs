using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberGrades)]
	public class MemberGrades : AdminPage
	{
		protected Grid grdMemberRankList;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdMemberRankList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberRankList_RowDeleting);
			this.grdMemberRankList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdMemberRankList_RowCommand);
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
			int gradeId = (int)this.grdMemberRankList.DataKeys[e.RowIndex].Value;
			if (MemberHelper.DeleteMemberGrade(gradeId))
			{
				this.BindMemberRanks();
				this.ShowMsg("已经成功删除选择的会员等级", true);
				return;
			}
			this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
		}
		private void grdMemberRankList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "SetYesOrNo")
			{
				System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
				int num = (int)this.grdMemberRankList.DataKeys[gridViewRow.RowIndex].Value;
				MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(num);
				if (!memberGrade.IsDefault)
				{
					MemberHelper.SetDefalutMemberGrade(num);
					this.BindMemberRanks();
				}
			}
		}
		private void BindMemberRanks()
		{
			this.grdMemberRankList.DataSource = MemberHelper.GetMemberGrades();
			this.grdMemberRankList.DataBind();
		}
	}
}
