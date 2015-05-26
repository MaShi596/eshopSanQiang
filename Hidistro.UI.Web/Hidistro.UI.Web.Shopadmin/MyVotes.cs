using Hidistro.Entities.Store;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyVotes : DistributorPage
	{
		protected System.Web.UI.WebControls.DataList dlstVote;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstVote.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstVote_ItemDataBound);
			this.dlstVote.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstVote_DeleteCommand);
			this.dlstVote.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstVote_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindVote();
			}
		}
		private void dlstVote_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				long voteId = System.Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex]);
				VoteInfo voteById = SubsiteStoreHelper.GetVoteById(voteId);
				System.Collections.Generic.IList<VoteItemInfo> voteItems = SubsiteStoreHelper.GetVoteItems(voteId);
				for (int i = 0; i < voteItems.Count; i++)
				{
					if (voteById.VoteCounts != 0)
					{
						decimal num = voteItems[i].ItemCount / voteById.VoteCounts * 100m;
						voteItems[i].Percentage = decimal.Parse(num.ToString("F", System.Globalization.CultureInfo.InvariantCulture));
					}
					else
					{
						voteItems[i].Percentage = 0m;
					}
				}
				System.Web.UI.WebControls.GridView gridView = (System.Web.UI.WebControls.GridView)e.Item.FindControl("grdVoteItem");
				if (gridView != null)
				{
					gridView.DataSource = voteItems;
					gridView.DataBind();
				}
			}
		}
		private void dlstVote_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			long voteId = System.Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex]);
			if (SubsiteStoreHelper.DeleteVote(voteId) > 0)
			{
				this.BindVote();
				this.ShowMsg("成功删除了选择的投票", true);
				return;
			}
			this.ShowMsg("删除投票失败", false);
		}
		private void dlstVote_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.CommandName != "Sort" && e.CommandName == "IsBackup")
			{
				SubsiteStoreHelper.SetVoteIsBackup(System.Convert.ToInt64(this.dlstVote.DataKeys[e.Item.ItemIndex]));
				this.BindVote();
			}
		}
		private void BindVote()
		{
			this.dlstVote.DataSource = SubsiteStoreHelper.GetVotes();
			this.dlstVote.DataBind();
		}
	}
}
