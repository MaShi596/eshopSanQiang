using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.UserIncreaseStatistics)]
	public class UserIncreaseStatistics : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlImage imgChartOfSevenDay;
		protected System.Web.UI.WebControls.Literal litlOfMonth;
		protected YearDropDownList drpYearOfMonth;
		protected MonthDropDownList drpMonthOfMonth;
		protected System.Web.UI.WebControls.Button btnOfMonth;
		protected System.Web.UI.HtmlControls.HtmlImage imgChartOfMonth;
		protected System.Web.UI.WebControls.Literal litlOfYear;
		protected YearDropDownList drpYearOfYear;
		protected System.Web.UI.WebControls.Button btnOfYear;
		protected System.Web.UI.HtmlControls.HtmlImage imgChartOfYear;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOfMonth.Click += new System.EventHandler(this.btnOfMonth_Click);
			this.btnOfYear.Click += new System.EventHandler(this.btnOfYear_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindDaysAddUser();
				this.BindMonthAddUser();
				this.BindYearAddUser();
			}
		}
		private void btnOfYear_Click(object sender, System.EventArgs e)
		{
			this.BindYearAddUser();
		}
		private void btnOfMonth_Click(object sender, System.EventArgs e)
		{
			this.BindMonthAddUser();
		}
		private void BindDaysAddUser()
		{
			System.Collections.Generic.IList<UserStatisticsForDate> userAdd = SalesHelper.GetUserAdd(null, null, new int?(7));
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (UserStatisticsForDate current in userAdd)
			{
				if (string.IsNullOrEmpty(text))
				{
					if (System.DateTime.Now.Date.Day < 7 && current.TimePoint > 7)
					{
						text += ((System.DateTime.Now.Month > 9) ? (System.DateTime.Now.Month - 1).ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + (System.DateTime.Now.Month - 1).ToString(System.Globalization.CultureInfo.InvariantCulture) + "-" + ((current.TimePoint > 9) ? current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
					}
					else
					{
						text += ((System.DateTime.Now.Month > 9) ? System.DateTime.Now.Month.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + System.DateTime.Now.Month.ToString(System.Globalization.CultureInfo.InvariantCulture) + "-" + ((current.TimePoint > 9) ? current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture)))));
					}
				}
				else
				{
					if (System.DateTime.Now.Date.Day < 7 && current.TimePoint > 7)
					{
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							"|",
							(System.DateTime.Now.Month > 10) ? (System.DateTime.Now.Month - 1).ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + (System.DateTime.Now.Month - 1).ToString(System.Globalization.CultureInfo.InvariantCulture)),
							"-",
							(current.TimePoint > 9) ? current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture))
						});
					}
					else
					{
						string text4 = text;
						text = string.Concat(new string[]
						{
							text4,
							"|",
							(System.DateTime.Now.Month > 10) ? System.DateTime.Now.Month.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + System.DateTime.Now.Month.ToString(System.Globalization.CultureInfo.InvariantCulture)),
							"-",
							(current.TimePoint > 9) ? current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture) : ("0" + current.TimePoint.ToString(System.Globalization.CultureInfo.InvariantCulture))
						});
					}
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 += current.UserCounts;
				}
				else
				{
					text2 = text2 + "|" + current.UserCounts;
				}
			}
			this.imgChartOfSevenDay.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", text, text2);
		}
		private void BindMonthAddUser()
		{
			System.Collections.Generic.IList<UserStatisticsForDate> userAdd = SalesHelper.GetUserAdd(new int?(this.drpYearOfMonth.SelectedValue), new int?(this.drpMonthOfMonth.SelectedValue), null);
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (UserStatisticsForDate current in userAdd)
			{
				if (string.IsNullOrEmpty(text))
				{
					text += current.TimePoint;
				}
				else
				{
					text = text + "|" + current.TimePoint;
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 += current.UserCounts;
				}
				else
				{
					text2 = text2 + "|" + current.UserCounts;
				}
			}
			this.imgChartOfMonth.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", text, text2);
			this.litlOfMonth.Text = this.drpYearOfMonth.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + "年" + this.drpMonthOfMonth.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + "月";
		}
		private void BindYearAddUser()
		{
			System.Collections.Generic.IList<UserStatisticsForDate> userAdd = SalesHelper.GetUserAdd(new int?(this.drpYearOfYear.SelectedValue), null, null);
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (UserStatisticsForDate current in userAdd)
			{
				if (string.IsNullOrEmpty(text))
				{
					text += current.TimePoint;
				}
				else
				{
					text = text + "|" + current.TimePoint;
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 += current.UserCounts;
				}
				else
				{
					text2 = text2 + "|" + current.UserCounts;
				}
			}
			this.imgChartOfYear.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", text, text2);
			this.litlOfYear.Text = this.drpYearOfYear.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + "年";
		}
	}
}
