using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorArticleCategoriesListBox : System.Web.UI.WebControls.ListBox
	{
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
			System.Collections.Generic.IList<ArticleCategoryInfo> mainArticleCategories = SubsiteCommentsHelper.GetMainArticleCategories();
			foreach (ArticleCategoryInfo current in mainArticleCategories)
			{
				this.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.CategoryId.ToString()));
			}
		}
	}
}
