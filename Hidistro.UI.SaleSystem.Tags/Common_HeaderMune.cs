using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_HeaderMune : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DataTable headerMune = this.GetHeaderMune();
			if (headerMune.Rows.Count > 0)
			{
				DataRow[] array = headerMune.Select("", "DisplaySequence ASC");
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow dataRow = array2[i];
					stringBuilder.AppendFormat("<li> <a href=\"{0}\"><span>{1}</span></a></li>", this.GetUrl((string)dataRow["Category"], (string)dataRow["Url"], (string)dataRow["Where"]), dataRow["Title"]);
				}
				writer.Write(stringBuilder.ToString());
			}
		}
		private string GetUrl(string category, string string_0, string where)
		{
			string text = string_0;
			if (category == "1")
			{
				text = Globals.GetSiteUrls().UrlData.FormatUrl(string_0);
			}
			else
			{
				if (category == "2")
				{
					string[] array = where.Split(new char[]
					{
						','
					});
					text = Globals.ApplicationPath + string.Format("/SubCategory.aspx?keywords={0}&minSalePrice={1}&maxSalePrice={2}", array[5], array[3], array[4]);
					if (array[0] != "0")
					{
						text = text + "&categoryId=" + array[0];
					}
					if (array[1] != "0")
					{
						text = text + "&brand=" + array[1];
					}
					if (array[2] != "0")
					{
						text = text + "&TagIds=" + array[2];
					}
				}
			}
			return text;
		}
		private DataTable GetHeaderMune()
		{
			string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.SiteSettings.UserId + "/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Title");
			dataTable.Columns.Add("DisplaySequence", typeof(int));
			dataTable.Columns.Add("Category");
			dataTable.Columns.Add("Url");
			dataTable.Columns.Add("Where");
			dataTable.Columns.Add("Visible");
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode xmlNode in childNodes)
			{
				if (xmlNode.Attributes["Visible"].Value.ToLower() == "true")
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["Title"] = xmlNode.Attributes["Title"].Value;
					dataRow["DisplaySequence"] = int.Parse(xmlNode.Attributes["DisplaySequence"].Value);
					dataRow["Category"] = xmlNode.Attributes["Category"].Value;
					dataRow["Url"] = xmlNode.Attributes["Url"].Value;
					dataRow["Where"] = xmlNode.Attributes["Where"].Value;
					dataRow["Visible"] = xmlNode.Attributes["Visible"].Value;
					dataTable.Rows.Add(dataRow);
				}
			}
			return dataTable;
		}
	}
}
