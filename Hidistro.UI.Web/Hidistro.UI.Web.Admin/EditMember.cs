using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMember : AdminPage
	{
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected ApprovedDropDownList ddlApproved;
		protected MemberGradeDropDownList drpMemberRankList;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected WebCalendar calBirthday;
		protected GenderRadioButtonList gender;
		protected System.Web.UI.WebControls.TextBox txtprivateEmail;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtWangwang;
		protected System.Web.UI.WebControls.TextBox txtQQ;
		protected System.Web.UI.WebControls.TextBox txtMSN;
		protected System.Web.UI.WebControls.TextBox txtTel;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected FormatedTimeLabel lblRegsTimeValue;
		protected FormatedTimeLabel lblLastLoginTimeValue;
		protected System.Web.UI.WebControls.Literal lblTotalAmountValue;
		protected System.Web.UI.WebControls.Button btnEditUser;
		private int currentUserId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
			if (!this.Page.IsPostBack)
			{
				this.drpMemberRankList.AllowNull = false;
				this.drpMemberRankList.DataBind();
				this.ddlApproved.AllowNull = false;
				this.ddlApproved.DataBind();
				this.LoadMemberInfo();
			}
		}
		private void LoadMemberInfo()
		{
			Hidistro.Membership.Context.Member member = MemberHelper.GetMember(this.currentUserId);
			if (member == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.ddlApproved.SelectedValue = new bool?(member.IsApproved);
			this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
			this.lblLoginNameValue.Text = member.Username;
			this.lblRegsTimeValue.Time = member.CreateDate;
			this.lblLastLoginTimeValue.Time = member.LastLoginDate;
			this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
			this.txtRealName.Text = member.RealName;
            this.calBirthday.SelectedDate = member.BirthDate;
			this.txtAddress.Text = Globals.HtmlDecode(member.Address);
			this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
			this.txtQQ.Text = member.QQ;
			this.txtMSN.Text = member.MSN;
			this.txtTel.Text = member.TelPhone;
			this.txtCellPhone.Text = member.CellPhone;
			this.txtprivateEmail.Text = member.Email;
			this.gender.SelectedValue = member.Gender;
			this.txtWangwang.Text = Globals.HtmlDecode(member.Wangwang);
		}
		protected void btnEditUser_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.Member member = MemberHelper.GetMember(this.currentUserId);
			member.IsApproved = this.ddlApproved.SelectedValue.Value;
			member.Wangwang = Globals.HtmlEncode(this.txtWangwang.Text.Trim());
			member.GradeId = this.drpMemberRankList.SelectedValue.Value;
			member.RealName = this.txtRealName.Text.Trim();
			if (this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
				member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
			}
			member.Address = Globals.HtmlEncode(this.txtAddress.Text);
			member.QQ = this.txtQQ.Text;
			member.MSN = this.txtMSN.Text;
			member.TelPhone = this.txtTel.Text;
			member.CellPhone = this.txtCellPhone.Text;
			if (this.calBirthday.SelectedDate.HasValue)
			{
				member.BirthDate = new System.DateTime?(this.calBirthday.SelectedDate.Value);
			}
			member.Email = this.txtprivateEmail.Text;
			member.Gender = this.gender.SelectedValue;
			if (!this.ValidationMember(member))
			{
				return;
			}
			if (MemberHelper.Update(member))
			{
				this.ShowMsg("成功修改了当前会员的个人资料", true);
				return;
			}
			this.ShowMsg("当前会员的个人信息修改失败", false);
		}
		private bool ValidationMember(Hidistro.Membership.Context.Member member)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.Member>(member, new string[]
			{
				"ValMember"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
