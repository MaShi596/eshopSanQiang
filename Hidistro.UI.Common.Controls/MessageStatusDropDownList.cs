using Hidistro.Entities.Comments;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class MessageStatusDropDownList : DropDownList
	{
		public new MessageStatus SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return MessageStatus.All;
				}
				return (MessageStatus)int.Parse(base.SelectedValue);
			}
			set
			{
				ListItemCollection arg_20_0 = base.Items;
				ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}
		public MessageStatusDropDownList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("全部", 1.ToString()));
			this.Items.Add(new ListItem("已回复", 2.ToString()));
			this.Items.Add(new ListItem("未回复", 3.ToString()));
		}
	}
}
