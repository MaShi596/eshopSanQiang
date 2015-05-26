using System;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_Drop_OrderType : DropDownList
	{
		public Supplier_Drop_OrderType()
		{
			base.Items.Clear();
			base.Items.Add(new ListItem("--全部--", "0"));
			base.Items.Add(new ListItem("订单", "1"));
			base.Items.Add(new ListItem("采购单", "2"));
		}
	}
}
