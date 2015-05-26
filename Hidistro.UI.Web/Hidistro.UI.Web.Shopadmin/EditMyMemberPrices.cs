using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyMemberPrices : DistributorPage
	{
		private string productIds = string.Empty;
		protected UnderlingPriceDropDownList ddlUnderlingPrice;
		protected System.Web.UI.WebControls.TextBox txtTargetPrice;
		protected System.Web.UI.WebControls.Button btnTargetOK;
		protected UnderlingPriceDropDownList ddlUnderlingPrice2;
		protected SalePriceDropDownList ddlSalePrice;
		protected OperationDropDownList ddlOperation;
		protected System.Web.UI.WebControls.TextBox txtOperationPrice;
		protected System.Web.UI.WebControls.Button btnOperationOK;
		protected TrimTextBox txtPrices;
		protected System.Web.UI.WebControls.Button btnSavePrice;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSavePrice.Click += new System.EventHandler(this.btnSavePrice_Click);
			this.btnTargetOK.Click += new System.EventHandler(this.btnTargetOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlUnderlingPrice.DataBind();
				this.ddlUnderlingPrice.SelectedValue = new int?(-3);
				this.ddlUnderlingPrice2.DataBind();
				this.ddlUnderlingPrice2.SelectedValue = new int?(-3);
				this.ddlSalePrice.DataBind();
				this.ddlSalePrice.SelectedValue = "CostPrice";
				this.ddlOperation.DataBind();
				this.ddlOperation.SelectedValue = "+";
			}
		}
		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			if (!this.ddlUnderlingPrice.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
				return;
			}
			if (!this.ddlUnderlingPrice2.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
				return;
			}
			decimal num = 0m;
			if (!decimal.TryParse(this.txtOperationPrice.Text.Trim(), out num))
			{
				this.ShowMsg("请输入正确的价格", false);
				return;
			}
			if (this.ddlOperation.SelectedValue == "*" && num <= 0m)
			{
				this.ShowMsg("必须乘以一个正数", false);
				return;
			}
			if (this.ddlOperation.SelectedValue == "+" && num < 0m)
			{
				decimal checkPrice = -num;
				if (SubSiteProducthelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue, checkPrice))
				{
					this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
					return;
				}
			}
			if (SubSiteProducthelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue, num, this.ddlOperation.SelectedValue))
			{
				this.ShowMsg("公式调价的计算结果导致价格超出了系统表示范围", false);
				return;
			}
			if (SubSiteProducthelper.UpdateSkuUnderlingPrices(this.productIds, this.ddlUnderlingPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue, this.ddlOperation.SelectedValue, num))
			{
				this.ShowMsg("修改商品的价格成功", true);
			}
		}
		private void btnTargetOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			if (!this.ddlUnderlingPrice.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
				return;
			}
			decimal num = 0m;
			if (!decimal.TryParse(this.txtTargetPrice.Text.Trim(), out num))
			{
				this.ShowMsg("请输入正确的价格", false);
				return;
			}
			if (num <= 0m)
			{
				this.ShowMsg("直接调价必须输入正数", false);
				return;
			}
			if (num > 10000000m)
			{
				this.ShowMsg("直接调价超出了系统表示范围", false);
				return;
			}
			if (SubSiteProducthelper.UpdateSkuUnderlingPrices(this.productIds, this.ddlUnderlingPrice.SelectedValue.Value, num))
			{
				this.ShowMsg("修改商品的价格成功", true);
			}
		}
		private void btnSavePrice_Click(object sender, System.EventArgs e)
		{
			string empty = string.Empty;
			System.Data.DataSet skuPrices = this.GetSkuPrices(out empty);
			if (string.IsNullOrEmpty(empty))
			{
				this.ShowMsg("没有任何要修改的项", false);
				return;
			}
			if (SubSiteProducthelper.UpdateSkuUnderlingPrices(skuPrices, empty))
			{
				this.ShowMsg("修改商品的价格成功", true);
			}
		}
		private System.Data.DataSet GetSkuPrices(out string skuIds)
		{
			System.Data.DataSet dataSet = null;
			skuIds = string.Empty;
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Data.DataSet result;
			try
			{
				xmlDocument.LoadXml(this.txtPrices.Text);
				System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList != null && xmlNodeList.Count != 0)
				{
					System.Collections.Generic.IList<SKUItem> skus = SubSiteProducthelper.GetSkus(this.productIds);
					dataSet = new System.Data.DataSet();
					System.Data.DataTable dataTable = new System.Data.DataTable("skuPriceTable");
					dataTable.Columns.Add("skuId");
					dataTable.Columns.Add("salePrice");
					System.Data.DataTable dataTable2 = new System.Data.DataTable("skuMemberPriceTable");
					dataTable2.Columns.Add("skuId");
					dataTable2.Columns.Add("gradeId");
					dataTable2.Columns.Add("memberPrice");
					foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
					{
						string value = xmlNode.Attributes["skuId"].Value;
						decimal num = decimal.Parse(xmlNode.Attributes["salePrice"].Value);
						skuIds = skuIds + "'" + value + "',";
						foreach (SKUItem current in skus)
						{
							if (current.SkuId == value && current.SalePrice != num)
							{
								System.Data.DataRow dataRow = dataTable.NewRow();
								dataRow["skuId"] = value;
								dataRow["salePrice"] = num;
								dataTable.Rows.Add(dataRow);
							}
						}
						System.Xml.XmlNodeList childNodes = xmlNode.SelectSingleNode("skuMemberPrices").ChildNodes;
						foreach (System.Xml.XmlNode xmlNode2 in childNodes)
						{
							System.Data.DataRow dataRow2 = dataTable2.NewRow();
							dataRow2["skuId"] = xmlNode.Attributes["skuId"].Value;
							dataRow2["gradeId"] = int.Parse(xmlNode2.Attributes["gradeId"].Value);
							dataRow2["memberPrice"] = decimal.Parse(xmlNode2.Attributes["memberPrice"].Value);
							dataTable2.Rows.Add(dataRow2);
						}
					}
					dataSet.Tables.Add(dataTable);
					dataSet.Tables.Add(dataTable2);
					if (skuIds.Length > 1)
					{
						skuIds = skuIds.Remove(skuIds.Length - 1);
					}
					return dataSet;
				}
				result = null;
			}
			catch
			{
				return dataSet;
			}
			return result;
		}
	}
}
