using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Coupon_ChangeCouponList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e);
		public const string TagID = "Common_Coupon_ChangeCouponList";
		private System.Web.UI.WebControls.DataList dataListCoupon;
		public event Common_Coupon_ChangeCouponList.CommandEventHandler ItemCommand;
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
		public System.Web.UI.WebControls.DataKeyCollection DataKeys
		{
			get
			{
				return this.dataListCoupon.DataKeys;
			}
		}
		public Common_Coupon_ChangeCouponList()
		{
			base.ID = "Common_Coupon_ChangeCouponList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_ChangeCouponList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListCoupon = (System.Web.UI.WebControls.DataList)this.FindControl("dataListCoupon");
			this.dataListCoupon.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dataListCoupon_ItemCommand);
		}
		private void dataListCoupon_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
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
