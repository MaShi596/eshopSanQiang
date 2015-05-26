using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class AdminPage : Page
	{
		protected override void OnInit(EventArgs eventArgs_0)
		{
			this.RegisterFrameScript();
			this.CheckPageAccess();
			base.OnInit(eventArgs_0);
		}
		protected virtual void RegisterFrameScript()
		{
			string key = "admin-frame";
			string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", Globals.ApplicationPath + "/admin/default.html");
			ClientScriptManager clientScript = this.Page.ClientScript;
			if (!clientScript.IsClientScriptBlockRegistered(key))
			{
				clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
			}
		}
		protected virtual void ShowMsg(ValidationResults validateResults)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValidationResult current in (IEnumerable<ValidationResult>)validateResults)
			{
				stringBuilder.Append(Formatter.FormatErrorMessage(current.Message));
			}
			this.ShowMsg(stringBuilder.ToString(), false);
		}
		protected virtual void ShowMsg(string string_0, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1})", string_0, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}
		protected virtual void CloseWindow()
		{
			string str = "var win = art.dialog.open.origin; win.location.reload();";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
			}
		}
		protected void ReloadPage(NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}
		protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}
		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			string result;
			if (queryStrings == null || queryStrings.Count == 0)
			{
				result = base.Request.Url.AbsolutePath;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.Request.Url.AbsolutePath).Append("?");
				foreach (string text in queryStrings.Keys)
				{
					string text2 = queryStrings[text].Trim().Replace("'", "");
					if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
					{
						stringBuilder.Append(text).Append("=").Append(base.Server.UrlEncode(text2)).Append("&");
					}
				}
				queryStrings.Clear();
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				result = stringBuilder.ToString();
			}
			return result;
		}
		protected void GotoResourceNotFound()
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound.aspx"));
		}
		private void CheckPageAccess()
		{
			IUser user = Hidistro.Membership.Context.HiContext.Current.User;
			if (user.UserRole != UserRole.SiteManager)
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().Login, true);
			}
			else
			{
				Hidistro.Membership.Context.SiteManager siteManager = user as Hidistro.Membership.Context.SiteManager;
				if (!siteManager.IsAdministrator)
				{
					AdministerCheckAttribute administerCheckAttribute = (AdministerCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(AdministerCheckAttribute));
					if (administerCheckAttribute != null && administerCheckAttribute.AdministratorOnly)
					{
						this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx"));
					}
					PrivilegeCheckAttribute privilegeCheckAttribute = (PrivilegeCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(PrivilegeCheckAttribute));
					if (privilegeCheckAttribute != null && !siteManager.HasPrivilege((int)privilegeCheckAttribute.Privilege))
					{
						this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilegeCheckAttribute.Privilege.ToString()));
					}
				}
			}
		}
	}
}
