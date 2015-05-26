using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Security;
namespace Hidistro.Membership.Context
{
	public sealed class HiContext
	{
		private const string string_0 = "Hishop_ContextStore";
		private HiConfiguration hiConfiguration_0;
		private IUser iuser_0;
		private string string_1 = "";
		private HttpContext httpContext_0;
		private bool bool_0;
		private NameValueCollection nameValueCollection_0;
		private SiteSettings siteSettings_0;
		private string string_2 = "";
		private string string_3 = "VerifyCode";
		private string string_4 = "";
		public ApplicationType ApplicationType
		{
			get
			{
				return this.Config.AppLocation.CurrentApplicationType;
			}
		}
		public HiConfiguration Config
		{
			get
			{
				if (this.hiConfiguration_0 == null)
				{
					this.hiConfiguration_0 = HiConfiguration.GetConfig();
				}
				return this.hiConfiguration_0;
			}
		}
		public HttpContext Context
		{
			get
			{
				return this.httpContext_0;
			}
		}
		public static HiContext Current
		{
			get
			{
				HttpContext current = HttpContext.Current;
				HiContext hiContext = current.Items["Hishop_ContextStore"] as HiContext;
				if (hiContext == null)
				{
					if (current == null)
					{
						throw new System.Exception("No HiContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
					}
					hiContext = new HiContext(current);
					HiContext.smethod_0(hiContext);
				}
				return hiContext;
			}
		}
		public string HostPath
		{
			get
			{
				if (string.IsNullOrEmpty(this.string_1))
				{
					Uri url = this.Context.Request.Url;
					string arg = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString());
					this.string_1 = string.Format("{0}://{1}{2}", url.Scheme, url.Host, arg);
				}
				return this.string_1;
			}
		}
		public bool IsUrlReWritten
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
			}
		}
		public int ReferralUserId
		{
			get
			{
				int result = 0;
				if (string.Compare(Globals.DomainName, HiContext.Current.SiteSettings.SiteUrl, true) == 0)
				{
					HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						int.TryParse(httpCookie.Value, out result);
					}
				}
				return result;
			}
		}
		public string RolesCacheKey
		{
			get
			{
				return this.string_2;
			}
			set
			{
				this.string_2 = value;
			}
		}
		public SiteSettings SiteSettings
		{
			get
			{
				if (this.siteSettings_0 == null)
				{
					this.siteSettings_0 = SettingsManager.GetSiteSettings();
				}
				return this.siteSettings_0;
			}
		}
		public string SiteUrl
		{
			get
			{
				return this.string_4;
			}
		}
		public IUser User
		{
			get
			{
				if (this.iuser_0 == null)
				{
					this.iuser_0 = Users.GetUser();
				}
				return this.iuser_0;
			}
			set
			{
				this.iuser_0 = value;
			}
		}
		private HiContext(HttpContext httpContext_1)
		{
			this.httpContext_0 = httpContext_1;
			this.method_1(new NameValueCollection(httpContext_1.Request.QueryString), httpContext_1.Request.Url, httpContext_1.Request.RawUrl, this.method_0());
		}
		public bool CheckVerifyCode(string verifyCode)
		{
			bool result = false;
			if (this.Config.UseUniversalCode && verifyCode.Equals("8888"))
			{
				this.method_2();
				result = true;
			}
			else
			{
				if (HttpContext.Current.Request.Cookies[this.string_3] != null)
				{
					result = (string.Compare(HttpContext.Current.Request.Cookies[this.string_3].Value, verifyCode, true) == 0);
					this.method_2();
				}
			}
			return result;
		}
		public static HiContext Create(HttpContext context)
		{
			return HiContext.Create(context, false);
		}
		public static HiContext Create(HttpContext context, UrlReWriterDelegate rewriter)
		{
			HiContext hiContext = new HiContext(context);
			HiContext.smethod_0(hiContext);
			if (rewriter != null)
			{
				hiContext.IsUrlReWritten = rewriter(context);
			}
			return hiContext;
		}
		public static HiContext Create(HttpContext context, bool isReWritten)
		{
			HiContext hiContext = new HiContext(context);
			hiContext.IsUrlReWritten = isReWritten;
			HiContext.smethod_0(hiContext);
			return hiContext;
		}
		public string CreateVerifyCode(int length)
		{
			System.Random random = new System.Random();
			string text = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= 4; i++)
			{
				stringBuilder.Append(text[random.Next(0, text.Length - 1)]);
			}
			this.method_2();
			HttpCookie httpCookie = new HttpCookie(this.string_3);
			httpCookie.Value = stringBuilder.ToString();
			HttpContext.Current.Response.Cookies.Add(httpCookie);
			return httpCookie.Value;
		}
		private string method_0()
		{
			return this.httpContext_0.Request.Url.Host;
		}
		public string GetSkinPath()
		{
			if (this.SiteSettings.IsDistributorSettings)
			{
				return string.Concat(new string[]
				{
					Globals.ApplicationPath,
					"/Templates/sites/",
					this.SiteSettings.UserId.Value.ToString(),
					"/",
					this.SiteSettings.Theme
				}).ToLower();
			}
			return (Globals.ApplicationPath + "/Templates/master/" + this.SiteSettings.Theme).ToLower();
		}
		public string GetStoragePath()
		{
			if (this.SiteSettings.IsDistributorSettings)
			{
				return "/Storage/sites/" + this.SiteSettings.UserId.Value.ToString();
			}
			if (HiContext.Current.User.UserRole == UserRole.Distributor)
			{
				return "/Storage/sites/" + HiContext.Current.User.UserId.ToString();
			}
			if (HiContext.Current.ApplicationType == ApplicationType.Distributor)
			{
				return "/Storage/sites/" + HiContext.Current.User.UserId.ToString();
			}
			return "/Storage/master";
		}
		private void method_1(NameValueCollection nameValueCollection_1, Uri uri_0, string string_5, string string_6)
		{
			this.nameValueCollection_0 = nameValueCollection_1;
			this.string_4 = string_6.ToLower();
			if (this.nameValueCollection_0 != null && this.nameValueCollection_0.Count > 0 && !string.IsNullOrEmpty(this.nameValueCollection_0["ReferralUserId"]))
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Site_ReferralUser");
				}
				httpCookie.Value = this.nameValueCollection_0["ReferralUserId"];
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
		}
		private void method_2()
		{
			HttpContext.Current.Response.Cookies[this.string_3].Value = null;
			HttpContext.Current.Response.Cookies[this.string_3].Expires = new System.DateTime(1911, 10, 12);
		}
		public void LogOut()
		{
			if (this.Context.Request.IsAuthenticated)
			{
				IUser user = HiContext.Current.User;
				FormsAuthentication.SignOut();
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[string.Format("Token_{0}", user.UserId)];
				if (httpCookie != null)
				{
					httpCookie.Expires = System.DateTime.Now.AddDays(-1.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies[string.Format("TopSession_{0}", user.UserId)];
				if (httpCookie2 != null)
				{
					httpCookie2.Expires = System.DateTime.Now.AddDays(-1.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
				}
				HttpCookie authCookie = FormsAuthentication.GetAuthCookie(user.Username, true);
				IUserCookie userCookie = user.GetUserCookie();
				if (userCookie != null)
				{
					userCookie.DeleteCookie(authCookie);
				}
				Users.ClearUserCache(user);
				RoleHelper.SignOut(user.Username);
				HttpCookie httpCookie3 = this.Context.Response.Cookies["hishopLoginStatus"];
				if (httpCookie3 != null)
				{
					httpCookie3.Value = "";
				}
			}
		}
		private static void smethod_0(HiContext hiContext_0)
		{
			hiContext_0.Context.Items["Hishop_ContextStore"] = hiContext_0;
		}
	}
}
