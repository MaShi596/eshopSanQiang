using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class PromoteTypeRadioButtonList : RadioButtonList
	{
		public bool IsProductPromote
		{
			get;
			set;
		}
		public bool IsWholesale
		{
			get;
			set;
		}
		public bool IsSubSite
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsProductPromote)
			{
				if (this.IsWholesale)
				{
					stringBuilder.AppendFormat("<input id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />批发打折", 4);
				}
				else
				{
					stringBuilder.AppendFormat("<input id=\"radPromoteType_Discount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />直接打折", 1);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_Amount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />固定金额出售", 2);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_Reduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />减价优惠", 3);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_SentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />买商品赠送礼品", 5);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_SentProduct\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />有买有送", 6);
					if (this.IsSubSite)
					{
						stringBuilder.AppendFormat("<input id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />批发打折", 4);
					}
				}
			}
			else
			{
				if (this.IsWholesale)
				{
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发打折", 13);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发优惠金额", 14);
				}
				else
				{
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullAmountDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额打折", 11);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullAmountReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额优惠金额", 12);
					if (this.IsSubSite)
					{
						stringBuilder.AppendFormat("<input id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发打折", 13);
						stringBuilder.AppendFormat("<input id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />混合批发优惠金额", 14);
					}
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullAmountSentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额送礼品", 15);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullAmountSentTimesPoint\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额送倍数积分", 16);
					stringBuilder.AppendFormat("<input id=\"radPromoteType_FullAmountSentFreight\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" />满额免运费", 17);
				}
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
