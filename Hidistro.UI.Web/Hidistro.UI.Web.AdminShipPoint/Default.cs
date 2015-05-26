using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.AdminShipPoint
{
	public class Default : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string a = base.Request["ctype"];
				string arg = "";
				if (a == "CheckRole")
				{
					if (!this.CheckRole())
					{
						arg = "0";
					}
					else
					{
						arg = "1";
					}
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				base.Response.Write("}");
				base.Response.End();
			}
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.HiContext.Current.User;
			if (!user.IsInRole("区域发货点"))
			{
				this.Page.Response.Redirect("login.aspx");
				return;
			}
			this.Page.Response.Redirect("default.html");
		}
		private bool CheckRole()
		{
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.HiContext.Current.User;
			return user.IsInRole("区域发货点");
		}
	}
}
