using ASPNET.WebControls;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SelectedPurchaseProducts : DistributorPage
	{
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Literal litPurchaseCount;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdSelectedProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdSelectedProducts_RowDeleting);
			if (!base.IsPostBack)
			{
				this.BindAddedData();
			}
		}
		private void BindAddedData()
		{
			System.Collections.Generic.IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
			int num = 0;
			decimal d = 0m;
			foreach (PurchaseShoppingCartItemInfo current in purchaseShoppingCartItemInfos)
			{
				num += current.Quantity;
				d += current.GetSubTotal();
			}
			this.grdSelectedProducts.DataSource = purchaseShoppingCartItemInfos;
			this.grdSelectedProducts.DataBind();
			this.litPurchaseCount.Text = string.Format("总共采购商品{0}件；总采购金额{1}元。", num, d.ToString("F2"));
		}
		private void grdSelectedProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem((string)this.grdSelectedProducts.DataKeys[e.RowIndex].Value))
			{
				this.BindAddedData();
				return;
			}
			this.ShowMsg("删除失败", false);
		}
	}
}
