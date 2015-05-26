using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Promotes : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptPromoteSales;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Promotes.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptPromoteSales = (ThemedTemplatedRepeater)this.FindControl("rptPromoteSales");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("优惠活动中心", Hidistro.Membership.Context.HiContext.Current.Context);
				this.BindPromoteSales();
			}
		}
		private void BindPromoteSales()
		{
			int promotiontype = 0;
			int num;
			if (int.TryParse(this.Page.Request.QueryString["promoteType"], out num))
			{
				promotiontype = num;
			}
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			int totalRecords = 0;
			DataTable promotes = CommentBrowser.GetPromotes(pagination, promotiontype, out totalRecords);
			if (promotes != null && promotes.Rows.Count > 0)
			{
				this.rptPromoteSales.DataSource = promotes;
				this.rptPromoteSales.DataBind();
			}
            this.pager.TotalRecords = totalRecords;
		}
	}
}
