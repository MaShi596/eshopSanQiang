using Hidistro.Membership.Context;
using System;
using System.Web;
namespace Hidistro.UI.Web.Admin
{
	public class LoginUser : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = "";
			string text2 = context.Request.QueryString["action"];
			if (!string.IsNullOrEmpty(text2) && text2 == "login")
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				text = "{\"sitename\":\"" + masterSettings.SiteName + "\",";
				text = text + "\"username\":\"" + Hidistro.Membership.Context.HiContext.Current.User.Username + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(text);
		}
	}
}
