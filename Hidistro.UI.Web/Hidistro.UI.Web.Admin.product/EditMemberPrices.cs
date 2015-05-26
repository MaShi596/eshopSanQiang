using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditMemberPrices : AdminPage
	{
		protected MemberPriceDropDownList ddlMemberPrice;
		protected System.Web.UI.WebControls.TextBox txtTargetPrice;
		protected System.Web.UI.WebControls.Button btnTargetOK;
		protected MemberPriceDropDownList ddlMemberPrice2;
		protected MemberPriceDropDownList ddlSalePrice;
		protected OperationDropDownList ddlOperation;
		protected System.Web.UI.WebControls.TextBox txtOperationPrice;
		protected System.Web.UI.WebControls.Button btnOperationOK;
		protected TrimTextBox txtPrices;
		protected System.Web.UI.WebControls.Button btnSavePrice;
		private string productIds = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSavePrice.Click += new System.EventHandler(this.btnSavePrice_Click);
			this.btnTargetOK.Click += new System.EventHandler(this.btnTargetOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlMemberPrice.DataBind();
				this.ddlMemberPrice.SelectedValue = new int?(-3);
				this.ddlMemberPrice2.DataBind();
				this.ddlMemberPrice2.SelectedValue = new int?(-3);
				this.ddlSalePrice.DataBind();
				this.ddlSalePrice.SelectedValue = new int?(-3);
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
			if (!this.ddlMemberPrice2.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
				return;
			}
			if ((this.ddlMemberPrice2.SelectedValue.Value == -2 || this.ddlMemberPrice2.SelectedValue.Value == -3) && this.ddlSalePrice.SelectedValue.Value != -2 && this.ddlSalePrice.SelectedValue.Value != -3)
			{
				this.ShowMsg("一口价或成本价不能用会员等级价作为标准来按公式计算", false);
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
				if (ProductHelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue.Value, checkPrice, true))
				{
					this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
					return;
				}
			}
			if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue.Value, this.ddlOperation.SelectedValue, num))
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
			if (!this.ddlMemberPrice.SelectedValue.HasValue)
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
			if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice.SelectedValue.Value, num))
			{
				this.ShowMsg("修改成功", true);
			}
		}
		private void btnSavePrice_Click(object sender, System.EventArgs e)
		{
			System.Data.DataSet skuPrices = this.GetSkuPrices();
			if (skuPrices != null && skuPrices.Tables["skuPriceTable"] != null && skuPrices.Tables["skuPriceTable"].Rows.Count != 0)
			{
				if (ProductHelper.UpdateSkuMemberPrices(skuPrices))
				{
					this.CloseWindow();
				}
				return;
			}
			this.ShowMsg("没有任何要修改的项", false);
		}
		private System.Data.DataSet GetSkuPrices()
		{
			System.Data.DataSet dataSet = null;
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Data.DataSet result;
			try
			{
				xmlDocument.LoadXml(this.txtPrices.Text);
				System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList != null && xmlNodeList.Count != 0)
				{
					dataSet = new System.Data.DataSet();
					System.Data.DataTable dataTable = new System.Data.DataTable("skuPriceTable");
					dataTable.Columns.Add("skuId");
					dataTable.Columns.Add("costPrice");
					dataTable.Columns.Add("salePrice");
					System.Data.DataTable dataTable2 = new System.Data.DataTable("skuMemberPriceTable");
					dataTable2.Columns.Add("skuId");
					dataTable2.Columns.Add("gradeId");
					dataTable2.Columns.Add("memberPrice");
					foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
					{
						System.Data.DataRow dataRow = dataTable.NewRow();
						dataRow["skuId"] = xmlNode.Attributes["skuId"].Value;
						dataRow["costPrice"] = (string.IsNullOrEmpty(xmlNode.Attributes["costPrice"].Value) ? 0m : decimal.Parse(xmlNode.Attributes["costPrice"].Value));
						dataRow["salePrice"] = decimal.Parse(xmlNode.Attributes["salePrice"].Value);
						dataTable.Rows.Add(dataRow);
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
