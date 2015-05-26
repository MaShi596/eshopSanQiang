using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorBalanceDrawRequest)]
	public class DistributorBalanceDrawRequest : AdminPage
	{
		private string searchKey;
		private int? userId;
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDrawRequest;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected Grid grdBalanceDrawRequest;
		protected Pager pager;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanBankName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanaccountName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanmerchantCode;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryBalanceDrawRequest.Click += new System.EventHandler(this.btnQueryBalanceDrawRequest_Click);
			this.grdBalanceDrawRequest.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdBalanceDrawRequest_RowCommand);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.GetBalanceDrawRequest();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				int value;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]) && int.TryParse(this.Page.Request.QueryString["userId"], out value))
				{
					this.userId = new int?(value);
					Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId.Value);
					if (distributor != null)
					{
						this.txtUserName.Text = distributor.Username;
					}
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
				{
					this.txtUserName.Text = this.searchKey;
				}
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.searchKey = this.txtUserName.Text;
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtUserName.Text);
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void grdBalanceDrawRequest_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
			int num = (int)this.grdBalanceDrawRequest.DataKeys[gridViewRow.RowIndex].Value;
			if (e.CommandName == "UnLineReCharge")
			{
				if (DistributorHelper.DealDistributorBalanceDrawRequest(num, true))
				{
					this.GetBalanceDrawRequest();
				}
				else
				{
					this.ShowMsg("预付款提现申请操作失败", false);
				}
			}
			if (e.CommandName == "RefuseRequest")
			{
				if (DistributorHelper.DealDistributorBalanceDrawRequest(num, false))
				{
					this.GetBalanceDrawRequest();
					return;
				}
				this.ShowMsg("预付款提现申请操作失败", false);
			}
		}
		private void btnQueryBalanceDrawRequest_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		public void GetBalanceDrawRequest()
		{
			DbQueryResult distributorBalanceDrawRequests = DistributorHelper.GetDistributorBalanceDrawRequests(new BalanceDrawRequestQuery
			{
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				UserName = this.txtUserName.Text,
				UserId = this.userId,
				PageIndex = this.pager.PageIndex
			});
			this.grdBalanceDrawRequest.DataSource = distributorBalanceDrawRequests.Data;
			this.grdBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = distributorBalanceDrawRequests.TotalRecords;
		}
	}
}
