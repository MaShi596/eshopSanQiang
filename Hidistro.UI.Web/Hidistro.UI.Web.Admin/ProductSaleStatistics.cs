using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductSaleStatistics)]
	public class ProductSaleStatistics : AdminPage
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
			System.Data.DataTable productVisitAndBuyStatistics = SalesHelper.GetProductVisitAndBuyStatistics(saleStatisticsQuery, out totalRecords);
			this.grdProductSaleStatistics.DataSource = productVisitAndBuyStatistics;
			this.grdProductSaleStatistics.DataBind();
            this.pager.TotalRecords = totalRecords;
			this.hidPageSize.Value = this.pager.PageSize.ToString();
			this.hidPageIndex.Value = this.pager.PageIndex.ToString();
		}
		private void grdProductSaleStatistics_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}
