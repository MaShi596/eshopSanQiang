using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Messages;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorsRegister : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private System.Web.UI.WebControls.TextBox txtPasswordCompare;
		private System.Web.UI.WebControls.TextBox txtTransactionPassword;
		private System.Web.UI.WebControls.TextBox txtTransactionPasswordCompare;
		private System.Web.UI.WebControls.TextBox txtRealName;
		private System.Web.UI.WebControls.TextBox txtCompanyName;
		private RegionSelector dropRegion;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtZipcode;
		private System.Web.UI.WebControls.TextBox txtQQ;
		private System.Web.UI.WebControls.TextBox txtWangwang;
		private System.Web.UI.WebControls.TextBox txtMSN;
		private System.Web.UI.WebControls.TextBox txtTelPhone;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private System.Web.UI.WebControls.TextBox txtPasswordQuestion;
		private System.Web.UI.WebControls.TextBox txtPasswordAnswer;
		private System.Web.UI.WebControls.Button btnOK;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorsRegister.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.txtPasswordCompare = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPasswordCompare");
			this.txtTransactionPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTransactionPassword");
			this.txtTransactionPasswordCompare = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTransactionPasswordCompare");
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
			this.txtCompanyName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCompanyName");
			this.dropRegion = (RegionSelector)this.FindControl("dropRegion");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtZipcode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtZipcode");
			this.txtQQ = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQQ");
			this.txtWangwang = (System.Web.UI.WebControls.TextBox)this.FindControl("txtWangwang");
			this.txtMSN = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMSN");
			this.txtTelPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTelPhone");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.txtPasswordQuestion = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPasswordQuestion");
			this.txtPasswordAnswer = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPasswordAnswer");
			this.btnOK = (System.Web.UI.WebControls.Button)this.FindControl("btnOK");
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (this.ValidationInput())
			{
				int? selectedRegionId = this.dropRegion.GetSelectedRegionId();
				Hidistro.Membership.Core.HiMembershipUser membershipUser = new Hidistro.Membership.Core.HiMembershipUser(false, Hidistro.Membership.Core.Enums.UserRole.Distributor);
				Hidistro.Membership.Context.Distributor distributor = new Hidistro.Membership.Context.Distributor(membershipUser);
				distributor.IsApproved = false;
				distributor.Username = this.txtUserName.Text;
				distributor.Email = this.txtEmail.Text;
				distributor.Password = this.txtPasswordCompare.Text;
				if (!string.IsNullOrEmpty(this.txtTransactionPasswordCompare.Text))
				{
					distributor.TradePassword = this.txtTransactionPasswordCompare.Text;
				}
				else
				{
					distributor.TradePassword = distributor.Password;
				}
				distributor.RealName = this.txtRealName.Text;
				distributor.CompanyName = this.txtCompanyName.Text;
				if (selectedRegionId.HasValue)
				{
					distributor.RegionId = selectedRegionId.Value;
					distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
				}
				distributor.Address = this.txtAddress.Text;
				distributor.Zipcode = this.txtZipcode.Text;
				distributor.QQ = this.txtQQ.Text;
				distributor.Wangwang = this.txtWangwang.Text;
				distributor.MSN = this.txtMSN.Text;
				distributor.TelPhone = this.txtTelPhone.Text;
				distributor.CellPhone = this.txtCellPhone.Text;
				distributor.Remark = string.Empty;
				if (this.ValidationDistributorRequest(distributor))
				{
					switch (SubsiteStoreHelper.CreateDistributor(distributor))
					{
					case Hidistro.Membership.Core.Enums.CreateUserStatus.UnknownFailure:
						this.ShowMessage("未知错误", false);
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.Created:
						distributor.ChangePasswordQuestionAndAnswer(null, this.txtPasswordQuestion.Text, this.txtPasswordAnswer.Text);
						Messenger.UserRegister(distributor, this.txtPasswordCompare.Text);
						distributor.OnRegister(new Hidistro.Membership.Context.UserEventArgs(distributor.Username, this.txtPasswordCompare.Text, null));
						this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/DistributorsRegisterComplete.aspx");
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateUsername:
						this.ShowMessage("您输入的用户名已经被注册使用", false);
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateEmailAddress:
						this.ShowMessage("您输入的电子邮件地址已经被注册使用", false);
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.DisallowedUsername:
						this.ShowMessage("用户名被禁止注册", false);
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidPassword:
						this.ShowMessage("无效的密码", false);
						break;
					case Hidistro.Membership.Core.Enums.CreateUserStatus.InvalidEmail:
						this.ShowMessage("无效的电子邮件地址", false);
						break;
					}
				}
			}
		}
		private bool ValidationInput()
		{
			string text = string.Empty;
			if (this.txtUserName.Text.Trim().Length <= 1)
			{
				text += "请输入至少两位长度的字符";
			}
			if (string.Compare(this.txtPassword.Text, this.txtPasswordCompare.Text) != 0)
			{
				text += "请确定两次输入的登录密码相同";
			}
			if (string.IsNullOrEmpty(this.txtTransactionPassword.Text.Trim()))
			{
				text += "<br/>交易密码不允许为空！";
			}
			if (string.IsNullOrEmpty(this.txtTransactionPasswordCompare.Text.Trim()))
			{
				text += "<br/>重复交易密码不允许为空！";
			}
			if (!string.IsNullOrEmpty(this.txtTransactionPassword.Text) && string.Compare(this.txtTransactionPassword.Text, this.txtTransactionPasswordCompare.Text) != 0)
			{
				text += "<br/>请确定两次输入的交易密码相同";
			}
			if (string.IsNullOrEmpty(this.txtQQ.Text) && string.IsNullOrEmpty(this.txtWangwang.Text) && string.IsNullOrEmpty(this.txtMSN.Text))
			{
				text += "<br/>QQ,旺旺,MSN,三者必填其一";
			}
			if (string.IsNullOrEmpty(this.txtTelPhone.Text) && string.IsNullOrEmpty(this.txtCellPhone.Text))
			{
				text += "<br/>固定电话和手机,二者必填其一";
			}
			bool result;
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		private bool ValidationDistributorRequest(Hidistro.Membership.Context.Distributor distributor)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.Distributor>(distributor, new string[]
			{
				"ValDistributor"
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
