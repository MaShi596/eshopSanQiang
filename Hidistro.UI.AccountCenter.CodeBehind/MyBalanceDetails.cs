using ASPNET.WebControls;
using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class MyBalanceDetails : MemberTemplatedWebControl
	{
		private Common_Advance_AccountList accountList;
		private Pager pager;
		private WebCalendar calendarStart;
		private WebCalendar calendarEnd;
		private TradeTypeDropDownList dropTradeType;
		private IButton btnSearchBalanceDetails;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyBalanceDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.accountList = (Common_Advance_AccountList)this.FindControl("Common_Advance_AccountList");
			this.pager = (Pager)this.FindControl("pager");
			this.calendarStart = (WebCalendar)this.FindControl("calendarStart");
			this.calendarEnd = (WebCalendar)this.FindControl("calendarEnd");
			this.dropTradeType = (TradeTypeDropDownList)this.FindControl("dropTradeType");
			this.btnSearchBalanceDetails = ButtonManager.Create(this.FindControl("btnSearchBalanceDetails"));
			PageTitle.AddSiteNameTitle("帐户明细", HiContext.Current.Context);
			this.btnSearchBalanceDetails.Click += new System.EventHandler(this.btnSearchBalanceDetails_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.BindBalanceDetails();
			}
		}
		private void btnSearchBalanceDetails_Click(object sender, System.EventArgs e)
		{
			this.ReloadMyBalanceDetails(true);
		}
		private void BindBalanceDetails()
		{
			BalanceDetailQuery balanceDetailQuery = this.GetBalanceDetailQuery();
			DbQueryResult balanceDetails = PersonalHelper.GetBalanceDetails(balanceDetailQuery);
			this.accountList.DataSource = balanceDetails.Data;
			this.accountList.DataBind();
			this.dropTradeType.DataBind();
			this.dropTradeType.SelectedValue = balanceDetailQuery.TradeType;
            this.calendarStart.SelectedDate = balanceDetailQuery.FromDate;
            this.calendarEnd.SelectedDate = balanceDetailQuery.ToDate;
            this.pager.TotalRecords = balanceDetails.TotalRecords;
		}
		private BalanceDetailQuery GetBalanceDetailQuery()
		{
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			balanceDetailQuery.UserId = new int?(HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
			{
				balanceDetailQuery.FromDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
			{
				balanceDetailQuery.ToDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
			{
				balanceDetailQuery.TradeType = (TradeTypes)System.Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["tradeType"]));
			}
			balanceDetailQuery.PageIndex = this.pager.PageIndex;
			balanceDetailQuery.PageSize = this.pager.PageSize;
			return balanceDetailQuery;
		}
		private void ReloadMyBalanceDetails(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			nameValueCollection.Add("tradeType", ((int)this.dropTradeType.SelectedValue).ToString());
			base.ReloadPage(nameValueCollection);
		}
	}
}
