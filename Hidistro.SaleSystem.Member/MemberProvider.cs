using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.SaleSystem.Member
{
	public abstract class MemberProvider
	{
		public static MemberProvider Instance()
		{
			MemberProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = MemberSubsiteProvider.CreateInstance();
			}
			else
			{
				result = MemberMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract int GetDefaultMemberGrade();
		public abstract int GetMemberDiscount(int gradeId);
		public abstract IList<ShippingAddressInfo> GetShippingAddresses(int userId);
		public abstract ShippingAddressInfo GetShippingAddress(int shippingId);
		public abstract OpenIdSettingsInfo GetOpenIdSettings(string openIdType);
		public abstract IList<OpenIdSettingsInfo> GetConfigedItems();
		public static OpenIdSettingsInfo PopulateOpenIdSettings(IDataReader reader)
		{
			OpenIdSettingsInfo openIdSettingsInfo = new OpenIdSettingsInfo
			{
				OpenIdType = (string)reader["OpenIdType"],
				Name = (string)reader["Name"],
				Settings = (string)reader["Settings"]
			};
			if (reader["Description"] != DBNull.Value)
			{
				openIdSettingsInfo.Description = (string)reader["Description"];
			}
			return openIdSettingsInfo;
		}
	}
}
