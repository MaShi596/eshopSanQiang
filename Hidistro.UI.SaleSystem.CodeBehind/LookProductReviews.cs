using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Globalization;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class LookProductReviews : HtmlTemplatedWebControl
	{
		private int productId = 0;
		private ThemedTemplatedRepeater rptRecords;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-LookProductReviews.html";
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
				PageTitle.AddSiteNameTitle("商品评论", Hidistro.Membership.Context.HiContext.Current.Context);
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
			DbQueryResult productReviews = ProductBrowser.GetProductReviews(new Pagination
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			}, this.productId);
			this.rptRecords.DataSource = productReviews.Data;
			this.rptRecords.DataBind();
            this.pager.TotalRecords = productReviews.TotalRecords;
		}
	}
}
