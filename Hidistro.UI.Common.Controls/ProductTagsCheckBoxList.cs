using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsCheckBoxList : CheckBoxList
	{
		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
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
					for (int j = 0; j < this.Items.Count; j++)
					{
						if (this.Items[j].Value == current.ToString())
						{
							this.Items[j].Selected = true;
						}
					}
				}
			}
		}
		public override void DataBind()
		{
			base.Items.Clear();
			DataTable tags = ControlProvider.Instance().GetTags();
			foreach (DataRow dataRow in tags.Rows)
			{
				ListItem item = new ListItem(Globals.HtmlEncode(dataRow["TagName"].ToString()), dataRow["TagID"].ToString());
				base.Items.Add(item);
			}
		}
	}
}
