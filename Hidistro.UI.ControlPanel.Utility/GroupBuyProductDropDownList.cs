using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class GroupBuyProductDropDownList : DropDownList
	{
		private string productCode;
		private int? categoryId;
		private string productName;
		public string ProductCode
		{
			get
			{
				return this.productCode;
			}
			set
			{
				this.productCode = value;
			}
		}
		public int? CategoryId
		{
			get
			{
				return this.categoryId;
			}
			set
			{
				this.categoryId = value;
			}
		}
		public string ProductName
		{
			get
			{
				return this.productName;
			}
			set
			{
				this.productName = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue));
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = this.productName;
			productQuery.ProductCode = this.productCode;
			productQuery.CategoryId = this.categoryId;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(productQuery);
			base.Items.Add(new ListItem("--请选择--", string.Empty));
			foreach (DataRow dataRow in groupBuyProducts.Rows)
			{
				base.Items.Add(new ListItem(dataRow["ProductName"].ToString(), dataRow["ProductId"].ToString()));
			}
		}
	}
}
