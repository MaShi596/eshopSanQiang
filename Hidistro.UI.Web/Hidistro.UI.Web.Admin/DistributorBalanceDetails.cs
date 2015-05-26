using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorAccount)]
	public class DistributorBalanceDetails : AdminPage
	{
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		private int userId;
		private TradeTypes tradeType;
		protected System.Web.UI.WebControls.Literal litUser;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected TradeTypeDropDownList ddlTradeType;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDetails;
		protected Pager pager1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryBalanceDetails.Click += new System.EventHandler(this.btnQueryBalanceDetails_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
				if (distributor == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litUser.Text = distributor.Username;
				this.GetBalanceDetails();
			}
		}
		private void ddlTradeType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}
		private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userId", this.Page.Request.QueryString["userId"]);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			nameValueCollection.Add("tradeType", ((int)this.ddlTradeType.SelectedValue).ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
				{
					int num = 0;
					int.TryParse(this.Page.Request.QueryString["tradeType"], out num);
					this.tradeType = (TradeTypes)num;
				}
				this.ddlTradeType.DataBind();
				this.ddlTradeType.SelectedValue = this.tradeType;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.tradeType = this.ddlTradeType.SelectedValue;
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void GetBalanceDetails()
		{
			DbQueryResult distributorBalanceDetails = DistributorHelper.GetDistributorBalanceDetails(new BalanceDetailQuery
			{
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				UserId = new int?(this.userId),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				TradeType = this.tradeType
			});
			this.grdBalanceDetails.DataSource = distributorBalanceDetails.Data;
			this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = distributorBalanceDetails.TotalRecords;
            this.pager1.TotalRecords = distributorBalanceDetails.TotalRecords;
		}
	}
}
