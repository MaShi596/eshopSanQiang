using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ShippingModeRadioButtonList : RadioButtonList
	{
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			IList<ShippingModeInfo> shippingModes = ControlProvider.Instance().GetShippingModes();
			foreach (ShippingModeInfo current in shippingModes)
			{
				string name = current.Name;
				this.Items.Add(new ListItem(name, current.ModeId.ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
