using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class Common_UserLogin : AscxTemplatedWebControl
	{
		private Panel pnlLogin;
		private Panel pnlLogout;
		private Literal litAccount;
		private Literal litMemberGrade;
		private Literal litPoint;
		private Literal litNum;
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_UserLogin.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.pnlLogin = (Panel)this.FindControl("pnlLogin");
			this.pnlLogout = (Panel)this.FindControl("pnlLogout");
			this.litAccount = (Literal)this.FindControl("litAccount");
			this.litMemberGrade = (Literal)this.FindControl("litMemberGrade");
			this.litPoint = (Literal)this.FindControl("litPoint");
			this.litNum = (Literal)this.FindControl("litNum");
			this.pnlLogout.Visible = !HiContext.Current.User.IsAnonymous;
			this.pnlLogin.Visible = HiContext.Current.User.IsAnonymous;
			if (!this.Page.IsPostBack && (HiContext.Current.User.UserRole == UserRole.Member || HiContext.Current.User.UserRole == UserRole.Underling))
			{
				Member member = HiContext.Current.User as Member;
				this.litAccount.Text = Globals.FormatMoney(member.Balance);
				this.litPoint.Text = member.Points.ToString();
				string text;
				int num;
				ControlProvider.Instance().GetMemberExpandInfo(member.GradeId, member.Username, out text, out num);
				this.litMemberGrade.Text = text;
				this.litNum.Text = num.ToString();
			}
		}
	}
}
