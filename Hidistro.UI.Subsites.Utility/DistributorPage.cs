using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorPage : System.Web.UI.Page
	{
		protected virtual void ShowMsg(ValidationResults validateResults)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validateResults)
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
		protected void ReloadPage(System.Collections.Specialized.NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}
		protected void ReloadPage(System.Collections.Specialized.NameValueCollection queryStrings, bool endResponse)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}
		protected void GotoResourceNotFound()
		{
			base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/ResourceNotFound.aspx");
		}
		private string GenericReloadUrl(System.Collections.Specialized.NameValueCollection queryStrings)
		{
			string result;
			if (queryStrings == null || queryStrings.Count == 0)
			{
				result = base.Request.Url.AbsolutePath;
			}
			else
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
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
	}
}
