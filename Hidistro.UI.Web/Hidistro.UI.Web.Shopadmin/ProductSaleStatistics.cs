using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ProductSaleStatistics : DistributorPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPageSize;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPageIndex;
		protected Grid grdProductSaleStatistics;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
            base.OnInitComplete(e);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindProductSaleStatistics();
			}
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindProductSaleStatistics()
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
			saleStatisticsQuery.SortBy = "BuyPercentage";
			saleStatisticsQuery.SortOrder = SortAction.Desc;
			int totalRecords = 0;
			System.Data.DataTable productVisitAndBuyStatistics = SubsiteSalesHelper.GetProductVisitAndBuyStatistics(saleStatisticsQuery, out totalRecords);
			this.grdProductSaleStatistics.DataSource = productVisitAndBuyStatistics;
			this.grdProductSaleStatistics.DataBind();
            this.pager.TotalRecords = totalRecords;
		}
		private void grdProductSaleStatistics_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}
