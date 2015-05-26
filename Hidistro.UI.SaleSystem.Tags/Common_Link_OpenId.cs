using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Link_OpenId : WebControl
	{
		public string ImageUrl
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
			if (!string.IsNullOrEmpty(this.ImageUrl))
			{
				if (this.ImageUrl.StartsWith("~"))
				{
					this.ImageUrl = base.ResolveUrl(this.ImageUrl);
				}
				else
				{
					if (this.ImageUrl.StartsWith("/"))
					{
						this.ImageUrl = HiContext.Current.GetSkinPath() + this.ImageUrl;
					}
					else
					{
						this.ImageUrl = HiContext.Current.GetSkinPath() + "/" + this.ImageUrl;
					}
				}
			}
			if (configedItems != null && configedItems.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (OpenIdSettingsInfo current in configedItems)
				{
					string arg = this.ImageUrl;
					arg = Globals.ApplicationPath + "/plugins/openid/images/" + current.OpenIdType + ".gif";
					if (HiContext.Current.User.UserRole != UserRole.Member && HiContext.Current.User.UserRole != UserRole.Underling)
					{
						stringBuilder.AppendFormat("<a href=\"{0}/OpenId/RedirectLogin.aspx?ot={1}\">", Globals.ApplicationPath, current.OpenIdType);
					}
					else
					{
						stringBuilder.Append("<a href=\"#\">");
					}
					stringBuilder.AppendFormat("<img src=\"{0}\" alt=\"{1}\" /> ", arg, current.Name);
					stringBuilder.Append("</a>");
				}
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
