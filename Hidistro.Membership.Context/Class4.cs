using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.Security;
internal class Class4 : Class0
{
	private static readonly Class4 class4_0;
	private BizActorProvider bizActorProvider_0;
	static Class4()
	{
		Class4.class4_0 = new Class4();
		Class4.class4_0.bizActorProvider_0 = BizActorProvider.Instance();
	}
	private Class4()
	{
	}
	public override bool vmethod_0(string string_0, string string_1)
	{
		if (!(HiContext.Current.User is SiteManager))
		{
			return false;
		}
		string string_2 = this.vmethod_5(string_0);
		return this.vmethod_1(string_0, string_2, string_1);
	}
	public override bool vmethod_1(string string_0, string string_1, string string_2)
	{
		return this.bizActorProvider_0.ChangeMemberTradePassword(string_0, string_1, string_2);
	}
	public override bool vmethod_2(IUser iuser_0)
	{
		bool result;
		try
		{
			result = this.bizActorProvider_0.CreateMember(iuser_0 as Member);
		}
		catch
		{
			result = false;
		}
		return result;
	}
	private static void smethod_1(string string_0, out int int_0, out string string_1)
	{
		int_0 = 0;
		string_1 = null;
		Database database = DatabaseFactory.CreateDatabase();
		System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Members WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
		database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, string_0);
		using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
		{
			if (dataReader.Read())
			{
				int_0 = dataReader.GetInt32(0);
				string_1 = dataReader.GetString(1);
			}
		}
	}
	public override IUser vmethod_3(HiMembershipUser hiMembershipUser_0)
	{
		return this.bizActorProvider_0.GetMember(hiMembershipUser_0);
	}
	public static Class4 smethod_2()
	{
		return Class4.class4_0;
	}
	public override bool vmethod_4(int int_0, string string_0)
	{
		Database database = DatabaseFactory.CreateDatabase();
		System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET IsOpenBalance = 'true', TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = @UserId");
		string text = UserHelper.CreateSalt();
		database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(MembershipPasswordFormat.Hashed, string_0, text));
		database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
		database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, 1);
		database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, int_0);
		return database.ExecuteNonQuery(sqlStringCommand) > 0;
	}
	public override string vmethod_5(string string_0)
	{
		if (!(HiContext.Current.User is SiteManager))
		{
			return null;
		}
		string text = Membership.GeneratePassword(10, 0);
		int num;
		string text2;
		Class4.smethod_1(string_0, out num, out text2);
		string text3 = UserHelper.EncodePassword((MembershipPasswordFormat)num, text, text2);
		if (text3.Length > 128)
		{
			return null;
		}
		Database database = DatabaseFactory.CreateDatabase();
		System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
		database.AddInParameter(sqlStringCommand, "NewTradePassword", System.Data.DbType.String, text3);
		database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, text2);
		database.AddInParameter(sqlStringCommand, "PasswordFormat", System.Data.DbType.Int32, num);
		database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, string_0);
		database.ExecuteNonQuery(sqlStringCommand);
		return text;
	}
	public override bool vmethod_6(IUser iuser_0)
	{
		return this.bizActorProvider_0.UpdateMember(iuser_0 as Member);
	}
	public override bool vmethod_7(string string_0, string string_1)
	{
		return this.bizActorProvider_0.ValidMemberTradePassword(string_0, string_1);
	}
}
