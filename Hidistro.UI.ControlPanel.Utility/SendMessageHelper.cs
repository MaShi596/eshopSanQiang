using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Comments;
using System;
using System.Collections.Generic;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class SendMessageHelper
	{
		public static void SendMessageToDistributors(string productId, int type)
		{
			int num = 0;
			IList<string> list = new List<string>();
			string str;
			string format;
			string format2;
			switch (type)
			{
			case 1:
				str = ProductHelper.GetProductNameByProductIds(productId, out num);
				list = ProductHelper.GetUserNameByProductId(productId.ToString());
				format = "供应商下架了{0}个商品";
				format2 = "尊敬的各位分销商，供应商已下架了{0}个商品，如下：";
				break;
			case 2:
				str = ProductHelper.GetProductNameByProductIds(productId, out num);
				list = ProductHelper.GetUserNameByProductId(productId.ToString());
				format = "供应商入库了{0}个商品";
				format2 = "尊敬的各位分销商，供应商已入库了{0}个商品，如下：";
				break;
			case 3:
				str = ProductHelper.GetProductNameByProductIds(productId, out num);
				list = ProductHelper.GetUserNameByProductId(productId.ToString());
				format = "供应商删除了{0}个商品";
				format2 = "尊敬的各位分销商，供应商已删除了{0}个商品，如下：";
				break;
			case 4:
			{
				string text = productId.Split(new char[]
				{
					'|'
				})[1].ToString();
				string text2 = productId.Split(new char[]
				{
					'|'
				})[2].ToString();
				str = ProductHelper.GetProductNamesByLineId(Convert.ToInt32(productId.Split(new char[]
				{
					'|'
				})[0].ToString()), out num);
				list = ProductHelper.GetUserIdByLineId(Convert.ToInt32(productId.Split(new char[]
				{
					'|'
				})[0].ToString()));
				format = "供应商转移了{0}个商品";
				format2 = string.Concat(new string[]
				{
					"尊敬的各位分销商，供应商已将整个产品线",
					text,
					"的商品转移移至产品线",
					text2,
					"目录下，共{0}个，如下："
				});
				break;
			}
			case 5:
				str = ProductHelper.GetProductNameByProductIds(productId, out num);
				list = ProductHelper.GetUserNameByProductId(productId.ToString());
				format = "供应商取消了{0}个商品的铺货状态";
				format2 = "尊敬的各位分销商，供应商已取消了{0}个商品的铺货，如下：";
				break;
			default:
			{
				string text2 = productId.Split(new char[]
				{
					'|'
				})[1].ToString();
				str = ProductHelper.GetProductNameByProductIds(productId.Split(new char[]
				{
					'|'
				})[0].ToString(), out num);
				list = ProductHelper.GetUserNameByProductId(productId.Split(new char[]
				{
					'|'
				})[0].ToString());
				format = "供应商转移了{0}个商品至产品线" + text2;
				format2 = "尊敬的各位分销商，供应商已转移了{0}个商品至产品线" + text2 + "，如下：";
				break;
			}
			}
			if (num > 0)
			{
				IList<MessageBoxInfo> list2 = new List<MessageBoxInfo>();
				foreach (string current in list)
				{
					list2.Add(new MessageBoxInfo
					{
						Accepter = current,
						Sernder = "admin",
						Title = string.Format(format, num),
						Content = string.Format(format2, num) + str
					});
				}
				if (list2.Count > 0)
				{
					NoticeHelper.AddMessageToDistributor(list2);
				}
			}
		}
	}
}
