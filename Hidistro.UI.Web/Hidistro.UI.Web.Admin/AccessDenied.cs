using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class AccessDenied : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litMessage;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.HiContext.Current.User;
			if (user.IsInRole("供应商") || user.IsInRole("区域发货点"))
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Login.aspx"));
			}
			this.litMessage.Text = string.Format("您登录的管理员帐号 “{0}” 没有权限访问当前页面或进行当前操作", Hidistro.Membership.Context.HiContext.Current.User.Username);
		}
	}
}
