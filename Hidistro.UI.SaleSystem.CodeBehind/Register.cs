using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Web;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Register : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.CheckBox chkAgree;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private System.Web.UI.WebControls.TextBox txtPassword2;
		private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private System.Web.UI.WebControls.TextBox txtNumber;
		private IButton btnRegister;
		private string verifyCodeKey = "VerifyCode";
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value, verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Register.html";
			}
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["isCallback"]) && System.Web.HttpContext.Current.Request["isCallback"] == "true")
			{
				string verifyCode = System.Web.HttpContext.Current.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.ContentType = "application/json";
				System.Web.HttpContext.Current.Response.Write("{ ");
				System.Web.HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				System.Web.HttpContext.Current.Response.Write("}");
				System.Web.HttpContext.Current.Response.End();
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.chkAgree = (System.Web.UI.WebControls.CheckBox)this.FindControl("chkAgree");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.txtPassword2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword2");
			this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.txtNumber = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNumber");
			this.btnRegister = ButtonManager.Create(this.FindControl("btnRegister"));
			PageTitle.AddSiteNameTitle("会员注册", Hidistro.Membership.Context.HiContext.Current.Context);
			this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
		}
		private void btnRegister_Click(object sender, System.EventArgs e)
		{
			if (!this.chkAgree.Checked)
			{
				this.ShowMessage("您必须先阅读并同意注册协议", false);
			}
			else
			{
				if (string.Compare(this.txtUserName.Text.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture), "anonymous", false, System.Globalization.CultureInfo.InvariantCulture) == 0)
				{
					this.ShowMessage("已经存在相同的用户名", false);
				}
				else
				{
					if (this.txtUserName.Text.Trim().Length < 2 || this.txtUserName.Text.Trim().Length > 20)
					{
						this.ShowMessage("用户名不能为空，且在2-20个字符之间", false);
					}
					else
					{
						if (string.Compare(this.txtPassword.Text, this.txtPassword2.Text) != 0)
						{
							this.ShowMessage("两次输入的密码不相同", false);
						}
						else
						{
							if (this.txtPassword.Text.Length == 0)
							{
								this.ShowMessage("密码不能为空", false);
							}
							else
							{
								if (this.txtPassword.Text.Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength || this.txtPassword.Text.Length > HiConfiguration.GetConfig().PasswordMaxLength)
								{
									this.ShowMessage(string.Format("密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, HiConfiguration.GetConfig().PasswordMaxLength), false);
								}
								else
								{
									Hidistro.Membership.Context.Member member;
									if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
									{
										member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Underling);
										member.ParentUserId = Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId;
									}
									else
									{
										member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Member);
									}
									if (Hidistro.Membership.Context.HiContext.Current.ReferralUserId > 0)
									{
										member.ReferralUserId = new int?(Hidistro.Membership.Context.HiContext.Current.ReferralUserId);
									}
									member.GradeId = MemberProcessor.GetDefaultMemberGrade();
									member.Username = Globals.HtmlEncode(this.txtUserName.Text.Trim());
									member.Email = this.txtEmail.Text;
									member.Password = this.txtPassword.Text;
									member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
									member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
									member.TradePassword = this.txtPassword.Text;
									if (this.txtCellPhone != null)
									{
										member.CellPhone = this.txtCellPhone.Text;
									}
									member.IsApproved = true;
									member.RealName = string.Empty;
									member.Address = string.Empty;
									if (this.ValidationMember(member))
									{
										if (!Hidistro.Membership.Context.HiContext.Current.CheckVerifyCode(this.txtNumber.Text))
										{
											this.ShowMessage("验证码输入错误", false);
										}
										else
										{
											switch (MemberProcessor.CreateMember(member))
											{
											case Hidistro.Membership.Core.Enums.CreateUserStatus.UnknownFailure:
												this.ShowMessage("未知错误", false);
												break;
											case Hidistro.Membership.Core.Enums.CreateUserStatus.Created:
											{
												Messenger.UserRegister(member, this.txtPassword.Text);
												member.OnRegister(new Hidistro.Membership.Context.UserEventArgs(member.Username, this.txtPassword.Text, null));
												Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, member.Username, false, true);
												ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
												CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
												cookieShoppingProvider.ClearShoppingCart();
												Hidistro.Membership.Context.HiContext.Current.User = user;
												if (shoppingCart != null)
												{
													ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
												}
												System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
												Hidistro.Membership.Core.IUserCookie userCookie = user.GetUserCookie();
												userCookie.WriteCookie(authCookie, 30, false);
												this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("registerUserSave") + "?UserId=" + user.UserId);
												break;
											}
											case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateUsername:
												this.ShowMessage("已经存在相同的用户名", false);
												break;
											case Hidistro.Membership.Core.Enums.CreateUserStatus.DuplicateEmailAddress:
												this.ShowMessage("电子邮件地址已经存在", false);
												break;
											case Hidistro.Membership.Core.Enums.CreateUserStatus.DisallowedUsername:
												this.ShowMessage("用户名禁止注册", false);
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
							}
						}
					}
				}
			}
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
				this.ShowMessage(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
