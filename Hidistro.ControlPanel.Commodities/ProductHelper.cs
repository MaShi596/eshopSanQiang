using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;
namespace Hidistro.ControlPanel.Commodities
{
	public static class ProductHelper
	{
		public static DbQueryResult GetUnclassifiedProducts(ProductQuery query)
		{
			return ProductProvider.Instance().GetUnclassifiedProducts(query);
		}
		public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds, out IList<int> tagsId)
		{
			return ProductProvider.Instance().GetProductDetails(productId, out attrs, out distributorUserIds, out tagsId);
		}
		public static DbQueryResult GetProducts(ProductQuery query)
		{
			return ProductProvider.Instance().GetProducts(query);
		}
		public static System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			return ProductProvider.Instance().GetGroupBuyProducts(query);
		}
		public static IList<ProductInfo> GetProducts(IList<int> productIds)
		{
			return ProductProvider.Instance().GetProducts(productIds);
		}
		public static IList<int> GetProductIds(ProductQuery query)
		{
			return ProductProvider.Instance().GetProductIds(query);
		}
		public static DbQueryResult GetSubjectProducts(int tagId, Pagination page)
		{
			return ProductProvider.Instance().GetSubjectProducts(tagId, page);
		}
		public static IList<int> GetSubjectProductIds(int tagId)
		{
			return ProductProvider.Instance().GetSubjectProductIds(tagId);
		}
		public static string GetProductNameByProductIds(string productId, out int sumcount)
		{
			return ProductProvider.Instance().GetProductNameByProductIds(productId, out sumcount);
		}
		public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
		{
			ProductActionStatus result;
			if (null == product)
			{
				result = ProductActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				product.LowestSalePrice = Math.Round(product.LowestSalePrice, decimalLength);
				ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						ProductProvider productProvider = ProductProvider.Instance();
						int num = productProvider.AddProduct(product, dbTransaction);
						if (num == 0)
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.DuplicateSKU;
							return result;
						}
						product.ProductId = num;
						if (skus != null && skus.Count > 0 && !productProvider.AddProductSKUs(num, skus, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.SKUError;
							return result;
						}
						if (attrs != null && attrs.Count > 0 && !productProvider.AddProductAttributes(num, attrs, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.AttributeError;
							return result;
						}
						if (tagsId != null && tagsId.Count > 0 && !productProvider.AddProductTags(num, tagsId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.ProductTagEroor;
							return result;
						}
						dbTransaction.Commit();
						productActionStatus = ProductActionStatus.Success;
					}
					catch (Exception)
					{
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (productActionStatus == ProductActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[]
					{
						product.ProductName
					}));
				}
				result = productActionStatus;
			}
			return result;
		}
		public static int GetMaxSequence()
		{
			return ProductProvider.Instance().GetMaxSequence();
		}
		public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> distributorUserIds, IList<int> tagIds)
		{
			ProductActionStatus result;
			if (null == product)
			{
				result = ProductActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(product, true);
				int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
				if (product.MarketPrice.HasValue)
				{
					product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
				}
				product.LowestSalePrice = Math.Round(product.LowestSalePrice, decimalLength);
				ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						ProductProvider productProvider = ProductProvider.Instance();
						if (!productProvider.UpdateProduct(product, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.DuplicateSKU;
							return result;
						}
						if (!productProvider.DeleteProductSKUS(product.ProductId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.SKUError;
							return result;
						}
						if (skus != null && skus.Count > 0 && !productProvider.AddProductSKUs(product.ProductId, skus, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.SKUError;
							return result;
						}
						if (!productProvider.AddProductAttributes(product.ProductId, attrs, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.AttributeError;
							return result;
						}
						if (!productProvider.OffShelfProductExcludedSalePrice(product.ProductId, product.LowestSalePrice, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.OffShelfError;
							return result;
						}
						if (!productProvider.DeleteProductTags(product.ProductId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.ProductTagEroor;
							return result;
						}
						if (tagIds.Count > 0 && !productProvider.AddProductTags(product.ProductId, tagIds, dbTransaction))
						{
							dbTransaction.Rollback();
							result = ProductActionStatus.ProductTagEroor;
							return result;
						}
						dbTransaction.Commit();
						productActionStatus = ProductActionStatus.Success;
					}
					catch (Exception)
					{
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (productActionStatus == ProductActionStatus.Success)
				{
					ProductProvider.Instance().DeleteSkuUnderlingPrice();
					if (product.PenetrationStatus == PenetrationStatus.Notyet)
					{
						ProductProvider.Instance().CanclePenetrationProducts(product.ProductId.ToString(), null);
					}
					if (distributorUserIds != null && distributorUserIds.Count != 0)
					{
						foreach (int current in distributorUserIds)
						{
							ProductHelper.DeleteNotinProductLines(current);
						}
					}
					EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[]
					{
						product.ProductId
					}));
				}
				result = productActionStatus;
			}
			return result;
		}
		public static void DeleteNotinProductLines(int distributorUserId)
		{
			ProductProvider.Instance().DeleteNotinProductLines(distributorUserId);
		}
		public static bool UpdateProductCategory(int productId, int newCategoryId)
		{
			bool flag;
			if (newCategoryId != 0)
			{
				flag = ProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
			}
			else
			{
				flag = ProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, null);
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[]
				{
					productId,
					newCategoryId
				}));
			}
			return flag;
		}
		public static int DeleteProduct(string productIds, bool isDeleteImage)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				string[] array = productIds.Split(new char[]
				{
					','
				});
				IList<int> list = new List<int>();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string s = array2[i];
					list.Add(int.Parse(s));
				}
				IList<ProductInfo> products = ProductProvider.Instance().GetProducts(list);
				int num = ProductProvider.Instance().DeleteProduct(productIds);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[]
					{
						list.Count
					}));
					if (isDeleteImage)
					{
						foreach (ProductInfo current in products)
						{
							try
							{
								ProductHelper.DeleteProductImage(current);
							}
							catch
							{
							}
						}
					}
				}
				result = num;
			}
			return result;
		}
		public static int UpShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static int OffShelf(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static int InStock(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static int RemoveProduct(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
			int result;
			if (string.IsNullOrEmpty(productIds))
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
				if (num > 0)
				{
					ProductProvider.Instance().CanclePenetrationProducts(productIds, null);
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品到回收站", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static bool AddSubjectProducts(int tagId, IList<int> productIds)
		{
			return ProductProvider.Instance().AddSubjectProducts(tagId, productIds);
		}
		public static bool AddSubjectProduct(int tagId, int productId)
		{
			IList<int> list = new List<int>();
			list.Add(productId);
			return ProductProvider.Instance().AddSubjectProducts(tagId, list);
		}
		public static bool RemoveSubjectProduct(int tagId, int productId)
		{
			return ProductProvider.Instance().RemoveSubjectProduct(tagId, productId);
		}
		public static bool ClearSubjectProducts(int tagId)
		{
			return ProductProvider.Instance().ClearSubjectProducts(tagId);
		}
		public static int PenetrationProducts(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.PackProduct);
			int num = ProductProvider.Instance().PenetrationProducts(productIds);
			if (num > 0)
			{
				EventLogs.WriteOperationLog(Privilege.PackProduct, string.Format(CultureInfo.InvariantCulture, "对 “{0}” 件商品进行了铺货", new object[]
				{
					num
				}));
			}
			return num;
		}
		public static int CanclePenetrationProducts(string productIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpPackProduct);
			Database database = DatabaseFactory.CreateDatabase();
			int num;
			int result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					num = ProductProvider.Instance().CanclePenetrationProducts(productIds, dbTransaction);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						result = 0;
						return result;
					}
					if (!ProductProvider.Instance().DeleteCanclePenetrationProducts(productIds, dbTransaction))
					{
						dbTransaction.Rollback();
						result = 0;
						return result;
					}
					dbTransaction.Commit();
				}
				catch
				{
					dbTransaction.Rollback();
					result = 0;
					return result;
				}
				finally
				{
					dbConnection.Close();
				}
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.UpPackProduct, string.Format(CultureInfo.InvariantCulture, "对 “{0}” 件商品进行了取消铺货", new object[]
					{
						productIds.Split(new char[]
						{
							','
						}).Length
					}));
				}
			}
			result = num;
			return result;
		}
		public static IList<string> GetUserNameByProductId(string productIds)
		{
			return ProductProvider.Instance().GetUserNameByProductId(productIds);
		}
		public static IList<string> GetUserIdByLineId(int lineId)
		{
			return ProductProvider.Instance().GetUserIdByLineId(lineId);
		}
		public static string GetProductNamesByLineId(int lineId, out int count)
		{
			return ProductProvider.Instance().GetProductNamesByLineId(lineId, out count);
		}
		public static System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			return ProductProvider.Instance().GetProductBaseInfo(productIds);
		}
		public static bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			return ProductProvider.Instance().UpdateProductNames(productIds, prefix, suffix);
		}
		public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			return ProductProvider.Instance().ReplaceProductNames(productIds, oldWord, newWord);
		}
		public static bool UpdateProductBaseInfo(System.Data.DataTable dataTable_0)
		{
			return dataTable_0 != null && dataTable_0.Rows.Count > 0 && ProductProvider.Instance().UpdateProductBaseInfo(dataTable_0);
		}
		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			return ProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts);
		}
		public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			return ProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
		}
		public static bool UpdateShowSaleCounts(System.Data.DataTable dataTable_0)
		{
			return dataTable_0 != null && dataTable_0.Rows.Count > 0 && ProductProvider.Instance().UpdateShowSaleCounts(dataTable_0);
		}
		public static System.Data.DataTable GetSkuStocks(string productIds)
		{
			return ProductProvider.Instance().GetSkuStocks(productIds);
		}
		public static bool UpdateSkuStock(string productIds, int stock)
		{
			return ProductProvider.Instance().UpdateSkuStock(productIds, stock);
		}
		public static bool AddSkuStock(string productIds, int addStock)
		{
			return ProductProvider.Instance().AddSkuStock(productIds, addStock);
		}
		public static bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			return ProductProvider.Instance().UpdateSkuStock(skuStocks);
		}
		public static System.Data.DataTable GetSkuMemberPrices(string productIds)
		{
			return ProductProvider.Instance().GetSkuMemberPrices(productIds);
		}
		public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			return ProductProvider.Instance().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
		}
		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			return ProductProvider.Instance().UpdateSkuMemberPrices(productIds, gradeId, price);
		}
		public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			return ProductProvider.Instance().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
		}
		public static bool UpdateSkuMemberPrices(System.Data.DataSet dataSet_0)
		{
			return ProductProvider.Instance().UpdateSkuMemberPrices(dataSet_0);
		}
		public static System.Data.DataTable GetSkuDistributorPrices(string productIds)
		{
			return ProductProvider.Instance().GetSkuDistributorPrices(productIds);
		}
		public static bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price)
		{
			return ProductProvider.Instance().UpdateSkuDistributorPrices(productIds, gradeId, price);
		}
		public static bool UpdateSkuDistributorPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			return ProductProvider.Instance().UpdateSkuDistributorPrices(productIds, gradeId, baseGradeId, operation, price);
		}
		public static bool UpdateSkuDistributorPrices(System.Data.DataSet dataSet_0)
		{
			return ProductProvider.Instance().UpdateSkuDistributorPrices(dataSet_0);
		}
		public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			return ProductProvider.Instance().GetRelatedProducts(page, productId);
		}
		public static bool AddRelatedProduct(int productId, int relatedProductId)
		{
			return ProductProvider.Instance().AddRelatedProduct(productId, relatedProductId);
		}
		public static bool RemoveRelatedProduct(int productId, int relatedProductId)
		{
			return ProductProvider.Instance().RemoveRelatedProduct(productId, relatedProductId);
		}
		public static bool ClearRelatedProducts(int productId)
		{
			return ProductProvider.Instance().ClearRelatedProducts(productId);
		}
		public static System.Data.DataSet GetTaobaoProductDetails(int productId)
		{
			return ProductProvider.Instance().GetTaobaoProductDetails(productId);
		}
		public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			return ProductProvider.Instance().UpdateToaobProduct(taobaoProduct);
		}
		public static bool IsExitTaobaoProduct(long taobaoProductId)
		{
			return ProductProvider.Instance().IsExitTaobaoProduct(taobaoProductId);
		}
		public static string UploadDefaltProductImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		private static void DeleteProductImage(ProductInfo product)
		{
			if (product != null)
			{
				if (!string.IsNullOrEmpty(product.ImageUrl1))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl1);
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl2))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl2);
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl3))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl3);
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl4))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl4);
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
				if (!string.IsNullOrEmpty(product.ImageUrl5))
				{
					ResourcesHelper.DeleteImage(product.ImageUrl5);
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
					ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
				}
			}
		}
		public static System.Data.DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId)
		{
			return ProductProvider.Instance().GetSkusByProductIdByDistorId(productId, distorUserId);
		}
		public static System.Data.DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId)
		{
			return ProductProvider.Instance().GetSkuContentBySkuBuDistorUserId(skuId, distorUserId);
		}
		public static DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId)
		{
			return ProductProvider.Instance().GetSubmitPuchaseProductsByDistorUserId(query, distorUserId);
		}
		public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			return ProductProvider.Instance().GetExportProducts(query, removeProductIds);
		}
		public static System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			System.Data.DataSet exportProducts = ProductProvider.Instance().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
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
		public static void EnsureMapping(System.Data.DataSet mappingSet)
		{
			ProductProvider.Instance().EnsureMapping(mappingSet);
		}
		public static void ImportProducts(System.Data.DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
		{
			if (productData != null && productData.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in productData.Rows)
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.CategoryId = categoryId;
					productInfo.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
					productInfo.ProductName = (string)dataRow["ProductName"];
					productInfo.ProductCode = (string)dataRow["SKU"];
					productInfo.LineId = lineId;
					productInfo.BrandId = brandId;
					if (dataRow["Description"] != DBNull.Value)
					{
						productInfo.Description = (string)dataRow["Description"];
					}
					productInfo.PenetrationStatus = PenetrationStatus.Notyet;
					productInfo.AddedDate = DateTime.Now;
					productInfo.SaleStatus = saleStatus;
					productInfo.HasSKU = false;
					HttpContext current = HttpContext.Current;
					if (dataRow["ImageUrl1"] != DBNull.Value)
					{
						productInfo.ImageUrl1 = (string)dataRow["ImageUrl1"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
						productInfo.ThumbnailUrl40 = array[0];
						productInfo.ThumbnailUrl60 = array[1];
						productInfo.ThumbnailUrl100 = array[2];
						productInfo.ThumbnailUrl160 = array[3];
						productInfo.ThumbnailUrl180 = array[4];
						productInfo.ThumbnailUrl220 = array[5];
						productInfo.ThumbnailUrl310 = array[6];
						productInfo.ThumbnailUrl410 = array[7];
					}
					if (dataRow["ImageUrl2"] != DBNull.Value)
					{
						productInfo.ImageUrl2 = (string)dataRow["ImageUrl2"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
					}
					if (dataRow["ImageUrl3"] != DBNull.Value)
					{
						productInfo.ImageUrl3 = (string)dataRow["ImageUrl3"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
					}
					if (dataRow["ImageUrl4"] != DBNull.Value)
					{
						productInfo.ImageUrl4 = (string)dataRow["ImageUrl4"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
					}
					if (dataRow["ImageUrl5"] != DBNull.Value)
					{
						productInfo.ImageUrl5 = (string)dataRow["ImageUrl5"];
					}
					if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
					{
						string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
					}
					SKUItem sKUItem = new SKUItem();
					sKUItem.SkuId = "_0";
					sKUItem.SKU = (string)dataRow["SKU"];
					productInfo.LowestSalePrice = (sKUItem.PurchasePrice = (sKUItem.SalePrice = (decimal)dataRow["SalePrice"]));
					if (dataRow["Stock"] != DBNull.Value)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, new Dictionary<string, SKUItem>
					{

						{
							sKUItem.SkuId,
							sKUItem
						}
					}, null, null);
					if (isImportFromTaobao && productActionStatus == ProductActionStatus.Success)
					{
						TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
						taobaoProductInfo.ProductId = productInfo.ProductId;
						taobaoProductInfo.ProTitle = productInfo.ProductName;
						taobaoProductInfo.Cid = (long)dataRow["Cid"];
						if (dataRow["StuffStatus"] != DBNull.Value)
						{
							taobaoProductInfo.StuffStatus = (string)dataRow["StuffStatus"];
						}
						taobaoProductInfo.Num = (long)dataRow["Num"];
						taobaoProductInfo.LocationState = (string)dataRow["LocationState"];
						taobaoProductInfo.LocationCity = (string)dataRow["LocationCity"];
						taobaoProductInfo.FreightPayer = (string)dataRow["FreightPayer"];
						if (dataRow["PostFee"] != DBNull.Value)
						{
							taobaoProductInfo.PostFee = (decimal)dataRow["PostFee"];
						}
						if (dataRow["ExpressFee"] != DBNull.Value)
						{
							taobaoProductInfo.ExpressFee = (decimal)dataRow["ExpressFee"];
						}
						if (dataRow["EMSFee"] != DBNull.Value)
						{
							taobaoProductInfo.EMSFee = (decimal)dataRow["EMSFee"];
						}
						taobaoProductInfo.HasInvoice = (bool)dataRow["HasInvoice"];
						taobaoProductInfo.HasWarranty = (bool)dataRow["HasWarranty"];
						taobaoProductInfo.HasDiscount = (bool)dataRow["HasDiscount"];
						taobaoProductInfo.ValidThru = (long)dataRow["ValidThru"];
						if (dataRow["ListTime"] != DBNull.Value)
						{
							taobaoProductInfo.ListTime = (DateTime)dataRow["ListTime"];
						}
						else
						{
							taobaoProductInfo.ListTime = DateTime.Now;
						}
						if (dataRow["PropertyAlias"] != DBNull.Value)
						{
							taobaoProductInfo.PropertyAlias = (string)dataRow["PropertyAlias"];
						}
						if (dataRow["InputPids"] != DBNull.Value)
						{
							taobaoProductInfo.InputPids = (string)dataRow["InputPids"];
						}
						if (dataRow["InputStr"] != DBNull.Value)
						{
							taobaoProductInfo.InputStr = (string)dataRow["InputStr"];
						}
						if (dataRow["SkuProperties"] != DBNull.Value)
						{
							taobaoProductInfo.SkuProperties = (string)dataRow["SkuProperties"];
						}
						if (dataRow["SkuQuantities"] != DBNull.Value)
						{
							taobaoProductInfo.SkuQuantities = (string)dataRow["SkuQuantities"];
						}
						if (dataRow["SkuPrices"] != DBNull.Value)
						{
							taobaoProductInfo.SkuPrices = (string)dataRow["SkuPrices"];
						}
						if (dataRow["SkuOuterIds"] != DBNull.Value)
						{
							taobaoProductInfo.SkuOuterIds = (string)dataRow["SkuOuterIds"];
						}
						ProductHelper.UpdateToaobProduct(taobaoProductInfo);
					}
				}
			}
		}
		public static void ImportProducts(System.Data.DataSet productData, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
		{
			foreach (System.Data.DataRow dataRow in productData.Tables["products"].Rows)
			{
				int mappedProductId = (int)dataRow["ProductId"];
				ProductInfo product = ProductHelper.ConverToProduct(dataRow, categoryId, lineId, bandId, saleStatus, includeImages);
				Dictionary<string, SKUItem> skus = ProductHelper.ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
				Dictionary<int, IList<int>> attrs = ProductHelper.ConvertToAttributes(mappedProductId, productData);
				ProductHelper.AddProduct(product, skus, attrs, null);
			}
		}
		private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, System.Data.DataSet productData)
		{
			System.Data.DataRow[] array = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<int, IList<int>> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					int key = (int)dataRow["SelectedAttributeId"];
					if (!dictionary.ContainsKey(key))
					{
						IList<int> value = new List<int>();
						dictionary.Add(key, value);
					}
					dictionary[key].Add((int)dataRow["SelectedValueId"]);
				}
				result = dictionary;
			}
			return result;
		}
		private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, System.Data.DataSet productData, bool includeCostPrice, bool includeStock)
		{
			System.Data.DataRow[] array = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
			Dictionary<string, SKUItem> result;
			if (array.Length == 0)
			{
				result = null;
			}
			else
			{
				Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					string text = (string)dataRow["NewSkuId"];
					SKUItem sKUItem = new SKUItem
					{
						SkuId = text,
						SKU = (string)dataRow["SKU"],
						SalePrice = (decimal)dataRow["SalePrice"],
						PurchasePrice = (decimal)dataRow["PurchasePrice"],
						AlertStock = (int)dataRow["AlertStock"]
					};
					if (dataRow["Weight"] != DBNull.Value)
					{
						sKUItem.Weight = (decimal)dataRow["Weight"];
					}
					if (includeCostPrice && dataRow["CostPrice"] != DBNull.Value)
					{
						sKUItem.CostPrice = (decimal)dataRow["CostPrice"];
					}
					if (includeStock)
					{
						sKUItem.Stock = (int)dataRow["Stock"];
					}
					System.Data.DataRow[] array3 = productData.Tables["skuItems"].Select("NewSkuId='" + text + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
					System.Data.DataRow[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						System.Data.DataRow dataRow2 = array4[j];
						sKUItem.SkuItems.Add((int)dataRow2["SelectedAttributeId"], (int)dataRow2["SelectedValueId"]);
					}
					dictionary.Add(text, sKUItem);
				}
				result = dictionary;
			}
			return result;
		}
		private static ProductInfo ConverToProduct(System.Data.DataRow productRow, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
		{
			ProductInfo productInfo = new ProductInfo
			{
				CategoryId = categoryId,
				TypeId = new int?((int)productRow["SelectedTypeId"]),
				ProductName = (string)productRow["ProductName"],
				ProductCode = (string)productRow["ProductCode"],
				LineId = lineId,
				BrandId = bandId,
				LowestSalePrice = (decimal)productRow["LowestSalePrice"],
				Unit = (string)productRow["Unit"],
				ShortDescription = (string)productRow["ShortDescription"],
				Description = (string)productRow["Description"],
				PenetrationStatus = PenetrationStatus.Notyet,
				Title = (string)productRow["Title"],
				MetaDescription = (string)productRow["Meta_Description"],
				MetaKeywords = (string)productRow["Meta_Keywords"],
				AddedDate = DateTime.Now,
				SaleStatus = saleStatus,
				HasSKU = (bool)productRow["HasSKU"],
				MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
				ImageUrl1 = (string)productRow["ImageUrl1"],
				ImageUrl2 = (string)productRow["ImageUrl2"],
				ImageUrl3 = (string)productRow["ImageUrl3"],
				ImageUrl4 = (string)productRow["ImageUrl4"],
				ImageUrl5 = (string)productRow["ImageUrl5"]
			};
			if (productRow["MarketPrice"] != DBNull.Value)
			{
				productInfo.MarketPrice = new decimal?((decimal)productRow["MarketPrice"]);
			}
			if (includeImages)
			{
				HttpContext current = HttpContext.Current;
				if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
					productInfo.ThumbnailUrl40 = array[0];
					productInfo.ThumbnailUrl60 = array[1];
					productInfo.ThumbnailUrl100 = array[2];
					productInfo.ThumbnailUrl160 = array[3];
					productInfo.ThumbnailUrl180 = array[4];
					productInfo.ThumbnailUrl220 = array[5];
					productInfo.ThumbnailUrl310 = array[6];
					productInfo.ThumbnailUrl410 = array[7];
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
				}
				if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
				{
					string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
				}
			}
			return productInfo;
		}
		private static string[] ProcessImages(HttpContext context, string originalSavePath)
		{
			string fileName = Path.GetFileName(originalSavePath);
			string text = "/Storage/master/product/thumbs40/40_" + fileName;
			string text2 = "/Storage/master/product/thumbs60/60_" + fileName;
			string text3 = "/Storage/master/product/thumbs100/100_" + fileName;
			string text4 = "/Storage/master/product/thumbs160/160_" + fileName;
			string text5 = "/Storage/master/product/thumbs180/180_" + fileName;
			string text6 = "/Storage/master/product/thumbs220/220_" + fileName;
			string text7 = "/Storage/master/product/thumbs310/310_" + fileName;
			string text8 = "/Storage/master/product/thumbs410/410_" + fileName;
			string text9 = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
			if (File.Exists(text9))
			{
				try
				{
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text), 40, 40);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text2), 60, 60);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text3), 100, 100);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text4), 160, 160);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text5), 180, 180);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text6), 220, 220);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text7), 310, 310);
					ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text8), 410, 410);
				}
				catch
				{
				}
			}
			return new string[]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			};
		}
		public static System.Data.DataSet GetProductsByQuery(ProductQuery query, out int totalrecord)
		{
			return ProductProvider.Instance().GetProductsByQuery(query, out totalrecord);
		}
		public static System.Data.DataSet GetProductSkuDetials(int productId)
		{
			return ProductProvider.Instance().GetProductSkuDetials(productId);
		}
		public static int OffShelf(int productId)
		{
			int result;
			if (productId <= 0)
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productId.ToString(), ProductSaleStatus.UnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static int UpShelf(int productId)
		{
			int result;
			if (productId <= 0)
			{
				result = 0;
			}
			else
			{
				int num = ProductProvider.Instance().UpdateProductSaleStatus(productId.ToString(), ProductSaleStatus.OnSale);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[]
					{
						num
					}));
				}
				result = num;
			}
			return result;
		}
		public static ApiErrorCode UpdateProductStock(int productId, string skuId, string string_0, int type, int stock)
		{
			ApiErrorCode result;
			if (productId <= 0 || (type == 1 && stock <= 0))
			{
				result = ApiErrorCode.Format_Eroor;
			}
			else
			{
				skuId = DataHelper.CleanSearchString(skuId);
				string_0 = DataHelper.CleanSearchString(string_0);
				System.Data.DataTable skuStocks = ProductProvider.Instance().GetSkuStocks(productId.ToString());
				string key = "";
				bool flag = false;
				if (skuStocks.Rows.Count <= 0)
				{
					result = ApiErrorCode.Exists_Error;
				}
				else
				{
					int num = Convert.ToInt32(skuStocks.Rows[0]["Stock"]);
					if (!string.IsNullOrEmpty(skuId))
					{
						System.Data.DataRow[] array = skuStocks.Select("SkuId='" + skuId + "'");
						if (array.Length <= 0)
						{
							result = ApiErrorCode.Exists_Error;
							return result;
						}
						num = Convert.ToInt32(array[0]["Stock"]);
						key = skuId;
						flag = true;
					}
					if (!string.IsNullOrEmpty(string_0) && string.IsNullOrEmpty(skuId))
					{
						System.Data.DataRow[] array = skuStocks.Select("SKU='" + string_0 + "'");
						if (array.Length <= 0)
						{
							result = ApiErrorCode.Exists_Error;
							return result;
						}
						num = Convert.ToInt32(array[0]["Stock"]);
						key = array[0]["SkuId"].ToString();
						flag = true;
					}
					if (type != 1)
					{
						if (num + stock <= 0)
						{
							stock = 0;
						}
						else
						{
							stock += num;
						}
					}
					bool flag2;
					if (!flag)
					{
						flag2 = ProductProvider.Instance().UpdateSkuStock(productId.ToString(), stock);
					}
					else
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>();
						dictionary.Add(key, stock);
						flag2 = ProductProvider.Instance().UpdateSkuStock(dictionary);
					}
					if (flag2)
					{
						result = ApiErrorCode.Success;
					}
					else
					{
						result = ApiErrorCode.Unknown_Error;
					}
				}
			}
			return result;
		}
		public static bool AddProductTags(int productId, IList<int> tagsId, System.Data.Common.DbTransaction dbtran)
		{
			return ProductProvider.Instance().AddProductTags(productId, tagsId, dbtran);
		}
		public static bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
		{
			return ProductProvider.Instance().DeleteProductTags(productId, tran);
		}
		public static System.Data.DataTable GetSkusByProductId(int productId)
		{
			return ProductProvider.Instance().GetSkusByProductId(productId);
		}
		public static DbQueryResult GetBindProducts(ProductQuery query)
		{
			return ProductProvider.Instance().GetBindProducts(query);
		}
	}
}
