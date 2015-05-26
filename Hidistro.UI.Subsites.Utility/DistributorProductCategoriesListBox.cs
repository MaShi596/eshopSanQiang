using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorProductCategoriesListBox : System.Web.UI.WebControls.ListBox
	{
		private string strDepth = "\u3000\u3000";
		public int SelectedCategoryId
		{
			get
			{
				int result = 0;
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						result = int.Parse(this.Items[i].Value);
					}
				}
				return result;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Value == value.ToString())
					{
						this.Items[i].Selected = true;
					}
					else
					{
						this.Items[i].Selected = false;
					}
				}
			}
		}
		public new System.Collections.Generic.IList<int> SelectedValue
		{
			get
			{
				System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int current in value)
				{
					for (int i = 0; i < this.Items.Count; i++)
					{
						if (this.Items[i].Value == current.ToString())
						{
							this.Items[i].Selected = true;
						}
					}
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			System.Collections.Generic.IList<CategoryInfo> sequenceCategories = SubsiteCatalogHelper.GetSequenceCategories();
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
			}
			System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("--所有--", "0");
			this.Items.Insert(0, item);
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
