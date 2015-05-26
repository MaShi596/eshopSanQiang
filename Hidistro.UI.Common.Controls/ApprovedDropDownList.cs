using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ApprovedDropDownList : DropDownList
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
		public new bool? SelectedValue
		{
			get
			{
				bool value = true;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				bool.TryParse(base.SelectedValue, out value);
				return new bool?(value);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedValue = value.Value.ToString();
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem("全部", string.Empty));
			}
			this.Items.Add(new ListItem("通过", "True"));
			this.Items.Add(new ListItem("禁止", "False"));
		}
	}
}
