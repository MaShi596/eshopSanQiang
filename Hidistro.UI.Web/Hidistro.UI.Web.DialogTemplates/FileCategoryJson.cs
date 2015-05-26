using Hidistro.Membership.Context;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
namespace Hidistro.UI.Web.DialogTemplates
{
	public class FileCategoryJson : System.Web.IHttpHandler
	{
		private string message = "";
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			if (Hidistro.Membership.Context.HiContext.Current.Context.User.IsInRole("manager") || Hidistro.Membership.Context.HiContext.Current.Context.User.IsInRole("systemadministrator"))
			{
				System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
				hashtable["category_list"] = list;
				System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
				hashtable2["cId"] = "AdvertImg";
				hashtable2["cName"] = "广告位图片";
				list.Add(hashtable2);
				hashtable2 = new System.Collections.Hashtable();
				hashtable2["cId"] = "TitleImg";
				hashtable2["cName"] = "标题图片";
				list.Add(hashtable2);
			}
			this.message = JsonMapper.ToJson(hashtable);
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
	}
}
