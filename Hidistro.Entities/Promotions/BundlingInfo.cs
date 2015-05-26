using System;
using System.Collections.Generic;
namespace Hidistro.Entities.Promotions
{
	public class BundlingInfo
	{
		public int BundlingID
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public int Num
		{
			get;
			set;
		}
		public string ShortDescription
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public decimal Price
		{
			get;
			set;
		}
		public int SaleStatus
		{
			get;
			set;
		}
		public System.DateTime AddTime
		{
			get;
			set;
		}
		public System.Collections.Generic.List<BundlingItemInfo> BundlingItemInfos
		{
			get;
			set;
		}
		public BundlingInfo()
		{
			if (this.BundlingItemInfos == null)
			{
				this.BundlingItemInfos = new System.Collections.Generic.List<BundlingItemInfo>();
			}
		}
	}
}
