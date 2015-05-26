using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Sales
{
	public class PurchaseOrderInfo
	{
		private System.Collections.Generic.IList<PurchaseOrderItemInfo> purchaseOrderItems;
		private System.Collections.Generic.IList<PurchaseOrderGiftInfo> purchaseOrderGifts;
		public System.Collections.Generic.IList<PurchaseOrderItemInfo> PurchaseOrderItems
		{
			get
			{
				if (this.purchaseOrderItems == null)
				{
					this.purchaseOrderItems = new System.Collections.Generic.List<PurchaseOrderItemInfo>();
				}
				return this.purchaseOrderItems;
			}
		}
		public System.Collections.Generic.IList<PurchaseOrderGiftInfo> PurchaseOrderGifts
		{
			get
			{
				if (this.purchaseOrderGifts == null)
				{
					this.purchaseOrderGifts = new System.Collections.Generic.List<PurchaseOrderGiftInfo>();
				}
				return this.purchaseOrderGifts;
			}
		}
		public string PurchaseOrderId
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public OrderMark? ManagerMark
		{
			get;
			set;
		}
		public string ManagerRemark
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValPurchaseOrder", MessageTemplate = "采购单折扣不能为空，金额大小负1000万-1000万之间")]
		public decimal AdjustedDiscount
		{
			get;
			set;
		}
		public OrderStatus PurchaseStatus
		{
			get;
			set;
		}
		public string CloseReason
		{
			get;
			set;
		}
		public System.DateTime PurchaseDate
		{
			get;
			set;
		}
		public System.DateTime PayDate
		{
			get;
			set;
		}
		public int PaymentTypeId
		{
			get;
			set;
		}
		public string PaymentType
		{
			get;
			set;
		}
		public string Gateway
		{
			get;
			set;
		}
		public System.DateTime ShippingDate
		{
			get;
			set;
		}
		public System.DateTime FinishDate
		{
			get;
			set;
		}
		public int DistributorId
		{
			get;
			set;
		}
		public string Distributorname
		{
			get;
			set;
		}
		public string DistributorEmail
		{
			get;
			set;
		}
		public string DistributorRealName
		{
			get;
			set;
		}
		public string DistributorQQ
		{
			get;
			set;
		}
		public string DistributorWangwang
		{
			get;
			set;
		}
		public string DistributorMSN
		{
			get;
			set;
		}
		public string ShippingRegion
		{
			get;
			set;
		}
		public string Address
		{
			get;
			set;
		}
		public string ZipCode
		{
			get;
			set;
		}
		public string ShipTo
		{
			get;
			set;
		}
		public string TelPhone
		{
			get;
			set;
		}
		public string CellPhone
		{
			get;
			set;
		}
		public string ShipToDate
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
		public int ShippingModeId
		{
			get;
			set;
		}
		public string ModeName
		{
			get;
			set;
		}
		public int RealShippingModeId
		{
			get;
			set;
		}
		public string RealModeName
		{
			get;
			set;
		}
		public int RegionId
		{
			get;
			set;
		}
		public decimal Freight
		{
			get;
			set;
		}
		public decimal AdjustedFreight
		{
			get;
			set;
		}
		public string ShipOrderNumber
		{
			get;
			set;
		}
		public string ExpressCompanyName
		{
			get;
			set;
		}
		public string ExpressCompanyAbb
		{
			get;
			set;
		}
		public decimal Weight
		{
			get;
			set;
		}
		public RefundStatus RefundStatus
		{
			get;
			set;
		}
		public decimal RefundAmount
		{
			get;
			set;
		}
		public string RefundRemark
		{
			get;
			set;
		}
		public decimal OrderTotal
		{
			get;
			set;
		}
		public string TaobaoOrderId
		{
			get;
			set;
		}
		public bool IsManualPurchaseOrder
		{
			get
			{
				return string.IsNullOrEmpty(this.OrderId);
			}
		}
		public decimal Tax
		{
			get;
			set;
		}
		public string InvoiceTitle
		{
			get;
			set;
		}
		public decimal GetPurchaseProfit()
		{
			return this.GetPurchaseTotal() - this.RefundAmount - this.GetPurchaseCostPrice();
		}
		public decimal GetPurchaseCostPrice()
		{
			decimal num = 0m;
			foreach (PurchaseOrderItemInfo current in this.PurchaseOrderItems)
			{
				num += current.ItemCostPrice * current.Quantity;
			}
			foreach (PurchaseOrderGiftInfo current2 in this.PurchaseOrderGifts)
			{
				num += current2.CostPrice * current2.Quantity;
			}
			return num;
		}
		public decimal GetPurchaseTotal()
		{
			return this.GetProductAmount() + this.GetGiftAmount() + this.AdjustedFreight + this.AdjustedDiscount + this.Tax;
		}
		public decimal GetProductAmount()
		{
			decimal num = 0m;
			foreach (PurchaseOrderItemInfo current in this.PurchaseOrderItems)
			{
				num += current.GetSubTotal();
			}
			return num;
		}
		public decimal GetGiftAmount()
		{
			decimal num = 0m;
			foreach (PurchaseOrderGiftInfo current in this.PurchaseOrderGifts)
			{
				num += current.GetSubTotal();
			}
			return num;
		}
		public bool CheckAction(PurchaseOrderActions action)
		{
			bool result;
			if (this.PurchaseStatus == OrderStatus.Finished || this.PurchaseStatus == OrderStatus.Closed)
			{
				result = false;
			}
			else
			{
				switch (action)
				{
				case PurchaseOrderActions.DISTRIBUTOR_CLOSE:
				case PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS:
				case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY:
				case PurchaseOrderActions.MASTER__CLOSE:
				case PurchaseOrderActions.MASTER__MODIFY_AMOUNT:
				case PurchaseOrderActions.MASTER_CONFIRM_PAY:
					result = (this.PurchaseStatus == OrderStatus.WaitBuyerPay);
					break;
				case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS:
				case PurchaseOrderActions.MASTER_FINISH_TRADE:
					result = (this.PurchaseStatus == OrderStatus.SellerAlreadySent);
					break;
				case PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE:
					result = (this.PurchaseStatus == OrderStatus.WaitBuyerPay || this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid);
					break;
				case PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS:
					result = (this.PurchaseStatus == OrderStatus.WaitBuyerPay || this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid);
					break;
				case PurchaseOrderActions.MASTER_SEND_GOODS:
					result = (this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid || (this.PurchaseStatus == OrderStatus.WaitBuyerPay && this.Gateway == "hishop.plugins.payment.podrequest"));
					break;
				case PurchaseOrderActions.MASTER_REJECT_REFUND:
					result = (this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid || this.PurchaseStatus == OrderStatus.SellerAlreadySent);
					break;
				default:
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
