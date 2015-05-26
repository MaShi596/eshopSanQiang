using Hidistro.Entities.Commodities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class PenetrationStatusDropDownList : DropDownList
	{
		private bool allowNull = true;
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
		public new PenetrationStatus SelectedValue
		{
			get
			{
				PenetrationStatus result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = PenetrationStatus.NotSet;
				}
				else
				{
					result = (PenetrationStatus)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				ListItemCollection arg_20_0 = base.Items;
				ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}
		public PenetrationStatusDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			this.Items.Add(new ListItem("已铺货", "1"));
			this.Items.Add(new ListItem("未铺货", "2"));
		}
	}
}
