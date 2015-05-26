using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SaleTargets : DistributorPage
	{
		protected Grid grdOrderAvPrice;
		protected Grid grdVisitOrderAvPrice;
		protected Grid grdOrderTranslatePercentage;
		protected Grid grdUserOrderPercentage;
		protected Grid grdUserOrderAvNumb;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				DbQueryResult saleTargets = SubsiteSalesHelper.GetSaleTargets();
				this.grdOrderAvPrice.DataSource = saleTargets.Data;
				this.grdOrderAvPrice.DataBind();
				this.grdOrderTranslatePercentage.DataSource = saleTargets.Data;
				this.grdOrderTranslatePercentage.DataBind();
				this.grdUserOrderAvNumb.DataSource = saleTargets.Data;
				this.grdUserOrderAvNumb.DataBind();
				this.grdVisitOrderAvPrice.DataSource = saleTargets.Data;
				this.grdVisitOrderAvPrice.DataBind();
				this.grdUserOrderPercentage.DataSource = saleTargets.Data;
				this.grdUserOrderPercentage.DataBind();
			}
		}
	}
}
