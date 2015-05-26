using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class MyAccountSummary : MemberTemplatedWebControl
	{
		private FormatedMoneyLabel litAccountAmount;
		private FormatedMoneyLabel litRequestBalance;
		private FormatedMoneyLabel litUseableBalance;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyAccountSummary.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			PageTitle.AddSiteNameTitle("预付款账户", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.litAccountAmount.Money = member.Balance;
				this.litRequestBalance.Money = member.RequestBalance;
				this.litUseableBalance.Money = member.Balance - member.RequestBalance;
			}
		}
	}
}
