using System.Text.RegularExpressions;
using System.Web.Security;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Membership.Context
{
	[HasSelfValidation]
	public class Distributor : IUser
	{
		private static System.EventHandler<UserEventArgs> eventHandler_0;
		private static System.EventHandler<UserEventArgs> eventHandler_1;
		private static System.EventHandler<System.EventArgs> eventHandler_2;
		private static System.EventHandler<UserEventArgs> eventHandler_3;
		private static System.EventHandler<UserEventArgs> eventHandler_4;
		private static System.EventHandler<UserEventArgs> eventHandler_5;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private decimal decimal_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_1;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_2;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private decimal decimal_1;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private bool bool_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_1;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private HiMembershipUser hiMembershipUser_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_3;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_2;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_3;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_4;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_5;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_4;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_6;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private decimal decimal_2;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_7;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private int int_5;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_8;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_9;
		public static event System.EventHandler<UserEventArgs> DealPasswordChanged
		{
			add
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_0;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_0, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_0;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_0, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		public static event System.EventHandler<UserEventArgs> FindPassword
		{
			add
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_1;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_1, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_1;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_1, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		public static event System.EventHandler<System.EventArgs> Login
		{
			add
			{
				System.EventHandler<System.EventArgs> eventHandler = Distributor.eventHandler_2;
				System.EventHandler<System.EventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<System.EventArgs> value2 = (System.EventHandler<System.EventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<System.EventArgs>>(ref Distributor.eventHandler_2, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<System.EventArgs> eventHandler = Distributor.eventHandler_2;
				System.EventHandler<System.EventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<System.EventArgs> value2 = (System.EventHandler<System.EventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<System.EventArgs>>(ref Distributor.eventHandler_2, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		public static event System.EventHandler<UserEventArgs> Logout
		{
			add
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_3;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_3, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_3;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_3, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		public static event System.EventHandler<UserEventArgs> PasswordChanged
		{
			add
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_4;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_4, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_4;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_4, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		public static event System.EventHandler<UserEventArgs> Register
		{
			add
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_5;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Combine(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_5, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				System.EventHandler<UserEventArgs> eventHandler = Distributor.eventHandler_5;
				System.EventHandler<UserEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					System.EventHandler<UserEventArgs> value2 = (System.EventHandler<UserEventArgs>)System.Delegate.Remove(eventHandler2, value);
					eventHandler = System.Threading.Interlocked.CompareExchange<System.EventHandler<UserEventArgs>>(ref Distributor.eventHandler_5, value2, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}
		[HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValDistributor", MessageTemplate = "详细地址必须控制在100个字符以内")]
		public string Address
		{
			get;
			set;
		}
		public decimal Balance
		{
			get;
			set;
		}
		public System.DateTime? BirthDate
		{
			get
			{
				return this.MembershipUser.BirthDate;
			}
			set
			{
				this.MembershipUser.BirthDate = value;
			}
		}
		public string CellPhone
		{
			get;
			set;
		}
		public string Comment
		{
			get
			{
				return this.MembershipUser.Comment;
			}
			set
			{
				this.MembershipUser.Comment = value;
			}
		}
		[HtmlCoding, StringLengthValidator(0, 60, Ruleset = "ValDistributor", MessageTemplate = "公司名称必须控制在60个字符以内")]
		public string CompanyName
		{
			get;
			set;
		}
		public System.DateTime CreateDate
		{
			get
			{
				return this.MembershipUser.CreateDate;
			}
		}
		public string Email
		{
			get
			{
				return this.MembershipUser.Email;
			}
			set
			{
				this.MembershipUser.Email = value;
			}
		}
		public decimal Expenditure
		{
			get;
			set;
		}
		public Gender Gender
		{
			get
			{
				return this.MembershipUser.Gender;
			}
			set
			{
				this.MembershipUser.Gender = value;
			}
		}
		public int GradeId
		{
			get;
			set;
		}
		public bool IsAnonymous
		{
			get
			{
				return this.MembershipUser.IsAnonymous;
			}
		}
		public bool IsApproved
		{
			get
			{
				return this.MembershipUser.IsApproved;
			}
			set
			{
				this.MembershipUser.IsApproved = value;
			}
		}
		public bool IsCreate
		{
			get;
			set;
		}
		public bool IsLockedOut
		{
			get
			{
				return this.MembershipUser.IsLockedOut;
			}
		}
		public System.DateTime LastActivityDate
		{
			get
			{
				return this.MembershipUser.LastActivityDate;
			}
			set
			{
				this.MembershipUser.LastActivityDate = value;
			}
		}
		public System.DateTime LastLockoutDate
		{
			get
			{
				return this.MembershipUser.LastLockoutDate;
			}
		}
		public System.DateTime LastLoginDate
		{
			get
			{
				return this.MembershipUser.LastLoginDate;
			}
		}
		public System.DateTime LastPasswordChangedDate
		{
			get
			{
				return this.MembershipUser.LastPasswordChangedDate;
			}
		}
		public int MemberCount
		{
			get;
			set;
		}
		public HiMembershipUser MembershipUser
		{
			get;
			set;
		}
		public string MobilePIN
		{
			get
			{
				return this.MembershipUser.MobilePIN;
			}
			set
			{
				this.MembershipUser.MobilePIN = value;
			}
		}
		public string MSN
		{
			get;
			set;
		}
		public string Password
		{
			get
			{
				return this.MembershipUser.Password;
			}
			set
			{
				this.MembershipUser.Password = value;
			}
		}
		public MembershipPasswordFormat PasswordFormat
		{
			get
			{
				return this.MembershipUser.PasswordFormat;
			}
			set
			{
				this.MembershipUser.PasswordFormat = value;
			}
		}
		public string PasswordQuestion
		{
			get
			{
				return this.MembershipUser.PasswordQuestion;
			}
		}
		public int Points
		{
			get;
			set;
		}
		public int PurchaseOrder
		{
			get;
			set;
		}
		public string QQ
		{
			get;
			set;
		}
		[StringLengthValidator(0, 20, Ruleset = "ValDistributor", MessageTemplate = "真实姓名必须控制在20个字符以内")]
		public string RealName
		{
			get;
			set;
		}
		public int RegionId
		{
			get;
			set;
		}
		[StringLengthValidator(0, 300, Ruleset = "ValDistributor", MessageTemplate = "合作备忘录必须控制在300个字符以内")]
		public string Remark
		{
			get;
			set;
		}
		public decimal RequestBalance
		{
			get;
			set;
		}
		public string TelPhone
		{
			get;
			set;
		}
		public int TopRegionId
		{
			get;
			set;
		}
		public string TradePassword
		{
			get
			{
				return this.MembershipUser.TradePassword;
			}
			set
			{
				this.MembershipUser.TradePassword = value;
			}
		}
		public MembershipPasswordFormat TradePasswordFormat
		{
			get
			{
				return this.MembershipUser.TradePasswordFormat;
			}
			set
			{
				this.MembershipUser.TradePasswordFormat = value;
			}
		}
		public int UserId
		{
			get
			{
				return this.MembershipUser.UserId;
			}
			set
			{
				this.MembershipUser.UserId = value;
			}
		}
		public string Username
		{
			get
			{
				return this.MembershipUser.Username;
			}
			set
			{
				this.MembershipUser.Username = value;
			}
		}
		public UserRole UserRole
		{
			get
			{
				return this.MembershipUser.UserRole;
			}
		}
		public string Wangwang
		{
			get;
			set;
		}
		public string Zipcode
		{
			get;
			set;
		}
		public Distributor()
		{
			this.MembershipUser = new HiMembershipUser(false, UserRole.Distributor);
		}
		public Distributor(HiMembershipUser membershipUser)
		{
			this.MembershipUser = membershipUser;
		}
		public bool ChangePassword(string newPassword)
		{
			if (HiContext.Current.User.UserRole == UserRole.SiteManager)
			{
				string password = this.MembershipUser.Membership.ResetPassword();
				if (this.MembershipUser.ChangePassword(password, newPassword))
				{
					return true;
				}
			}
			return false;
		}
		public bool ChangePassword(string oldPassword, string newPassword)
		{
			return this.MembershipUser.ChangePassword(oldPassword, newPassword);
		}
		public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
		{
			return this.MembershipUser.ChangePasswordQuestionAndAnswer(newQuestion, newAnswer);
		}
		public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
		{
			return this.MembershipUser.ChangePasswordQuestionAndAnswer(oldAnswer, newQuestion, newAnswer);
		}
		public bool ChangePasswordWithAnswer(string answer, string newPassword)
		{
			return this.MembershipUser.ChangePasswordWithAnswer(answer, newPassword);
		}
		public bool ChangeTradePassword(string newPassword)
		{
			return Class3.smethod_2().vmethod_0(this.Username, newPassword);
		}
		public bool ChangeTradePassword(string oldPassword, string newPassword)
		{
			return Class3.smethod_2().vmethod_1(this.Username, oldPassword, newPassword);
		}
		[SelfValidation(Ruleset = "ValDistributor")]
		public void CheckDistributor(ValidationResults results)
		{
			HiConfiguration config = HiConfiguration.GetConfig();
			if (!string.IsNullOrEmpty(this.Username) && this.Username.Length <= config.UsernameMaxLength && this.Username.Length >= config.UsernameMinLength)
			{
				if (!Regex.IsMatch(this.Username, config.UsernameRegex))
				{
					results.AddResult(new ValidationResult("用户名的格式错误", this, "", "", null));
				}
			}
			else
			{
				results.AddResult(new ValidationResult(string.Format("用户名不能为空，长度必须在{0}-{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength), this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.Email) && this.Email.Length <= 256)
			{
				if (!Regex.IsMatch(this.Email, config.EmailRegex))
				{
					results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
				}
			}
			else
			{
				results.AddResult(new ValidationResult("电子邮件不能为空，长度必须小于256个字符", this, "", "", null));
			}
			if (this.IsCreate)
			{
				if (string.IsNullOrEmpty(this.Password) || this.Password.Length > config.PasswordMaxLength || this.Password.Length < 6)
				{
					results.AddResult(new ValidationResult(string.Format("密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
				}
				if (string.IsNullOrEmpty(this.TradePassword) || this.TradePassword.Length > config.PasswordMaxLength || this.TradePassword.Length < 6)
				{
					results.AddResult(new ValidationResult(string.Format("交易密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
				}
			}
			if (!string.IsNullOrEmpty(this.QQ) && (this.QQ.Length > 20 || this.QQ.Length < 3 || !Regex.IsMatch(this.QQ, "^[0-9]*$")))
			{
				results.AddResult(new ValidationResult("QQ号长度限制在3-20个字符之间，只能输入数字", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.Zipcode) && (this.Zipcode.Length > 10 || this.Zipcode.Length < 3 || !Regex.IsMatch(this.Zipcode, "^[0-9]*$")))
			{
				results.AddResult(new ValidationResult("邮编长度限制在3-10个字符之间，只能输入数字", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.Wangwang) && (this.Wangwang.Length > 20 || this.Wangwang.Length < 3))
			{
				results.AddResult(new ValidationResult("旺旺长度限制在3-20个字符之间", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.MSN) && (this.MSN.Length > 256 || this.MSN.Length < 1 || !Regex.IsMatch(this.MSN, config.EmailRegex)))
			{
				results.AddResult(new ValidationResult("请输入正确MSN帐号，长度在1-256个字符以内", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.CellPhone) && (this.CellPhone.Length > 20 || this.CellPhone.Length < 3 || !Regex.IsMatch(this.CellPhone, "^[0-9]*$")))
			{
				results.AddResult(new ValidationResult("手机号码长度限制在3-20个字符之间,只能输入数字", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.TelPhone) && (this.TelPhone.Length > 20 || this.TelPhone.Length < 3 || !Regex.IsMatch(this.TelPhone, "^[0-9-]*$")))
			{
				results.AddResult(new ValidationResult("电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”", this, "", "", null));
			}
		}
		public IUserCookie GetUserCookie()
		{
			return new UserCookie(this);
		}
		public bool IsInRole(string roleName)
		{
			return roleName.Equals(HiContext.Current.Config.RolesConfiguration.Distributor);
		}
		public void OnDealPasswordChanged(UserEventArgs args)
		{
			if (Distributor.eventHandler_0 != null)
			{
				Distributor.eventHandler_0(this, args);
			}
		}
		public static void OnDealPasswordChanged(Member member, UserEventArgs args)
		{
			if (Distributor.eventHandler_0 != null)
			{
				Distributor.eventHandler_0(member, args);
			}
		}
		public void OnFindPassword(UserEventArgs args)
		{
			if (Distributor.eventHandler_1 != null)
			{
				Distributor.eventHandler_1(this, args);
			}
		}
		public static void OnFindPassword(Member member, UserEventArgs args)
		{
			if (Distributor.eventHandler_1 != null)
			{
				Distributor.eventHandler_1(member, args);
			}
		}
		public void OnLogin()
		{
			if (Distributor.eventHandler_2 != null)
			{
				Distributor.eventHandler_2(this, new System.EventArgs());
			}
		}
		public static void OnLogin(Member member)
		{
			if (Distributor.eventHandler_2 != null)
			{
				Distributor.eventHandler_2(member, new System.EventArgs());
			}
		}
		public static void OnLogout(UserEventArgs args)
		{
			if (Distributor.eventHandler_3 != null)
			{
				Distributor.eventHandler_3(null, args);
			}
		}
		public void OnPasswordChanged(UserEventArgs args)
		{
			if (Distributor.eventHandler_4 != null)
			{
				Distributor.eventHandler_4(this, args);
			}
		}
		public static void OnPasswordChanged(Member member, UserEventArgs args)
		{
			if (Distributor.eventHandler_4 != null)
			{
				Distributor.eventHandler_4(member, args);
			}
		}
		public void OnRegister(UserEventArgs args)
		{
			if (Distributor.eventHandler_5 != null)
			{
				Distributor.eventHandler_5(this, args);
			}
		}
		public static void OnRegister(Member member, UserEventArgs args)
		{
			if (Distributor.eventHandler_5 != null)
			{
				Distributor.eventHandler_5(member, args);
			}
		}
		public string ResetPassword(string answer)
		{
			return this.MembershipUser.ResetPassword(answer);
		}
		public bool ValidatePasswordAnswer(string answer)
		{
			return this.MembershipUser.ValidatePasswordAnswer(answer);
		}
	}
}
