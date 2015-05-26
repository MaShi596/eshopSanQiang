using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.promotion.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class AddProductPromotion : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl h1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl span1;
		protected PromoteTypeRadioButtonList radPromoteType;
		protected TrimTextBox txtPromoteType;
		protected System.Web.UI.WebControls.TextBox txtCondition;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected PromotionView promotionView;
		protected System.Web.UI.WebControls.Button btnNext;
		public bool isWholesale;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
			if (this.isWholesale)
			{
				this.radPromoteType.IsWholesale = true;
				this.h1.InnerText = "添加批发规则";
				this.span1.InnerText = "添加批发规则";
			}
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
		}
		private void btnNext_Click(object sender, System.EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
			if (promotion.MemberGradeIds.Count <= 0)
			{
				this.ShowMsg("必须选择一个适合的客户", false);
				return;
			}
			if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
			{
				this.ShowMsg("开始日期应该小于结束日期", false);
				return;
			}
			promotion.PromoteType = (PromoteType)int.Parse(this.txtPromoteType.Text);
			decimal condition = 0m;
			decimal discountValue = 0m;
			decimal.TryParse(this.txtCondition.Text.Trim(), out condition);
			decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discountValue);
			promotion.Condition = condition;
			promotion.DiscountValue = discountValue;
			int num = PromoteHelper.AddPromotion(promotion);
			if (num == -1)
			{
				this.ShowMsg("添加促销活动失败，可能是信填写有误，请重试", false);
				return;
			}
			if (num == -2)
			{
				this.ShowMsg("添加促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
				return;
			}
			if (num == 0)
			{
				this.ShowMsg("添加促销活动失败，请重试", false);
				return;
			}
			base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Concat(new object[]
			{
				"/promotion/SetPromotionProducts.aspx?ActivityId=",
				num,
				"&isWholesale=",
				this.isWholesale
			})), true);
		}
	}
}
