using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class BrandCategoriesList : ListBox
	{
		public override void DataBind()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem("--任意--", "0"));
			DataTable dataTable = new DataTable();
			dataTable = ControlProvider.Instance().GetBrandCategories();
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.Items.Add(new ListItem((string)dataRow["BrandName"], ((int)dataRow["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
