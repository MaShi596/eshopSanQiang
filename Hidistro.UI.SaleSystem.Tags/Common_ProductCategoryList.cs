using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductCategoryList : ThemedTemplatedRepeater
	{
		private int categoryId;
		private int maxNum = 1000;
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		public bool IsShowSubCategory
		{
			get;
			set;
		}
		protected override void OnLoad(EventArgs eventArgs_0)
		{
			base.ItemDataBound += new RepeaterItemEventHandler(this.Common_ProductCategoryList_ItemDataBound);
			if (this.IsShowSubCategory)
			{
				int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			}
			this.BindList();
		}
		private void Common_ProductCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			int parentCategoryId = ((CategoryInfo)e.Item.DataItem).CategoryId;
			Repeater repeater = (Repeater)e.Item.Controls[0].FindControl("rptSubCategries");
			if (repeater != null)
			{
				repeater.DataSource = CategoryBrowser.GetMaxSubCategories(parentCategoryId, 1000);
				repeater.DataBind();
			}
		}
		private void BindList()
		{
			if (this.categoryId != 0)
			{
				base.DataSource = CategoryBrowser.GetMaxSubCategories(this.categoryId, this.MaxNum);
				base.DataBind();
				return;
			}
			base.DataSource = CategoryBrowser.GetMaxMainCategories(this.MaxNum);
			base.DataBind();
		}
	}
}
