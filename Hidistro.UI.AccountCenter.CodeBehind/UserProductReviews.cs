using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserProductReviews : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPts;
		private Pager pager;
		private System.Web.UI.WebControls.Literal litReviewCount;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProductReviews.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dlstPts = (ThemedTemplatedList)this.FindControl("dlstPts");
			this.pager = (Pager)this.FindControl("pager");
			this.litReviewCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewCount");
			PageTitle.AddSiteNameTitle("我参与的评论", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				if (this.litReviewCount != null)
				{
					this.litReviewCount.Text = CommentsHelper.GetUserProductReviewsCount().ToString();
				}
				this.BindPtAndReviewsAndReplys();
			}
		}
		private void BindPtAndReviewsAndReplys()
		{
			UserProductReviewAndReplyQuery userProductReviewAndReplyQuery = new UserProductReviewAndReplyQuery();
			userProductReviewAndReplyQuery.PageIndex = this.pager.PageIndex;
			userProductReviewAndReplyQuery.PageSize = this.pager.PageSize;
			int totalRecords = 0;
			DataSet userProductReviewsAndReplys = CommentsHelper.GetUserProductReviewsAndReplys(userProductReviewAndReplyQuery, out totalRecords);
			this.dlstPts.DataSource = userProductReviewsAndReplys.Tables[0].DefaultView;
			this.dlstPts.DataBind();
            this.pager.TotalRecords = totalRecords;
		}
	}
}
