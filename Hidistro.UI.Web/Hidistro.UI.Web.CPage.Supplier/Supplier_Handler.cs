using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Text;
using System.Web;
namespace Hidistro.UI.Web.CPage.Supplier
{
	public class Supplier_Handler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				string text = context.Request["action"];
				string a;
				if ((a = text) != null && a == "SupplierGet")
				{
					this.SupplierGet(context);
				}
			}
			catch (System.Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.Write("{\"Status\":\"" + ex.Message.Replace("\"", "'") + "\"}");
			}
		}
		private void SupplierGet(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int userid = 0;
			int.TryParse(context.Request["userid"], out userid);
			System.Data.DataTable dataTable = Methods.Supplier_SupGet(userid);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				System.Data.DataRow dataRow = dataTable.Rows[0];
				decimal num = (decimal)dataRow["SalePrice"];
				decimal num2 = (decimal)dataRow["PurchasePrice"];
				decimal num3 = (decimal)dataRow["LowestSalePrice"];
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"1\",");
				stringBuilder.AppendFormat("\"plSalePrice\":\"{0}\",", num.ToString("f2"));
				stringBuilder.AppendFormat("\"plPurchasePrice\":\"{0}\",", num2.ToString("f2"));
				stringBuilder.AppendFormat("\"plLowestSalePrice\":\"{0}\"", num3.ToString("f2"));
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			context.Response.Write("{\"Status\":\"0\"}");
		}
	}
}
