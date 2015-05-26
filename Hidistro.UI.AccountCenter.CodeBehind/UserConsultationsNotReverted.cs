using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserConsultationsNotReverted : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPtConsultationReply;
		private Pager pagerConsultationReply;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultationsNotReverted.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReply = (ThemedTemplatedList)this.FindControl("dlstPtConsultationReply");
			this.pagerConsultationReply = (Pager)this.FindControl("pagerConsultationReply");
			PageTitle.AddSiteNameTitle("咨询/未回复", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				this.BindPtConsultationReply();
			}
		}
		private void BindPtConsultationReply()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReply.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.User.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.NoReply;
			int totalRecords = 0;
			DataSet productConsultationsAndReplys = CommentsHelper.GetProductConsultationsAndReplys(productConsultationAndReplyQuery, out totalRecords);
			this.dlstPtConsultationReply.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
			this.dlstPtConsultationReply.DataBind();
            this.pagerConsultationReply.TotalRecords = totalRecords;
		}
	}
}
