using ASPNET.WebControls;
using Hidistro.AccountCenter.Profile;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class ReferralMembers : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtSearchText;
		private IButton btnSearchButton;
		private Common_Referral_MemberList grdReferralmembers;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralMembers.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtSearchText = (System.Web.UI.WebControls.TextBox)this.FindControl("txtSearchText");
			this.btnSearchButton = ButtonManager.Create(this.FindControl("btnSearchButton"));
			this.grdReferralmembers = (Common_Referral_MemberList)this.FindControl("Common_Referral_MemberList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("会员中心首页", HiContext.Current.Context);
				MemberQuery memberQuery = new MemberQuery();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
				{
					memberQuery.Username = this.Page.Server.UrlDecode(this.Page.Request.QueryString["username"]);
				}
				memberQuery.PageIndex = this.pager.PageIndex;
				memberQuery.PageSize = this.pager.PageSize;
				DbQueryResult myReferralMembers = PersonalHelper.GetMyReferralMembers(memberQuery);
				this.grdReferralmembers.DataSource = myReferralMembers.Data;
				this.grdReferralmembers.DataBind();
				this.txtSearchText.Text = memberQuery.Username;
                this.pager.TotalRecords = myReferralMembers.TotalRecords;
			}
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadReferralMembers(true);
		}
		private void ReloadReferralMembers(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("username", this.txtSearchText.Text.Trim());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
