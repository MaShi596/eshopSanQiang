using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Commodities
{
	public static class SubSiteProducthelper
	{
		public static DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload)
		{
			return SubsiteProductProvider.Instance().GetAuthorizeProducts(query, onlyNotDownload);
		}
		public static int GetUpProducts()
		{
			return SubsiteProductProvider.Instance().GetUpProducts();
		}
		public static System.Data.DataTable GetPuchaseProducts(ProductQuery query, out int count)
		{
			return SubsiteProductProvider.Instance().GetPuchaseProducts(query, out count);
		}
		public static System.Data.DataTable GetPuchaseProduct(string skuId)
		{
			return SubsiteProductProvider.Instance().GetPuchaseProduct(skuId);
		}
		public static bool DownloadProduct(int productId, bool isDownCategory)
		{
			return SubsiteProductProvider.Instance().DownloadProduct(productId, isDownCategory);
		}
		public static DbQueryResult GetUnclassifiedProducts(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetUnclassifiedProducts(query);
		}
		public static DbQueryResult GetProducts(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetProducts(query);
		}
		public static System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetGroupBuyProducts(query);
		}
		public static IList<ProductInfo> GetProducts(IList<int> productIds)
		{
			return SubsiteProductProvider.Instance().GetProducts(productIds);
		}
		public static DbQueryResult GetSubjectProducts(int tagId, Pagination page)
		{
			return SubsiteProductProvider.Instance().GetSubjectProducts(tagId, page);
		}
		public static bool IsOnSale(string productIds)
		{
			return SubsiteProductProvider.Instance().IsOnSale(productIds);
		}
		public static int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
		{
			return SubsiteProductProvider.Instance().UpdateProductSaleStatus(productIds, saleStatus);
		}
		public static int DeleteProducts(string productIds)
		{
			return SubsiteProductProvider.Instance().DeleteProducts(productIds);
		}
		public static bool UpdateProduct(ProductInfo product, Dictionary<string, decimal> skuSalePrice, IList<int> tagIdList)
		{
			bool result;
			if (null == product)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				using (System.Data.Common.DbConnection dbConnection = DatabaseFactory.CreateDatabase().CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						if (!SubsiteProductProvider.Instance().UpdateProduct(product, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							if (!SubsiteProductProvider.Instance().AddSkuSalePrice(product.ProductId, skuSalePrice, dbTransaction))
							{
								dbTransaction.Rollback();
								result = false;
							}
							else
							{
								if (!SubSiteProducthelper.DeleteProductTags(product.ProductId, dbTransaction))
								{
									dbTransaction.Rollback();
									result = false;
								}
								else
								{
									if (tagIdList != null && tagIdList.Count > 0 && !SubSiteProducthelper.AddProductTags(product.ProductId, tagIdList, dbTransaction))
									{
										dbTransaction.Rollback();
										result = false;
									}
									else
									{
										dbTransaction.Commit();
										result = true;
									}
								}
							}
						}
					}
					catch
					{
						dbTransaction.Rollback();
						result = false;
					}
				}
			}
			return result;
		}
		public static bool UpdateProductCategory(int productId, int newCategoryId)
		{
			bool result;
			if (newCategoryId != 0)
			{
				result = SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, SubsiteCatalogHelper.GetCategory(newCategoryId).Path + "|");
			}
			else
			{
				result = SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, null);
			}
			return result;
		}
		public static ProductInfo GetProduct(int productId)
		{
			return SubsiteProductProvider.Instance().GetProduct(productId);
		}
		public static System.Data.DataTable GetSkuContentBySku(string skuId)
		{
			return SubsiteProductProvider.Instance().GetSkuContentBySku(skuId);
		}
		public static IList<int> GetProductIds(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetProductIds(query);
		}
		public static bool AddSubjectProducts(int tagId, IList<int> productIds)
		{
			return SubsiteProductProvider.Instance().AddSubjectProducts(tagId, productIds);
		}
		public static bool AddSubjectProduct(int tagId, int productId)
		{
			IList<int> list = new List<int>();
			list.Add(productId);
			return SubsiteProductProvider.Instance().AddSubjectProducts(tagId, list);
		}
		public static bool RemoveSubjectProduct(int tagId, int productId)
		{
			return SubsiteProductProvider.Instance().RemoveSubjectProduct(tagId, productId);
		}
		public static bool ClearSubjectProducts(int tagId)
		{
			return SubsiteProductProvider.Instance().ClearSubjectProducts(tagId);
		}
		public static System.Data.DataTable GetProductAttribute(int productId)
		{
			return SubsiteProductProvider.Instance().GetProductAttribute(productId);
		}
		public static System.Data.DataTable GetProductSKU(int productId)
		{
			return SubsiteProductProvider.Instance().GetProductSKU(productId);
		}
		public static IList<SKUItem> GetSkus(string productIds)
		{
			return SubsiteProductProvider.Instance().GetSkus(productIds);
		}
		public static System.Data.DataTable GetAuthorizeProductLines()
		{
			return SubsiteProductProvider.Instance().GetAuthorizeProductLines();
		}
		public static IList<ProductLineInfo> GetAuthorizeProductLineList()
		{
			return SubsiteProductProvider.Instance().GetAuthorizeProductLineList();
		}
		public static System.Data.DataTable GetSkusByProductId(int productId)
		{
			return SubsiteProductProvider.Instance().GetSkusByProductId(productId);
		}
		public static DbQueryResult GetSubmitPuchaseProducts(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetSubmitPuchaseProducts(query);
		}
		public static System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			return SubsiteProductProvider.Instance().GetProductBaseInfo(productIds);
		}
		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			return SubsiteProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts);
		}
		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			return SubsiteProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
		}
		public static bool UpdateShowSaleCounts(System.Data.DataTable dataTable_0)
		{
			return dataTable_0 != null && dataTable_0.Rows.Count > 0 && SubsiteProductProvider.Instance().UpdateShowSaleCounts(dataTable_0);
		}
		public static System.Data.DataTable GetSkuUnderlingPrices(string productIds)
		{
			return SubsiteProductProvider.Instance().GetSkuUnderlingPrices(productIds);
		}
		public static bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
		{
			return SubsiteProductProvider.Instance().CheckPrice(productIds, basePriceName, checkPrice);
		}
		public static bool CheckPrice(string productIds, string basePriceName, decimal checkPrice, string operation)
		{
			return SubsiteProductProvider.Instance().CheckPrice(productIds, basePriceName, checkPrice, operation);
		}
		public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price)
		{
			return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, price);
		}
		public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
		{
			return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, basePriceName, operation, price);
		}
		public static bool UpdateSkuUnderlingPrices(System.Data.DataSet dataSet_0, string skuIds)
		{
			return dataSet_0 != null && !string.IsNullOrEmpty(skuIds) && SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(dataSet_0, skuIds);
		}
		public static bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			return SubsiteProductProvider.Instance().UpdateProductNames(productIds, prefix, suffix);
		}
		public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			return SubsiteProductProvider.Instance().ReplaceProductNames(productIds, oldWord, newWord);
		}
		public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			return SubsiteProductProvider.Instance().GetRelatedProducts(page, productId);
		}
		public static bool AddRelatedProduct(int productId, int relatedProductId)
		{
			return SubsiteProductProvider.Instance().AddRelatedProduct(productId, relatedProductId);
		}
		public static bool RemoveRelatedProduct(int productId, int relatedProductId)
		{
			return SubsiteProductProvider.Instance().RemoveRelatedProduct(productId, relatedProductId);
		}
		public static bool ClearRelatedProducts(int productId)
		{
			return SubsiteProductProvider.Instance().ClearRelatedProducts(productId);
		}
		public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			return SubsiteProductProvider.Instance().GetExportProducts(query, removeProductIds);
		}
		public static System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			System.Data.DataSet exportProducts = SubsiteProductProvider.Instance().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
			exportProducts.Tables[0].TableName = "types";
			exportProducts.Tables[1].TableName = "attributes";
			exportProducts.Tables[2].TableName = "values";
			exportProducts.Tables[3].TableName = "products";
			exportProducts.Tables[4].TableName = "skus";
			exportProducts.Tables[5].TableName = "skuItems";
			exportProducts.Tables[6].TableName = "productAttributes";
			exportProducts.Tables[7].TableName = "taobaosku";
			return exportProducts;
		}
		public static DbQueryResult GetToTaobaoProducts(ProductQuery query)
		{
			return SubsiteProductProvider.Instance().GetToTaobaoProducts(query);
		}
		public static PublishToTaobaoProductInfo GetTaobaoProduct(int productId, int distributorId)
		{
			return SubsiteProductProvider.Instance().GetTaobaoProduct(productId, distributorId);
		}
		public static bool AddTaobaoProductId(int productId, long taobaoProductId, int distributorId)
		{
			return SubsiteProductProvider.Instance().AddTaobaoProductId(productId, taobaoProductId, distributorId);
		}
		public static System.Data.DataTable GetSkuContent(long taobaoProductId, string taobaoSkuId, int distributorId)
		{
			string skuIdByTaobao = SubsiteProductProvider.Instance().GetSkuIdByTaobao(taobaoProductId, taobaoSkuId, distributorId);
			return SubsiteProductProvider.Instance().GetSkuContentBySku(skuIdByTaobao, distributorId);
		}
		public static System.Data.DataTable GetTags()
		{
			return SubsiteProductProvider.Instance().GetTags();
		}
		public static bool AddProductTags(int productId, IList<int> tagsId, System.Data.Common.DbTransaction tran)
		{
			return SubsiteProductProvider.Instance().AddProductTags(productId, tagsId, tran);
		}
		public static bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
		{
			return SubsiteProductProvider.Instance().DeleteProductTags(productId, tran);
		}
		public static IList<int> GetProductTags(int productId)
		{
			return SubsiteProductProvider.Instance().GetProductTags(productId);
		}
	}
}
