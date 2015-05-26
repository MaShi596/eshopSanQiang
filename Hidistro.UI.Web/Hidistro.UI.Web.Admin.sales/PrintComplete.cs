using Hidistro.ControlPanel.Sales;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
namespace Hidistro.UI.Web.Admin.sales
{
	public class PrintComplete : AdminPage
	{
		protected string script;
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
			OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), base.Request["templateName"], base.Request["templateName"]);
			OrderHelper.SetOrderShipNumber(array, startNumber);
			OrderHelper.SetOrderPrinted(array, true);
		}
	}
}
