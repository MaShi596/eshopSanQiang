using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class BundlingProducts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BundlingProducts.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.pager = (Pager)this.FindControl("pager");
			this.rptProduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptProduct_ItemDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindBundlingProducts();
			}
		}
		protected void rptProduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item)
			{
				DataRowView arg_37_0 = (DataRowView)e.Item.DataItem;
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.Controls[0].FindControl("lbBundlingID");
				FormatedMoneyLabel formatedMoneyLabel = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lbltotalPrice");
				FormatedMoneyLabel formatedMoneyLabel2 = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lblbundlingPrice");
				FormatedMoneyLabel formatedMoneyLabel3 = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lblsaving");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.Controls[0].FindControl("hlBuy");
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.Controls[0].FindControl("rpbundlingitem");
				System.Collections.Generic.List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(System.Convert.ToInt32(label.Text));
				decimal num = 0m;
				foreach (BundlingItemInfo current in bundlingItemsByID)
				{
					num += current.ProductNum * current.ProductPrice;
				}
				formatedMoneyLabel.Money = num;
				formatedMoneyLabel3.Money = System.Convert.ToDecimal(formatedMoneyLabel.Money) - System.Convert.ToDecimal(formatedMoneyLabel2.Money);
				if (!Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && !Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsOpenSiteSale)
				{
					hyperLink.Visible = false;
				}
				repeater.DataSource = bundlingItemsByID;
				repeater.DataBind();
			}
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindBundlingProducts()
		{
			DbQueryResult bundlingProductList = ProductBrowser.GetBundlingProductList(new BundlingInfoQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			});
			this.rptProduct.DataSource = bundlingProductList.Data;
			this.rptProduct.DataBind();
            this.pager.TotalRecords = bundlingProductList.TotalRecords;
		}
		public System.Collections.Generic.List<BundlingItemInfo> BindBundlingItems(int int_0)
		{
			return ProductBrowser.GetBundlingItemsByID(int_0);
		}
	}
}
