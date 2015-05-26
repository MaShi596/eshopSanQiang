using Hidistro.ControlPanel.Distribution;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Distribution;
using Hidistro.Membership.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
namespace Hidistro.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class DistributorHandler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string str = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			string str2 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			string str3 = "";
			string text = "";
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			new System.Text.StringBuilder();
			string text2 = context.Request.QueryString["action"].ToString();
			string sign = context.Request.Form["sign"];
			string text3 = context.Request.Form["format"];
			string checkCode = masterSettings.CheckCode;
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			try
			{
				string a;
				if ((a = text2) != null && a == "distribution_list")
				{
					string text4 = context.Request.Form["parma"].Trim();
					str3 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "parma");
					if (!string.IsNullOrEmpty(text4))
					{
						str3 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
						DistributorQuery query = new DistributorQuery();
						query = (DistributorQuery)JavaScriptConvert.DeserializeObject(text4, typeof(DistributorQuery));
						sortedDictionary = this.GetDistriubots(query);
						sortedDictionary.Add("action", "distribution_list");
						sortedDictionary.Add("format", text3);
						if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
						{
							DbQueryResult distributors = DistributorHelper.GetDistributors(query);
							string format = str + "<response_distributors>{0}<totalcount>{1}</totalcount></response_distributors>";
							if (distributors.Data != null)
							{
								text = string.Format(format, this.ConvertTableToXml((System.Data.DataTable)distributors.Data), distributors.TotalRecords.ToString());
							}
							else
							{
								text = string.Format(format, "", "0");
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				str3 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, ex.Message);
			}
			if (text == "")
			{
				text = text + str2 + str3;
			}
			context.Response.ContentType = "text/xml";
			if (text3 == "json")
			{
				text = text.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>", "");
				xmlDocument.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(text)));
				text = JavaScriptConvert.SerializeXmlNode(xmlDocument);
				context.Response.ContentType = "text/json";
			}
			context.Response.Write(text);
		}
		public System.Collections.Generic.SortedDictionary<string, string> GetDistriubots(DistributorQuery query)
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			if (query.GradeId.HasValue)
			{
				sortedDictionary.Add("GradeId", query.GradeId.Value.ToString());
			}
			if (query.LineId.HasValue)
			{
				sortedDictionary.Add("LineId", query.LineId.Value.ToString());
			}
			sortedDictionary.Add("SortBy", query.SortBy);
			if (query.SortOrder == SortAction.Desc)
			{
				sortedDictionary.Add("SortOrder", "0");
			}
			else
			{
				sortedDictionary.Add("SortOrder", "1");
			}
			sortedDictionary.Add("RealName", query.RealName);
			sortedDictionary.Add("Username", query.Username);
			sortedDictionary.Add("PageIndex", query.PageIndex.ToString());
			sortedDictionary.Add("PageSize", query.PageSize.ToString());
			return sortedDictionary;
		}
		public string ConvertTableToXml(System.Data.DataTable table)
		{
			string text = "";
			string format = "<distributor><userid>{0}</userid><username>{1}</username><email>{2}</email><createdate>{3}</createdate><regionid>{4}</regionid><realname>{5}</realname><balance>{6}</balance><address>{7}</address><zipcode>{8}</zipcode><telphone>{9}</telphone><cellphone>{10}</cellphone><productcount>{11}</productcount><gradeid>{12}</gradeid></distributor>";
			foreach (System.Data.DataRow dataRow in table.Rows)
			{
				text += string.Format(format, new object[]
				{
					dataRow["UserId"].ToString(),
					dataRow["UserName"].ToString(),
					dataRow["Email"].ToString(),
					dataRow["CreateDate"].ToString(),
					dataRow["RegionId"].ToString(),
					dataRow["RealName"].ToString(),
					dataRow["Balance"].ToString(),
					dataRow["Address"].ToString(),
					dataRow["Zipcode"].ToString(),
					dataRow["TelPhone"].ToString(),
					dataRow["CellPhone"].ToString(),
					dataRow["ProductCount"],
					dataRow["GradeId"].ToString()
				});
			}
			return text;
		}
	}
}
