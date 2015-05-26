using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
internal abstract class Class0
{
	public abstract bool vmethod_0(string string_0, string string_1);
	public abstract bool vmethod_1(string string_0, string string_1, string string_2);
	public abstract bool vmethod_2(IUser iuser_0);
	public abstract IUser vmethod_3(HiMembershipUser hiMembershipUser_0);
	public abstract bool vmethod_4(int int_0, string string_0);
	public abstract string vmethod_5(string string_0);
	public abstract bool vmethod_6(IUser iuser_0);
	public abstract bool vmethod_7(string string_0, string string_1);
	public static Class0 smethod_0(UserRole userRole_0)
	{
		switch (userRole_0)
		{
		case UserRole.SiteManager:
			return Class2.smethod_1();
		case UserRole.Distributor:
			return Class3.smethod_2();
		case UserRole.Member:
			return Class4.smethod_2();
		case UserRole.Underling:
			return Class1.smethod_2();
		default:
			return null;
		}
	}
}
