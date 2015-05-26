using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web.Security;
namespace Hidistro.Membership.Data
{
	public class BizActorData : BizActorProvider
	{
		private Database database;
		public BizActorData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override Distributor GetDistributor(HiMembershipUser membershipUser)
		{
			Distributor distributor = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Distributors WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					distributor = new Distributor(membershipUser);
					distributor.GradeId = (int)dataReader["GradeId"];
					distributor.TradePassword = (string)dataReader["TradePassword"];
					distributor.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
					distributor.PurchaseOrder = (int)dataReader["PurchaseOrder"];
					distributor.Expenditure = (decimal)dataReader["Expenditure"];
					distributor.Balance = (decimal)dataReader["Balance"];
					distributor.RequestBalance = (decimal)dataReader["RequestBalance"];
					distributor.MemberCount = (int)dataReader["MemberCount"];
					if (dataReader["TopRegionId"] != DBNull.Value)
					{
						distributor.TopRegionId = (int)dataReader["TopRegionId"];
					}
					if (dataReader["RegionId"] != DBNull.Value)
					{
						distributor.RegionId = (int)dataReader["RegionId"];
					}
					if (dataReader["RealName"] != DBNull.Value)
					{
						distributor.RealName = (string)dataReader["RealName"];
					}
					if (dataReader["CompanyName"] != DBNull.Value)
					{
						distributor.CompanyName = (string)dataReader["CompanyName"];
					}
					if (dataReader["Address"] != DBNull.Value)
					{
						distributor.Address = (string)dataReader["Address"];
					}
					if (dataReader["Zipcode"] != DBNull.Value)
					{
						distributor.Zipcode = (string)dataReader["Zipcode"];
					}
					if (dataReader["TelPhone"] != DBNull.Value)
					{
						distributor.TelPhone = (string)dataReader["TelPhone"];
					}
					if (dataReader["CellPhone"] != DBNull.Value)
					{
						distributor.CellPhone = (string)dataReader["CellPhone"];
					}
					if (dataReader["QQ"] != DBNull.Value)
					{
						distributor.QQ = (string)dataReader["QQ"];
					}
					if (dataReader["Wangwang"] != DBNull.Value)
					{
						distributor.Wangwang = (string)dataReader["Wangwang"];
					}
					if (dataReader["MSN"] != DBNull.Value)
					{
						distributor.MSN = (string)dataReader["MSN"];
					}
					if (dataReader["Remark"] != DBNull.Value)
					{
						distributor.Remark = (string)dataReader["Remark"];
					}
				}
			}
			return distributor;
		}
		public override bool CreateDistributor(Distributor distributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Distributors (UserId, GradeId, TradePassword, TradePasswordSalt, TradePasswordFormat, PurchaseOrder, Expenditure, Balance, TopRegionId, RegionId, RealName,CompanyName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang, MSN, Remark) VALUES (@UserId, @GradeId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @PurchaseOrder, @Expenditure, @Balance, @TopRegionId, @RegionId, @RealName,@CompanyName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN, @Remark)");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributor.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, distributor.GradeId);
			this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(distributor.TradePasswordFormat, distributor.TradePassword, text));
			this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, distributor.TradePasswordFormat);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrder", System.Data.DbType.Int32, distributor.PurchaseOrder);
			this.database.AddInParameter(sqlStringCommand, "Expenditure", System.Data.DbType.Currency, distributor.Expenditure);
			this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, distributor.Balance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, distributor.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, distributor.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, distributor.RealName);
			this.database.AddInParameter(sqlStringCommand, "CompanyName", System.Data.DbType.String, distributor.CompanyName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, distributor.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, distributor.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, distributor.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, distributor.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, distributor.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, distributor.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, distributor.MSN);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, distributor.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateDistributor(Distributor distributor)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET GradeId = @GradeId, TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat, TopRegionId = @TopRegionId, RegionId = @RegionId, RealName = @RealName,CompanyName=@CompanyName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN, Remark=@Remark WHERE UserId = @UserId");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributor.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, distributor.GradeId);
			this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(distributor.TradePasswordFormat, distributor.TradePassword, text));
			this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, distributor.TradePasswordFormat);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, distributor.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, distributor.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, distributor.RealName);
			this.database.AddInParameter(sqlStringCommand, "CompanyName", System.Data.DbType.String, distributor.CompanyName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, distributor.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, distributor.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, distributor.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, distributor.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, distributor.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, distributor.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, distributor.MSN);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, distributor.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ChangeDistributorTradePassword(string username, string oldPassword, string newPassword)
		{
			return this.ChangeTradePassword("aspnet_Distributors", username, oldPassword, newPassword);
		}
		public override bool ValidDistributorTradePassword(string username, string password)
		{
			return this.CheckTradePassword("aspnet_Distributors", username, password);
		}
		public override SiteManager GetManager(HiMembershipUser membershipUser)
		{
			SiteManager result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(UserId) FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) == 1)
			{
				result = new SiteManager(membershipUser);
			}
			return result;
		}
		public override bool CreateManager(SiteManager manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (UserId) VALUES (@UserId)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, manager.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override Member GetMember(HiMembershipUser membershipUser)
		{
			Member member = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					member = new Member(UserRole.Member, membershipUser);
					member.GradeId = (int)dataReader["GradeId"];
					if (dataReader["ReferralUserId"] != DBNull.Value)
					{
						member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
					}
					member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
					member.TradePassword = (string)dataReader["TradePassword"];
					member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
					member.OrderNumber = (int)dataReader["OrderNumber"];
					member.Expenditure = (decimal)dataReader["Expenditure"];
					member.Points = (int)dataReader["Points"];
					member.Balance = (decimal)dataReader["Balance"];
					member.RequestBalance = (decimal)dataReader["RequestBalance"];
					if (dataReader["TopRegionId"] != DBNull.Value)
					{
						member.TopRegionId = (int)dataReader["TopRegionId"];
					}
					if (dataReader["RegionId"] != DBNull.Value)
					{
						member.RegionId = (int)dataReader["RegionId"];
					}
					if (dataReader["RealName"] != DBNull.Value)
					{
						member.RealName = (string)dataReader["RealName"];
					}
					if (dataReader["Address"] != DBNull.Value)
					{
						member.Address = (string)dataReader["Address"];
					}
					if (dataReader["Zipcode"] != DBNull.Value)
					{
						member.Zipcode = (string)dataReader["Zipcode"];
					}
					if (dataReader["TelPhone"] != DBNull.Value)
					{
						member.TelPhone = (string)dataReader["TelPhone"];
					}
					if (dataReader["CellPhone"] != DBNull.Value)
					{
						member.CellPhone = (string)dataReader["CellPhone"];
					}
					if (dataReader["QQ"] != DBNull.Value)
					{
						member.QQ = (string)dataReader["QQ"];
					}
					if (dataReader["Wangwang"] != DBNull.Value)
					{
						member.Wangwang = (string)dataReader["Wangwang"];
					}
					if (dataReader["MSN"] != DBNull.Value)
					{
						member.MSN = (string)dataReader["MSN"];
					}
				}
			}
			return member;
		}
		public override bool CreateMember(Member member)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Members (UserId, GradeId,ReferralUserId, TradePassword, TradePasswordSalt, TradePasswordFormat, OrderNumber, Expenditure, Points, Balance, TopRegionId, RegionId, RealName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang, MSN) VALUES (@UserId, @GradeId, @ReferralUserId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @OrderNumber, @Expenditure, @Points, @Balance, @TopRegionId, @RegionId, @RealName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN)");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, member.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(member.TradePasswordFormat, member.TradePassword, text));
			this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, member.TradePasswordFormat);
			this.database.AddInParameter(sqlStringCommand, "OrderNumber", System.Data.DbType.Int32, member.OrderNumber);
			this.database.AddInParameter(sqlStringCommand, "Expenditure", System.Data.DbType.Currency, member.Expenditure);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, member.Points);
			this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, member.Balance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, member.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, member.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, member.RealName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, member.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, member.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, member.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, member.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, member.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, member.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, member.MSN);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateMember(Member member)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId, IsOpenBalance = @IsOpenBalance, TopRegionId=@TopRegionId, RegionId = @RegionId, RealName = @RealName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN WHERE UserId = @UserId");
			UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "IsOpenBalance", System.Data.DbType.Boolean, member.IsOpenBalance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, member.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, member.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, member.RealName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, member.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, member.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, member.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, member.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, member.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, member.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, member.MSN);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword)
		{
			return this.ChangeTradePassword("aspnet_Members", username, oldPassword, newPassword);
		}
		public override bool ValidMemberTradePassword(string username, string password)
		{
			return this.CheckTradePassword("aspnet_Members", username, password);
		}
		public override Member GetUnderling(HiMembershipUser membershipUser)
		{
			Member member = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Members WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					member = new Member(UserRole.Underling, membershipUser);
					member.GradeId = (int)dataReader["GradeId"];
					member.ParentUserId = new int?((int)dataReader["ParentUserId"]);
					if (dataReader["ReferralUserId"] != DBNull.Value)
					{
						member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
					}
					member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
					member.TradePassword = (string)dataReader["TradePassword"];
					member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
					member.OrderNumber = (int)dataReader["OrderNumber"];
					member.Expenditure = (decimal)dataReader["Expenditure"];
					member.Points = (int)dataReader["Points"];
					member.Balance = (decimal)dataReader["Balance"];
					member.RequestBalance = (decimal)dataReader["RequestBalance"];
					if (dataReader["TopRegionId"] != DBNull.Value)
					{
						member.TopRegionId = (int)dataReader["TopRegionId"];
					}
					if (dataReader["RegionId"] != DBNull.Value)
					{
						member.RegionId = (int)dataReader["RegionId"];
					}
					if (dataReader["RealName"] != DBNull.Value)
					{
						member.RealName = (string)dataReader["RealName"];
					}
					if (dataReader["Address"] != DBNull.Value)
					{
						member.Address = (string)dataReader["Address"];
					}
					if (dataReader["Zipcode"] != DBNull.Value)
					{
						member.Zipcode = (string)dataReader["Zipcode"];
					}
					if (dataReader["TelPhone"] != DBNull.Value)
					{
						member.TelPhone = (string)dataReader["TelPhone"];
					}
					if (dataReader["CellPhone"] != DBNull.Value)
					{
						member.CellPhone = (string)dataReader["CellPhone"];
					}
					if (dataReader["QQ"] != DBNull.Value)
					{
						member.QQ = (string)dataReader["QQ"];
					}
					if (dataReader["Wangwang"] != DBNull.Value)
					{
						member.Wangwang = (string)dataReader["Wangwang"];
					}
					if (dataReader["MSN"] != DBNull.Value)
					{
						member.MSN = (string)dataReader["MSN"];
					}
				}
			}
			return member;
		}
		public override bool CreateUnderling(Member underling)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Members (UserId, GradeId, ParentUserId, ReferralUserId, TradePassword, TradePasswordSalt, TradePasswordFormat, OrderNumber, Expenditure, Points, Balance, TopRegionId, RegionId, RealName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang,  MSN) VALUES (@UserId, @GradeId, @ParentUserId, @ReferralUserId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @OrderNumber, @Expenditure, @Points, @Balance, @TopRegionId, @RegionId, @RealName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN)");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, underling.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, underling.GradeId);
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, underling.ParentUserId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, underling.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(underling.TradePasswordFormat, underling.TradePassword, text));
			this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, underling.TradePasswordFormat);
			this.database.AddInParameter(sqlStringCommand, "OrderNumber", System.Data.DbType.Int32, underling.OrderNumber);
			this.database.AddInParameter(sqlStringCommand, "Expenditure", System.Data.DbType.Currency, underling.Expenditure);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, underling.Points);
			this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, underling.Balance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, underling.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, underling.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, underling.RealName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, underling.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, underling.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, underling.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, underling.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, underling.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, underling.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, underling.MSN);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool UpdateUnderling(Member underling)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET GradeId = @GradeId,IsOpenBalance = @IsOpenBalance, TopRegionId=@TopRegionId, RegionId = @RegionId, RealName = @RealName, Address = @Address, Zipcode = @Zipcode, TelPhone = @TelPhone, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang, MSN = @MSN WHERE UserId = @UserId");
			UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, underling.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, underling.GradeId);
			this.database.AddInParameter(sqlStringCommand, "IsOpenBalance", System.Data.DbType.Boolean, underling.IsOpenBalance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, underling.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, underling.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, underling.RealName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, underling.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, underling.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, underling.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, underling.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, underling.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, underling.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, underling.MSN);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ChangeUnderlingTradePassword(string username, string oldPassword, string newPassword)
		{
			return this.ChangeTradePassword("distro_Members", username, oldPassword, newPassword);
		}
		public override bool ValidUnderlingTradePassword(string username, string password)
		{
			return this.CheckTradePassword("distro_Members", username, password);
		}
		private bool ChangeTradePassword(string tableName, string username, string oldPassword, string newPassword)
		{
			string text;
			int num;
			bool result;
			if (!this.CheckTradePassword(tableName, username, oldPassword, out text, out num))
			{
				result = false;
			}
			else
			{
                MembershipProvider provider = System.Web.Security.Membership.Provider;
				if (newPassword.Length < provider.MinRequiredPasswordLength || newPassword.Length > 128)
				{
					result = false;
				}
				else
				{
					int num2 = 0;
					for (int i = 0; i < newPassword.Length; i++)
					{
						if (!char.IsLetterOrDigit(newPassword, i))
						{
							num2++;
						}
					}
					if (num2 < provider.MinRequiredNonAlphanumericCharacters)
					{
						result = false;
					}
					else
					{
						if (provider.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(newPassword, provider.PasswordStrengthRegularExpression))
						{
							result = false;
						}
						else
						{
							string text2 = UserHelper.EncodePassword((MembershipPasswordFormat)num, newPassword, text);
							if (text2.Length > 128)
							{
								result = false;
							}
							else
							{
								System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE " + tableName + " SET TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
								this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, text2);
								this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
								this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, num);
								this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
								result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
							}
						}
					}
				}
			}
			return result;
		}
		private bool CheckTradePassword(string tableName, string username, string password)
		{
			string text;
			int num;
			return this.CheckTradePassword(tableName, username, password, out text, out num);
		}
		private bool CheckTradePassword(string tableName, string username, string password, out string salt, out int passwordFormat)
		{
			bool flag;
			string text;
			this.GetPasswordWithFormat(tableName, username, out flag, out passwordFormat, out salt, out text);
			bool result;
			if (!flag)
			{
				result = false;
			}
			else
			{
				string value = UserHelper.EncodePassword((MembershipPasswordFormat)passwordFormat, password, salt);
				result = text.Equals(value);
			}
			return result;
		}
		private void GetPasswordWithFormat(string tableName, string username, out bool success, out int passwordFormat, out string passwordSalt, out string passwordFromDb)
		{
			passwordFormat = 0;
			passwordSalt = null;
			passwordFromDb = null;
			success = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT biz.TradePasswordFormat, biz.TradePasswordSalt, biz.TradePassword FROM " + tableName + " AS biz INNER JOIN aspnet_Users AS u ON biz.UserId = u.UserId WHERE u.LoweredUserName = LOWER(@Username)");
			this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					passwordFormat = dataReader.GetInt32(0);
					passwordSalt = dataReader.GetString(1);
					passwordFromDb = dataReader.GetString(2);
					success = true;
				}
			}
		}
	}
}
