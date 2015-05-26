using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_DBLabel_ShipOrderStatus : Label
	{
		private string orderId = string.Empty;
		private OrderStatus orderstatus;
		public override void DataBind()
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), "OrderId");
			if (obj != null && obj != DBNull.Value)
			{
				this.orderId = (string)obj;
			}
			obj = DataBinder.Eval(this.Page.GetDataItem(), "OrderStatus");
			if (obj != null && obj != DBNull.Value)
			{
				this.orderstatus = (OrderStatus)((int)obj);
			}
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			switch (this.orderstatus)
			{
			case OrderStatus.BuyerAlreadyPaid:
				if (HiContext.Current.User.IsInRole("Member"))
				{
					base.Text = "<span>配货中";
				}
				else
				{
					base.Text = "<span>待发货";
				}
				break;
			case OrderStatus.SellerAlreadySent:
				base.Text = "<span style=\"color:green;\">已发货";
				break;
			default:
				base.Text = "<span style=\"color:green;\">已发货";
				break;
			}
			if (HiContext.Current.User.IsInRole("区域发货点"))
			{
				base.Text += string.Format(" <a style=\"color:red;cursor:pointer;margin-left:5px;\" target=\"_blank\" onclick=\"{0}\">查看到货情况</a>", "showWindow_ShipInfoPage('" + this.orderId.Replace(HiContext.Current.User.UserId + "_", "") + "')");
			}
			else
			{
				if (HiContext.Current.User.IsInRole("供应商"))
				{
					base.Text += string.Format(" <a style=\"color:red;cursor:pointer;margin-left:5px;\" target=\"_blank\" onclick=\"{0}\">查看发货情况</a>", string.Concat(new object[]
					{
						"showWindow_ShipInfoPageForSupplier('",
						this.orderId.Replace(HiContext.Current.User.UserId + "_", ""),
						"',",
						HiContext.Current.User.UserId,
						")"
					}));
				}
			}
			base.Text += "</span>";
			base.Render(writer);
		}
	}
}
