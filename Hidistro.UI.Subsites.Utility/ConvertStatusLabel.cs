using Hidistro.Subsites.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class ConvertStatusLabel : System.Web.UI.WebControls.Literal
	{
		private bool isExit;
		private long tradeId;
		protected override void OnDataBinding(System.EventArgs eventArgs_0)
		{
			object obj = System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), "tid");
			if (obj != null && obj != System.DBNull.Value)
			{
				this.tradeId = (long)obj;
				this.isExit = SubsiteSalesHelper.IsExitPurchaseOrder(this.tradeId);
			}
			base.OnDataBinding(eventArgs_0);
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (this.isExit)
			{
				writer.Write("<label style=\"color:Red;\">此订单已经生成过采购单</label>");
			}
			else
			{
				writer.Write(string.Format("<input name=\"CheckBoxGroup\" type=\"checkbox\" value='{0}'>", this.tradeId));
			}
		}
	}
}
