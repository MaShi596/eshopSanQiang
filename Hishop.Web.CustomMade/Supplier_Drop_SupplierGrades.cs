using Hidistro.Core;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_Drop_SupplierGrades : DropDownList
	{
		public Supplier_Drop_SupplierGrades()
		{
			base.Items.Clear();
			base.Items.Add(new ListItem("", ""));
			System.Data.DataTable dataTable = Methods.Supplier_SupplierGradeSGet();
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				base.Items.Add(new ListItem(Globals.HtmlDecode((string)dataRow["Name"]), ((int)dataRow["auto"]).ToString()));
			}
		}
	}
}
