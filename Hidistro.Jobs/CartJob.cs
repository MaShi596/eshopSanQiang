using Hidistro.Core.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;
namespace Hidistro.Jobs
{
	public class CartJob : IJob
	{
		public void Execute(XmlNode node)
		{
			int num = 5;
			XmlAttribute xmlAttribute = node.Attributes["expires"];
			if (xmlAttribute != null)
			{
				int.TryParse(xmlAttribute.Value, out num);
			}
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM distro_ShoppingCarts WHERE AddTime <= @CurrentTime;DELETE FROM Hishop_ShoppingCarts WHERE AddTime <= @CurrentTime;DELETE FROM Hishop_PurchaseShoppingCarts WHERE AddTime <= @CurrentTime;");
			database.AddInParameter(sqlStringCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now.AddDays((double)(-(double)num)));
			database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
