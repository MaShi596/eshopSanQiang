using ASPNET.WebControls;
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
	public class AddMyCoupon : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtCouponName;
		protected System.Web.UI.WebControls.TextBox txtAmount;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtNeedPoint;
		protected System.Web.UI.WebControls.Button btnAddCoupons;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCoupons.Click += new System.EventHandler(this.btnAddCoupons_Click);
		}
		private void btnAddCoupons_Click(object sender, System.EventArgs e)
		{
			string text = string.Empty;
			decimal? amount;
			decimal discountValue;
			int needPoint;
			if (!this.ValidateValues(out amount, out discountValue, out needPoint))
			{
				return;
			}
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择开始日期！", false);
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
			CouponInfo couponInfo = new CouponInfo();
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
			string empty = string.Empty;
			CouponActionStatus couponActionStatus = SubsiteCouponHelper.CreateCoupon(couponInfo, 0, out empty);
			if (couponActionStatus == CouponActionStatus.UnknowError)
			{
				this.ShowMsg("未知错误", false);
			}
			else
			{
				if (couponActionStatus == CouponActionStatus.DuplicateName)
				{
					this.ShowMsg("已经存在相同的优惠券名称", false);
					return;
				}
				if (couponActionStatus == CouponActionStatus.CreateClaimCodeError)
				{
					this.ShowMsg("生成优惠券号码错误", false);
					return;
				}
				this.ShowMsg("添加优惠券成功", true);
				this.RestCoupon();
				return;
			}
		}
		private void RestCoupon()
		{
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
