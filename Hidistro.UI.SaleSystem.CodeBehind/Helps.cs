using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Helps : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptHelps;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Helps.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptHelps = (ThemedTemplatedRepeater)this.FindControl("rptHelps");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int categoryId = 0;
					int.TryParse(this.Page.Request.QueryString["CategoryId"], out categoryId);
					HelpCategoryInfo helpCategory = CommentBrowser.GetHelpCategory(categoryId);
					if (helpCategory != null)
					{
						PageTitle.AddSiteNameTitle(helpCategory.Name, Hidistro.Membership.Context.HiContext.Current.Context);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("帮助中心", Hidistro.Membership.Context.HiContext.Current.Context);
				}
				this.BindList();
			}
		}
		private void BindList()
		{
			HelpQuery helpQuery = this.GetHelpQuery();
			DbQueryResult dbQueryResult = new DbQueryResult();
			dbQueryResult = CommentBrowser.GetHelpList(helpQuery);
			this.rptHelps.DataSource = dbQueryResult.Data;
			this.rptHelps.DataBind();
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
		}
		private HelpQuery GetHelpQuery()
		{
			HelpQuery helpQuery = new HelpQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
				{
					helpQuery.CategoryId = new int?(value);
				}
			}
			helpQuery.PageIndex = this.pager.PageIndex;
			helpQuery.PageSize = this.pager.PageSize;
			helpQuery.SortBy = "AddedDate";
			helpQuery.SortOrder = SortAction.Desc;
			return helpQuery;
		}
	}
}
