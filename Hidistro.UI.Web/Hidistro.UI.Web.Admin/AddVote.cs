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
	public class AddVote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtAddVoteName;
		protected System.Web.UI.WebControls.CheckBox checkIsBackup;
		protected System.Web.UI.WebControls.TextBox txtMaxCheck;
		protected System.Web.UI.WebControls.TextBox txtValues;
		protected System.Web.UI.WebControls.Button btnAddVote;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddVote.Click += new System.EventHandler(this.btnAddVote_Click);
		}
		private void btnAddVote_Click(object sender, System.EventArgs e)
		{
			VoteInfo voteInfo = new VoteInfo();
			voteInfo.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
			voteInfo.IsBackup = this.checkIsBackup.Checked;
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
			if (StoreHelper.CreateVote(voteInfo) > 0)
			{
				this.ShowMsg("成功的添加了一个投票", true);
				this.txtAddVoteName.Text = string.Empty;
				this.checkIsBackup.Checked = false;
				this.txtMaxCheck.Text = string.Empty;
				this.txtValues.Text = string.Empty;
				return;
			}
			this.ShowMsg("添加投票失败", false);
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
