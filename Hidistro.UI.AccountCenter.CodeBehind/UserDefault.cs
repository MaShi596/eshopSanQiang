using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserDefault : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.Literal litUserPoint;
		private System.Web.UI.WebControls.Literal litUserRank;
		private System.Web.UI.WebControls.Literal litUserLink;
		private System.Web.UI.WebControls.Literal litNoPayOrderNum;
		private System.Web.UI.WebControls.Literal litNoReplyLeaveWordNum;
		private FormatedMoneyLabel litAccountAmount;
		private FormatedMoneyLabel litUseableBalance;
		private FormatedMoneyLabel litRequestBalance;
		private System.Web.UI.WebControls.HyperLink hpOrder;
		private System.Web.UI.WebControls.HyperLink hpMes;
		private System.Web.UI.WebControls.HyperLink hpRepay;
		private System.Web.UI.HtmlControls.HtmlGenericControl divBalance;
		private System.Web.UI.HtmlControls.HtmlGenericControl divOpenBalance;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserDefault.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.litUserPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litUserPoint");
			this.litUserRank = (System.Web.UI.WebControls.Literal)this.FindControl("litUserRank");
			this.litUserLink = (System.Web.UI.WebControls.Literal)this.FindControl("litUserLink");
			this.litNoPayOrderNum = (System.Web.UI.WebControls.Literal)this.FindControl("litNoPayOrderNum");
			this.litNoReplyLeaveWordNum = (System.Web.UI.WebControls.Literal)this.FindControl("litNoReplyLeaveWordNum");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.hpOrder = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpOrder");
			this.hpMes = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpMes");
			this.hpRepay = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpRepaly");
			this.divBalance = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("divBalance");
			this.divOpenBalance = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("divOpenBalance");
			PageTitle.AddSiteNameTitle("会员中心首页", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.divBalance.Visible = false;
					this.divOpenBalance.Visible = true;
				}
				this.litUserPoint.Text = member.Points.ToString();
				this.litUserName.Text = member.Username;
				MemberGradeInfo memberGrade = PersonalHelper.GetMemberGrade(member.GradeId);
				if (memberGrade != null)
				{
					this.litUserRank.Text = memberGrade.Name;
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				PersonalHelper.GetStatisticsNum(out num, out num2, out num3);
				this.litNoPayOrderNum.Text = num.ToString();
				this.litNoReplyLeaveWordNum.Text = num3.ToString();
				this.hpMes.Text = num2.ToString();
				this.litAccountAmount.Money = member.Balance;
				this.litRequestBalance.Money = member.RequestBalance;
				this.litUseableBalance.Money = member.Balance - member.RequestBalance;
				if (num > 0)
				{
					this.hpOrder.Visible = true;
					this.hpOrder.NavigateUrl = "UserOrders.aspx?orderStatus=" + 1;
				}
				this.hpMes.NavigateUrl = "UserReceivedMessages.aspx";
				if (num3 > 0)
				{
					this.hpRepay.Visible = true;
					this.hpRepay.NavigateUrl = "UserConsultations.aspx";
				}
				Uri url = System.Web.HttpContext.Current.Request.Url;
				string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
				this.litUserLink.Text = string.Concat(new object[]
				{
					string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
					{
						url.Scheme,
						HiContext.Current.SiteSettings.SiteUrl,
						text
					}),
					Globals.ApplicationPath,
					"/?ReferralUserId=",
					HiContext.Current.User.UserId
				});
			}
		}
	}
}
