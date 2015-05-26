using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class UnderlingBalanceDrawRequest : DistributorPage
	{
		private int userId;
		private string userName;
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDrawRequest;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDrawRequest;
		protected Pager pager1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanBankName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanaccountName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanmerchantCode;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanRemark;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnQueryBalanceDrawRequest.Click += new System.EventHandler(this.btnQueryBalanceDrawRequest_Click);
			this.grdBalanceDrawRequest.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdBalanceDrawRequest_RowCommand);
			this.GetBalanceDrawRequestQuery();
			if (!this.Page.IsPostBack)
			{
				if (int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
				{
					Hidistro.Membership.Context.Member member = UnderlingHelper.GetMember(this.userId);
					if (member != null)
					{
						this.txtUserName.Text = member.Username;
					}
				}
				this.BindBalanceDrawRequest();
			}
		}
		private void grdBalanceDrawRequest_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
			int num = (int)this.grdBalanceDrawRequest.DataKeys[gridViewRow.RowIndex].Value;
			if (e.CommandName == "UnLineReCharge")
			{
				if (UnderlingHelper.DealBalanceDrawRequest(num, true))
				{
					this.BindBalanceDrawRequest();
				}
				else
				{
					this.ShowMsg("预付款提现申请操作失败", false);
				}
			}
			if (e.CommandName == "RefuseRequest")
			{
				if (UnderlingHelper.DealBalanceDrawRequest(num, false))
				{
					this.BindBalanceDrawRequest();
					return;
				}
				this.ShowMsg("预付款提现申请操作失败", false);
			}
		}
		private void btnQueryBalanceDrawRequest_Click(object sender, System.EventArgs e)
		{
			this.ReloadUnderlingBalanceDrawRequest(true);
		}
		public void BindBalanceDrawRequest()
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			if (this.userId > 0)
			{
				balanceDrawRequestQuery.UserId = new int?(this.userId);
			}
			balanceDrawRequestQuery.UserName = this.userName;
			balanceDrawRequestQuery.FromDate = this.dateStart;
			balanceDrawRequestQuery.ToDate = this.dateEnd;
			balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			DbQueryResult balanceDrawRequests = UnderlingHelper.GetBalanceDrawRequests(balanceDrawRequestQuery);
			this.grdBalanceDrawRequest.DataSource = balanceDrawRequests.Data;
			this.grdBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequests.TotalRecords;
            this.pager1.TotalRecords = balanceDrawRequests.TotalRecords;
		}
		private void GetBalanceDrawRequestQuery()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
				{
					int.TryParse(this.Page.Request.QueryString["userId"], out this.userId);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}
				this.txtUserName.Text = this.userName;
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
				return;
			}
			this.userName = this.txtUserName.Text.Trim();
		}
		private void ReloadUnderlingBalanceDrawRequest(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtUserName.Text);
			nameValueCollection.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
