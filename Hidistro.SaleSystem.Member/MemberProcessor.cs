using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
namespace Hidistro.SaleSystem.Member
{
	public static class MemberProcessor
	{
		public static int GetDefaultMemberGrade()
		{
			return MemberProvider.Instance().GetDefaultMemberGrade();
		}
		public static CreateUserStatus CreateMember(Hidistro.Membership.Context.Member member)
		{
			CreateUserStatus result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = Users.CreateUser(member, HiContext.Current.Config.RolesConfiguration.Underling);
			}
			else
			{
				result = Users.CreateUser(member, HiContext.Current.Config.RolesConfiguration.Member);
			}
			return result;
		}
        public static LoginUserStatus ValidLogin(Hidistro.Membership.Context.Member member)
		{
			LoginUserStatus result;
			if (member == null)
			{
				result = LoginUserStatus.InvalidCredentials;
			}
			else
			{
				result = Users.ValidateUser(member);
			}
			return result;
		}
		public static IList<ShippingAddressInfo> GetShippingAddresses(int userId)
		{
			return MemberProvider.Instance().GetShippingAddresses(userId);
		}
		public static ShippingAddressInfo GetShippingAddress(int shippingId)
		{
			return MemberProvider.Instance().GetShippingAddress(shippingId);
		}
		public static OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			return MemberProvider.Instance().GetOpenIdSettings(openIdType);
		}
		public static IList<OpenIdSettingsInfo> GetConfigedItems()
		{
			return MemberProvider.Instance().GetConfigedItems();
		}
	}
}
