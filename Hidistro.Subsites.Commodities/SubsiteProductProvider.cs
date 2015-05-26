using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.Subsites.Commodities
{
	public abstract class SubsiteProductProvider
	{
		private static readonly SubsiteProductProvider _defaultInstance;
		static SubsiteProductProvider()
		{
			SubsiteProductProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.ProductData,Hidistro.Subsites.Data") as SubsiteProductProvider);
		}
		public static SubsiteProductProvider Instance()
		{
			return SubsiteProductProvider._defaultInstance;
		}
		public abstract System.Data.DataTable GetCategories();
		public abstract CategoryInfo GetCategory(int categoryId);
		public abstract int CreateCategory(CategoryInfo category);
		public abstract CategoryActionStatus UpdateCategory(CategoryInfo category);
		public abstract bool DeleteCategory(int categoryId);
		public abstract int DisplaceCategory(int oldCategoryId, int newCategory);
		public abstract bool SwapCategorySequence(int categoryId, int displaysequence);
		public abstract bool SetProductExtendCategory(int productId, string extendCategoryPath);
		public abstract bool SetCategoryThemes(int categoryId, string themeName);
		public abstract int DownloadCategory();
		public abstract DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload);
		public abstract System.Data.DataTable GetPuchaseProducts(ProductQuery query, out int count);
		public abstract System.Data.DataTable GetPuchaseProduct(string skuId);
		public abstract DbQueryResult GetSubmitPuchaseProducts(ProductQuery query);
		public abstract bool DownloadProduct(int productId, bool isDownCategory);
		public abstract DbQueryResult GetUnclassifiedProducts(ProductQuery query);
		public abstract DbQueryResult GetProducts(ProductQuery query);
		public abstract System.Data.DataTable GetGroupBuyProducts(ProductQuery query);
		public abstract int GetUpProducts();
		public abstract IList<ProductInfo> GetProducts(IList<int> productIds);
		public abstract DbQueryResult GetSubjectProducts(int tagId, Pagination page);
		public abstract bool IsOnSale(string productIds);
		public abstract int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus);
		public abstract bool UpdateProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran);
		public abstract bool AddSkuSalePrice(int productId, Dictionary<string, decimal> skuSalePrice, System.Data.Common.DbTransaction dbTran);
		public abstract bool UpdateProductCategory(int productId, int newCategoryId, string maiCategoryPath);
		public abstract int DeleteProducts(string productIds);
		public abstract ProductInfo GetProduct(int productId);
		public abstract System.Data.DataTable GetSkuContentBySku(string skuId);
		public abstract System.Data.DataTable GetSkusByProductId(int productId);
		public abstract IList<int> GetProductIds(ProductQuery query);
		public abstract bool AddSubjectProducts(int tagId, IList<int> productIds);
		public abstract bool RemoveSubjectProduct(int tagId, int productId);
		public abstract bool ClearSubjectProducts(int tagId);
		public abstract System.Data.DataTable GetProductAttribute(int productId);
		public abstract System.Data.DataTable GetProductSKU(int productId);
		public abstract IList<SKUItem> GetSkus(string productIds);
		public abstract System.Data.DataTable GetAuthorizeProductLines();
		public abstract IList<ProductLineInfo> GetAuthorizeProductLineList();
		public abstract System.Data.DataTable GetProductBaseInfo(string productIds);
		public abstract bool UpdateShowSaleCounts(string productIds, int showSaleCounts);
		public abstract bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation);
		public abstract bool UpdateShowSaleCounts(System.Data.DataTable dataTable_0);
		public abstract System.Data.DataTable GetSkuUnderlingPrices(string productIds);
		public abstract bool CheckPrice(string productIds, string basePriceName, decimal checkPrice);
		public abstract bool CheckPrice(string productIds, string basePriceName, decimal checkPrice, string operation);
		public abstract bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price);
		public abstract bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price);
		public abstract bool UpdateSkuUnderlingPrices(System.Data.DataSet dataSet_0, string skuIds);
		public abstract bool UpdateProductNames(string productIds, string prefix, string suffix);
		public abstract bool ReplaceProductNames(string productIds, string oldWord, string newWord);
		public abstract DbQueryResult GetRelatedProducts(Pagination page, int productId);
		public abstract bool AddRelatedProduct(int productId, int relatedProductId);
		public abstract bool RemoveRelatedProduct(int productId, int relatedProductId);
		public abstract bool ClearRelatedProducts(int productId);
		public abstract System.Data.DataTable GetTags();
		public abstract bool AddProductTags(int productId, IList<int> tagIds, System.Data.Common.DbTransaction tran);
		public abstract bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran);
		public abstract IList<int> GetProductTags(int productId);
		public abstract DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds);
		public abstract System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds);
		public abstract DbQueryResult GetToTaobaoProducts(ProductQuery query);
		public abstract PublishToTaobaoProductInfo GetTaobaoProduct(int productId, int distributorId);
		public abstract bool AddTaobaoProductId(int productId, long taobaoProductId, int distributorId);
		public abstract string GetSkuIdByTaobao(long taobaoProductId, string taobaoSkuId, int distributorId);
		public abstract System.Data.DataTable GetSkuContentBySku(string skuId, int distributorId);
		public static string BuildProductQuery(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId FROM distro_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
			stringBuilder.AppendFormat(" AND p.DistributorUserId={0} ", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.ProductCode) && query.ProductCode.Length > 0)
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM distro_Categories WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY p.{0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
