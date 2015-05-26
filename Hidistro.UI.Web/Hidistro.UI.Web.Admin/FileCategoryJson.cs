using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.UI.Web.Admin
{
	public class FileCategoryJson : AdminPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, Hidistro.Membership.Context.Users.GetLoggedOnUsername(), true, true);
			if (user.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
			{
				base.Response.Write(JsonMapper.ToJson(hashtable));
				base.Response.End();
				return;
			}
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			hashtable["category_list"] = list;
			System.Data.DataTable photoCategories = GalleryHelper.GetPhotoCategories();
			foreach (System.Data.DataRow dataRow in photoCategories.Rows)
			{
				System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
				hashtable2["cId"] = dataRow["CategoryId"];
				hashtable2["cName"] = dataRow["CategoryName"];
				list.Add(hashtable2);
			}
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
	}
}
