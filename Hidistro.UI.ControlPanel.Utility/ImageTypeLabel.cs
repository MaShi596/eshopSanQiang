using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class ImageTypeLabel : Label
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string text = "<ul>";
			string str = string.Empty;
			DataTable photoCategories = GalleryHelper.GetPhotoCategories();
			int defaultPhotoCount = GalleryHelper.GetDefaultPhotoCount();
			string text2 = this.Page.Request.QueryString["ImageTypeId"];
			string text3 = string.Empty;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = this.Page.Request.QueryString["ImageTypeId"];
			}
			if (text2 == "0")
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<li><a href=\"",
					Globals.ApplicationPath,
					"/admin/store/ImageData.aspx?pageSize=20&ImageTypeId=0\" class='classLink'><s></s><strong>默认分类<span>(",
					defaultPhotoCount,
					")</span></strong></a></li>"
				});
			}
			else
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<li><a href=\"",
					Globals.ApplicationPath,
					"/admin/store/ImageData.aspx?pageSize=20&ImageTypeId=0\"><s></s><strong>默认分类<span>(",
					defaultPhotoCount,
					")</span></strong></a></li>"
				});
			}
			foreach (DataRow dataRow in photoCategories.Rows)
			{
				if (dataRow["CategoryId"].ToString() == text2)
				{
					text3 = "class='classLink'";
				}
				else
				{
					text3 = "";
				}
				str = string.Format(string.Concat(new string[]
				{
					"<li><a href=\"",
					Globals.ApplicationPath,
					"/admin/store/ImageData.aspx?pageSize=20&ImageTypeId={0}\" ",
					text3,
					"><s></s>{1}<span>({2})</span></a></li>"
				}), dataRow["CategoryId"], dataRow["CategoryName"], dataRow["PhotoCounts"].ToString());
				text += str;
			}
			text += "</ul>";
			base.Text = text;
			base.Render(writer);
		}
	}
}
