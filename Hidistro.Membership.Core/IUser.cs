using Hidistro.Membership.Core.Enums;
using System;
using System.Web.Security;
namespace Hidistro.Membership.Core
{
	public interface IUser
	{
		HiMembershipUser MembershipUser
		{
			get;
		}
		int UserId
		{
			get;
			set;
		}
		string Username
		{
			get;
			set;
		}
		string MobilePIN
		{
			get;
			set;
		}
		bool IsAnonymous
		{
			get;
		}
		DateTime LastActivityDate
		{
			get;
			set;
		}
		string Password
		{
			get;
			set;
		}
		MembershipPasswordFormat PasswordFormat
		{
			get;
			set;
		}
		string Email
		{
			get;
			set;
		}
		string PasswordQuestion
		{
			get;
		}
		bool IsApproved
		{
			get;
			set;
		}
		bool IsLockedOut
		{
			get;
		}
		DateTime CreateDate
		{
			get;
		}
		DateTime LastLoginDate
		{
			get;
		}
		DateTime LastPasswordChangedDate
		{
			get;
		}
		DateTime LastLockoutDate
		{
			get;
		}
		string Comment
		{
			get;
			set;
		}
		Gender Gender
		{
			get;
			set;
		}
		DateTime? BirthDate
		{
			get;
			set;
		}
		UserRole UserRole
		{
			get;
		}
		string TradePassword
		{
			get;
			set;
		}
		MembershipPasswordFormat TradePasswordFormat
		{
			get;
			set;
		}
		bool IsInRole(string roleName);
		string ResetPassword(string answer);
		bool ChangePassword(string newPassword);
		bool ChangePassword(string oldPassword, string newPassword);
		bool ChangePasswordWithAnswer(string answer, string newPassword);
		bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer);
		bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer);
		bool ValidatePasswordAnswer(string answer);
		IUserCookie GetUserCookie();
		bool ChangeTradePassword(string oldPassword, string newPassword);
		bool ChangeTradePassword(string newPassword);
	}
}
