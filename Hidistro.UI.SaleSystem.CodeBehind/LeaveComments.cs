using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class LeaveComments : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptLeaveComments;
		private Pager pager;
		private System.Web.UI.WebControls.TextBox txtTitle;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtContent;
		private IButton btnRefer;
		private System.Web.UI.HtmlControls.HtmlControl spLeaveUserName;
		private System.Web.UI.HtmlControls.HtmlControl spLeavePsw;
		private System.Web.UI.HtmlControls.HtmlControl spLeaveReg;
		private System.Web.UI.HtmlControls.HtmlInputText txtLeaveUserName;
		private System.Web.UI.HtmlControls.HtmlInputText txtLeavePsw;
		private System.Web.UI.HtmlControls.HtmlInputText txtLeaveCode;
		private string verifyCodeKey = "VerifyCode";
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value, verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-LeaveComments.html";
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
			this.rptLeaveComments = (ThemedTemplatedRepeater)this.FindControl("rptLeaveComments");
			this.pager = (Pager)this.FindControl("pager");
			this.txtTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTitle");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.spLeaveUserName = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spLeaveUserName");
			this.spLeavePsw = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spLeavePsw");
			this.spLeaveReg = (System.Web.UI.HtmlControls.HtmlControl)this.FindControl("spLeaveReg");
			this.txtLeaveUserName = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtLeaveUserName");
			this.txtLeavePsw = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtLeavePsw");
			this.txtLeaveCode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtLeaveCode");
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			PageTitle.AddSiteNameTitle("客户留言", Hidistro.Membership.Context.HiContext.Current.Context);
			if (Hidistro.Membership.Context.HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || Hidistro.Membership.Context.HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling)
			{
				this.txtUserName.Text = Hidistro.Membership.Context.HiContext.Current.User.Username;
				this.txtLeaveUserName.Value = string.Empty;
				this.txtLeavePsw.Value = string.Empty;
				this.spLeaveUserName.Visible = false;
				this.spLeavePsw.Visible = false;
				this.spLeaveReg.Visible = false;
				this.btnRefer.Text = "发表";
			}
			else
			{
				this.spLeaveUserName.Visible = true;
				this.spLeavePsw.Visible = true;
				this.spLeaveReg.Visible = true;
				this.btnRefer.Text = "登录并留言";
			}
			this.txtLeaveCode.Value = string.Empty;
			this.BindList();
		}
		public void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!Hidistro.Membership.Context.HiContext.Current.CheckVerifyCode(this.txtLeaveCode.Value))
			{
				this.ShowMessage("验证码不正确", false);
			}
			else
			{
				if (this.ValidateConvert() && (Hidistro.Membership.Context.HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || Hidistro.Membership.Context.HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling || this.userRegion(this.txtLeaveUserName.Value, this.txtLeavePsw.Value)))
				{
					LeaveCommentInfo leaveCommentInfo = new LeaveCommentInfo();
					leaveCommentInfo.UserName = Globals.HtmlEncode(this.txtUserName.Text);
					leaveCommentInfo.UserId = new int?(Hidistro.Membership.Context.HiContext.Current.User.UserId);
					leaveCommentInfo.Title = Globals.HtmlEncode(this.txtTitle.Text);
					leaveCommentInfo.PublishContent = Globals.HtmlEncode(this.txtContent.Text);
					ValidationResults validationResults = Validation.Validate<LeaveCommentInfo>(leaveCommentInfo, new string[]
					{
						"Refer"
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
					else
					{
						if (CommentProcessor.InsertLeaveComment(leaveCommentInfo))
						{
							this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "留言成功，管理员回复后即可显示", Globals.GetSiteUrls().UrlData.FormatUrl("LeaveComments")));
						}
						else
						{
							this.ShowMessage("留言失败", false);
						}
						this.txtTitle.Text = string.Empty;
						this.txtContent.Text = string.Empty;
					}
				}
			}
		}
		private void BindList()
		{
			DbQueryResult leaveComments = CommentBrowser.GetLeaveComments(new LeaveCommentQuery
			{
				PageSize = 3,
				PageIndex = this.pager.PageIndex
			});
			this.rptLeaveComments.DataSource = leaveComments.Data;
			this.rptLeaveComments.DataBind();
            this.pager.TotalRecords = (int)((double)leaveComments.TotalRecords * (System.Convert.ToDouble(this.pager.PageSize) / 3.0));
		}
		private bool userRegion(string username, string password)
		{
			Hidistro.Membership.Context.HiContext current = Hidistro.Membership.Context.HiContext.Current;
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(0, username, false, true) as Hidistro.Membership.Context.Member;
			bool result;
			if (member == null || member.IsAnonymous)
			{
				this.ShowMessage("用户名或密码错误", false);
				result = false;
			}
			else
			{
				member.Password = password;
				Hidistro.Membership.Core.Enums.LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
				if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.Success)
				{
					System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
					Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
					userCookie.WriteCookie(authCookie, 30, false);
					current.User = member;
					result = true;
				}
				else
				{
					if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountPending)
					{
						this.ShowMessage("用户账号还没有通过审核", false);
						result = false;
					}
					else
					{
						if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials)
						{
							this.ShowMessage("用户名或密码错误", false);
							result = false;
						}
						else
						{
							this.ShowMessage("未知错误", false);
							result = false;
						}
					}
				}
			}
			return result;
		}
		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member && Hidistro.Membership.Context.HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Underling && (string.IsNullOrEmpty(this.txtLeaveUserName.Value) || string.IsNullOrEmpty(this.txtLeavePsw.Value)))
			{
				text += Formatter.FormatErrorMessage("请填写用户名和密码");
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
	}
}
