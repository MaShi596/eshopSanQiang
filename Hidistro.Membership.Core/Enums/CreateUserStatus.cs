using System;
namespace Hidistro.Membership.Core.Enums
{
	public enum CreateUserStatus
	{
		UnknownFailure,
		Created,
		DuplicateUsername,
		DuplicateEmailAddress,
		InvalidFirstCharacter,
		DisallowedUsername,
		Updated,
		Deleted,
		InvalidQuestionAnswer,
		InvalidPassword,
		InvalidEmail,
		InvalidUserName
	}
}
