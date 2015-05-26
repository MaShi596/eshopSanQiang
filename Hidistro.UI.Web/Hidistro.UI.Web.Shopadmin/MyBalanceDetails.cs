using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyBalanceDetails : DistributorPage
	{
		private int typeId;
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected TradeTypeDropDownList dropTradeType;
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
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.GetBalanceDetails();
			}
		}
		private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"typeId",
					((int)this.dropTradeType.SelectedValue).ToString(System.Globalization.CultureInfo.InvariantCulture)
				},

				{
					"dataStart",
					this.calendarStart.SelectedDate.ToString()
				},

				{
					"dataEnd",
					this.calendarEnd.SelectedDate.ToString()
				},

				{
					"pageSize",
					this.pager.PageSize.ToString()
				}
			});
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
				{
					int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				this.dropTradeType.DataBind();
				this.dropTradeType.SelectedValue = (TradeTypes)this.typeId;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.typeId = (int)this.dropTradeType.SelectedValue;
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void GetBalanceDetails()
		{
			DbQueryResult myBalanceDetails = SubsiteStoreHelper.GetMyBalanceDetails(new BalanceDetailQuery
			{
				TradeType = (TradeTypes)this.typeId,
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdBalanceDetails.DataSource = myBalanceDetails.Data;
			this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = myBalanceDetails.TotalRecords;
            this.pager1.TotalRecords = myBalanceDetails.TotalRecords;
		}
	}
}
