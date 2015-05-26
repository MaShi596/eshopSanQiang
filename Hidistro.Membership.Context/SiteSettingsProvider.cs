using Hidistro.Core;
using System;
using System.Data;
namespace Hidistro.Membership.Context
{
	public abstract class SiteSettingsProvider
	{
		private static readonly SiteSettingsProvider siteSettingsProvider_0 = DataProviders.CreateInstance("Hidistro.Membership.Data.SettingsData,Hidistro.Membership.Data") as SiteSettingsProvider;
		public abstract SiteSettings GetDistributorSettings(int userId);
		public abstract SiteSettings GetDistributorSettings(string siteUrl);
		public static SiteSettingsProvider Instance()
		{
			if (SiteSettingsProvider.siteSettingsProvider_0 == null)
			{
				throw new System.Exception("反射对象：Hidistro.Membership.Data.SettingsData,Hidistro.Membership.Data 失败！");
			}
			return SiteSettingsProvider.siteSettingsProvider_0;
		}
		public static SiteSettings PopulateDistributorSettings(System.Data.IDataReader reader)
		{
			SiteSettings siteSettings = new SiteSettings(reader["SiteUrl"].ToString().ToLower(), new int?((int)reader["UserId"]));
			if (reader["LogoUrl"] != System.DBNull.Value)
			{
				siteSettings.LogoUrl = reader["LogoUrl"].ToString();
			}
			if (reader["RequestDate"] != System.DBNull.Value)
			{
				siteSettings.RequestDate = new System.DateTime?((System.DateTime)reader["RequestDate"]);
			}
			if (reader["CreateDate"] != System.DBNull.Value)
			{
				siteSettings.CreateDate = new System.DateTime?((System.DateTime)reader["CreateDate"]);
			}
			if (reader["SiteDescription"] != System.DBNull.Value)
			{
				siteSettings.SiteDescription = reader["SiteDescription"].ToString();
			}
			if (reader["SiteName"] != System.DBNull.Value)
			{
				siteSettings.SiteName = reader["SiteName"].ToString();
			}
			if (reader["Theme"] != System.DBNull.Value)
			{
				siteSettings.Theme = reader["Theme"].ToString();
			}
			if (reader["Footer"] != System.DBNull.Value)
			{
				siteSettings.Footer = reader["Footer"].ToString();
			}
			if (reader["SearchMetaKeywords"] != System.DBNull.Value)
			{
				siteSettings.SearchMetaKeywords = reader["SearchMetaKeywords"].ToString();
			}
			if (reader["SearchMetaDescription"] != System.DBNull.Value)
			{
				siteSettings.SearchMetaDescription = reader["SearchMetaDescription"].ToString();
			}
			if (reader["DecimalLength"] != System.DBNull.Value)
			{
				siteSettings.DecimalLength = (int)reader["DecimalLength"];
			}
			if (reader["YourPriceName"] != System.DBNull.Value)
			{
				siteSettings.YourPriceName = reader["YourPriceName"].ToString();
			}
			if (reader["Disabled"] != System.DBNull.Value)
			{
				siteSettings.Disabled = (bool)reader["Disabled"];
			}
			if (reader["ReferralDeduct"] != System.DBNull.Value)
			{
				siteSettings.ReferralDeduct = (int)reader["ReferralDeduct"];
			}
			if (reader["DefaultProductImage"] != System.DBNull.Value)
			{
				siteSettings.DefaultProductImage = reader["DefaultProductImage"].ToString();
			}
			if (reader["PointsRate"] != System.DBNull.Value)
			{
				siteSettings.PointsRate = (decimal)reader["PointsRate"];
			}
			if (reader["OrderShowDays"] != System.DBNull.Value)
			{
				siteSettings.OrderShowDays = (int)reader["OrderShowDays"];
			}
			if (reader["HtmlOnlineServiceCode"] != System.DBNull.Value)
			{
				siteSettings.HtmlOnlineServiceCode = reader["HtmlOnlineServiceCode"].ToString();
			}
			if (reader["EmailSender"] != System.DBNull.Value)
			{
				siteSettings.EmailSender = reader["EmailSender"].ToString();
			}
			if (reader["EmailSettings"] != System.DBNull.Value)
			{
				siteSettings.EmailSettings = reader["EmailSettings"].ToString();
			}
			if (reader["SMSSender"] != System.DBNull.Value)
			{
				siteSettings.SMSSender = reader["SMSSender"].ToString();
			}
			if (reader["SMSSettings"] != System.DBNull.Value)
			{
				siteSettings.SMSSettings = reader["SMSSettings"].ToString();
			}
			if (reader["IsOpenEtao"] != System.DBNull.Value)
			{
				siteSettings.IsOpenEtao = System.Convert.ToBoolean(reader["IsOpenEtao"]);
			}
			if (reader["EtaoID"] != System.DBNull.Value)
			{
				siteSettings.EtaoID = System.Convert.ToString(reader["EtaoID"]);
			}
			if (reader["EtaoApplyTime"] != System.DBNull.Value)
			{
				siteSettings.EtaoApplyTime = new System.DateTime?(System.Convert.ToDateTime(reader["EtaoApplyTime"]));
			}
			if (reader["EtaoStatus"] != System.DBNull.Value)
			{
				siteSettings.EtaoStatus = System.Convert.ToInt32(reader["EtaoStatus"]);
			}
			if (reader["RegisterAgreement"] != System.DBNull.Value)
			{
				siteSettings.RegisterAgreement = System.Convert.ToString(reader["RegisterAgreement"]);
			}
			return siteSettings;
		}
		public abstract void SaveDistributorSettings(SiteSettings settings);
	}
}
