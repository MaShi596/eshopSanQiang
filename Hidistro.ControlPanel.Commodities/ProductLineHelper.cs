using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
namespace Hidistro.ControlPanel.Commodities
{
	public class ProductLineHelper
	{
		public static bool AddProductLine(ProductLineInfo productLine)
		{
			Globals.EntityCoding(productLine, true);
			bool result;
			if (result = ProductProvider.Instance().AddProductLine(productLine))
			{
				EventLogs.WriteOperationLog(Privilege.AddProductLine, string.Format(CultureInfo.InvariantCulture, "成功的添加了一条产品线", new object[0]));
			}
			return result;
		}
		public static bool UpdateProductLine(ProductLineInfo productLine)
		{
			Globals.EntityCoding(productLine, true);
			bool result;
			if (result = ProductProvider.Instance().UpdateProductLine(productLine))
			{
				EventLogs.WriteOperationLog(Privilege.EditProductLine, string.Format(CultureInfo.InvariantCulture, "修改了产品线", new object[0]));
			}
			return result;
		}
		public static bool DeleteProductLine(int lineId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductLine);
			bool result;
			if (result = ProductProvider.Instance().DeleteProductLine(lineId))
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductLine, string.Format(CultureInfo.InvariantCulture, "删除了一条产品线", new object[0]));
			}
			return result;
		}
		public static ProductLineInfo GetProductLine(int lineId)
		{
			return ProductProvider.Instance().GetProductLine(lineId);
		}
		public static bool ReplaceProductLine(int fromlineId, int replacelineId)
		{
			return ProductProvider.Instance().ReplaceProductLine(fromlineId, replacelineId);
		}
		public static bool UpdateProductLine(int replacelineId, int productId)
		{
			return ProductProvider.Instance().UpdateProductLine(replacelineId, productId);
		}
		public static System.Data.DataTable GetProductLines()
		{
			return ProductProvider.Instance().GetProductLines();
		}
		public static IList<ProductLineInfo> GetProductLineList()
		{
			return ProductProvider.Instance().GetProductLineList();
		}
		public static bool AddSupplier(string supplierName, string remark)
		{
			return ProductProvider.Instance().AddSupplier(supplierName, remark);
		}
		public static DbQueryResult GetSuppliers(Pagination page)
		{
			return ProductProvider.Instance().GetSuppliers(page);
		}
		public static IList<string> GetSuppliers()
		{
			return ProductProvider.Instance().GetSuppliers();
		}
		public static string GetSupplierRemark(string supplierName)
		{
			return ProductProvider.Instance().GetSupplierRemark(supplierName);
		}
		public static bool UpdateSupplier(string oldName, string newName, string remark)
		{
			return ProductProvider.Instance().UpdateSupplier(oldName, newName, remark);
		}
		public static void DeleteSupplier(string supplierName)
		{
			ProductProvider.Instance().DeleteSupplier(supplierName);
		}
	}
}
