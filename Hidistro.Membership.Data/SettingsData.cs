using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace Hidistro.Membership.Data
{
	public class SettingsData : SiteSettingsProvider
	{
		private Database database;
		public SettingsData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override SiteSettings GetDistributorSettings(string siteUrl)
		{
			SiteSettings result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Settings WHERE LOWER(@SiteUrl) = LOWER(SiteUrl)");
			this.database.AddInParameter(sqlStringCommand, "SiteUrl", System.Data.DbType.String, siteUrl);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = SiteSettingsProvider.PopulateDistributorSettings(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override SiteSettings GetDistributorSettings(int userId)
		{
			SiteSettings result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Settings WHERE @UserId = UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = SiteSettingsProvider.PopulateDistributorSettings(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override void SaveDistributorSettings(SiteSettings settings)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Settings SET SiteUrl=@SiteUrl, LogoUrl=@LogoUrl, SiteDescription=@SiteDescription, SiteName=@SiteName, Theme=@Theme, Footer=@Footer, SearchMetaKeywords=@SearchMetaKeywords, SearchMetaDescription=@SearchMetaDescription,DecimalLength=@DecimalLength, YourPriceName=@YourPriceName, Disabled=@Disabled, ReferralDeduct = @ReferralDeduct, DefaultProductImage=@DefaultProductImage, PointsRate=@PointsRate,OrderShowDays=@OrderShowDays, HtmlOnlineServiceCode=@HtmlOnlineServiceCode,EmailSender=@EmailSender, EmailSettings=@EmailSettings, SMSSender=@SMSSender, SMSSettings=@SMSSettings,RegisterAgreement=@RegisterAgreement,IsOpenEtao=@IsOpenEtao,EtaoID=@EtaoID, EtaoApplyTime=@EtaoApplyTime,EtaoStatus=@EtaoStatus WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, settings.UserId);
			this.database.AddInParameter(sqlStringCommand, "SiteUrl", System.Data.DbType.String, settings.SiteUrl);
			this.database.AddInParameter(sqlStringCommand, "LogoUrl", System.Data.DbType.String, settings.LogoUrl);
			this.database.AddInParameter(sqlStringCommand, "SiteDescription", System.Data.DbType.String, settings.SiteDescription);
			this.database.AddInParameter(sqlStringCommand, "SiteName", System.Data.DbType.String, settings.SiteName);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, settings.Theme);
			this.database.AddInParameter(sqlStringCommand, "Footer", System.Data.DbType.String, settings.Footer);
			this.database.AddInParameter(sqlStringCommand, "SearchMetaKeywords", System.Data.DbType.String, settings.SearchMetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "SearchMetaDescription", System.Data.DbType.String, settings.SearchMetaDescription);
			this.database.AddInParameter(sqlStringCommand, "DecimalLength", System.Data.DbType.Int32, settings.DecimalLength);
			this.database.AddInParameter(sqlStringCommand, "YourPriceName", System.Data.DbType.String, settings.YourPriceName);
			this.database.AddInParameter(sqlStringCommand, "Disabled", System.Data.DbType.Boolean, settings.Disabled);
			this.database.AddInParameter(sqlStringCommand, "ReferralDeduct", System.Data.DbType.Int32, settings.ReferralDeduct);
			this.database.AddInParameter(sqlStringCommand, "DefaultProductImage", System.Data.DbType.String, settings.DefaultProductImage);
			this.database.AddInParameter(sqlStringCommand, "PointsRate", System.Data.DbType.Decimal, settings.PointsRate);
			this.database.AddInParameter(sqlStringCommand, "OrderShowDays", System.Data.DbType.Int32, settings.OrderShowDays);
			this.database.AddInParameter(sqlStringCommand, "HtmlOnlineServiceCode", System.Data.DbType.String, settings.HtmlOnlineServiceCode);
			this.database.AddInParameter(sqlStringCommand, "EmailSender", System.Data.DbType.String, settings.EmailSender);
			this.database.AddInParameter(sqlStringCommand, "EmailSettings", System.Data.DbType.String, settings.EmailSettings);
			this.database.AddInParameter(sqlStringCommand, "RegisterAgreement", System.Data.DbType.String, settings.RegisterAgreement);
			this.database.AddInParameter(sqlStringCommand, "SMSSender", System.Data.DbType.String, settings.SMSSender);
			this.database.AddInParameter(sqlStringCommand, "SMSSettings", System.Data.DbType.String, settings.SMSSettings);
			this.database.AddInParameter(sqlStringCommand, "IsOpenEtao", System.Data.DbType.Boolean, settings.IsOpenEtao);
			this.database.AddInParameter(sqlStringCommand, "EtaoID", System.Data.DbType.String, settings.EtaoID);
			this.database.AddInParameter(sqlStringCommand, "EtaoApplyTime", System.Data.DbType.DateTime, settings.EtaoApplyTime);
			this.database.AddInParameter(sqlStringCommand, "EtaoStatus", System.Data.DbType.Int32, settings.EtaoStatus);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
