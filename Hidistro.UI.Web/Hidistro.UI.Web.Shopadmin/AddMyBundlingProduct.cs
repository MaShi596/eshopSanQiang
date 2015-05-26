using Hidistro.ControlPanel.Commodities;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyBundlingProduct : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtBindName;
		protected YesNoRadioButtonList radstock;
		protected System.Web.UI.WebControls.TextBox txtNum;
		protected System.Web.UI.WebControls.TextBox txtSalePrice;
		protected TrimTextBox txtShortDescription;
		protected System.Web.UI.WebControls.Button btnAddBindProduct;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
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
		protected void DoCallback()
		{
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
			productQuery.UserId = new int?(Hidistro.Membership.Context.HiContext.Current.User.UserId);
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
		protected void btnAddBindProduct_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(base.Request.Form["selectProductsinfo"]))
			{
				this.ShowMsg("获取绑定商品信息错误", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtBindName.Text) || string.IsNullOrEmpty(this.txtNum.Text) || string.IsNullOrEmpty(this.txtSalePrice.Text))
			{
				this.ShowMsg("你的资料填写不完整", false);
				return;
			}
			BundlingInfo bundlingInfo = new BundlingInfo();
			bundlingInfo.AddTime = System.DateTime.Now;
			bundlingInfo.Name = this.txtBindName.Text;
			bundlingInfo.Price = System.Convert.ToDecimal(this.txtSalePrice.Text);
			bundlingInfo.SaleStatus = System.Convert.ToInt32(this.radstock.SelectedValue);
			bundlingInfo.Num = System.Convert.ToInt32(this.txtNum.Text);
			if (!string.IsNullOrEmpty(this.txtShortDescription.Text))
			{
				bundlingInfo.ShortDescription = this.txtShortDescription.Text;
			}
			string text = base.Request.Form["selectProductsinfo"];
			string[] array = text.Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<BundlingItemInfo> list = new System.Collections.Generic.List<BundlingItemInfo>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				BundlingItemInfo bundlingItemInfo = new BundlingItemInfo();
				string[] array3 = text2.Split(new char[]
				{
					'|'
				});
				bundlingItemInfo.ProductID = System.Convert.ToInt32(array3[0]);
				bundlingItemInfo.SkuId = array3[1];
				bundlingItemInfo.ProductNum = System.Convert.ToInt32(array3[2]);
				list.Add(bundlingItemInfo);
			}
			bundlingInfo.BundlingItemInfos = list;
			if (SubsitePromoteHelper.AddBundlingProduct(bundlingInfo))
			{
				this.ShowMsg("添加绑定商品成功", true);
				return;
			}
			this.ShowMsg("添加绑定商品错误", false);
		}
	}
}
