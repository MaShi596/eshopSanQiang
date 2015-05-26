using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductSales : AscxTemplatedWebControl
	{
		private Repeater rp_productsales;
		public int maxNum = 6;
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductSales.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rp_productsales = (Repeater)this.FindControl("rp_productsales");
			DataTable lineItems = ProductBrowser.GetLineItems(Convert.ToInt32(this.Page.Request.QueryString["productId"]), this.maxNum);
			foreach (DataRow dataRow in lineItems.Rows)
			{
				string text = (string)dataRow["Username"];
				if (text.ToLower() == "anonymous")
				{
					dataRow["Username"] = "匿名用户";
				}
				else
				{
					dataRow["Username"] = text.Substring(0, 1) + "**" + text.Substring(text.Length - 1);
				}
			}
			this.rp_productsales.DataSource = lineItems;
			this.rp_productsales.DataBind();
		}
	}
}
