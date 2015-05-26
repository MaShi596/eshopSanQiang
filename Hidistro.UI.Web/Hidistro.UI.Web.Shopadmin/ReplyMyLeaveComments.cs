using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ReplyMyLeaveComments : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Label litContent;
		protected KindeditorControl fckReplyContent;
		protected System.Web.UI.WebControls.Button btnReplyLeaveComments;
		private long LeaveId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["LeaveId"], out this.LeaveId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyLeaveComments.Click += new System.EventHandler(this.btnReplyLeaveComments_Click);
			if (!this.Page.IsPostBack)
			{
				LeaveCommentInfo leaveComment = SubsiteCommentsHelper.GetLeaveComment(this.LeaveId);
				if (leaveComment == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litTitle.Text = Globals.HtmlDecode(leaveComment.Title);
				this.litContent.Text = Globals.HtmlDecode(leaveComment.PublishContent);
				this.litUserName.Text = Globals.HtmlDecode(leaveComment.UserName);
			}
		}
		protected void btnReplyLeaveComments_Click(object sender, System.EventArgs e)
		{
			LeaveCommentReplyInfo leaveCommentReplyInfo = new LeaveCommentReplyInfo();
			leaveCommentReplyInfo.LeaveId = this.LeaveId;
			if (string.IsNullOrEmpty(this.fckReplyContent.Text))
			{
				leaveCommentReplyInfo.ReplyContent = null;
			}
			else
			{
				leaveCommentReplyInfo.ReplyContent = this.fckReplyContent.Text;
			}
			leaveCommentReplyInfo.UserId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
			ValidationResults validationResults = Validation.Validate<LeaveCommentReplyInfo>(leaveCommentReplyInfo, new string[]
			{
				"ValLeaveCommentReply"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			int num = SubsiteCommentsHelper.ReplyLeaveComment(leaveCommentReplyInfo);
			if (num > 0)
			{
				base.Response.Redirect(Globals.ApplicationPath + string.Format("/Shopadmin/comment/ReplyedLeaveCommentsSuccsed.aspx?leaveId={0}", this.LeaveId), true);
			}
			else
			{
				this.ShowMsg("回复客户留言失败", false);
			}
            this.fckReplyContent.Text = string.Empty;
		}
	}
}
