using Hidistro.Core;
using Hidistro.Membership.Core.Enums;
using System;
namespace Hidistro.Membership.Core
{
	public abstract class MemberUserProvider
	{
		private static readonly MemberUserProvider _defaultInstance;
		static MemberUserProvider()
		{
			MemberUserProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.UserData,Hidistro.Membership.Data") as MemberUserProvider);
		}
		public static MemberUserProvider Instance()
		{
			return MemberUserProvider._defaultInstance;
		}
		public abstract CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer);
		public abstract HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline);
		public abstract AnonymousUser GetAnonymousUser();
		public abstract bool UpdateMembershipUser(HiMembershipUser user);
		public abstract bool ValidatePasswordAnswer(string username, string answer);
		public abstract bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer);
		public abstract string GetUsernameWithOpenId(string openId, string openIdType);
		public abstract bool BindOpenId(string username, string openId, string openIdType);
	}
}
