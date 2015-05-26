using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class SalePriceDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "";
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
		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			}
			base.Items.Add(new System.Web.UI.WebControls.ListItem("我的采购价", "PurchasePrice"));
			base.Items.Add(new System.Web.UI.WebControls.ListItem("一口价", "SalePrice"));
		}
	}
}
