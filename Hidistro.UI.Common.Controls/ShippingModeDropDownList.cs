using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ShippingModeDropDownList : DropDownList
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
		public string NullToDisplay
		{
			get;
			set;
		}
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue));
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
					return;
				}
				base.SelectedValue = value.ToString();
			}
		}
		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			IList<ShippingModeInfo> shippingModes = ControlProvider.Instance().GetShippingModes();
			foreach (ShippingModeInfo current in shippingModes)
			{
				base.Items.Add(new ListItem(current.Name, current.ModeId.ToString()));
			}
		}
	}
}
