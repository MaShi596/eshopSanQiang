using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImageRadioButtonList : RadioButtonList
	{
		public new OrderMark? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new OrderMark?((OrderMark)Enum.Parse(typeof(OrderMark), base.SelectedValue));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int)value.Value).ToString(CultureInfo.InvariantCulture)));
					return;
				}
				base.SelectedIndex = -1;
			}
		}
		public OrderRemarkImageRadioButtonList()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconaf.gif\"></img>", 1.ToString()));
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconb.gif\"></img>", 2.ToString()));
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconc.gif\"></img>", 3.ToString()));
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/icona.gif\"></img>", 4.ToString()));
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconad.gif\"></img>", 5.ToString()));
			this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconae.gif\"></img>", 6.ToString()));
			this.RepeatDirection = RepeatDirection.Horizontal;
		}
	}
}
