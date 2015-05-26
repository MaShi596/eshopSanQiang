using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class MemberDetails : AdminPage
	{
		private int currentUserId;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal lblUserLink;
		protected System.Web.UI.WebControls.Literal litIsApproved;
		protected System.Web.UI.WebControls.Literal litGrade;
		protected System.Web.UI.WebControls.Literal litRealName;
		protected System.Web.UI.WebControls.Literal litBirthDate;
		protected System.Web.UI.WebControls.Literal litGender;
		protected System.Web.UI.WebControls.Literal litEmail;
		protected System.Web.UI.WebControls.Literal litAddress;
		protected System.Web.UI.WebControls.Literal litWangwang;
		protected System.Web.UI.WebControls.Literal litQQ;
		protected System.Web.UI.WebControls.Literal litMSN;
		protected System.Web.UI.WebControls.Literal litTelPhone;
		protected System.Web.UI.WebControls.Literal litCellPhone;
		protected System.Web.UI.WebControls.Literal litCreateDate;
		protected System.Web.UI.WebControls.Literal litLastLoginDate;
		protected System.Web.UI.WebControls.Button btnEdit;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadMemberInfo();
			}
		}
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/EditMember.aspx?userId=" + this.Page.Request.QueryString["userId"]), true);
		}
		private void LoadMemberInfo()
		{
			Hidistro.Membership.Context.Member member = MemberHelper.GetMember(this.currentUserId);
			if (member == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			System.Uri url = System.Web.HttpContext.Current.Request.Url;
			string text = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(System.Globalization.CultureInfo.InvariantCulture));
			this.lblUserLink.Text = string.Concat(new object[]
			{
				string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
				{
					url.Scheme,
					Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteUrl,
					text
				}),
				Globals.ApplicationPath,
				"/?ReferralUserId=",
				member.UserId
			});
			this.litUserName.Text = member.Username;
			this.litIsApproved.Text = (member.IsApproved ? "通过" : "禁止");
			this.litGrade.Text = MemberHelper.GetMemberGrade(member.GradeId).Name;
			this.litCreateDate.Text = member.CreateDate.ToString();
			this.litLastLoginDate.Text = member.LastLoginDate.ToString();
			this.litRealName.Text = member.RealName;
			this.litBirthDate.Text = member.BirthDate.ToString();
			this.litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
			this.litQQ.Text = member.QQ;
			this.litMSN.Text = member.MSN;
			this.litTelPhone.Text = member.TelPhone;
			this.litCellPhone.Text = member.CellPhone;
			this.litEmail.Text = member.Email;
			if (member.Gender == Hidistro.Membership.Core.Enums.Gender.Female)
			{
				this.litGender.Text = "女";
				return;
			}
			if (member.Gender == Hidistro.Membership.Core.Enums.Gender.Male)
			{
				this.litGender.Text = "男";
				return;
			}
			this.litGender.Text = "保密";
		}
	}
}
