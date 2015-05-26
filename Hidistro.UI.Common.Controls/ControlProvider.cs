using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.UI.Common.Controls
{
	public abstract class ControlProvider
	{
		private static readonly ControlProvider _defaultInstance;
		static ControlProvider()
		{
			ControlProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.UI.Common.Data.SqlCommonDataProvider, Hidistro.UI.Common.Data") as ControlProvider);
		}
		public static ControlProvider Instance()
		{
			return ControlProvider._defaultInstance;
		}
		public abstract IList<ProductTypeInfo> GetProductTypes();
		public abstract DataTable GetBrandCategories();
		public abstract DataTable GetBrandCategoriesByTypeId(int typeId);
		public abstract IList<ProductLineInfo> GetProductLineList();
		public abstract IList<ShippingModeInfo> GetShippingModes();
		public abstract void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum);
		public abstract DataTable GetTags();
		public abstract DataTable GetSkuContentBySku(string skuId);
	}
}
