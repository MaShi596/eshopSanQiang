using ASPNET.WebControls;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyCountDown : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected DistributorGroupBuyProductDropDownList dropGroupBuyProduct;
		protected System.Web.UI.WebControls.Label lblPrice;
		protected WebCalendar calendarStartDate;
		protected HourDropDownList drophours;
		protected WebCalendar calendarEndDate;
		protected HourDropDownList HourDropDownList1;
		protected System.Web.UI.WebControls.TextBox txtPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxCount;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnAddCountDown;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCountDown.Click += new System.EventHandler(this.btnbtnAddCountDown_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropGroupBuyProduct.DataBind();
				this.HourDropDownList1.DataBind();
				this.drophours.DataBind();
			}
		}
		private void btnbtnAddCountDown_Click(object sender, System.EventArgs e)
		{
			CountDownInfo countDownInfo = new CountDownInfo();
			string text = string.Empty;
			if (this.dropGroupBuyProduct.SelectedValue > 0)
			{
				if (SubsitePromoteHelper.ProductCountDownExist(this.dropGroupBuyProduct.SelectedValue.Value))
				{
					this.ShowMsg("已经存在此商品的限时抢购活动", false);
					return;
				}
				countDownInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
			}
			else
			{
				text += Formatter.FormatErrorMessage("请选择限时抢购商品");
			}
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择开始日期");
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
				if (System.DateTime.Compare(countDownInfo.EndDate, System.DateTime.Now) <= 0)
				{
					text += Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
				}
				else
				{
					if (System.DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value), countDownInfo.EndDate) >= 0)
					{
						text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
					}
					else
					{
						countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
					}
				}
			}
			int maxCount;
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				countDownInfo.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}
			if (!string.IsNullOrEmpty(this.txtPrice.Text))
			{
				decimal countDownPrice;
				if (decimal.TryParse(this.txtPrice.Text.Trim(), out countDownPrice))
				{
					countDownInfo.CountDownPrice = countDownPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("价格填写格式不正确");
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return;
			}
			countDownInfo.Content = this.txtContent.Text;
			if (SubsitePromoteHelper.AddCountDown(countDownInfo))
			{
				this.ShowMsg("添加限时抢购活动成功", true);
				return;
			}
			this.ShowMsg("添加限时抢购活动失败", true);
		}
	}
}
