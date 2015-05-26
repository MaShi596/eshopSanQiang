using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hidistro.UI.Web.Shopadmin.promotion.Ascx;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditeMyOrderPromotions : DistributorPage
	{
		protected PromoteTypeRadioButtonList radPromoteType;
		protected TrimTextBox txtPromoteType;
		protected System.Web.UI.WebControls.TextBox txtCondition;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected MyPromotionView promotionView;
		protected System.Web.UI.WebControls.Button btnSave;
		private int activityId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!this.Page.IsPostBack)
			{
				PromotionInfo promotion = SubsitePromoteHelper.GetPromotion(this.activityId);
				this.promotionView.Promotion = promotion;
				this.txtPromoteType.Text = ((int)promotion.PromoteType).ToString();
				if (promotion.PromoteType != PromoteType.FullQuantityDiscount)
				{
					if (promotion.PromoteType != PromoteType.FullQuantityReduced)
					{
						this.txtCondition.Text = promotion.Condition.ToString("F2");
						goto IL_DB;
					}
				}
				this.txtCondition.Text = promotion.Condition.ToString("F0");
				IL_DB:
				this.txtDiscountValue.Text = promotion.DiscountValue.ToString("F2");
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
			promotion.ActivityId = this.activityId;
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
			int num = SubsitePromoteHelper.EditPromotion(promotion);
			if (num == -1)
			{
				this.ShowMsg("编辑促销活动失败，可能是信填写有误，请重试", false);
				return;
			}
			if (num == -2)
			{
				this.ShowMsg("编辑促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
				return;
			}
			if (num == 0)
			{
				this.ShowMsg("编辑促销活动失败，请重试", false);
				return;
			}
			this.ShowMsg("编辑促销活动成功", true);
		}
	}
}
