using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_DBLabel_SendGoodStatus : Literal
	{
		private string orderId = string.Empty;
		public override void DataBind()
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), "orderId");
			if (obj != null && obj != DBNull.Value)
			{
				this.orderId = (string)obj;
			}
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			string text = string.Empty;
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo != null)
			{
				bool arg_3A_0;
				if (orderInfo.OrderStatus != OrderStatus.SellerAlreadySent)
				{
					if (orderInfo.OrderStatus != OrderStatus.Finished)
					{
						arg_3A_0 = true;
						goto IL_3A;
					}
				}
				arg_3A_0 = string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb);
				IL_3A:
				if (!arg_3A_0)
				{
					string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
					text += expressData;
				}
			}
			writer.Write(text);
		}
	}
}
