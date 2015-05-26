using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class supplier_Admin_StockAdd : AdminPage
	{
		protected WebCalendar calendarStartDate;
		protected System.Web.UI.WebControls.TextBox txtStockCode;
		protected System.Web.UI.WebControls.TextBox txt_Options;
		protected System.Web.UI.WebControls.Button btn_Submits;
		protected System.Web.UI.WebControls.Button btn_Submit_Add;
		protected System.Web.UI.WebControls.Button btn_Submits_Add;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true";
			this.btn_Submits.Click += new System.EventHandler(this.btn_Submits_Click);
			this.btn_Submit_Add.Click += new System.EventHandler(this.btn_Submit_Add_Click);
			this.btn_Submits_Add.Click += new System.EventHandler(this.btn_Submits_Add_Click);
			string numPwd = supplier_Admin_StockAdd.GetNumPwd(6);
			string text = System.DateTime.Now.ToString("yyyy-MM-dd");
			this.calendarStartDate.Text = text;
			this.txtStockCode.Text = System.DateTime.Now.ToString("yyyyMMdd") + Hidistro.Membership.Context.HiContext.Current.User.UserId + numPwd;
			if (flag)
			{
				this.DoCallback_S();
			}
		}
		public string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				System.Data.DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(skuId);
				foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
				{
					if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							dataRow["AttributeName"],
							":",
							dataRow["ValueStr"],
							"; "
						});
					}
				}
			}
			if (!(text == ""))
			{
				return text;
			}
			return "无规格";
		}
		protected void DoCallback_S()
		{
			this.txtStockCode.Text = "14e24234";
		}
		protected void DoCallback()
		{
			this.DoCallback_S();
			this.Page.Response.Clear();
			base.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int num = 0;
			int num2 = 0;
			int pageIndex = 1;
			int.TryParse(base.Request.Params["categoryId"], out num);
			int.TryParse(base.Request.Params["brandId"], out num2);
			int.TryParse(base.Request.Params["page"], out pageIndex);
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = 5;
			productQuery.PageIndex = pageIndex;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.Keywords = base.Request.Params["serachName"];
			if (num2 != 0)
			{
				productQuery.BrandId = new int?(num2);
			}
			productQuery.CategoryId = new int?(num);
			if (num != 0)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
			}
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			stringBuilder.Append("{'data':[");
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{'ProductId':'");
				stringBuilder.Append(dataTable.Rows[i]["ProductId"].ToString().Trim());
				stringBuilder.Append("','Name':'");
				stringBuilder.Append(dataTable.Rows[i]["ProductName"].ToString());
				stringBuilder.Append("','Price':'");
				stringBuilder.Append(((decimal)dataTable.Rows[i]["SalePrice"]).ToString("F2"));
				stringBuilder.Append("','Stock':'");
				stringBuilder.Append(dataTable.Rows[i]["Stock"].ToString());
				stringBuilder.Append("','ProductCode':'");
				stringBuilder.Append(dataTable.Rows[i]["ProductCode"].ToString());
				System.Data.DataTable skusByProductId = ProductHelper.GetSkusByProductId((int)dataTable.Rows[i]["ProductId"]);
				stringBuilder.Append("','skus':[");
				int count = skusByProductId.Rows.Count;
				for (int j = 0; j < count; j++)
				{
					stringBuilder.Append("{'skuid':'");
					stringBuilder.Append(skusByProductId.Rows[j]["skuid"].ToString());
					stringBuilder.Append("','stock':'");
					stringBuilder.Append(skusByProductId.Rows[j]["stock"].ToString());
					stringBuilder.Append("','skucontent':'");
					stringBuilder.Append(this.GetSkuContent(skusByProductId.Rows[j]["skuid"].ToString()));
					stringBuilder.Append("','saleprice':'");
					stringBuilder.Append(((decimal)skusByProductId.Rows[j]["saleprice"]).ToString("F2"));
					stringBuilder.Append("'},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("],'recCount':'");
			stringBuilder.Append(products.TotalRecords);
			stringBuilder.Append("'}");
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}
		protected void btn_Submit_Add_Click(object sender, System.EventArgs e)
		{
			this.InsertStock();
		}
		protected void btn_Submits_Click(object sender, System.EventArgs e)
		{
		}
		protected void btn_Submits_Add_Click(object sender, System.EventArgs e)
		{
			this.InsertStock();
		}
		private void InsertStock()
		{
			if (!string.IsNullOrEmpty(this.calendarStartDate.Text) && !string.IsNullOrEmpty(this.txtStockCode.Text))
			{
				System.DateTime addDate = System.DateTime.Parse(this.calendarStartDate.Text);
				string text = this.txtStockCode.Text;
				if (string.IsNullOrEmpty(text))
				{
					text = System.DateTime.Now.ToString("yyyyMMdd") + Hidistro.Membership.Context.HiContext.Current.User.UserId + supplier_Admin_StockAdd.GetNumPwd(6);
				}
				int status = 1;
				string options = this.txt_Options.Text.ToString();
				int allCount = int.Parse(base.Request.Form["selectAllNums"]);
				int userId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
				string text2 = base.Request.Form["selectProductsinfo"];
				string[] array = text2.Split(new char[]
				{
					','
				});
				if (string.IsNullOrEmpty(text2))
				{
					return;
				}
				string s = Methods.Supplier_StockInfoInsert(addDate, text, status, allCount, options, userId);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text3 = array2[i];
					string[] array3 = text3.Split(new char[]
					{
						'|'
					});
					Methods.Supplier_StockItemInsert(int.Parse(s), array3[1], int.Parse(array3[2]), decimal.Parse(array3[3]));
					Methods.Supplier_StockAddfor_UpdateSkus(array3[1], int.Parse(array3[2]));
				}
			}
			else
			{
				this.ShowMsg("你的资料填写不完整", false);
			}
			this.ShowMsg("录入库存成功", true);
		}
		public static string GetNumPwd(int numCount)
		{
			string text = "0123456789";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < numCount; i++)
			{
				stringBuilder.Append(text[new System.Random(System.Guid.NewGuid().GetHashCode()).Next(0, text.Length - 1)]);
			}
			return stringBuilder.ToString();
		}
	}
}
