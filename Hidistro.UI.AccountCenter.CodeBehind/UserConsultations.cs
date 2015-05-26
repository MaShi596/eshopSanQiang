using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.AccountCenter.Profile;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserConsultations : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPtConsultationReplyed;
		private Pager pagerConsultationReplyed;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultations.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReplyed = (ThemedTemplatedList)this.FindControl("dlstPtConsultationReplyed");
			this.pagerConsultationReplyed = (Pager)this.FindControl("pagerConsultationReplyed");
			PageTitle.AddSiteNameTitle("咨询/已回复", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				PersonalHelper.ViewProductConsultations();
				this.BindPtConsultationReplyed();
			}
		}
		private void BindPtConsultationReplyed()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReplyed.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.User.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.Replyed;
			int totalRecords = 0;
			DataSet productConsultationsAndReplys = CommentsHelper.GetProductConsultationsAndReplys(productConsultationAndReplyQuery, out totalRecords);
			this.dlstPtConsultationReplyed.DataSource = productConsultationsAndReplys.Tables[0].DefaultView;
			this.dlstPtConsultationReplyed.DataBind();
            this.pagerConsultationReplyed.TotalRecords = totalRecords;
		}
	}
}
