using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorProductCategoriesDropDownList : System.Web.UI.WebControls.DropDownList
	{
		private string m_NullToDisplay = "";
		private bool m_AutoDataBind = false;
		private string strDepth = "\u3000\u3000";
		private bool isTopCategory = false;
		public string NullToDisplay
		{
			get
			{
				return this.m_NullToDisplay;
			}
			set
			{
				this.m_NullToDisplay = value;
			}
		}
		public bool AutoDataBind
		{
			get
			{
				return this.m_AutoDataBind;
			}
			set
			{
				this.m_AutoDataBind = value;
			}
		}
		public bool IsTopCategory
		{
			get
			{
				return this.isTopCategory;
			}
			set
			{
				this.isTopCategory = value;
			}
		}
		public bool IsUnclassified
		{
			get;
			set;
		}
		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					result = new int?(int.Parse(base.SelectedValue, System.Globalization.CultureInfo.InvariantCulture));
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}
		protected override void OnLoad(System.EventArgs eventArgs_0)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new System.Web.UI.WebControls.ListItem(this.NullToDisplay, string.Empty));
			if (this.IsUnclassified)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem("未分类商品", "0"));
			}
			System.Collections.Generic.IList<CategoryInfo> list;
			if (this.IsTopCategory)
			{
				list = SubsiteCatalogHelper.GetMainCategories();
				using (System.Collections.Generic.IEnumerator<CategoryInfo> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CategoryInfo current = enumerator.Current;
						this.Items.Add(new System.Web.UI.WebControls.ListItem(Globals.HtmlDecode(current.Name), current.CategoryId.ToString()));
					}
					return;
				}
			}
			list = SubsiteCatalogHelper.GetSequenceCategories();
			for (int i = 0; i < list.Count; i++)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(this.FormatDepth(list[i].Depth, Globals.HtmlDecode(list[i].Name)), list[i].CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
			}
		}
		private string FormatDepth(int depth, string categoryName)
		{
			for (int i = 1; i < depth; i++)
			{
				categoryName = this.strDepth + categoryName;
			}
			return categoryName;
		}
	}
}
