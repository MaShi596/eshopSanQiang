using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.GroupBuy)]
	public class AddGroupBuy : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected GroupBuyProductDropDownList dropGroupBuyProduct;
		protected System.Web.UI.WebControls.Label lblPrice;
		protected WebCalendar calendarStartDate;
		protected HourDropDownList drophours;
		protected WebCalendar calendarEndDate;
		protected HourDropDownList HourDropDownList1;
		protected System.Web.UI.WebControls.TextBox txtNeedPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxCount;
		protected System.Web.UI.WebControls.TextBox txtCount;
		protected System.Web.UI.WebControls.TextBox txtPrice;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnAddGroupBuy;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
				return;
			}
			this.btnAddGroupBuy.Click += new System.EventHandler(this.btnAddGroupBuy_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropGroupBuyProduct.DataBind();
				this.HourDropDownList1.DataBind();
				this.drophours.DataBind();
			}
		}
		private void btnAddGroupBuy_Click(object sender, System.EventArgs e)
		{
			GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
			string text = string.Empty;
			if (this.dropGroupBuyProduct.SelectedValue > 0)
			{
				if (PromoteHelper.ProductGroupBuyExist(this.dropGroupBuyProduct.SelectedValue.Value))
				{
					this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
					return;
				}
				groupBuyInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
			}
			else
			{
				text += Formatter.FormatErrorMessage("请选择团购商品");
			}
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择开始日期");
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				groupBuyInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
				if (System.DateTime.Compare(groupBuyInfo.EndDate, System.DateTime.Now) < 0)
				{
					text += Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
				}
				else
				{
					if (System.DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value), groupBuyInfo.EndDate) >= 0)
					{
						text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
					}
					else
					{
						groupBuyInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.txtNeedPrice.Text))
			{
				decimal needPrice;
				if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out needPrice))
				{
					groupBuyInfo.NeedPrice = needPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("违约金填写格式不正确");
				}
			}
			int maxCount;
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				groupBuyInfo.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}
			GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
			int count;
			if (int.TryParse(this.txtCount.Text.Trim(), out count))
			{
				gropBuyConditionInfo.Count = count;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
			}
			decimal price;
			if (decimal.TryParse(this.txtPrice.Text.Trim(), out price))
			{
				gropBuyConditionInfo.Price = price;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
			}
			groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
			if (groupBuyInfo.MaxCount < groupBuyInfo.GroupBuyConditions[0].Count)
			{
				text += Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return;
			}
			groupBuyInfo.Content = this.txtContent.Text;
			if (PromoteHelper.AddGroupBuy(groupBuyInfo))
			{
				this.ShowMsg("添加团购活动成功", true);
				return;
			}
			this.ShowMsg("添加团购活动失败", true);
		}
		private void DoCallback()
		{
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			string text = base.Request.QueryString["action"];
			if (text.Equals("getGroupBuyProducts"))
			{
				ProductQuery productQuery = new ProductQuery();
				int num;
				int.TryParse(base.Request.QueryString["categoryId"], out num);
				string productCode = base.Request.QueryString["sku"];
				string keywords = base.Request.QueryString["productName"];
				productQuery.Keywords = keywords;
				productQuery.ProductCode = productCode;
				productQuery.SaleStatus = ProductSaleStatus.OnSale;
				if (num > 0)
				{
					productQuery.CategoryId = new int?(num);
					productQuery.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
				}
				System.Data.DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(productQuery);
				if (groupBuyProducts != null && groupBuyProducts.Rows.Count != 0)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.Append("{\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"Product\":[{0}]", this.GenerateBrandString(groupBuyProducts));
					stringBuilder.Append("}");
					base.Response.Write(stringBuilder.ToString());
				}
				else
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
			}
			base.Response.End();
		}
		private string GenerateBrandString(System.Data.DataTable tb)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in tb.Rows)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"ProductId\":\"{0}\",\"ProductName\":\"{1}\"", dataRow["ProductId"], dataRow["ProductName"]);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
	}
}
