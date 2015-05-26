using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.SaleSystem.Data
{
	public class MemberData : MemberMasterProvider
	{
		private Database database;
		public MemberData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override int GetDefaultMemberGrade()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
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
		public override IList<ShippingAddressInfo> GetShippingAddresses(int userId)
		{
			IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
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
