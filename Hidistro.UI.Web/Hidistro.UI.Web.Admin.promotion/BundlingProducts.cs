using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.BindProduct)]
	public class BundlingProducts : AdminPage
	{
		private string productName = string.Empty;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected Grid grdBundlingList;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindBundlingProducts();
			}
			this.grdBundlingList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdBundlingList_RowDeleting);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void BindBundlingProducts()
		{
			DbQueryResult bundlingProducts = PromoteHelper.GetBundlingProducts(new BundlingInfoQuery
			{
				ProductName = this.productName,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc
			});
			this.grdBundlingList.DataSource = bundlingProducts.Data;
			this.grdBundlingList.DataBind();
            this.pager.TotalRecords = bundlingProducts.TotalRecords;
            this.pager1.TotalRecords = bundlingProducts.TotalRecords;
		}
		private void grdBundlingList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (PromoteHelper.DeleteBundlingProduct((int)this.grdBundlingList.DataKeys[e.RowIndex].Value))
			{
				this.BindBundlingProducts();
				this.ShowMsg("成功删除了选择的捆绑商品！", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdBundlingList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int bundlingID = System.Convert.ToInt32(this.grdBundlingList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					PromoteHelper.DeleteBundlingProduct(bundlingID);
				}
			}
			if (num != 0)
			{
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除\"{0}\"个捆绑商品", new object[]
				{
					num
				}), true);
				this.BindBundlingProducts();
				return;
			}
			this.ShowMsg("请先选择需要删除的捆绑商品", false);
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			nameValueCollection.Add("SortBy", this.grdBundlingList.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
				{
					this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				}
				this.txtProductName.Text = this.productName;
				return;
			}
			this.productName = this.txtProductName.Text;
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
	}
}
