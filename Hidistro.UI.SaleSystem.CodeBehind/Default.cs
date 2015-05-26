using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class Default : HtmlTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Default.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void OnLoad(System.EventArgs eventArgs_0)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.Params["OrderId"]))
			{
				this.SearchOrder();
			}
			base.OnLoad(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			Hidistro.Membership.Context.HiContext current = Hidistro.Membership.Context.HiContext.Current;
			PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, Hidistro.Membership.Context.HiContext.Current.Context);
		}
		private void SearchOrder()
		{
			string text = "[{";
			string orderId = this.Page.Request["OrderId"];
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"OrderId\":\"",
					orderInfo.OrderId,
					"\",\"ShippingStatus\":\"",
					OrderInfo.GetOrderStatusName(orderInfo.OrderStatus),
					"\",\"ShipOrderNumber\":\"",
					orderInfo.ShipOrderNumber,
					"\",\"ShipModeName\":\"",
					orderInfo.RealModeName,
					"\""
				});
			}
			text += "}]";
			this.Page.Response.ContentType = "text/plain";
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
	}
}
