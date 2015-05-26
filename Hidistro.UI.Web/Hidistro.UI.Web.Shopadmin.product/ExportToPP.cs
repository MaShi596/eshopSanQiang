using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.product
{
	public class ExportToPP : DistributorPage
	{
		private string _productName;
		private string _productCode;
		private int? _lineId;
		private System.DateTime? _startDate;
		private System.DateTime? _endDate;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductLineDropDownList dropLines;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.Label lblTotals;
		protected System.Web.UI.WebControls.DropDownList dropExportVersions;
		protected System.Web.UI.WebControls.CheckBox chkExportStock;
		protected System.Web.UI.WebControls.Button btnExport;
		protected Grid grdProducts;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.grdProducts.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdProducts_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.dropLines.DataBind();
				this.BindExporter();
			}
			this.LoadParameters();
		}
		private void grdProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Remove")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int num = (int)this.grdProducts.DataKeys[rowIndex].Value;
				string text = (string)this.ViewState["RemoveProductIds"];
				if (string.IsNullOrEmpty(text))
				{
					text = num.ToString();
				}
				else
				{
					text = text + "," + num.ToString();
				}
				this.ViewState["RemoveProductIds"] = text;
				this.BindProducts();
			}
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			string selectedValue = this.dropExportVersions.SelectedValue;
			if (!string.IsNullOrEmpty(selectedValue) && selectedValue.Length != 0)
			{
				bool flag = false;
				bool @checked = this.chkExportStock.Checked;
				bool flag2 = true;
				string text = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ((System.Web.HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + System.Web.HttpContext.Current.Request.Url.Port)) + Globals.ApplicationPath;
				string applicationPath = Globals.ApplicationPath;
				System.Data.DataSet exportProducts = SubSiteProducthelper.GetExportProducts(this.GetQuery(), flag, @checked, (string)this.ViewState["RemoveProductIds"]);
				ExportAdapter exporter = TransferHelper.GetExporter(selectedValue, new object[]
				{
					exportProducts,
					flag,
					@checked,
					flag2,
					text,
					applicationPath
				});
				exporter.DoExport();
				return;
			}
			this.ShowMsg("请选择一个导出版本", false);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReSearchProducts();
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindProducts();
			}
		}
		private void BindExporter()
		{
			this.dropExportVersions.Items.Clear();
			this.dropExportVersions.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
			System.Collections.Generic.Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "拍拍助理");
			foreach (string current in exportAdapters.Keys)
			{
				this.dropExportVersions.Items.Add(new System.Web.UI.WebControls.ListItem(exportAdapters[current], current));
			}
		}
		private void BindProducts()
		{
			AdvancedProductQuery query = this.GetQuery();
			DbQueryResult exportProducts = SubSiteProducthelper.GetExportProducts(query, (string)this.ViewState["RemoveProductIds"]);
			this.grdProducts.DataSource = exportProducts.Data;
			this.grdProducts.DataBind();
            this.pager.TotalRecords = exportProducts.TotalRecords;
			this.lblTotals.Text = exportProducts.TotalRecords.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
		private AdvancedProductQuery GetQuery()
		{
			AdvancedProductQuery advancedProductQuery = new AdvancedProductQuery
			{
				Keywords = this._productName,
				ProductCode = this._productCode,
				ProductLineId = this._lineId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SaleStatus = ProductSaleStatus.OnSale,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				StartDate = this._startDate,
				EndDate = this._endDate
			};
			Globals.EntityCoding(advancedProductQuery, true);
			return advancedProductQuery;
		}
		private void ReSearchProducts()
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection
			{

				{
					"productName",
					Globals.UrlEncode(this.txtSearchText.Text.Trim())
				},

				{
					"productCode",
					Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim()))
				},

				{
					"pageSize",
					this.pager.PageSize.ToString()
				}
			};
			if (this.dropLines.SelectedValue.HasValue)
			{
				nameValueCollection.Add("lineId", this.dropLines.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			this._productName = this.txtSearchText.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this._productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				this.txtSearchText.Text = this._productName;
			}
			this._productCode = this.txtSKU.Text.Trim();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this._productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
				this.txtSKU.Text = this._productCode;
			}
			this._lineId = this.dropLines.SelectedValue;
			int value;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]) && int.TryParse(this.Page.Request.QueryString["lineId"], out value))
			{
				this._lineId = new int?(value);
				this.dropLines.SelectedValue = this._lineId;
			}
			this._startDate = this.calendarStartDate.SelectedDate;
			System.DateTime value2;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["startDate"], out value2))
			{
				this._startDate = new System.DateTime?(value2);
                this.calendarStartDate.SelectedDate = this._startDate;
			}
			this._endDate = this.calendarEndDate.SelectedDate;
			System.DateTime value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) && System.DateTime.TryParse(this.Page.Request.QueryString["endDate"], out value3))
			{
				this._endDate = new System.DateTime?(value3);
                this.calendarEndDate.SelectedDate = this._endDate;
			}
		}
	}
}
