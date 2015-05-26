using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CategoriesWithWindow : AscxTemplatedWebControl
	{
		private Repeater recordsone;
        //×Ó·ÖÀà
        private Repeater rphotkey;
		private int maxCNum = 13;
		private int maxBNum = 1000;
		public int MaxCNum
		{
			get
			{
				return this.maxCNum;
			}
			set
			{
				this.maxCNum = value;
			}
		}
		public int MaxBNum
		{
			get
			{
				return this.maxBNum;
			}
			set
			{
				this.maxBNum = value;
			}
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-CategoriesWithWindow.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.recordsone = (Repeater)this.FindControl("recordsone");
			this.recordsone.ItemDataBound += new RepeaterItemEventHandler(this.recordsone_ItemDataBound);
			this.recordsone.ItemCreated += new RepeaterItemEventHandler(this.recordsone_ItemCreated);
			IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(0, this.maxCNum);
			if (maxSubCategories != null && maxSubCategories.Count > 0)
			{
				this.recordsone.DataSource = maxSubCategories;
				this.recordsone.DataBind();
			}
		}
		private void recordsone_ItemCreated(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordstwo");
			repeater.ItemDataBound += new RepeaterItemEventHandler(this.recordstwo_ItemDataBound);
		}
		private void recordsone_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordstwo");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidMainCategoryId");
			Repeater repeater2 = (Repeater)control.FindControl("recordsbrands");
            rphotkey = (Repeater)control.FindControl("rphotkey");
			repeater2.DataSource = CategoryBrowser.GetBrandCategories(int.Parse(htmlInputHidden.Value), 12);
			repeater2.DataBind();
			repeater.DataSource = CategoryBrowser.GetMaxSubCategories(int.Parse(htmlInputHidden.Value), 1000);
			repeater.DataBind();
            this.rphotkey.DataSource = CategoryBrowser.GetMaxSubCategories(int.Parse(htmlInputHidden.Value), 3);
            rphotkey.DataBind();
		}
		private void recordstwo_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordsthree");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidTwoCategoryId");
			repeater.DataSource = CategoryBrowser.GetMaxSubCategories(int.Parse(htmlInputHidden.Value), 1000);
			repeater.DataBind();
		}
	}
}
