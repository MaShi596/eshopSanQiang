using Hidistro.Membership.Core.Enums;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class GenderRadioButtonList : RadioButtonList
	{
		public new Gender SelectedValue
		{
			get
			{
				return (Gender)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				ListItemCollection arg_20_0 = base.Items;
				ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}
		public GenderRadioButtonList()
		{
			this.Items.Add(new ListItem("保密", 0.ToString(CultureInfo.InvariantCulture)));
			this.Items.Add(new ListItem("男性", 1.ToString(CultureInfo.InvariantCulture)));
			this.Items.Add(new ListItem("女性", 2.ToString(CultureInfo.InvariantCulture)));
		}
	}
}
