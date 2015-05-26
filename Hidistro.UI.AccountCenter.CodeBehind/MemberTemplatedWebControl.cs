using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class MemberTemplatedWebControl : HtmlTemplatedWebControl
	{
		protected MemberTemplatedWebControl()
		{
			if (HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member && HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Underling)
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[]
				{
					this.Page.Request.RawUrl
				}), true);
			}
		}
	}
}
