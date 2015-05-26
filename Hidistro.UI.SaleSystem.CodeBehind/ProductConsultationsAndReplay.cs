using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ProductConsultationsAndReplay : HtmlTemplatedWebControl
	{
		private int productId = 0;
		private Pager pager;
		private ThemedTemplatedRepeater rptRecords;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductConsultationsAndReplay.html";
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
				PageTitle.AddSiteNameTitle("商品咨询", Hidistro.Membership.Context.HiContext.Current.Context);
				this.BindData();
			}
		}
		private void BindData()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = pagination.PageSize;
			DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(pagination, this.productId);
			this.rptRecords.DataSource = productConsultations.Data;
			this.rptRecords.DataBind();
            this.pager.TotalRecords = productConsultations.TotalRecords;
		}
	}
}
