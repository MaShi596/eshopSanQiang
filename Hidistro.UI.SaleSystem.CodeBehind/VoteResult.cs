using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VoteResult : HtmlTemplatedWebControl
	{
		private long voteId;
		private System.Web.UI.WebControls.Label lblTotal;
		private System.Web.UI.WebControls.Literal lblVoteName;
		private ThemedTemplatedRepeater rptVoteItem;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VoteResult.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
			{
				base.GotoResourceNotFound();
			}
			this.lblTotal = (System.Web.UI.WebControls.Label)this.FindControl("lblTotal");
			this.lblVoteName = (System.Web.UI.WebControls.Literal)this.FindControl("lblVoteName");
			this.rptVoteItem = (ThemedTemplatedRepeater)this.FindControl("rptVoteItem");
			if (!this.Page.IsPostBack)
			{
				VoteInfo voteById = CommentBrowser.GetVoteById(this.voteId);
				if (voteById != null)
				{
					PageTitle.AddSiteNameTitle(voteById.VoteName, Hidistro.Membership.Context.HiContext.Current.Context);
					this.Vote(voteById);
					VoteInfo voteById2 = CommentBrowser.GetVoteById(this.voteId);
					this.BindVoteItem(voteById2);
				}
			}
		}
		private void BindVoteItem(VoteInfo vote)
		{
			int num = 0;
			if (vote != null)
			{
				this.lblVoteName.Text = vote.VoteName;
				System.Collections.Generic.IList<VoteItemInfo> voteItems = CommentBrowser.GetVoteItems(this.voteId);
				for (int i = 0; i < voteItems.Count; i++)
				{
					if (vote.VoteCounts != 0)
					{
						voteItems[i].Percentage = voteItems[i].ItemCount / vote.VoteCounts * 100m;
						num += voteItems[i].ItemCount;
					}
					else
					{
						voteItems[i].Percentage = 0m;
					}
				}
				this.rptVoteItem.DataSource = voteItems;
				this.rptVoteItem.DataBind();
				if (this.lblTotal != null)
				{
					this.lblTotal.Text = num.ToString();
				}
			}
		}
		private void Vote(VoteInfo vote)
		{
			System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies[this.voteId.ToString()];
			if (httpCookie == null)
			{
				if (this.Page.Request.Params["VoteItemId"] != null)
				{
					string text = this.Page.Request.Params["VoteItemId"];
					if (!string.IsNullOrEmpty(text))
					{
						string[] array = text.Split(new char[]
						{
							','
						});
						for (int i = 0; i < array.Length; i++)
						{
							if (!string.IsNullOrEmpty(array[i]) && i + 1 <= vote.MaxCheck)
							{
								long voteItemId = System.Convert.ToInt64(array[i]);
								VoteItemInfo voteItemById = CommentBrowser.GetVoteItemById(voteItemId);
								if (vote.VoteId == voteItemById.VoteId)
								{
									CommentBrowser.Vote(voteItemId);
								}
							}
						}
						httpCookie = new System.Web.HttpCookie(this.voteId.ToString());
						httpCookie.Expires = System.DateTime.Now.AddYears(100);
						httpCookie.Value = this.voteId.ToString();
						Hidistro.Membership.Context.HiContext.Current.Context.Response.Cookies.Add(httpCookie);
						this.ShowMessage("投票成功", true);
					}
				}
			}
			else
			{
				if (httpCookie != null && !string.IsNullOrEmpty(this.Page.Request.QueryString["VoteItemId"].ToString()))
				{
					this.ShowMessage("该用户已经投过票了", false);
				}
			}
		}
	}
}
