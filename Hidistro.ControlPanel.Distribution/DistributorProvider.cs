using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.ControlPanel.Distribution
{
	public abstract class DistributorProvider
	{
		private static readonly DistributorProvider _defaultInstance;
		static DistributorProvider()
		{
			DistributorProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.DistributionData,Hidistro.ControlPanel.Data") as DistributorProvider);
		}
		public static DistributorProvider Instance()
		{
			return DistributorProvider._defaultInstance;
		}
		public abstract bool AddDistributorGrade(DistributorGradeInfo distributorGrade);
		public abstract bool ExistGradeName(string gradeName);
		public abstract bool UpdateDistributorGrade(DistributorGradeInfo distributorGrade);
		public abstract bool DeleteDistributorGrade(int gradeId);
		public abstract bool ExistDistributor(int gradeId);
		public abstract System.Data.DataTable GetDistributorGrades();
		public abstract DistributorGradeInfo GetDistributorGradeInfo(int gradId);
		public abstract DbQueryResult GetDistributors(DistributorQuery query);
		public abstract IList<Distributor> GetDistributors();
		public abstract System.Data.DataTable GetDistributorsNopage(DistributorQuery query, IList<string> fields);
		public abstract bool Delete(int userId);
		public abstract IList<int> GetDistributorProductLines(int userId);
		public abstract bool AddDistributorProductLines(int userId, IList<int> lineIds);
		public abstract DbQueryResult GetDistributorBalanceDetails(BalanceDetailQuery query);
		public abstract DbQueryResult GetDistributorBalanceDetailsNoPage(BalanceDetailQuery query);
		public abstract DbQueryResult GetDistributorBalanceDrawRequests(BalanceDrawRequestQuery query);
		public abstract bool DealDistributorBalanceDrawRequest(int userId, bool agree);
		public abstract bool InsertBalanceDetail(BalanceDetailInfo balanceDetail, System.Data.Common.DbTransaction dbTran);
		public abstract DbQueryResult GetDistributorBalance(DistributorQuery query);
		public abstract System.Data.DataTable GetDomainRequests(Pagination pagination, string name, out int total);
		public abstract System.Data.DataTable GetEtaoSites(Pagination pageination, string name, string trueName, out int total);
		public abstract System.Data.DataTable GetEtaoRequests(Pagination pagination, string name, string trueName, out int total);
		public abstract bool AddInitData(int distributorId, System.Data.Common.DbTransaction dbtran);
		public abstract SiteRequestInfo GetSiteRequestInfo(int requestId);
		public abstract bool AcceptSiteRequest(int siteQty, int requestId, System.Data.Common.DbTransaction dbTran);
		public abstract bool RefuseSiteRequest(int requestId, string reason);
		public abstract bool AddSiteSettings(SiteSettings siteSettings, int requestId, System.Data.Common.DbTransaction dbtran);
		public abstract System.Data.DataTable GetDistributorSites(Pagination pagination, string name, string trueName, out int total);
		public abstract decimal GetMonthDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType);
		public abstract System.Data.DataTable GetDayDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType);
		public abstract System.Data.DataTable GetMonthDistributionTotal(int year, SaleStatisticsType saleStatisticsType);
		public abstract decimal GetYearDistributionTotal(int year, SaleStatisticsType saleStatisticsType);
		public abstract OrderStatisticsInfo GetPurchaseOrders(UserOrderQuery order);
		public abstract OrderStatisticsInfo GetPurchaseOrdersNoPage(UserOrderQuery order);
		public abstract OrderStatisticsInfo GetDistributorStatistics(SaleStatisticsQuery query, out int totalDistributors);
		public abstract OrderStatisticsInfo GetDistributorStatisticsNoPage(SaleStatisticsQuery query);
		public abstract System.Data.DataTable GetDistributionProductSales(SaleStatisticsQuery productSale, out int totalProductSales);
		public abstract System.Data.DataTable GetDistributionProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales);
	}
}
