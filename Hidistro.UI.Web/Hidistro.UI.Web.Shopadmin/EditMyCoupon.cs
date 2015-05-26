using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyCoupon : DistributorPage
	{
		private int couponId;
		protected System.Web.UI.WebControls.Label lblEditCouponId;
		protected System.Web.UI.WebControls.TextBox txtCouponName;
		protected System.Web.UI.WebControls.TextBox txtAmount;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtNeedPoint;
		protected System.Web.UI.WebControls.Button btnEditCoupons;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditCoupons.Click += new System.EventHandler(this.btnEditCoupons_Click);
			if (!this.Page.IsPostBack)
			{
				CouponInfo coupon = SubsiteCouponHelper.GetCoupon(this.couponId);
				if (coupon == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (coupon.ClosingTime.CompareTo(System.DateTime.Now) < 0)
				{
					this.ShowMsg("该优惠券已经结束！", false);
					return;
				}
				Globals.EntityCoding(coupon, false);
				this.lblEditCouponId.Text = coupon.CouponId.ToString();
				this.txtCouponName.Text = coupon.Name;
				if (coupon.Amount.HasValue)
				{
					this.txtAmount.Text = string.Format("{0:F2}", coupon.Amount);
				}
				this.txtDiscountValue.Text = coupon.DiscountValue.ToString("F2");
                this.calendarEndDate.SelectedDate = new System.DateTime?(coupon.ClosingTime);
                this.calendarStartDate.SelectedDate = new System.DateTime?(coupon.StartTime);
				this.txtNeedPoint.Text = coupon.NeedPoint.ToString();
			}
		}
		private void btnEditCoupons_Click(object sender, System.EventArgs e)
		{
			decimal? amount;
			decimal discountValue;
			int needPoint;
			if (!this.ValidateValues(out amount, out discountValue, out needPoint))
			{
				return;
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择结束日期！", false);
				return;
			}
			if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
			{
				this.ShowMsg("开始日期不能晚于结束日期！", false);
				return;
			}
			string text = string.Empty;
			CouponInfo couponInfo = new CouponInfo();
			couponInfo.CouponId = this.couponId;
			couponInfo.Name = this.txtCouponName.Text;
			couponInfo.ClosingTime = this.calendarEndDate.SelectedDate.Value;
			couponInfo.StartTime = this.calendarStartDate.SelectedDate.Value;
			couponInfo.Amount = amount;
			couponInfo.DiscountValue = discountValue;
			couponInfo.NeedPoint = needPoint;
			ValidationResults validationResults = Validation.Validate<CouponInfo>(couponInfo, new string[]
			{
				"ValCoupon"
			});
			if (!validationResults.IsValid)
			{
				using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ValidationResult current = enumerator.Current;
						text += Formatter.FormatErrorMessage(current.Message);
						this.ShowMsg(text, false);
						return;
					}
				}
			}
			CouponActionStatus couponActionStatus = SubsiteCouponHelper.UpdateCoupon(couponInfo);
			if (couponActionStatus == CouponActionStatus.Success)
			{
				this.RestCoupon();
				this.ShowMsg("成功修改了优惠券信息", true);
			}
			else
			{
				if (couponActionStatus == CouponActionStatus.DuplicateName)
				{
					this.ShowMsg("修改优惠券信息错误,已经具有此优惠券名称", false);
					return;
				}
				this.ShowMsg("未知错误", false);
				this.RestCoupon();
				return;
			}
		}
		private void RestCoupon()
		{
			this.lblEditCouponId.Text = string.Empty;
			this.txtCouponName.Text = string.Empty;
			this.txtAmount.Text = string.Empty;
			this.txtDiscountValue.Text = string.Empty;
		}
		private bool ValidateValues(out decimal? amount, out decimal discount, out int needPoint)
		{
			string text = string.Empty;
			amount = null;
			if (!string.IsNullOrEmpty(this.txtAmount.Text.Trim()))
			{
				decimal value;
				if (decimal.TryParse(this.txtAmount.Text.Trim(), out value))
				{
					amount = new decimal?(value);
				}
				else
				{
					text += Formatter.FormatErrorMessage("满足金额必须为0-1000万之间");
				}
			}
			if (!decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discount))
			{
				text += Formatter.FormatErrorMessage("可抵扣金额必须在0.01-1000万之间");
			}
			if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
			{
				text += Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
