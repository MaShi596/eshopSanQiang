using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class SkuContentLabel : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(base.Text);
			string text = string.Empty;
			foreach (DataRow dataRow in skuContentBySku.Rows)
			{
				if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						dataRow["AttributeName"],
						":",
						dataRow["ValueStr"],
						"; "
					});
				}
			}
			base.Text = text;
			base.Render(writer);
		}
	}
}
