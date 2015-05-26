using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class OnlineGifts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptGifts;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OnlineGifts.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptGifts = (ThemedTemplatedRepeater)this.FindControl("rptGifts");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				this.BindGift();
			}
		}
		private void BindGift()
		{
			DbQueryResult onlineGifts = ProductBrowser.GetOnlineGifts(new Pagination
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			});
			this.rptGifts.DataSource = onlineGifts.Data;
			this.rptGifts.DataBind();
            this.pager.TotalRecords = onlineGifts.TotalRecords;
		}
	}
}
