using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class ProductLineCheckBoxList : CheckBoxList
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
			IList<ProductLineInfo> productLineList = ProductLineHelper.GetProductLineList();
			foreach (ProductLineInfo current in productLineList)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.LineId.ToString()));
			}
		}
	}
}
