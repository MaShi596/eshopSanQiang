using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberAccount)]
	public class AccountSummaryList : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdMemberAccountList;
		protected Pager pager1;
		protected System.Web.UI.WebControls.TextBox txtReCharge;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden currentUserId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden curentBalance;
		protected System.Web.UI.WebControls.Button btnAddBalance;
		private string searchKey;
		private string realName;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			this.btnAddBalance.Click += new System.EventHandler(this.btnAddBalance_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.GetBalance();
			}
		}
		private void btnAddBalance_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<int> userPrivileges = Hidistro.Membership.Core.RoleHelper.GetUserPrivileges(Hidistro.Membership.Context.HiContext.Current.User.Username);
			if (!userPrivileges.Contains(9002) && Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
			{
				this.ShowMsg("权限不够", false);
				return;
			}
			int num = 0;
			if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
			}
			int userId = int.Parse(this.currentUserId.Value);
			decimal num2;
			if (!decimal.TryParse(this.txtReCharge.Text.Trim(), out num2) || num > 2)
			{
				this.ShowMsg("本次充值要给当前客户加款的金额只能是数值，且不能超过2位小数", false);
				return;
			}
			if (num2 < -10000000m || num2 > 10000000m)
			{
				this.ShowMsg("金额大小必须在正负1000万之间", false);
				return;
			}
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(userId, false) as Hidistro.Membership.Context.Member;
			if (member == null || !member.IsOpenBalance)
			{
				this.ShowMsg("本次充值已失败，该用户不存在或预付款还没有开通", false);
				return;
			}
			decimal balance = num2 + member.Balance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = userId;
			balanceDetailInfo.UserName = member.Username;
			balanceDetailInfo.TradeDate = System.DateTime.Now;
			balanceDetailInfo.TradeType = TradeTypes.BackgroundAddmoney;
			balanceDetailInfo.Income = new decimal?(num2);
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.Remark = Globals.HtmlEncode(this.txtRemark.Text.Trim());
			ValidationResults validationResults = Validation.Validate<BalanceDetailInfo>(balanceDetailInfo, new string[]
			{
				"ValBalanceDetail"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (MemberHelper.AddBalance(balanceDetailInfo, num2))
			{
				this.txtReCharge.Text = "";
				this.ReBind(false);
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
				{
					this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
				}
				this.txtUserName.Text = this.searchKey;
				this.txtRealName.Text = this.realName;
				return;
			}
			this.searchKey = this.txtUserName.Text.Trim();
			this.realName = this.txtRealName.Text.Trim();
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", base.Server.UrlEncode(this.txtUserName.Text));
			nameValueCollection.Add("realName", base.Server.UrlEncode(this.txtRealName.Text));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			base.ReloadPage(nameValueCollection);
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		public void GetBalance()
		{
            MemberQuery query = new MemberQuery
            {
                Username = this.searchKey,
                Realname = this.realName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            };
            DbQueryResult memberBlanceList = MemberHelper.GetMemberBlanceList(query);
            this.grdMemberAccountList.DataSource = memberBlanceList.Data;
            this.grdMemberAccountList.DataBind();
            this.pager.TotalRecords = this.pager.TotalRecords = memberBlanceList.TotalRecords;
            this.pager1.TotalRecords = this.pager.TotalRecords = memberBlanceList.TotalRecords;
		}
	}
}
