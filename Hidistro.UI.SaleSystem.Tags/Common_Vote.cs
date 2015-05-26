using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Vote : ThemedTemplatedRepeater
	{
		protected override void OnLoad(EventArgs eventArgs_0)
		{
			base.DataSource = CommentBrowser.GetVoteByIsShow();
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (base.Items.Count == 0)
			{
				return;
			}
			base.Render(writer);
			string text = string.Empty;
			text += "<script language=\"jscript\" type=\"text/javascript\">";
			text += "function setcheckbox(checkbox){";
			text += "var group = document.getElementsByName(checkbox.name);";
			text += "var voteValue = document.getElementById(checkbox.name + '_Value');";
			text += "var maxVote = parseInt(document.getElementById(checkbox.name + '_MaxVote').value);";
			text += "voteValue.value =''; var n = 0;";
			text += "for (index = 0;index < group.length;index ++){";
			text += "if (group[index].checked){n++; voteValue.value += group[index].value + ',';}}";
			text += "if (n > maxVote){var msg='";
			text += "最多能投票：";
			text += "'; alert(msg + maxVote); checkbox.checked = false; setcheckbox(checkbox);}}";
			text += "function voteOption(voteId, voteItemId) {";
			text += "window.document.location.href = applicationPath + \"/VoteResult.aspx?VoteId=\" + voteId + \"&&VoteItemId=\" + voteItemId;";
			text += "}";
			text += "</script>";
			writer.Write(text);
		}
	}
}
