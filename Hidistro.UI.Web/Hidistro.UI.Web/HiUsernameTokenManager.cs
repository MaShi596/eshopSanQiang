using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Xml;
namespace Hidistro.UI.Web
{
	public class HiUsernameTokenManager : UsernameTokenManager
	{
		public HiUsernameTokenManager()
		{
		}
		public HiUsernameTokenManager(System.Xml.XmlNodeList nodes) : base(nodes)
		{
		}
		protected override string AuthenticateToken(UsernameToken token)
		{
			Hidistro.Membership.Core.Enums.LoginUserStatus loginUserStatus;
			try
			{
				Hidistro.Membership.Context.SiteManager siteManager = Hidistro.Membership.Context.Users.GetUser(0, token.Identity.Name, false, false) as Hidistro.Membership.Context.SiteManager;
				if (siteManager != null && siteManager.IsAdministrator)
				{
					Hidistro.Membership.Context.HiContext arg_29_0 = Hidistro.Membership.Context.HiContext.Current;
					siteManager.Password = HiCryptographer.Decrypt(token.Password);
					loginUserStatus = Hidistro.Membership.Context.Users.ValidateUser(siteManager);
				}
				else
				{
					loginUserStatus = Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials;
				}
			}
			catch
			{
				loginUserStatus = Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials;
			}
			if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.Success)
			{
				return token.Password;
			}
			return HiCryptographer.CreateHash(token.Password);
		}
	}
}
