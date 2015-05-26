using Hidistro.Membership.Core;
using System;
using System.Text.RegularExpressions;
using System.Web;
namespace Hidistro.Membership.Context
{
	public class UserCookie : IUserCookie
	{
		private readonly HttpContext httpContext_0;
		private readonly HiContext hiContext_0 = HiContext.Current;
		public UserCookie(IUser user)
		{
			if (user != null && !user.IsAnonymous)
			{
				this.httpContext_0 = this.hiContext_0.Context;
			}
		}
		public void DeleteCookie(HttpCookie cookie)
		{
			if (cookie != null && this.httpContext_0 != null)
			{
				this.method_0(cookie);
				cookie.Expires = new System.DateTime(1911, 10, 12);
				this.httpContext_0.Response.Cookies.Add(cookie);
			}
		}
		private void method_0(HttpCookie httpCookie_0)
		{
			Regex regex = new Regex("[_a-zA-Z0-9-]+(\\.[_a-zA-Z0-9-]+)+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if (regex.IsMatch(this.httpContext_0.Request.Url.Host) && this.httpContext_0.Request.Url.Host.ToLower().EndsWith(this.hiContext_0.SiteSettings.SiteUrl.ToLower()))
			{
				httpCookie_0.Path = "/";
				httpCookie_0.Domain = this.hiContext_0.SiteSettings.SiteUrl;
			}
		}
		public void WriteCookie(HttpCookie cookie, int days, bool autoLogin)
		{
			if (cookie != null && this.httpContext_0 != null)
			{
				this.method_0(cookie);
				if (autoLogin)
				{
					cookie.Expires = System.DateTime.Now.AddDays((double)days);
				}
				this.httpContext_0.Response.Cookies.Add(cookie);
			}
		}
	}
}
