using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
internal class Class2 : Class0
{
	private static readonly Class2 class2_0;
	private BizActorProvider bizActorProvider_0;
	static Class2()
	{
		Class2.class2_0 = new Class2();
		Class2.class2_0.bizActorProvider_0 = BizActorProvider.Instance();
	}
	private Class2()
	{
	}
	public override bool vmethod_0(string string_0, string string_1)
	{
		return true;
	}
	public override bool vmethod_1(string string_0, string string_1, string string_2)
	{
		return true;
	}
	public override bool vmethod_2(IUser iuser_0)
	{
		bool result;
		try
		{
			result = this.bizActorProvider_0.CreateManager(iuser_0 as SiteManager);
		}
		catch
		{
			result = false;
		}
		return result;
	}
	public override IUser vmethod_3(HiMembershipUser hiMembershipUser_0)
	{
		return this.bizActorProvider_0.GetManager(hiMembershipUser_0);
	}
	public static Class2 smethod_1()
	{
		return Class2.class2_0;
	}
	public override bool vmethod_4(int int_0, string string_0)
	{
		return true;
	}
	public override string vmethod_5(string string_0)
	{
		return "000000";
	}
	public override bool vmethod_6(IUser iuser_0)
	{
		return true;
	}
	public override bool vmethod_7(string string_0, string string_1)
	{
		return true;
	}
}
