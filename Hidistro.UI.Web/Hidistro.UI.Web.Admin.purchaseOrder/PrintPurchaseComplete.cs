using Hidistro.ControlPanel.Sales;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class PrintPurchaseComplete : System.Web.UI.Page
	{
		protected string script;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string startNumber = base.Request["mailNo"];
			string[] array = base.Request["orderIds"].Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string str = array2[i];
				list.Add("'" + str + "'");
			}
			SalesHelper.SetPurchaseOrderExpressComputerpe(string.Join(",", list.ToArray()), base.Request["templateName"], base.Request["templateName"]);
			SalesHelper.SetPurchaseOrderShipNumber(array, startNumber);
			SalesHelper.SetPurchaseOrderPrinted(array, true);
		}
	}
}
