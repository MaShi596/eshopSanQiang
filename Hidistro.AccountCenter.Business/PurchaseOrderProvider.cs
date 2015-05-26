using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Data.Common;
namespace Hidistro.AccountCenter.Business
{
	public abstract class PurchaseOrderProvider
	{
		public static PurchaseOrderProvider CreateInstance()
		{
			return (PurchaseOrderProvider)DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.PurchaseOrderData,Hidistro.AccountCenter.DistributionData");
		}
		public abstract bool CreatePurchaseOrder(OrderInfo order, System.Data.Common.DbTransaction dbTran);
	}
}
