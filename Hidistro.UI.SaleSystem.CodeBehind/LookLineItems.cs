using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class LookLineItems : HtmlTemplatedWebControl
	{
		private int productId = 0;
		private ThemedTemplatedRepeater rptRecords;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-LookLineItems.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.rptRecords = (ThemedTemplatedRepeater)this.FindControl("rptRecords");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("商品成交记录", Hidistro.Membership.Context.HiContext.Current.Context);
				this.BindData();
			}
		}
		private void ReBind()
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"pageIndex",
					this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture)
				}
			});
		}
		private void BindData()
		{
			DbQueryResult lineItems = ProductBrowser.GetLineItems(new Pagination
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			}, this.productId);
			DataTable dataTable = lineItems.Data as DataTable;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string text = (string)dataRow["Username"];
				if (text.ToLower() == "anonymous")
				{
					dataRow["Username"] = "匿名用户";
				}
				else
				{
					dataRow["Username"] = text.Substring(0, 1) + "**" + text.Substring(text.Length - 1);
				}
			}
			this.rptRecords.DataSource = dataTable;
			this.rptRecords.DataBind();
            this.pager.TotalRecords = lineItems.TotalRecords;
		}
	}
}
