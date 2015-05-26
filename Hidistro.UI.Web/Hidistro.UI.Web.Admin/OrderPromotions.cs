using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderPromotion)]
	public class OrderPromotions : AdminPage
	{
		public bool isWholesale;
		protected System.Web.UI.WebControls.Literal litTite;
		protected System.Web.UI.WebControls.Literal litDec;
		protected System.Web.UI.WebControls.HyperLink hlinkAddPromotion;
		protected Grid grdPromoteSales;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
			if (this.isWholesale)
			{
				this.litTite.Text = "混批规则";
				this.litDec.Text = "针对订单中的所有商品满足一定数量时的打折促销，您可以添加新的混批促销活动或管理当前的混批促销活动";
				this.hlinkAddPromotion.Text = "添加新的混批规则";
				this.hlinkAddPromotion.NavigateUrl = "AddOrderPromotion.aspx?isWholesale=true";
			}
			this.grdPromoteSales.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdPromoteSales_RowDataBound);
			this.grdPromoteSales.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPromoteSales_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				this.BindProductPromotions();
			}
		}
		private void grdPromoteSales_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int activityId = (int)this.grdPromoteSales.DataKeys[e.RowIndex].Value;
			if (PromoteHelper.DeletePromotion(activityId))
			{
				this.ShowMsg("成功删除了选择的促销活动", true);
				this.BindProductPromotions();
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void BindProductPromotions()
		{
			this.grdPromoteSales.DataSource = PromoteHelper.GetPromotions(false, this.isWholesale);
			this.grdPromoteSales.DataBind();
		}
		private void grdPromoteSales_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int activityId = int.Parse(this.grdPromoteSales.DataKeys[e.Row.RowIndex].Value.ToString());
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblmemberGrades");
				System.Web.UI.WebControls.Label label2 = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblPromoteType");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("ltrPromotionInfo");
				System.Collections.Generic.IList<MemberGradeInfo> promoteMemberGrades = PromoteHelper.GetPromoteMemberGrades(activityId);
				string text = string.Empty;
				foreach (MemberGradeInfo current in promoteMemberGrades)
				{
					text = text + current.Name + ",";
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = text.Remove(text.Length - 1);
				}
				label.Text = text;
				switch ((int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PromoteType"))
				{
				case 11:
					label2.Text = "满额打折";
					literal.Text = string.Format("满足金额：{0} 折扣值：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
					return;
				case 12:
					label2.Text = "满额优惠金额";
					literal.Text = string.Format("满足金额：{0} 优惠金额：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
					return;
				case 13:
					label2.Text = "满量打折";
					literal.Text = string.Format("满足数量：{0} 折扣值：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
					return;
				case 14:
					label2.Text = "满量优惠金额";
					literal.Text = string.Format("满足数量：{0}，优惠金额：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f0}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
					return;
				case 15:
					label2.Text = "满额送礼品";
					literal.Text = string.Format("满足金额：{0} <a  href=\"javascript:DialogFrame('promotion/gifts.aspx?isPromotion=true','查看促销礼品',null,null)\">查看促销礼品</a>", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"));
					return;
				case 16:
					label2.Text = "满额送倍数积分";
					literal.Text = string.Format("满足金额：{0}，倍数：{1}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"), System.Web.UI.DataBinder.Eval(e.Row.DataItem, "DiscountValue", "{0:f2}"));
					return;
				case 17:
					label2.Text = "满额免运费";
					literal.Text = string.Format("满足金额：{0}", System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Condition", "{0:f2}"));
					break;
				default:
					return;
				}
			}
		}
	}
}
