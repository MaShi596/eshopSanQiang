using System;
using System.Web;
namespace Hidistro.Membership.Core
{
	public interface IUserCookie
	{
		void WriteCookie(HttpCookie cookie, int days, bool autoLogin);
		void DeleteCookie(HttpCookie cookie);
	}
}
