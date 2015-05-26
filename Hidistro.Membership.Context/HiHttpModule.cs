using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Core.Jobs;
using Hidistro.Core.Urls;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
namespace Hidistro.Membership.Context
{
	public class HiHttpModule : IHttpModule
	{
		private bool bool_0;
		private ApplicationType applicationType_0;
		private static readonly Regex regex_0 = new Regex("(loginentry.aspx|login.aspx|logout.aspx|resourcenotfound.aspx|verifycodeimage.aspx|SendPayment.aspx|PaymentReturn_url|PaymentNotify_url|InpourReturn_url|InpourNotify_url)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		public string ModuleName
		{
			get
			{
				return "HiHttpModule";
			}
		}
		private void method_0(object sender, System.EventArgs e)
		{
			if (this.applicationType_0 != ApplicationType.Installer)
			{
				HttpApplication httpApplication = (HttpApplication)sender;
				HttpContext context = httpApplication.Context;
				HiContext current = HiContext.Current;
				if (context.Request.IsAuthenticated)
				{
					string name = context.User.Identity.Name;
					if (!string.IsNullOrEmpty(name))
					{
						string[] rolesForUser = Roles.GetRolesForUser(name);
						if (rolesForUser != null && rolesForUser.Length > 0)
						{
							current.RolesCacheKey = string.Join(",", rolesForUser);
						}
					}
				}
			}
		}
		private void method_1(object sender, System.EventArgs e)
		{
			this.applicationType_0 = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;
			HttpApplication httpApplication = (HttpApplication)sender;
			HttpContext context = httpApplication.Context;
			if (context.Request.RawUrl.IndexOfAny(new char[]
			{
				'<',
				'>',
				'\'',
				'"'
			}) != -1)
			{
				context.Response.Redirect(context.Request.RawUrl.Replace("<", "%3c").Replace(">", "%3e").Replace("'", "%27").Replace("\"", "%22"), false);
				return;
			}
			this.method_2(context);
			if (this.applicationType_0 != ApplicationType.Installer)
			{
				if (this.applicationType_0 == ApplicationType.Admin && string.Compare(Globals.DomainName, "localhost", true) != 0 && string.Compare(Globals.DomainName, HiContext.Current.SiteSettings.SiteUrl, true) != 0)
				{
					context.Response.Redirect(Globals.GetSiteUrls().Home, true);
					return;
				}
				HiHttpModule.smethod_0(HiConfiguration.GetConfig().SSL, context);
				HiContext.Create(context, new UrlReWriterDelegate(HiHttpModule.smethod_1));
				if (HiContext.Current.SiteSettings.IsDistributorSettings)
				{
					if (HiContext.Current.SiteSettings.Disabled && this.applicationType_0 == ApplicationType.Common && !HiHttpModule.regex_0.IsMatch(context.Request.Url.AbsolutePath))
					{
						context.Response.Write("站点维护中，暂停访问！");
						context.Response.End();
						return;
					}
					if (this.applicationType_0 == ApplicationType.Admin)
					{
						context.Response.Redirect(Globals.GetSiteUrls().Home, false);
					}
				}
			}
		}
		private void method_2(HttpContext httpContext_0)
		{
			if (this.applicationType_0 == ApplicationType.Installer && this.bool_0)
			{
				httpContext_0.Response.Redirect(Globals.GetSiteUrls().Home, true);
				return;
			}
			if (!this.bool_0 && this.applicationType_0 != ApplicationType.Installer)
			{
				httpContext_0.Response.Redirect(Globals.ApplicationPath + "/installer/default.aspx", true);
			}
		}
		private static void smethod_0(SSLSettings sslsettings_0, HttpContext httpContext_0)
		{
			if (sslsettings_0 == SSLSettings.All)
			{
				Globals.RedirectToSSL(httpContext_0);
			}
		}
		public void Dispose()
		{
			if (this.applicationType_0 != ApplicationType.Installer)
			{
				Jobs.Instance().Stop();
			}
		}
		public void Init(HttpApplication application)
		{
			if (application != null)
			{
				application.BeginRequest += new System.EventHandler(this.method_1);
				application.AuthorizeRequest += new System.EventHandler(this.method_0);
				this.bool_0 = (ConfigurationManager.AppSettings["Installer"] == null);
				this.applicationType_0 = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;
				this.method_2(application.Context);
				if (this.applicationType_0 != ApplicationType.Installer)
				{
					Jobs.Instance().Start();
					Class5.smethod_0();
				}
			}
		}
		private static bool smethod_1(HttpContext httpContext_0)
		{
			string path = httpContext_0.Request.Path;
			string text = UrlReWriteProvider.Instance().RewriteUrl(path, httpContext_0.Request.Url.Query);
			if (text != null)
			{
				string queryString = null;
				int num = text.IndexOf('?');
				if (num >= 0)
				{
					queryString = ((num < text.Length - 1) ? text.Substring(num + 1) : string.Empty);
					text = text.Substring(0, num);
				}
				httpContext_0.RewritePath(text, null, queryString);
			}
			return text != null;
		}
	}
}
