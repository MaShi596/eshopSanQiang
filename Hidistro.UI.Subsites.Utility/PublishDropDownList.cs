using Hidistro.Entities.Commodities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class PublishDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "-请选择-";
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
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}
		public new PublishStatus SelectedValue
		{
			get
			{
				PublishStatus result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = PublishStatus.NotSet;
				}
				else
				{
					result = (PublishStatus)int.Parse(base.SelectedValue, System.Globalization.CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				System.Web.UI.WebControls.ListItemCollection arg_20_0 = base.Items;
				System.Web.UI.WebControls.ListItemCollection arg_1B_0 = base.Items;
				int num = (int)value;
				base.SelectedIndex = arg_20_0.IndexOf(arg_1B_0.FindByValue(num.ToString(System.Globalization.CultureInfo.InvariantCulture)));
			}
		}
		public PublishDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			}
			this.Items.Add(new System.Web.UI.WebControls.ListItem("已发布", "1"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("未发布", "2"));
			this.Items.Add(new System.Web.UI.WebControls.ListItem("有更新", "3"));
		}
	}
}
