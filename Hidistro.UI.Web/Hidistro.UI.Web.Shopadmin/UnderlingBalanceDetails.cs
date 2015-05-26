using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class UnderlingBalanceDetails : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litUser;
		protected System.Web.UI.WebControls.Literal litBalance;
		protected System.Web.UI.WebControls.Literal litUserBalance;
		protected System.Web.UI.WebControls.Literal litDrawBalance;
		protected System.Web.UI.WebControls.LinkButton lbtnDrawRequest;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected TradeTypeDropDownList dropTradeType;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDetails;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDetails;
		protected Pager pager1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanRemark;
		private int userId;
		private int typeId;
		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryBalanceDetails.Click += new System.EventHandler(this.btnQueryBalanceDetails_Click);
			this.lbtnDrawRequest.Click += new System.EventHandler(this.lbtnDrawRequest_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				if (this.userId != 0)
				{
					Hidistro.Membership.Context.Member member = UnderlingHelper.GetMember(this.userId);
					if (member != null)
					{
						this.litBalance.Text = member.Balance.ToString("F2");
						this.litDrawBalance.Text = member.RequestBalance.ToString("F2");
						this.litUserBalance.Text = (member.Balance - member.RequestBalance).ToString("F2");
						MemberGradeInfo memberGrade = UnderlingHelper.GetMemberGrade(member.GradeId);
						if (memberGrade != null)
						{
							this.litUser.Text = member.Username + "(" + memberGrade.Name + ")";
						}
						else
						{
							this.litUser.Text = member.Username;
						}
					}
				}
				this.BindBalanceDetails();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
				{
					int.TryParse(this.Page.Request.QueryString["userId"], out this.userId);
				}
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
				this.ViewState["UserId"] = this.userId;
				this.dropTradeType.DataBind();
				this.dropTradeType.SelectedValue = (TradeTypes)this.typeId;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}
			this.userId = (int)this.ViewState["UserId"];
			this.typeId = (int)this.dropTradeType.SelectedValue;
			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userId", this.ViewState["UserId"].ToString());
			nameValueCollection.Add("typeId", ((int)this.dropTradeType.SelectedValue).ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindBalanceDetails()
		{
			DbQueryResult balanceDetails = UnderlingHelper.GetBalanceDetails(new BalanceDetailQuery
			{
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				TradeType = (TradeTypes)this.typeId,
				PageIndex = this.pager.PageIndex,
				UserId = new int?(this.userId),
				PageSize = this.pager.PageSize
			});
			this.grdBalanceDetails.DataSource = balanceDetails.Data;
			this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = balanceDetails.TotalRecords;
            this.pager1.TotalRecords = balanceDetails.TotalRecords;
		}
		private void lbtnDrawRequest_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("UnderlingBalanceDrawRequest.aspx?userId=" + this.userId);
		}
		private void btnQueryBalanceDetails_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
