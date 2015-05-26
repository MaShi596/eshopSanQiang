using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RegisterUserSave : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtTradeKey;
		private System.Web.UI.WebControls.TextBox txtTradeKey2;
		private System.Web.UI.WebControls.TextBox txtQuestion;
		private System.Web.UI.WebControls.TextBox txtAnswer;
		private System.Web.UI.WebControls.TextBox txtRealName;
		private RegionSelector dropRegions;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtQQ;
		private System.Web.UI.WebControls.TextBox txtMSN;
		private System.Web.UI.WebControls.TextBox txtTel;
		private System.Web.UI.WebControls.TextBox txtHandSet;
		private IButton btnSaveUser;
		private int userId;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RegisterUserSave.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (string.IsNullOrEmpty(this.Page.Request["UserId"]) || !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			this.txtTradeKey = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTradeKey");
			this.txtTradeKey2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTradeKey2");
			this.txtQuestion = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQuestion");
			this.txtAnswer = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAnswer");
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
			this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtQQ = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQQ");
			this.txtMSN = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMSN");
			this.txtTel = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTel");
			this.txtHandSet = (System.Web.UI.WebControls.TextBox)this.FindControl("txtHandSet");
			this.btnSaveUser = ButtonManager.Create(this.FindControl("btnSaveUser"));
			this.btnSaveUser.Click += new System.EventHandler(this.btnSaveUser_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRegions.DataBind();
				Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(this.userId) as Hidistro.Membership.Context.Member;
				this.txtHandSet.Text = member.CellPhone;
			}
		}
		private void btnSaveUser_Click(object sender, System.EventArgs e)
		{
			if ((!string.IsNullOrEmpty(this.txtQuestion.Text) && string.IsNullOrEmpty(this.txtAnswer.Text)) || (string.IsNullOrEmpty(this.txtQuestion.Text) && !string.IsNullOrEmpty(this.txtAnswer.Text)))
			{
				this.ShowMessage("密码问题和问题答案要设置的话就两者都必须填写", false);
			}
			else
			{
				Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(this.userId, false) as Hidistro.Membership.Context.Member;
				if (!string.IsNullOrEmpty(this.txtTradeKey.Text))
				{
					if (this.txtTradeKey.Text.Length < 6 || this.txtTradeKey.Text.Length > 20)
					{
						this.ShowMessage("交易密码长度必须为6-20个字符", false);
						return;
					}
					if (string.Compare(this.txtTradeKey.Text, this.txtTradeKey2.Text) != 0)
					{
						this.ShowMessage("两次输入的交易密码不一致", false);
						return;
					}
					member.IsOpenBalance = true;
					member.TradePassword = this.txtTradeKey.Text;
				}
				if (!string.IsNullOrEmpty(this.txtQuestion.Text) && !string.IsNullOrEmpty(this.txtAnswer.Text))
				{
					member.ChangePasswordQuestionAndAnswer("", Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text));
				}
				member.RealName = this.txtRealName.Text;
				if (this.dropRegions.GetSelectedRegionId().HasValue)
				{
					member.RegionId = this.dropRegions.GetSelectedRegionId().Value;
					member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
				}
				member.Address = Globals.HtmlEncode(this.txtAddress.Text);
				member.QQ = this.txtQQ.Text;
				member.MSN = this.txtMSN.Text;
				member.TelPhone = this.txtTel.Text;
				member.CellPhone = this.txtHandSet.Text;
				if (Hidistro.Membership.Context.Users.UpdateUser(member))
				{
					string oldPassword = member.ResetTradePassword(member.Username);
					member.ChangeTradePassword(oldPassword, member.TradePassword);
					this.Page.Response.Redirect(Globals.ApplicationPath + "/user/MyAccountSummary.aspx");
				}
			}
		}
	}
}
