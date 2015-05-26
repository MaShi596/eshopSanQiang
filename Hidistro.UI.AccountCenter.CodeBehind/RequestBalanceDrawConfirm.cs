using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class RequestBalanceDrawConfirm : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private FormatedMoneyLabel lblDrawBanlance;
		private System.Web.UI.WebControls.Literal litBankName;
		private System.Web.UI.WebControls.Literal litAccountName;
		private System.Web.UI.WebControls.Literal litMerchantCode;
		private System.Web.UI.WebControls.Literal litRemark;
		private IButton btnDrawConfirm;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RequestBalanceDrawConfirm.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.lblDrawBanlance = (FormatedMoneyLabel)this.FindControl("lblDrawBanlance");
			this.litBankName = (System.Web.UI.WebControls.Literal)this.FindControl("litBankName");
			this.litAccountName = (System.Web.UI.WebControls.Literal)this.FindControl("litAccountName");
			this.litMerchantCode = (System.Web.UI.WebControls.Literal)this.FindControl("litMerchantCode");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.btnDrawConfirm = ButtonManager.Create(this.FindControl("btnDrawConfirm"));
			PageTitle.AddSiteNameTitle("确认申请提现", HiContext.Current.Context);
			this.btnDrawConfirm.Click += new System.EventHandler(this.btnDrawConfirm_Click);
			if (!this.Page.IsPostBack)
			{
				BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
				this.litUserName.Text = HiContext.Current.User.Username;
				this.lblDrawBanlance.Money = balanceDrawRequest.Amount;
				this.litBankName.Text = balanceDrawRequest.BankName;
				this.litAccountName.Text = balanceDrawRequest.AccountName;
				this.litMerchantCode.Text = balanceDrawRequest.MerchantCode;
				this.litRemark.Text = balanceDrawRequest.Remark;
			}
		}
		private void btnDrawConfirm_Click(object sender, System.EventArgs e)
		{
			Member member = Users.GetUser(HiContext.Current.User.UserId) as Member;
			if (member.RequestBalance > 0m)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false);
			}
			else
			{
				BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
				if (this.ValidateBalanceDrawRequest(balanceDrawRequest))
				{
					if (PersonalHelper.BalanceDrawRequest(balanceDrawRequest))
					{
						this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_MyBalanceDetails"));
					}
					else
					{
						this.ShowMessage("申请提现过程中出现未知错误", false);
					}
				}
			}
		}
		private BalanceDrawRequestInfo GetBalanceDrawRequest()
		{
			BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
			balanceDrawRequestInfo.UserId = HiContext.Current.User.UserId;
			balanceDrawRequestInfo.UserName = HiContext.Current.User.Username;
			balanceDrawRequestInfo.RequestTime = System.DateTime.Now;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["bankName"]))
			{
				balanceDrawRequestInfo.BankName = Globals.UrlDecode(this.Page.Request.QueryString["bankName"]);
			}
			else
			{
				balanceDrawRequestInfo.BankName = string.Empty;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["accountName"]))
			{
				balanceDrawRequestInfo.AccountName = Globals.UrlDecode(this.Page.Request.QueryString["accountName"]);
			}
			else
			{
				balanceDrawRequestInfo.AccountName = string.Empty;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["merchantCode"]))
			{
				balanceDrawRequestInfo.MerchantCode = Globals.UrlDecode(this.Page.Request.QueryString["merchantCode"]);
			}
			else
			{
				balanceDrawRequestInfo.MerchantCode = string.Empty;
			}
			decimal amount;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["amount"]) && decimal.TryParse(this.Page.Request.QueryString["amount"], out amount))
			{
				balanceDrawRequestInfo.Amount = amount;
			}
			else
			{
				balanceDrawRequestInfo.Amount = 0m;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["remark"]))
			{
				balanceDrawRequestInfo.Remark = Globals.UrlDecode(this.Page.Request.QueryString["remark"]);
			}
			else
			{
				balanceDrawRequestInfo.Remark = string.Empty;
			}
			return balanceDrawRequestInfo;
		}
		private bool ValidateBalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			ValidationResults validationResults = Validation.Validate<BalanceDrawRequestInfo>(balanceDrawRequest, new string[]
			{
				"ValBalanceDrawRequestInfo"
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
