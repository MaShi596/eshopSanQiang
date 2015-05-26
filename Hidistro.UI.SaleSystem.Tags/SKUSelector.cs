using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class SKUSelector : WebControl
	{
		public const string TagID = "SKUSelector";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public DataTable DataSource
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		public SKUSelector()
		{
			base.ID = "SKUSelector";
		}
		protected override void Render(HtmlTextWriter writer)
		{
			DataTable dataSource = this.DataSource;
			if (dataSource != null && dataSource.Rows.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				IList<string> list = new List<string>();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", "hiddenProductId", this.ProductId).AppendLine();
				stringBuilder.AppendFormat("<div id=\"productSkuSelector\" class=\"{0}\">", this.CssClass).AppendLine();
				string text = string.Empty;
				foreach (DataRow dataRow in dataSource.Rows)
				{
					if (!list.Contains((string)dataRow["AttributeName"]))
					{
						list.Add((string)dataRow["AttributeName"]);
						text = text + "\"" + (string)dataRow["AttributeName"] + "\" ";
						stringBuilder.AppendLine("<div class=\"SKURowClass\">");
						stringBuilder.AppendFormat("<span>{0}：</span><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", dataRow["AttributeName"], dataRow["AttributeId"]);
						stringBuilder.AppendFormat("<dl id=\"skuRow_{0}\">", dataRow["AttributeId"]);
						IList<string> list2 = new List<string>();
						foreach (DataRow dataRow2 in dataSource.Rows)
						{
							if (string.Compare((string)dataRow["AttributeName"], (string)dataRow2["AttributeName"]) == 0 && !list2.Contains((string)dataRow2["ValueStr"]))
							{
								string text2 = string.Concat(new object[]
								{
									"skuValueId_",
									dataRow["AttributeId"],
									"_",
									dataRow2["ValueId"]
								});
								list2.Add((string)dataRow2["ValueStr"]);
								if ((bool)dataRow["UseAttributeImage"])
								{
									stringBuilder.AppendFormat("<dd><img class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" src=\"{4}\" /></dd> ", new object[]
									{
										text2,
										dataRow["AttributeId"],
										dataRow2["ValueId"],
										dataRow2["ValueStr"],
										Globals.ApplicationPath + (string)dataRow2["ImageUrl"]
									});
								}
								else
								{
									stringBuilder.AppendFormat("<dd><input type=\"button\" class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" /></dd> ", new object[]
									{
										text2,
										dataRow["AttributeId"],
										dataRow2["ValueId"],
										dataRow2["ValueStr"]
									});
								}
							}
						}
						stringBuilder.AppendLine("</dl></div>");
					}
				}
				stringBuilder.AppendFormat("<div id=\"showSelectSKU\"  class=\"SKUShowSelectClass\">请选择：{0} </div>", text);
				stringBuilder.AppendLine("</div>");
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
