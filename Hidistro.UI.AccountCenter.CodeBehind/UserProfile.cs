using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserProfile : MemberTemplatedWebControl
	{
		private SmallStatusMessage Statuses;
		private System.Web.UI.WebControls.TextBox txtRealName;
		private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtQQ;
		private System.Web.UI.WebControls.TextBox txtMSN;
		private System.Web.UI.WebControls.TextBox txtTel;
		private System.Web.UI.WebControls.TextBox txtHandSet;
		private RegionSelector dropRegionsSelect;
		private GenderRadioButtonList gender;
		private WebCalendar calendDate;
		private IButton btnOK1;
		protected virtual void ShowMsgs(SmallStatusMessage state, string string_0, bool success)
		{
			if (state != null)
			{
				state.Success = success;
				state.Text = string_0;
				state.Visible = true;
			}
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProfile.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
			this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.gender = (GenderRadioButtonList)this.FindControl("gender");
			this.calendDate = (WebCalendar)this.FindControl("calendDate");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtQQ = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQQ");
			this.txtMSN = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMSN");
			this.txtTel = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTel");
			this.txtHandSet = (System.Web.UI.WebControls.TextBox)this.FindControl("txtHandSet");
			this.btnOK1 = ButtonManager.Create(this.FindControl("btnOK1"));
			this.Statuses = (SmallStatusMessage)this.FindControl("Statuses");
			this.btnOK1.Click += new System.EventHandler(this.btnOK1_Click);
			PageTitle.AddSiteNameTitle("个人信息", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				Member member = HiContext.Current.User as Member;
				if (member != null)
				{
					this.BindData(member);
				}
			}
		}
		private void btnOK1_Click(object sender, System.EventArgs e)
		{
			Member member = Users.GetUser(HiContext.Current.User.UserId, true) as Member;
			if (string.IsNullOrEmpty(this.txtEmail.Text))
			{
				this.ShowMessage("邮箱不能为空", false);
			}
			else
			{
				member.RealName = Globals.HtmlEncode(this.txtRealName.Text);
				member.Email = Globals.HtmlEncode(this.txtEmail.Text);
				if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue)
				{
					member.RegionId = 0;
				}
				else
				{
					member.RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value;
					member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
				}
				member.Gender = this.gender.SelectedValue;
				member.BirthDate = this.calendDate.SelectedDate;
				member.Address = Globals.HtmlEncode(this.txtAddress.Text);
				member.QQ = Globals.HtmlEncode(this.txtQQ.Text);
				member.MSN = Globals.HtmlEncode(this.txtMSN.Text);
				member.TelPhone = Globals.HtmlEncode(this.txtTel.Text);
				member.CellPhone = Globals.HtmlEncode(this.txtHandSet.Text);
				if (this.ValidationMember(member))
				{
					if (Users.UpdateUser(member))
					{
						this.ShowMessage("成功的修改了用户的个人资料", true);
					}
					else
					{
						this.ShowMessage("修改用户个人资料失败", false);
					}
				}
			}
		}
		private void BindData(Member user)
		{
			this.txtRealName.Text = Globals.HtmlDecode(user.RealName);
			this.txtEmail.Text = Globals.HtmlDecode(user.Email);
			this.gender.SelectedValue = user.Gender;
			if (user.BirthDate > System.DateTime.MinValue)
			{
                this.calendDate.SelectedDate = user.BirthDate;
			}
			this.dropRegionsSelect.SetSelectedRegionId(new int?(user.RegionId));
			this.txtAddress.Text = Globals.HtmlDecode(user.Address);
			this.txtQQ.Text = Globals.HtmlDecode(user.QQ);
			this.txtMSN.Text = Globals.HtmlDecode(user.MSN);
			this.txtTel.Text = Globals.HtmlDecode(user.TelPhone);
			this.txtHandSet.Text = Globals.HtmlDecode(user.CellPhone);
		}
		private bool ValidationMember(Member member)
		{
			ValidationResults validationResults = Validation.Validate<Member>(member, new string[]
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
				this.ShowMessage(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
