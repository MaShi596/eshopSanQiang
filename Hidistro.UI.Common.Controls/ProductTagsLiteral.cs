using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsLiteral : Literal
	{
		protected IList<int> _selectvalue;
		public IList<int> SelectedValue
		{
			set
			{
				this._selectvalue = value;
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			DataTable tags = ControlProvider.Instance().GetTags();
			if (tags.Rows.Count < 0)
			{
				base.Text = "无";
				return;
			}
			foreach (DataRow dataRow in tags.Rows)
			{
				string text = "";
				if (this._selectvalue != null)
				{
					foreach (int current in this._selectvalue)
					{
						if (current == Convert.ToInt32(dataRow["TagID"].ToString()))
						{
							text = "checked=\"checked\"";
						}
					}
				}
				string text2 = base.Text;
				base.Text = string.Concat(new string[]
				{
					text2,
					"<input type=\"checkbox\" onclick=\"javascript:CheckTagId(this)\" value=\"",
					dataRow["TagID"].ToString(),
					"\" ",
					text,
					"/>",
					dataRow["TagName"].ToString(),
					"\u3000"
				});
			}
			base.Render(writer);
		}
	}
}
