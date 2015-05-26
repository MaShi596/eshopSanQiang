using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_Drop_Suppliers : DropDownList
	{
		public Supplier_Drop_Suppliers()
		{
			base.Items.Clear();
			base.Items.Add(new ListItem("主站", "0"));
			System.Data.DataTable dataTable = (System.Data.DataTable)ManagerHelper.GetManagers(new ManagerQuery
			{
				RoleId = new Guid("625a27cc-7a55-41d6-8449-c6fe736003e5"),
				PageSize = 100000,
				PageIndex = 1
			}).Data;
			foreach (System.Data.DataRow dataRow in dataTable.Rows)
			{
				base.Items.Add(new ListItem(Globals.HtmlDecode((string)dataRow["UserName"]), ((int)dataRow["UserId"]).ToString()));
			}
		}
	}
}
