using Hidistro.UI.Subsites.Utility;
using LitJson;
using System;
using System.Collections;
namespace Hidistro.UI.Web.Shopadmin
{
	public class FileCategoryJson : DistributorPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
	}
}
