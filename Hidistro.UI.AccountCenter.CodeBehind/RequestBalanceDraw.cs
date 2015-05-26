using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class RequestBalanceDraw : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private FormatedMoneyLabel lblBanlance;
		private System.Web.UI.WebControls.TextBox txtAmount;
		private System.Web.UI.WebControls.TextBox txtBankName;
		private System.Web.UI.WebControls.TextBox txtAccountName;
		private System.Web.UI.WebControls.TextBox txtMerchantCode;
		private System.Web.UI.WebControls.TextBox txtRemark;
		private System.Web.UI.WebControls.TextBox txtTradePassword;
		private IButton btnDrawNext;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RequestBalanceDraw.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.lblBanlance = (FormatedMoneyLabel)this.FindControl("lblBanlance");
			this.txtAmount = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAmount");
			this.txtBankName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtBankName");
			this.txtAccountName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAccountName");
			this.txtMerchantCode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMerchantCode");
			this.txtRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRemark");
			this.txtTradePassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTradePassword");
			this.btnDrawNext = ButtonManager.Create(this.FindControl("btnDrawNext"));
			this.btnDrawNext.Click += new System.EventHandler(this.btnDrawNext_Click);
			PageTitle.AddSiteNameTitle("申请提现", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.litUserName.Text = HiContext.Current.User.Username;
				this.lblBanlance.Money = member.Balance - member.RequestBalance;
			}
		}
		private void btnDrawNext_Click(object sender, System.EventArgs e)
		{
			Member member = HiContext.Current.User as Member;
			if (member.RequestBalance > 0m)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false);
			}
			else
			{
				decimal num = 0m;
				if (!decimal.TryParse(this.txtAmount.Text.Trim(), out num))
				{
					this.ShowMessage("提现金额输入错误,请重新输入提现金额", false);
				}
				else
				{
					if (num > (decimal)this.lblBanlance.Money)
					{
						this.ShowMessage("预付款余额不足,请重新输入提现金额", false);
					}
					else
					{
						if (string.IsNullOrEmpty(this.txtTradePassword.Text))
						{
							this.ShowMessage("请输入交易密码", false);
						}
						else
						{
							member.TradePassword = this.txtTradePassword.Text;
							if (!Users.ValidTradePassword(member))
							{
								this.ShowMessage("交易密码不正确,请重新输入", false);
							}
							else
							{
								BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
								balanceDrawRequestInfo.BankName = this.txtBankName.Text.Trim();
								balanceDrawRequestInfo.AccountName = this.txtAccountName.Text.Trim();
								balanceDrawRequestInfo.MerchantCode = this.txtMerchantCode.Text.Trim();
								balanceDrawRequestInfo.Amount = num;
								balanceDrawRequestInfo.Remark = this.txtRemark.Text.Trim();
								if (this.ValidateBalanceDrawRequest(balanceDrawRequestInfo))
								{
									this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_RequestBalanceDrawConfirm", new object[]
									{
										Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.BankName)),
										Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.AccountName)),
										Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.MerchantCode)),
										balanceDrawRequestInfo.Amount,
										Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequestInfo.Remark))
									}));
								}
							}
						}
					}
				}
			}
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
