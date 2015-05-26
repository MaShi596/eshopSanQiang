using Hidistro.AccountCenter.Business;
using Hidistro.Membership.Context;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class MyChangeCoupons : MemberTemplatedWebControl
	{
		private Common_Coupon_ChangeCouponList changeCoupons;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyChangeCoupons.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.changeCoupons = (Common_Coupon_ChangeCouponList)this.FindControl("Common_Coupon_ChangeCouponList");
			this.changeCoupons.ItemCommand += new Common_Coupon_ChangeCouponList.CommandEventHandler(this.changeCoupons_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}
		private void changeCoupons_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int couponId = (int)this.changeCoupons.DataKeys[e.Item.ItemIndex];
			if (e.CommandName == "Change")
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litNeedPoint");
				int num = int.Parse(literal.Text);
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (num > member.Points)
				{
					this.ShowMessage("当前积分不够兑换此优惠券", false);
				}
				else
				{
					if (TradeHelper.PointChageCoupon(couponId, num, member.Points))
					{
						this.ShowMessage("兑换成功，请查看您的优惠券", true);
					}
					else
					{
						this.ShowMessage("兑换失败", false);
					}
				}
			}
		}
		private void BindCoupons()
		{
			this.changeCoupons.DataSource = TradeHelper.GetChangeCoupons();
			this.changeCoupons.DataBind();
		}
	}
}
