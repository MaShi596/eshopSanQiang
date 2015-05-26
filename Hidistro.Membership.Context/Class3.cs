using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.Security;
internal class Class3 : Class0
{
	private static readonly Class3 class3_0;
	private BizActorProvider bizActorProvider_0;
	static Class3()
	{
		Class3.class3_0 = new Class3();
		Class3.class3_0.bizActorProvider_0 = BizActorProvider.Instance();
	}
	private Class3()
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
		return this.bizActorProvider_0.ChangeDistributorTradePassword(string_0, string_1, string_2);
	}
	public override bool vmethod_2(IUser iuser_0)
	{
		bool result;
		try
		{
			result = this.bizActorProvider_0.CreateDistributor(iuser_0 as Distributor);
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
		string_1 = string.Empty;
		Database database = DatabaseFactory.CreateDatabase();
		System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Distributors WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
		database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, string_0);
		using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
		{
			if (dataReader != null && dataReader.Read())
			{
				int_0 = dataReader.GetInt32(0);
				string_1 = dataReader.GetString(1);
			}
		}
	}
	public override IUser vmethod_3(HiMembershipUser hiMembershipUser_0)
	{
		return this.bizActorProvider_0.GetDistributor(hiMembershipUser_0);
	}
	public static Class3 smethod_2()
	{
		return Class3.class3_0;
	}
	public override bool vmethod_4(int int_0, string string_0)
	{
		return true;
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
		Class3.smethod_1(string_0, out num, out text2);
		string text3 = UserHelper.EncodePassword((MembershipPasswordFormat)num, text, text2);
		if (text3.Length > 128)
		{
			return null;
		}
		Database database = DatabaseFactory.CreateDatabase();
		System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Distributors SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
		database.AddInParameter(sqlStringCommand, "NewTradePassword", System.Data.DbType.String, text3);
		database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, text2);
		database.AddInParameter(sqlStringCommand, "PasswordFormat", System.Data.DbType.Int32, num);
		database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, string_0);
		database.ExecuteNonQuery(sqlStringCommand);
		return text;
	}
	public override bool vmethod_6(IUser iuser_0)
	{
		return this.bizActorProvider_0.UpdateDistributor(iuser_0 as Distributor);
	}
	public override bool vmethod_7(string string_0, string string_1)
	{
		return this.bizActorProvider_0.ValidDistributorTradePassword(string_0, string_1);
	}
}
