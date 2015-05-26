using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Location : WebControl
	{
		public const string TagID = "common_Location";
		private string separatorString = ">>";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public string SeparatorString
		{
			get
			{
				return this.separatorString;
			}
			set
			{
				this.separatorString = value;
			}
		}
		public string CateGoryPath
		{
			get;
			set;
		}
		public string ProductName
		{
			get;
			set;
		}
		public Common_Location()
		{
			base.ID = "common_Location";
		}
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this.CateGoryPath))
			{
				string[] array = this.CateGoryPath.Split(new char[]
				{
					'|'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string s = array2[i];
					CategoryInfo category = CategoryBrowser.GetCategory(int.Parse(s));
					if (category != null)
					{
						stringBuilder.AppendFormat("<a href ='{0}'>{1}</a>{2}", Globals.GetSiteUrls().SubCategory(category.CategoryId, category.RewriteName), category.Name, this.SeparatorString);
					}
				}
				string text = stringBuilder.ToString();
				if (!string.IsNullOrEmpty(this.ProductName))
				{
					text += this.ProductName;
				}
				else
				{
					if (text.Length > this.SeparatorString.Length)
					{
						text = text.Remove(text.Length - this.SeparatorString.Length);
					}
				}
				writer.Write(text);
			}
		}
	}
}
