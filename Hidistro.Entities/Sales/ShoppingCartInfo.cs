using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
namespace Hidistro.Entities.Sales
{
	public class ShoppingCartInfo
	{
		private bool isSendGift;
		private decimal timesPoint = 1m;
		private System.Collections.Generic.Dictionary<string, ShoppingCartItemInfo> lineItems;
		private System.Collections.Generic.IList<ShoppingCartGiftInfo> lineGifts;
		public int ReducedPromotionId
		{
			get;
			set;
		}
		public string ReducedPromotionName
		{
			get;
			set;
		}
		public decimal ReducedPromotionAmount
		{
			get;
			set;
		}
		public bool IsReduced
		{
			get;
			set;
		}
		public int SendGiftPromotionId
		{
			get;
			set;
		}
		public string SendGiftPromotionName
		{
			get;
			set;
		}
		public bool IsSendGift
		{
			get
			{
				bool result;
				foreach (ShoppingCartItemInfo current in this.lineItems.Values)
				{
					if (current.IsSendGift)
					{
						result = true;
						return result;
					}
				}
				result = this.isSendGift;
				return result;
			}
			set
			{
				this.isSendGift = value;
			}
		}
		public int SentTimesPointPromotionId
		{
			get;
			set;
		}
		public string SentTimesPointPromotionName
		{
			get;
			set;
		}
		public bool IsSendTimesPoint
		{
			get;
			set;
		}
		public decimal TimesPoint
		{
			get
			{
				return this.timesPoint;
			}
			set
			{
				this.timesPoint = value;
			}
		}
		public int FreightFreePromotionId
		{
			get;
			set;
		}
		public string FreightFreePromotionName
		{
			get;
			set;
		}
		public bool IsFreightFree
		{
			get;
			set;
		}
		public System.Collections.Generic.Dictionary<string, ShoppingCartItemInfo> LineItems
		{
			get
			{
				if (this.lineItems == null)
				{
					this.lineItems = new System.Collections.Generic.Dictionary<string, ShoppingCartItemInfo>();
				}
				return this.lineItems;
			}
		}
		public System.Collections.Generic.IList<ShoppingCartGiftInfo> LineGifts
		{
			get
			{
				if (this.lineGifts == null)
				{
					this.lineGifts = new System.Collections.Generic.List<ShoppingCartGiftInfo>();
				}
				return this.lineGifts;
			}
		}
		public decimal Weight
		{
			get
			{
				decimal num = 0m;
				foreach (ShoppingCartItemInfo current in this.lineItems.Values)
				{
					num += current.GetSubWeight();
				}
				return num;
			}
		}
		public decimal GetTotal()
		{
			return this.GetAmount() - this.ReducedPromotionAmount;
		}
		public int GetTotalNeedPoint()
		{
			int num = 0;
			foreach (ShoppingCartGiftInfo current in this.LineGifts)
			{
				num += current.SubPointTotal;
			}
			return num;
		}
		public int GetPoint()
		{
			int result = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (this.GetTotal() * this.TimesPoint / masterSettings.PointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else
			{
				if (masterSettings.PointsRate != 0m)
				{
					result = (int)System.Math.Round(this.GetTotal() * this.TimesPoint / masterSettings.PointsRate, 0);
				}
			}
			return result;
		}
		public int GetPoint(decimal money)
		{
			int result = 0;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (money * this.TimesPoint / masterSettings.PointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else
			{
				if (masterSettings.PointsRate != 0m)
				{
					result = (int)System.Math.Round(money * this.TimesPoint / masterSettings.PointsRate, 0);
				}
			}
			return result;
		}
		public decimal GetAmount()
		{
			decimal num = 0m;
			foreach (ShoppingCartItemInfo current in this.lineItems.Values)
			{
				num += current.SubTotal;
			}
			return num;
		}
		public int GetQuantity()
		{
			int num = 0;
			foreach (ShoppingCartItemInfo current in this.lineItems.Values)
			{
				num += current.Quantity;
			}
			return num;
		}
	}
}
