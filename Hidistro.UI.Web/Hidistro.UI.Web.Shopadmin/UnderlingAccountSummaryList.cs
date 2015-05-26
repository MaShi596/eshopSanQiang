using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class UnderlingAccountSummaryList : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdUnderlingAccountList;
		protected Pager pager1;
		protected System.Web.UI.WebControls.TextBox txtReCharge;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnAddBalance;
		protected System.Web.UI.HtmlControls.HtmlInputHidden currentUserId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden curentBalance;
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
			int num = 0;
			if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
			}
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
			int userId = int.Parse(this.currentUserId.Value);
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(userId, false) as Hidistro.Membership.Context.Member;
			if (member == null || !member.IsOpenBalance)
			{
				this.ShowMsg("本次充值已失败，该用户的预付款还没有开通", false);
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
			if (UnderlingHelper.AddUnderlingBalanceDetail(balanceDetailInfo))
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
				this.txtRealName.Text = this.realName;
				this.txtUserName.Text = this.searchKey;
				return;
			}
			this.searchKey = this.txtUserName.Text;
			this.realName = this.txtRealName.Text;
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtUserName.Text);
			nameValueCollection.Add("realName", this.txtRealName.Text);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		public void GetBalance()
		{
			DbQueryResult underlingBlanceList = UnderlingHelper.GetUnderlingBlanceList(new MemberQuery
			{
				Username = this.searchKey,
				Realname = this.realName,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.grdUnderlingAccountList.DataSource = underlingBlanceList.Data;
			this.grdUnderlingAccountList.DataBind();
            this.pager.TotalRecords = underlingBlanceList.TotalRecords;
            this.pager1.TotalRecords = underlingBlanceList.TotalRecords;
		}
	}
}
