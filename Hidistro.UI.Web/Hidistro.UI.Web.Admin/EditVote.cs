using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Votes)]
	public class EditVote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAddVoteName;
		protected System.Web.UI.WebControls.TextBox txtMaxCheck;
		protected System.Web.UI.WebControls.TextBox txtValues;
		protected System.Web.UI.WebControls.Button btnEditVote;
		private long voteId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditVote.Click += new System.EventHandler(this.btnEditVote_Click);
			if (!this.Page.IsPostBack)
			{
				VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
				System.Collections.Generic.IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(this.voteId);
				if (voteById == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.txtAddVoteName.Text = Globals.HtmlDecode(voteById.VoteName);
				this.txtMaxCheck.Text = voteById.MaxCheck.ToString();
				string text = "";
				foreach (VoteItemInfo current in voteItems)
				{
					text = text + Globals.HtmlDecode(current.VoteItemName) + "\r\n";
				}
				this.txtValues.Text = text;
			}
		}
		private void btnEditVote_Click(object sender, System.EventArgs e)
		{
			if (StoreHelper.GetVoteCounts(this.voteId) > 0)
			{
				this.ShowMsg("投票已经开始，不能再对投票选项进行任何操作", false);
				return;
			}
			VoteInfo voteInfo = new VoteInfo();
			voteInfo.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
			voteInfo.VoteId = this.voteId;
			int maxCheck;
			if (int.TryParse(this.txtMaxCheck.Text.Trim(), out maxCheck))
			{
				voteInfo.MaxCheck = maxCheck;
			}
			else
			{
				voteInfo.MaxCheck = -2147483648;
			}
			if (string.IsNullOrEmpty(this.txtValues.Text.Trim()))
			{
				this.ShowMsg("投票选项不能为空", false);
				return;
			}
			System.Collections.Generic.IList<VoteItemInfo> list = new System.Collections.Generic.List<VoteItemInfo>();
			string text = this.txtValues.Text.Trim().Replace("\r\n", "\n");
			string[] array = text.Replace("\n", "*").Split(new char[]
			{
				'*'
			});
			for (int i = 0; i < array.Length; i++)
			{
				VoteItemInfo voteItemInfo = new VoteItemInfo();
				if (array[i].Length > 60)
				{
					this.ShowMsg("投票选项长度限制在60个字符以内", false);
					return;
				}
				voteItemInfo.VoteItemName = Globals.HtmlEncode(array[i]);
				list.Add(voteItemInfo);
			}
			voteInfo.VoteItems = list;
			if (!this.ValidationVote(voteInfo))
			{
				return;
			}
			if (StoreHelper.UpdateVote(voteInfo))
			{
				this.ShowMsg("修改投票成功", true);
				return;
			}
			this.ShowMsg("修改投票失败", false);
		}
		private bool ValidationVote(VoteInfo vote)
		{
			ValidationResults validationResults = Validation.Validate<VoteInfo>(vote, new string[]
			{
				"ValVote"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
