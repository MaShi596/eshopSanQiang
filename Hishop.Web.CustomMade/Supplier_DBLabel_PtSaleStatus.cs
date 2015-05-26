using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_DBLabel_PtSaleStatus : Literal
	{
		private int status;
		public override void DataBind()
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), "SaleStatus");
			if (obj != null && obj != DBNull.Value)
			{
				this.status = (int)obj;
			}
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			string value = string.Empty;
			switch (this.status)
			{
			case 1:
				value = "出售中";
				break;
			case 2:
				value = "下架中";
				break;
			case 3:
				value = "仓库中";
				break;
			}
			writer.Write(value);
		}
	}
}
