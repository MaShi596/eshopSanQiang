using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ReplyedLeaveCommentsSuccsed : DistributorPage
	{
		protected System.Web.UI.WebControls.Label lblUserName;
		protected FormatedTimeLabel litLeaveDate;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Literal litPublishContent;
		protected System.Web.UI.WebControls.DataList dtlistLeaveCommentsReply;
		protected System.Web.UI.WebControls.HyperLink hlReply;
		private long leaveId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["leaveId"], out this.leaveId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.dtlistLeaveCommentsReply.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtlistLeaveCommentsReply_DeleteCommand);
			if (!base.IsPostBack)
			{
				this.SetControl(this.leaveId);
				this.hlReply.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/comment/ReplyMyLeaveComments.aspx?LeaveId={0}", this.leaveId);
				this.BindList();
			}
		}
		private void SetControl(long leaveId)
		{
			LeaveCommentInfo leaveComment = SubsiteCommentsHelper.GetLeaveComment(leaveId);
			Globals.EntityCoding(leaveComment, false);
			this.litTitle.Text = leaveComment.Title;
			this.lblUserName.Text = leaveComment.UserName;
			this.litLeaveDate.Time = leaveComment.PublishDate;
			this.litPublishContent.Text = leaveComment.PublishContent;
		}
		private void dtlistLeaveCommentsReply_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			long leaveReplyId = System.Convert.ToInt64(this.dtlistLeaveCommentsReply.DataKeys[e.Item.ItemIndex]);
			if (SubsiteCommentsHelper.DeleteLeaveCommentReply(leaveReplyId))
			{
				this.ShowMsg("删除成功", true);
				this.BindList();
				return;
			}
			this.ShowMsg("删除失败，请重试", false);
		}
		private void BindList()
		{
			this.dtlistLeaveCommentsReply.DataSource = SubsiteCommentsHelper.GetReplyLeaveComments(this.leaveId);
			this.dtlistLeaveCommentsReply.DataBind();
		}
	}
}
