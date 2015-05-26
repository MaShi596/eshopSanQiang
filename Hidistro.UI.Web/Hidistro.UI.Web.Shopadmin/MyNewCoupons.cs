using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyNewCoupons : DistributorPage
	{
		private int pageIndex = 1;
		private int pageSize = 10;
		protected Grid grdCoupons;
		protected System.Web.UI.WebControls.TextBox tbcouponNum;
		protected System.Web.UI.WebControls.HiddenField txtcouponid;
		protected System.Web.UI.WebControls.Button btnExport;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdCoupons.RowDeleting += new GridViewDeleteEventHandler(this.grdCoupons_RowDeleting);
            this.grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(this.grdCoupons_ReBindData);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindCoupons();
            }
		}
		private void grdCoupons_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int couponId = (int)this.grdCoupons.DataKeys[e.RowIndex].Value;
			if (SubsiteCouponHelper.DeleteCoupon(couponId))
			{
				this.BindCoupons();
				this.ShowMsg("成功删除了选定张优惠券", true);
				return;
			}
			this.ShowMsg("删除优惠券失败", false);
		}
		private void grdCoupons_ReBindData(object sender)
		{
			this.BindCoupons();
		}
		protected bool IsCouponEnd(object endtime)
		{
			return System.Convert.ToDateTime(endtime).CompareTo(System.DateTime.Now) > 0;
		}
		protected void btnExport_Click(object sender, System.EventArgs e)
		{
			int num;
			if (!int.TryParse(this.tbcouponNum.Text, out num))
			{
				this.ShowMsg("导出数量必须为正数", false);
				return;
			}
			if (num <= 0)
			{
				this.ShowMsg("导出数量必须为正数", false);
				return;
			}
			int couponId;
			if (!int.TryParse(this.txtcouponid.Value, out couponId))
			{
				this.ShowMsg("参数错误", false);
				return;
			}
			CouponInfo coupon = SubsiteCouponHelper.GetCoupon(couponId);
			string empty = string.Empty;
			CouponActionStatus couponActionStatus = SubsiteCouponHelper.CreateCoupon(coupon, num, out empty);
			if (couponActionStatus == CouponActionStatus.UnknowError)
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			if (couponActionStatus == CouponActionStatus.CreateClaimCodeError)
			{
				this.ShowMsg("生成优惠券号码错误", false);
				return;
			}
			if (couponActionStatus == CouponActionStatus.CreateClaimCodeSuccess && !string.IsNullOrEmpty(empty))
			{
				System.Collections.Generic.IList<CouponItemInfo> couponItemInfos = SubsiteCouponHelper.GetCouponItemInfos(empty);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>优惠券批次号</td>");
				stringBuilder.AppendLine("<td>优惠券号码</td>");
				stringBuilder.AppendLine("<td>优惠券金额</td>");
				stringBuilder.AppendLine("<td>过期时间</td>");
				stringBuilder.AppendLine("</tr>");
				foreach (CouponItemInfo current in couponItemInfos)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + empty + "</td>");
					stringBuilder.AppendLine("<td>" + current.ClaimCode + "</td>");
					stringBuilder.AppendLine("<td>" + coupon.DiscountValue + "</td>");
					stringBuilder.AppendLine("<td>" + coupon.ClosingTime + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				stringBuilder.AppendLine("</table>");
				this.Page.Response.Clear();
				this.Page.Response.Buffer = false;
				this.Page.Response.Charset = "GB2312";
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=CouponsInfo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
				this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
				this.Page.Response.ContentType = "application/ms-excel";
				this.Page.EnableViewState = false;
				this.Page.Response.Write(stringBuilder.ToString());
				this.Page.Response.End();
			}
		}
		private void BindCoupons()
		{
			DbQueryResult newCoupons = SubsiteCouponHelper.GetNewCoupons(new Pagination
			{
				PageSize = this.pageSize,
				PageIndex = this.pageIndex
			});
			this.grdCoupons.DataSource = newCoupons.Data;
			this.grdCoupons.DataBind();
            this.pager.TotalRecords = newCoupons.TotalRecords;
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!int.TryParse(this.Page.Request.QueryString["PageIndex"], out this.pageIndex))
				{
					this.pageIndex = 1;
				}
				if (!int.TryParse(this.Page.Request.QueryString["pageSize"], out this.pageSize))
				{
					this.pageSize = 10;
					return;
				}
			}
			else
			{
				this.pageIndex = this.pager.PageIndex;
				this.pageSize = this.pager.PageSize;
			}
		}
	}
}
