using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Coupon_CouponList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Coupons_CouponsList";
		private System.Web.UI.WebControls.DataList dataListCoupon;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataListCoupon.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListCoupon.DataSource = value;
			}
		}
		public Common_Coupon_CouponList()
		{
			base.ID = "Common_Coupons_CouponsList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_CouponList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListCoupon = (System.Web.UI.WebControls.DataList)this.FindControl("dataListCoupon");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListCoupon.DataSource != null)
			{
				this.dataListCoupon.DataBind();
			}
		}
	}
}
