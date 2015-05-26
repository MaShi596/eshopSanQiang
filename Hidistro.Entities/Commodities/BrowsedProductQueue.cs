using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Web;
namespace Hidistro.Entities.Commodities
{
	public class BrowsedProductQueue
	{
		private static string browedProductList;
		static BrowsedProductQueue()
		{
			BrowsedProductQueue.browedProductList = "BrowedProductList-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				BrowsedProductQueue.browedProductList = string.Format("BrowedProductList-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
		}
		public static System.Collections.Generic.IList<int> GetBrowedProductList()
		{
			System.Collections.Generic.IList<int> result = new System.Collections.Generic.List<int>();
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[BrowsedProductQueue.browedProductList];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				result = (Serializer.ConvertToObject(HiContext.Current.Context.Server.UrlDecode(httpCookie.Value), typeof(System.Collections.Generic.List<int>)) as System.Collections.Generic.List<int>);
			}
			return result;
		}
		public static System.Collections.Generic.IList<int> GetBrowedProductList(int maxNum)
		{
			System.Collections.Generic.IList<int> list = BrowsedProductQueue.GetBrowedProductList();
			int count = list.Count;
			if (list.Count > maxNum)
			{
				for (int i = 0; i < count - maxNum; i++)
				{
					list.RemoveAt(0);
				}
			}
			return list;
		}
		public static void EnQueue(int productId)
		{
			System.Collections.Generic.IList<int> list = BrowsedProductQueue.GetBrowedProductList();
			int num = 0;
			foreach (int current in list)
			{
				if (productId == current)
				{
					list.RemoveAt(num);
					break;
				}
				num++;
			}
			if (list.Count <= 20)
			{
				list.Add(productId);
			}
			else
			{
				list.RemoveAt(0);
				list.Add(productId);
			}
			BrowsedProductQueue.SaveCookie(list);
		}
		public static void ClearQueue()
		{
			BrowsedProductQueue.SaveCookie(null);
		}
		private static void SaveCookie(System.Collections.Generic.IList<int> productIdList)
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[BrowsedProductQueue.browedProductList];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(BrowsedProductQueue.browedProductList);
			}
			httpCookie.Expires = System.DateTime.Now.AddDays(7.0);
			httpCookie.Value = Globals.UrlEncode(Serializer.ConvertToString(productIdList));
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}
	}
}
