using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.SaleSystem.DistributionData
{
	public class MemberData : MemberSubsiteProvider
	{
		private Database database;
		public MemberData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override int GetDefaultMemberGrade()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT GradeId FROM distro_MemberGrades WHERE IsDefault = 1 AND CreateUserId = {0}", HiContext.Current.SiteSettings.UserId.Value));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override int GetMemberDiscount(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM distro_MemberGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 100;
			}
			return result;
		}
		public override IList<ShippingAddressInfo> GetShippingAddresses(int userId)
		{
			IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ShippingAddresses WHERE  UserID = @UserID");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingAddress(dataReader));
				}
			}
			return list;
		}
		public override ShippingAddressInfo GetShippingAddress(int shippingId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ShippingAddresses WHERE ShippingId = @ShippingId");
			this.database.AddInParameter(sqlStringCommand, "ShippingId", System.Data.DbType.Int32, shippingId);
			ShippingAddressInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingAddress(dataReader);
				}
			}
			return result;
		}
		public override IList<OpenIdSettingsInfo> GetConfigedItems()
		{
			IList<OpenIdSettingsInfo> list = new List<OpenIdSettingsInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_OpenIdSettings WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					OpenIdSettingsInfo item = MemberProvider.PopulateOpenIdSettings(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			OpenIdSettingsInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_OpenIdSettings WHERE UserId=@UserId AND LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = MemberProvider.PopulateOpenIdSettings(dataReader);
				}
			}
			return result;
		}
	}
}
